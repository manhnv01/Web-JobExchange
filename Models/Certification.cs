using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobExchange.Models
{
    public partial class Certification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CertificationId { get; set; }
        public string? CertificationName { get; set; }
        public string? Organization { get; set; }
        public int? ReceivedMonth { get; set; }
        public int? ReceivedYear { get; set; }
        public int? ExpirationMonth { get; set; }
        public int? ExpirationYear { get; set; }
        public string? Image { get; set; }
        public string CandidateId { get; set; } = null!;

        [ForeignKey("CandidateId")]
        [ValidateNever]
        public Candidate Candidate { get; set; } = null!;
    }
}
