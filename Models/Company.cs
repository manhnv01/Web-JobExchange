using JobExchange.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace JobExchange.Models
{
    public partial class Company
    {
        [Key]
        public string? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? Introduce { get; set; }
        public string? Scale { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
        public string? CoverImage { get; set; }
        public int IndustryId { get; set; }

        [JsonIgnore]
        public virtual ICollection<Recruitment>? Recruitments { get; set; }

        [ForeignKey("IndustryId")]
        [ValidateNever]
        public Industry? Industry { get; set; }
        public string? AccountId { get; set; }

        [ForeignKey("AccountId")]
        [ValidateNever]
        public JobExchangeUser? JobExchangeUser { get; set; }
    }
}
