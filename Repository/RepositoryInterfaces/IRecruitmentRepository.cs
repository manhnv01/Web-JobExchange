using JobExchange.DataModel;
using JobExchange.Models;

namespace JobExchange.Repository.RepositoryInterfaces
{
    public interface IRecruitmentRepository
    {
        public Recruitment? GetById(string id);
        public Recruitment Create(Recruitment recruitment);
        public void Delete(Recruitment recruitment);
        public void Update(Recruitment recruitment);
        public List<Recruitment> GetAll();
        public List<Recruitment> GetAllByCompanyId(string companyId);
        public List<Recruitment> Search(string search);
        IEnumerable<Recruitment> GetRecruitmentsByCompanyId(string recruitmentId, string companyId);
        IEnumerable<object> GetRecruitmentsByCompanyId(string recruitmentId, string companyId, string candidateId);
        IEnumerable<object> GetRecruitmentsByCompanyId(string recruitmentId, string companyId, string candidateId, int limit);
        public List<Recruitment> SearchByCompanyAdmin(string companyId, string search);
        IEnumerable<Recruitment> GetRecruitmentsByIndustryId(string id, int industryId);
        IEnumerable<Recruitment> GetRecruitmentsByName(string companyId,string name);
        IEnumerable<object> GetRecruitments(string candidateId, int limit, string filter = null, string value1 = null, string value2 = null);
        IEnumerable<object> GetRecruitments(int limit, string filter = null, string value1 = null, string value2 = null);
    }
}

