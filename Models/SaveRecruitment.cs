using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobExchange.Models
{
    public partial class SaveRecruitment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaveRecruitmentId { get; set; }
        public string? RecruitmentId { get; set; }
        public string? CandidateId { get; set; }
        public DateTime? CreateDate { get; set; }

        [ForeignKey("CandidateId")]
        [ValidateNever]
        [JsonIgnore]
        public Candidate? Candidate { get; set; }

        [ForeignKey("RecruitmentId")]
        [ValidateNever]
        [JsonIgnore]
        public Recruitment? Recruitment { get; set; }
    }
}
