using JobExchange.Models;

namespace JobExchange.Repository
{
    public interface ISaveJobRepository
    {
        public SaveRecruitment? GetById(string candidateId, string recruitmentId);
        public SaveRecruitment Save (SaveRecruitment saveJob);
        public bool ExistsById(string candidateId, string recruitmentId);
        public void UnSave(SaveRecruitment saveJob);
        public List<SaveRecruitment> SearchByName(string name);
    }
}
