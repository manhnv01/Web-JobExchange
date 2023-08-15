using JobExchange.Models;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;

namespace JobExchange.Repository.RepositoryInterfaces
{
    public class IndustryRepository : IIndustryRepository
	{
        private readonly JobExchangeContext _jobExchangeContext;
        public IndustryRepository(JobExchangeContext jobExchangeContext)
        {
            _jobExchangeContext = jobExchangeContext;
        }
        public Industry Create (Industry industry)
        {
            _jobExchangeContext.Industries.Add(industry);
            _jobExchangeContext.SaveChanges();

            return industry;
        }

        public void Delete(Industry industry)
        {
            _jobExchangeContext.Industries.Remove(industry);
            _jobExchangeContext.SaveChanges();
        }

        public bool ExistsByName(string name)
        {
            return _jobExchangeContext.Industries.Any(i => i.IndustryName == name);
        }

        public List<Industry> GetAll()
        {
            return _jobExchangeContext.Industries.ToList();
        }

        public Industry? GetById(int id)
        {
            return _jobExchangeContext.Industries.FirstOrDefault(u => u.IndustryId == id);
        }

        public List<Industry> SearchByName(string name)
        {
            var results = _jobExchangeContext.Set<Industry>().Where(x => EF.Functions.Like(x.IndustryName, $"%{name}%")).ToList();
            return results;
        }

        public void Update(Industry industry)
        {
            _jobExchangeContext.Industries.Update(industry);
            _jobExchangeContext.SaveChanges();
        }
    }
}
