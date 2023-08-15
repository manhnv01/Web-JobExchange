using JobExchange.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobExchange.Models
{
    public partial class Manager
    {
        [Key]
        public string ManagerId { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string AccountId { get; set; } = null!;

        [ForeignKey("AccountId")]
        [ValidateNever]
        public JobExchangeUser JobExchangeUser { get; set; } = null!;
    }
}
