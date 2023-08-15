using JobExchange.Models;
using JobExchange.Repository.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace JobExchange.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly JobExchangeContext _jobExchangeContext;
        public CompanyRepository(JobExchangeContext jobExchangeContext)
        {
            _jobExchangeContext = jobExchangeContext;
        }

        public void AddCompany(Company company)
        {
           _jobExchangeContext.Companies.Add(company);
           _jobExchangeContext.SaveChanges();
        }

        public void DeleteCompany(string companyId)
        {
            var company = _jobExchangeContext.Companies.FirstOrDefault(c => c.CompanyId == companyId);

            if (company != null)
            {
                _jobExchangeContext.Companies.Remove(company);
                _jobExchangeContext.SaveChanges();
            }
        }

        public IEnumerable<Company> GetAllCompanies()
        {
            return _jobExchangeContext.Companies.ToList();
        }

        public IEnumerable<Company> GetCompaniesRelated(int industryId, string companyId)
        {
            return _jobExchangeContext.Companies.Include(r => r.Recruitments).Where(c => c.IndustryId == industryId && c.CompanyId != companyId).ToList();
        }

        public Company GetCompanyById(string companyId)
        {
            return _jobExchangeContext.Companies.FirstOrDefault(c => c.CompanyId == companyId);
        }

        public IEnumerable<Company> GetTopCompanies()
        {
            return _jobExchangeContext.Companies.OrderByDescending(c => c.Recruitments.Count()).Take(12).ToList();
        }

        public IEnumerable<CompanyWithRecruitmentCount> GetTopCompaniesWithRecruitmentCount()
        {
            return _jobExchangeContext.Companies
            .Select(c => new CompanyWithRecruitmentCount
            {
                Company = c,
                RecruitmentCount = c.Recruitments.Count()
            })
            .OrderByDescending(c => c.RecruitmentCount)
            .Take(12)
            .ToList();
        }

        public IEnumerable<Company> SearchAjaxCompanies(string name)
        {
            return _jobExchangeContext.Companies.Where(c => c.CompanyName.Contains(name)).ToList();
        }

        public IEnumerable<Company> SearchCompanies(string name)
        {
            return _jobExchangeContext.Companies.Include(c => c.Recruitments).Where(c => c.CompanyName.Contains(name)).ToList();
        }

        public void Update(Company company)
        {
            _jobExchangeContext.Companies.Update(company);
            _jobExchangeContext.SaveChanges();
        }

        public bool UpdateAvatar(string companyId, string filename)
        {
            var existingCompany = _jobExchangeContext.Companies.Find(companyId);

            if (existingCompany != null && filename != null)
            {
                existingCompany.Avatar = filename;

                _jobExchangeContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateCover(string companyId, string filename)
        {
            var existingCompany = _jobExchangeContext.Companies.Find(companyId);

            if (existingCompany != null && filename != null)
            {
                existingCompany.CoverImage = filename;

                _jobExchangeContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}