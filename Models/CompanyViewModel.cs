using System;
using System.Collections.Generic;

namespace JobExchange.Models
{
    public class CompanyViewModel
    {
        public Company Company { get; set; }
        public IEnumerable<Recruitment> Recruitments { get; set; }
        public IEnumerable<Company> RelatedCompanies { get; set; }
    }
}