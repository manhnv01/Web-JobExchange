using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace JobExchange.Areas.Identity.Data;

public class JobExchangeUser : IdentityUser
{
    [Required]

    public string? AccountName { get; set; }
}

