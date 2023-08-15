using System;
using System.Collections.Generic;

namespace JobExchange.Models
{
    public class RecruitmentViewModel
    {
        public Recruitment Recruitment { get; set; }
        public IEnumerable<object> RecruitmentsCompanyId { get; set; }
        public IEnumerable<Recruitment> RecruitmentsIndustryId { get; set; }
        public bool CheckApply { get; set; }
    }
}