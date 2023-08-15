using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobExchange.Models
{
    public partial class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public int? NumberMember { get; set; }
        public string? Position { get; set; }
        public string? Mission { get; set; }
        public string? Technology { get; set; }
        public int? StartMonth { get; set; }
        public int? EndYear { get; set; }
        public int? EndMonth { get; set; }
        public int? StartYear { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public string CandidateId { get; set; } = null!;

        [ForeignKey("CandidateId")]
        public Candidate Candidate { get; set; } = null!;
    }
}
