using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobExchange.Models
{
    public partial class CandidateRecruitment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CandidateRecruitmentId { get; set; }
        public string? RecruitmentId { get; set; }
        public string? CandidateId { get; set; }
        public string? UrlCv { get; set; }
        public string? ApplicationStatus { get; set; }
        public DateTime? CreatedAt { get; set; }

        [ForeignKey("CandidateId")]
        [ValidateNever]
        public Candidate? Candidate { get; set; }

        [ForeignKey("RecruitmentId")]
        [ValidateNever]
        public Recruitment? Recruitment { get; set; }
    }
}
