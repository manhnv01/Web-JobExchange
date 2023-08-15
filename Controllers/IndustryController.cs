using Abp.MimeTypes;
using JobExchange.Models;
using JobExchange.Repository;
using JobExchange.Repository.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList;
using Stripe;
using System;
using System.Drawing;
using System.Security.Claims;

namespace JobExchange.Controllers
{
    [Authorize]
    [Authorize(Roles = "ROLE_ADMIN")]
    public class IndustryController : Controller
    {
        private readonly IIndustryRepository _industryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public IndustryController(IIndustryRepository industryRepository, IWebHostEnvironment webHostEnvironment)
        {
            _industryRepository = industryRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        // GETALL
        public ActionResult Index(int page, int size)
        {
            int pageSize = size > 0 ? size : 5;
            int pageNumber = page > 0 ? page : 1;

            var industries = _industryRepository.GetAll();
            var pagedList = new PagedList<Industry>(industries, pageNumber, pageSize);

            return View(pagedList);
        }

        [HttpPost]
        public IActionResult Create(IFormFile IndustryImage, string IndustryName)
        {
            try
            {
                Industry industry = new Industry()
                {
                    IndustryName = IndustryName
                };
                if (_industryRepository.ExistsByName(industry.IndustryName))
                {
                    return Ok(new
                    {
                        message = "Tên ngành nghề đã tồn tại !"
                    });
                }
                if (IndustryImage != null && IndustryImage.Length > 0)
                {
                    string fileName = IndustryImage.FileName;
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "industry");
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        IndustryImage.CopyTo(stream);
                    }
                    industry.IndustryImage = fileName;
                    _industryRepository.Create(industry);
                    return Ok(industry);

                }
                else
                {
                    return Ok(new
                    {
                        message = "Không có ảnh !"
                    });
                }


            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public ActionResult Update(int IndustryId, IFormFile IndustryImage, string IndustryName)
        {
            try
            {
                Industry industry = new Industry()
                {
                    IndustryId = IndustryId,
                    IndustryName = IndustryName
                };
                var industryExists = _industryRepository.GetById(IndustryId);

                if (industryExists == null)
                {
                    return NotFound();
                }
                if (industry.IndustryName != industryExists.IndustryName && _industryRepository.ExistsByName(industry.IndustryName))
                {
                    return Ok(new
                    {
                        message = "Tên ngành nghề đã tồn tại !"
                    });
                }
                if (IndustryImage != null && IndustryImage.Length > 0)
                {
                    string fileName = IndustryImage.FileName;
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "industry");
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        IndustryImage.CopyTo(stream);
                    }
                    industryExists.IndustryName = industry.IndustryName;
                    industryExists.IndustryImage = fileName;
                    _industryRepository.Update(industryExists);
                    return Ok(Json(industryExists));

                }
                else
                {
                    return Ok(new
                    {
                        message = "Không có ảnh !"
                    });
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var industry = _industryRepository.GetById(id);
                if (industry == null)
                {
                    return NotFound();
                }

                _industryRepository.Delete(industry);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_industryRepository.GetById(id));
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Search(string name)
        {
            var results = _industryRepository.SearchByName(name);
            try
            {
                return Ok(results);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
