using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobExchange.Models
{
    public partial class Education
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EducationId { get; set; }
        [Required]
        public string? SchoolName { get; set; }
        [Required]
        public string? Major { get; set; }
        [Required]
        public int? StartMonth { get; set; }
        [Required]
        public int? StartYear { get; set; }
        public int? EndMonth { get; set; }
        public int? EndYear { get; set; }
        public string? Description { get; set; }
        public string CandidateId { get; set; } = null!;

        [ForeignKey("CandidateId")]
        [ValidateNever]
        public Candidate Candidate { get; set; } = null!;

        public override string ToString()
        {
            return $"EducationId: {EducationId}, " +
                   $"SchoolName: {SchoolName}, " +
                   $"Major: {Major}, " +
                   $"StartMonth: {StartMonth}, " +
                   $"StartYear: {StartYear}, " +
                   $"EndMonth: {EndMonth}, " +
                   $"EndYear: {EndYear}, " +
                   $"Description: {Description}, " +
                   $"CandidateId: {CandidateId}";
        }
    }
}
