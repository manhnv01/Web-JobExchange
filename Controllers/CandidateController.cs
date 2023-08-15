using Microsoft.AspNetCore.Mvc;
using JobExchange.Models;
using MimeKit;
using Microsoft.AspNetCore.Identity;
using JobExchange.Areas.Identity.Data;
using JobExchange.Repository.RepositoryInterfaces;

namespace JobExchange.Controllers
{
    public class CandidateController : Controller
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly UserManager<JobExchangeUser> _userManager;
        public CandidateController(ICandidateRepository candidateRepository, UserManager<JobExchangeUser> userManager)
        {
            _candidateRepository = candidateRepository;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            Candidate? candidate = _candidateRepository.GetCandidate(userId);

            if (candidate == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(candidate);
        }

        [HttpPost]
        public IActionResult UpdateInfoPersonal([FromBody] Candidate candidate)
        {
            _candidateRepository.UpdateInfoPersonal(candidate);
            return Json(candidate);

        }
        [HttpPost]
        public string UploadAvatar(IFormFile avatar_file)
        {

            if (avatar_file != null && avatar_file.Length > 0)
            {
                var userId = _userManager.GetUserId(User);

                string fileName = userId + Path.GetExtension(Path.GetFileName(avatar_file.FileName));
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "avatar");
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    avatar_file.CopyTo(stream);
                }
                if (_candidateRepository.UpdateAvatar(userId, fileName))
                {
                    return fileName;
                }
                return "error";

            }

            return "empty";
        }


        //edudation
        [HttpPost]
        public IActionResult GetAllEducation([FromQuery] string candidateId)
        {
            List<Education> educations = _candidateRepository.GetAllEducation(candidateId);
            return Json(educations);
        }

        [HttpPost]
        public IActionResult AddEdudation([FromBody] Education education)
        {
            var Education = _candidateRepository.AddEdudation(education);
            return Json(Education);
        }
        [HttpPost]
        public IActionResult UpdateEdudation([FromBody] Education education)
        {
            var Education = _candidateRepository.UpdateEdudation(education);
            return Json(Education);
        }
        [HttpPost]
        public IActionResult DeleteEducation(int Id)
        {
            var Education = _candidateRepository.DeleteEdudation(Id);
            return Json(Education);
        }
    }
}
//var messageInfo = new Dictionary<string, string>
//{
//    { "text", "Cập nhật thông tin thành công" },
//    { "type", "success" },
//    { "title", "Thông báo" },
//    { "icon", "glyphicon-ok" },
//    { "delay", "3000" }
//};

//TempData["messageInfo"] = messageInfo;