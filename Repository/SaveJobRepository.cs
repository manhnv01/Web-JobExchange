using JobExchange.Models;
using Microsoft.EntityFrameworkCore;

namespace JobExchange.Repository
{
    public class SaveJobRepository : ISaveJobRepository
    {
        private readonly JobExchangeContext _jobExchangeContext;
        public SaveJobRepository(JobExchangeContext jobExchangeContext)
        {
            _jobExchangeContext = jobExchangeContext;
        }
        public SaveRecruitment Save(SaveRecruitment saveJob)
        {
            _jobExchangeContext.SaveRecruitments.Add(saveJob);
            _jobExchangeContext.SaveChanges();

            return saveJob;
        }

        public void UnSave(SaveRecruitment saveJob)
        {
            _jobExchangeContext.SaveRecruitments.Remove(saveJob);
            _jobExchangeContext.SaveChanges();
        }

        //public IEnumerable<SaveRecruitment> GetAllByCandidateId(string id)
        //{
        //    var saves = _jobExchangeContext.SaveRecruitments
        //    .Where(s => s.CandidateId == id)
        //    .Select(s => new
        //    {
        //        SaveRecruitment = s,
        //        Recruitment = s.Recruitment,
        //        Company = s.Recruitment.Company
        //    })
        //    .ToList();

        //    return saves.Select(s => s.SaveRecruitment);

        //}

        public SaveRecruitment? GetById(string candidateId, string recruitmentId)
        {
            return _jobExchangeContext.SaveRecruitments.FirstOrDefault(u => u.CandidateId == candidateId && u.RecruitmentId == recruitmentId);
        }

        public List<SaveRecruitment> SearchByName(string name)
        {
            throw new NotImplementedException();
        }

        public bool ExistsById(string candidateId, string recruitmentId)
        {
            return _jobExchangeContext.SaveRecruitments.Any(i => i.CandidateId == candidateId && i.RecruitmentId == recruitmentId);
        }
    }
}
