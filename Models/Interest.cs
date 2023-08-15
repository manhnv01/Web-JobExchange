using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobExchange.Models
{
    public partial class Interest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InterestId { get; set; }
        public string? InterestName { get; set; }
        public string CandidateId { get; set; } = null!;

        [ForeignKey("CandidateId")]
        [ValidateNever]
        public Candidate Candidate { get; set; } = null!;
    }
}
