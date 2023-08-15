using JobExchange.Areas.Identity.Data;
using System.Security.Claims;
using JobExchange.Models;
using JobExchange.Repository;
using JobExchange.Repository.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using JobExchange.DataModel;
using Microsoft.EntityFrameworkCore;

namespace JobExchange.Controllers
{
    public class JobController : Controller
    {
        private readonly IRecruitmentRepository _recruitmentRepository;
        private readonly ICandidateRecruitmentRepository _candidateRecruitmentRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ISaveJobRepository _saveJobRepository;
        private readonly UserManager<JobExchangeUser> _userManager;
        private readonly SignInManager<JobExchangeUser> _signInManager;
        private readonly JobExchangeContext _context;
        public JobController(SignInManager<JobExchangeUser> signInManager, ICandidateRecruitmentRepository candidateRecruitmentRepository, IRecruitmentRepository recruitmentRepository, ICompanyRepository companyRepository , JobExchangeContext context, ISaveJobRepository saveJobRepository, UserManager<JobExchangeUser> userManager)
        {
            _context = context;
            _recruitmentRepository = recruitmentRepository;
            _companyRepository = companyRepository;
            _saveJobRepository = saveJobRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _candidateRecruitmentRepository = candidateRecruitmentRepository;
            _context = context;
        }
        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (User.IsInRole("ROLE_ADMIN"))
                {
                    return RedirectToAction("Index", "Industry");
                }
                else if (User.IsInRole("ROLE_COMPANY"))
                {
                    return RedirectToAction("Index", "Recruitment");
                }
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data.json");

            if (System.IO.File.Exists(filePath))
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
                var data = System.Text.Json.JsonSerializer.Deserialize<List<ProvinceDataModel>>(jsonData);

                // Check if the deserialization was successful and data is not null
                if (data != null)
                {
                    ViewData["IndustryId"] = new SelectList(_context.Industries, "IndustryId", "IndustryName");
                    ViewBag.City = data;
                    return View();
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Error");
            }
        }
        //GetRecruitments
        [HttpPost]
        public IActionResult GetRecruitments(string filter = null, string value1 = null, string value2 = null)
        {
            dynamic recruitments;
            if (string.IsNullOrEmpty(_userManager.GetUserId(User)))
            {
                recruitments = _recruitmentRepository.GetRecruitments(12, filter, value1, value2);
            } else
            {
                recruitments = _recruitmentRepository.GetRecruitments(_userManager.GetUserId(User), 12, filter, value1, value2);
            }

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // Bỏ qua vòng lặp tự tham chiếu
            };
            string json = JsonConvert.SerializeObject(recruitments, Formatting.Indented, settings);
            return Content(json, "application/json");
        }
        //GetTopCompanies
        [HttpPost]
        public IActionResult GetTopCompaniesWithRecruitmentCount()
        {
            var companyTop = _companyRepository.GetTopCompaniesWithRecruitmentCount();
           

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // Bỏ qua vòng lặp tự tham chiếu
            };

            string json = JsonConvert.SerializeObject(companyTop, Formatting.Indented, settings);
            return Content(json, "application/json");
        }
        public IActionResult DefaultJob(string? id)
        {
            var candidateId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var recruitment = _recruitmentRepository.GetById(id);
            var recruitmentsByCompanyId = _recruitmentRepository.GetRecruitmentsByCompanyId(id, recruitment.CompanyId, _userManager.GetUserId(User), 3);
            var recruitmentsByIndustryId = _recruitmentRepository.GetRecruitmentsByIndustryId(id, recruitment.IndustryId);
            var checkApply = _candidateRecruitmentRepository.checkApplication(candidateId, id);
            var recruitmentViewModel = new RecruitmentViewModel
            {
                Recruitment = recruitment,
                RecruitmentsCompanyId = recruitmentsByCompanyId,
                RecruitmentsIndustryId = recruitmentsByIndustryId,
                CheckApply = checkApply
            };
            bool isSave = _saveJobRepository.ExistsById(_userManager.GetUserId(User), id);

            ViewBag.IsSave = isSave; // Pass the result to the view
            return View(recruitmentViewModel);
           
        }
        public IActionResult CandidateHistory()
        {
            var candidateRecruitments = _candidateRecruitmentRepository.GetCandidateRecruitments(_userManager.GetUserId(User));
            return View(candidateRecruitments);
        }

        public IActionResult Saved()
        {
            var saved = _context.SaveRecruitments
                .Include(r => r.Recruitment)
                .ThenInclude(r => r.Company)
                .Where(m => m.CandidateId == _userManager.GetUserId(User)).ToList(); // 1 list
            //.FirstOrDefault(m => m.CandidateId == _userManager.GetUserId(User)); 1 đối tượng
            return View(saved);

        }

        [HttpPost]
        public IActionResult Save(string recruitmentId)
        {
            try
            {
                SaveRecruitment saveRecruitment = new SaveRecruitment();
                saveRecruitment.RecruitmentId = recruitmentId;
                saveRecruitment.CreateDate = DateTime.Now;
                saveRecruitment.CandidateId = _userManager.GetUserId(User);
                _saveJobRepository.Save(saveRecruitment);
                return Ok(saveRecruitment);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult UnSave(string Id)
        {
            try
            {
                var candidateId = _userManager.GetUserId(User);
                var saveRecruitment = _saveJobRepository.GetById(candidateId, Id);
                if (saveRecruitment == null)
                {
                    return NotFound();
                }

                _saveJobRepository.UnSave(saveRecruitment);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult ApplyJob(string candidateId, string recruitmentId)
        {
            var data = new CandidateRecruitment
            {
                RecruitmentId = recruitmentId,
                CandidateId = candidateId,
                ApplicationStatus = "Đang chờ xét duyệt",
                CreatedAt = DateTime.Now,
            };
            _candidateRecruitmentRepository.AddCandidateRecruitment(data);
            return Json("Success");
        }
    }
}
