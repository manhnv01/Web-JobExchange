using JobExchange.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobExchange.Models
{
    public partial class Skill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SkillId { get; set; }
        public string? SkillName { get; set; }
        public string? Description { get; set; }
        public string CandidateId { get; set; } = null!;

        [ForeignKey("CandidateId")]
        public Candidate Candidate { get; set; } = null!;
    }
}
