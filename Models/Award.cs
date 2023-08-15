using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobExchange.Models
{
    public partial class Award
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AwardId { get; set; } = null!;
        public string? AwardName { get; set; }
        public string? Organization { get; set; }
        public int? ReceivedMonth { get; set; }
        public int? ReceivedYear { get; set; }
        public string? Image { get; set; }
        public string CandidateId { get; set; } = null!;

        [ForeignKey("CandidateId")]
        [ValidateNever]
        public Candidate Candidate { get; set; } = null!;
    }
}
