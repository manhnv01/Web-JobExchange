using JobExchange.Areas.Identity.Data;
using JobExchange.Models;
using JobExchange.Repository;
using JobExchange.Repository.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PagedList;
using System.Diagnostics;
using System.Drawing;
using System.Security.Claims;

namespace JobExchange.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIndustryRepository _industryRepository;
        private readonly SignInManager<JobExchangeUser> _signInManager;
        private readonly UserManager <JobExchangeUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, UserManager<JobExchangeUser> userManager, SignInManager<JobExchangeUser> signInManager, IIndustryRepository industryRepository)
        {
            _logger = logger;
            _industryRepository = industryRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index(int page)
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

            int pageNumber = page > 0 ? page : 1;
            // Get the list of products from the database.
            var industries = _industryRepository.GetAll();
            // Create a PagedList from the list of products.
            var pagedList = new PagedList<Industry>(industries, pageNumber, 8);

            // Return the paged list of products to the view.
            return View(pagedList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
    }
}