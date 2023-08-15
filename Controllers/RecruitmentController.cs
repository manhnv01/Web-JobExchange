using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobExchange.Models;
using PagedList;
using JobExchange.Repository.RepositoryInterfaces;
using JobExchange.Utils;
using Microsoft.AspNetCore.Identity;
using JobExchange.Areas.Identity.Data;

using System.Text.Json;
using JobExchange.DataModel;
using Microsoft.AspNetCore.Authorization;

namespace JobExchange.Controllers
{
    [Authorize]
    [Authorize(Roles = "ROLE_COMPANY")]
    public class RecruitmentController : Controller
    {
        private readonly JobExchangeContext _context;
        private readonly IRecruitmentRepository _recruitmentRepository;
        private readonly VNCharacterUtils _vnCharacterUtils;
        private readonly UserManager<JobExchangeUser> _userManager;

        public RecruitmentController(JobExchangeContext context, IRecruitmentRepository recruitmentRepository, UserManager<JobExchangeUser> userManager)
        {
            _context = context;
            _recruitmentRepository = recruitmentRepository;
            _vnCharacterUtils = new VNCharacterUtils();
            _userManager = userManager;
        }


        // GET: Recruitment
        public ActionResult Index(int page, int size)
        {
            int pageSize = size > 0 ? size : 10;
            int pageNumber = page > 0 ? page : 1;

            var recruitments = _recruitmentRepository.GetAllByCompanyId(_userManager.GetUserId(User));
            var pagedList = new PagedList<Recruitment>(recruitments, pageNumber, pageSize);
            return View(pagedList);
        }

        // GET: Recruitment/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Recruitments == null)
            {
                return NotFound();
            }

            var recruitment = await _context.Recruitments
                .Include(r => r.Company)
                .Include(r => r.Industry)
                .FirstOrDefaultAsync(m => m.RecruitmentId == id);
            if (recruitment == null)
            {
                return NotFound();
            }

            return View(recruitment);
        }

        // GET: Recruitment/Create
        public IActionResult Create()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data.json");

            if (System.IO.File.Exists(filePath))
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<List<ProvinceDataModel>>(jsonData);

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

        // POST: Recruitment/Create
        [HttpPost]
        public IActionResult Create(Recruitment recruitment)
        {
            recruitment.RecruitmentId = DateTime.Now.Ticks.ToString();
            recruitment.CreatedAt = DateTime.Now;
            recruitment.UpdatedAt = DateTime.Now;
            recruitment.CompanyId = _userManager.GetUserId(User);
            recruitment.Status = Const.STATUS_ENABLE;
            recruitment.Slug = _vnCharacterUtils.ToSlug(recruitment.RecruitmentTitle) + "-" + recruitment.RecruitmentId;
            if (ModelState.IsValid)
            {
                _recruitmentRepository.Create(recruitment);
                TempData["success"] = "Đăng tin tuyển dụng thành công.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["IndustryId"] = new SelectList(_context.Industries, "IndustryId", "IndustryName", recruitment.IndustryId);
            return View(recruitment);
        }

        // GET: Recruitment/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null || _recruitmentRepository.GetById(id) == null)
            {
                return NotFound();
            }

            var recruitment = _recruitmentRepository.GetById(id);
            if (recruitment == null)
            {
                return NotFound();
            }
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data.json");

            if (System.IO.File.Exists(filePath))
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<List<ProvinceDataModel>>(jsonData);

                // Check if the deserialization was successful and data is not null
                if (data != null)
                {
                    ViewBag.City = data;

                    ViewBag.CitySelect = recruitment.City;
                    ViewBag.District = recruitment.District;
                    ViewBag.Ward = recruitment.Ward;

                    ViewData["IndustryId"] = new SelectList(_context.Industries, "IndustryId", "IndustryName", recruitment.IndustryId);
                    return View(recruitment);
                }
                else
                {
                    // Handle deserialization failure
                    // Redirect to an error page or return an error message
                    return View("Error");
                }
            }
            else
            {
                // Handle missing data.json file
                // Redirect to an error page or return an error message
                return View("Error");
            }
        }

        // POST: Recruitment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, Recruitment recruitment)
        {
            if (id != recruitment.RecruitmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    recruitment.Slug = _vnCharacterUtils.ToSlug(recruitment.RecruitmentTitle) ;
                    recruitment.UpdatedAt = DateTime.Now;
                    _recruitmentRepository.Update(recruitment);
                    TempData["success"] = "Cập nhật tin tuyển dụng thành công.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecruitmentExists(recruitment.RecruitmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IndustryId"] = new SelectList(_context.Industries, "IndustryId", "IndustryName", recruitment.IndustryId);
            return View(recruitment);
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            try
            {
                var recruitment = _recruitmentRepository.GetById(id);
                if (recruitment == null)
                {
                    return NotFound();
                }

                _recruitmentRepository.Delete(recruitment);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Search(string search)
        {
            var results = _recruitmentRepository.SearchByCompanyAdmin(_userManager.GetUserId(User), search);
            try
            {
                return Ok(results);
            }
            catch
            {
                return NotFound();
            }
        }

        private bool RecruitmentExists(string id)
        {
          return (_context.Recruitments?.Any(e => e.RecruitmentId == id)).GetValueOrDefault();
        }
    }
}
