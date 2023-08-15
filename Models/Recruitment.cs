using JobExchange.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobExchange.Models
{
    public partial class Recruitment
    {
        [Key]
        public string? RecruitmentId { get; set; }

        [Required(ErrorMessage = "Không được để trống!")]
        public string RecruitmentTitle { get; set; }

        [Required(ErrorMessage = "Không được để trống!")]
        [Range(1, 2000000000, ErrorMessage = "Lương phải lớn hơn 0")]
        public int? Salary { get; set; }

        [Required(ErrorMessage = "Không được để trống!")]
        public string? WorkType { get; set; }
        public string? GenderRequirement { get; set; }

        [Required(ErrorMessage = "Không được để trống!")]
        [Range(1, 2000000000, ErrorMessage = "Số lượng tuyển phải lớn hơn 0")]
        public int? HiringCount { get; set; }

        [Required(ErrorMessage = "Không được để trống!")]
        public string? PositionLevel { get; set; }
        public int? Experience { get; set; }

        [Required(ErrorMessage = "Không được để trống!")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Không được để trống!")]
        public string? District { get; set; }

        [Required(ErrorMessage = "Không được để trống!")]
        public string? Ward { get; set; }

        [Required(ErrorMessage = "Không được để trống!")]
        public string? AddressDetail { get; set; }

        [Required(ErrorMessage = "Không được để trống!")]
        public string? JobDescription { get; set; }

        [Required(ErrorMessage = "Không được để trống!")]
        public DateTime? ExpirationDate { get; set; }
        public string? CandidateRequirement { get; set; }
        public string? Benefit { get; set; }
        public string? Status { get; set; }
        public string? Slug { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [Required(ErrorMessage = "Không được để trống!")]
        public int IndustryId { get; set; }

        [ForeignKey("IndustryId")]
        public Industry? Industry { get; set; }
        public string? CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public Company? Company { get; set; }
    }
}
