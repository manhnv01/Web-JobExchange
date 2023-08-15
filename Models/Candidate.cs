using JobExchange.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobExchange.Models
{
    public partial class Candidate
    {
        [Key]
        public string CandidateId { get; set; } = null!;
        public string? FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Introduce { get; set; }
        public string? Avatar { get; set; }
        public string AccountId { get; set; } = null!;

        [ForeignKey("AccountId")]
        [ValidateNever]
        public JobExchangeUser JobExchangeUser { get; set; } = null!;
        public override string ToString()
        {
            return $"FullName: {FullName}, Birthday: {Birthday}, Phone: {Phone}, Gender: {Gender}, AccountId: {AccountId}, CandidateId: {CandidateId}"; ;
        }
    }
}
