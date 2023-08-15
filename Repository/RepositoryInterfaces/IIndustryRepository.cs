using JobExchange.Models;

namespace JobExchange.Repository
{
    public interface IIndustryRepository
    {
        public Industry? GetById(int id);
        public bool ExistsByName(string name);
        public Industry Create (Industry industry);
        public void Delete(Industry industry);
        public void Update(Industry industry);
        public List<Industry> GetAll();
        public List<Industry> SearchByName(string name);
    }
}
