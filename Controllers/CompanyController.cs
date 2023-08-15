using JobExchange.Areas.Identity.Data;
using JobExchange.Models;
using JobExchange.Repository;
using JobExchange.Repository.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace JobExchange.Controllers
{
    public class CompanyController : Controller
    {
        private readonly JobExchangeContext _context;
        private readonly ICompanyRepository _companyRepository;
        private readonly IRecruitmentRepository _recruitmentRepository;
        private readonly UserManager<JobExchangeUser> _userManager;
        public CompanyController(JobExchangeContext context, ICompanyRepository companyRepository, IRecruitmentRepository recruitmentRepository, UserManager<JobExchangeUser> userManager)
        {
            _companyRepository = companyRepository;
            _recruitmentRepository = recruitmentRepository;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var companies = _companyRepository.GetAllCompanies();
            return View(companies);
        }
        
        public IActionResult Top()
        {
            var companies = _companyRepository.GetTopCompanies();
            return View(companies);
        }

        public IActionResult Search(string? name)
        {
            var companies = _companyRepository.GetAllCompanies();
            var companiesSearch = _companyRepository.SearchCompanies(name);

            if (companiesSearch.Count() == 0)
            {
                ViewData["Count"] = "0";
                return View(companies);
            }
            ViewData["Count"] = companiesSearch.Count().ToString();
            return View(companiesSearch);
        }

        public IActionResult Detail(string? id)
        {
            var company = _companyRepository.GetCompanyById(id);
            var recruitments = _recruitmentRepository.GetRecruitmentsByCompanyId("", id);
            var relatedCompanies = _companyRepository.GetCompaniesRelated(company.IndustryId, id);
            var viewModel = new CompanyViewModel{
                Company = company,
                Recruitments = recruitments,
                RelatedCompanies = relatedCompanies,
            };
            return View(viewModel);
        }

        public IActionResult Recruitments(string companyId, string name)
        {
            var recruitments = _recruitmentRepository.GetRecruitmentsByName(companyId, name);
            return PartialView("_Recruitments", recruitments);
        }

        public IActionResult SearchCompanyAjax(string companyName)
        {
            var companies = _companyRepository.SearchAjaxCompanies(companyName);
            return PartialView("_SearchCompanyAjax", companies);
        }

        public IActionResult Profile()
        {
            var companyId = _userManager.GetUserId(User);
            var company = _context.Companies
                .Include(r => r.Industry)
                .FirstOrDefault(m => m.CompanyId == companyId);
            return View(company);
        }

        // GET:
        public IActionResult Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = _companyRepository.GetCompanyById(id);
            if (company == null)
            {
                return NotFound();
            }
            ViewData["IndustryId"] = new SelectList(_context.Industries, "IndustryId", "IndustryName");
            return View(company);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Company company)
        {
            try
            {
                ViewData["IndustryId"] = new SelectList(_context.Industries, "IndustryId", "IndustryName");
                _companyRepository.Update(company);
            }
            catch (DbUpdateConcurrencyException)
            {
                //return View(company);
                return NotFound();
            }
            return RedirectToAction(nameof(Profile));
        }

        [HttpPost]
        public string UploadAvatar(IFormFile avatar_file)
        {
            if (avatar_file != null && avatar_file.Length > 0)
            {
                var userId = _userManager.GetUserId(User);

                string fileName = userId + Path.GetExtension(Path.GetFileName(avatar_file.FileName));
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "companies");
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    avatar_file.CopyTo(stream);
                }
                if (_companyRepository.UpdateAvatar(userId, fileName))
                {
                    return fileName;
                }
                return "error";

            }

            return "empty";
        }

        [HttpPost]
        public string UploadCover(IFormFile cover_file)
        {
            if (cover_file != null && cover_file.Length > 0)
            {
                var userId = _userManager.GetUserId(User);

                string coverName = DateTime.Now.Ticks.ToString();

                string fileName = "cover-" + coverName + Path.GetExtension(Path.GetFileName(cover_file.FileName));
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "companies");
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    cover_file.CopyTo(stream);
                }
                if (_companyRepository.UpdateCover(userId, fileName))
                {
                    return fileName;
                }
                return "error";

            }

            return "empty";
        }
    }
}