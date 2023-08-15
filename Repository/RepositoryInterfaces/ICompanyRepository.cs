using JobExchange.Models;

namespace JobExchange.Repository.RepositoryInterfaces
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAllCompanies();
        IEnumerable<Company> GetTopCompanies();
        IEnumerable<CompanyWithRecruitmentCount> GetTopCompaniesWithRecruitmentCount();
        IEnumerable<Company> GetCompaniesRelated(int industryId, string companyId);
        IEnumerable<Company> SearchCompanies(string name);
        IEnumerable<Company> SearchAjaxCompanies(string name);
        Company GetCompanyById(string companyId);
        void AddCompany(Company company);
        void Update(Company company);
        void DeleteCompany(string companyId);
        public bool UpdateAvatar(string companyId, string filename);
        public bool UpdateCover(string companyId, string filename);
    }
}