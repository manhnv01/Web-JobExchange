using JobExchange.Models;
using JobExchange.Repository.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace JobExchange.Repository
{
    public class CandidateRecruitmentRepository : ICandidateRecruitmentRepository
    {
        private readonly JobExchangeContext _jobExchangeContext;
        public CandidateRecruitmentRepository(JobExchangeContext jobExchangeContext)
        {
            _jobExchangeContext = jobExchangeContext;
        }
        public CandidateRecruitment AddCandidateRecruitment(CandidateRecruitment candidateRecruitment)
        {
            _jobExchangeContext.CandidateRecruitments.Add(candidateRecruitment);
            _jobExchangeContext.SaveChanges();
            return candidateRecruitment;
        }

        public bool checkApplication(string candidateId, string recruitmentId)
        {
            bool isApplied = _jobExchangeContext.CandidateRecruitments
                .Any(cr => cr.CandidateId == candidateId && cr.RecruitmentId == recruitmentId);

            return isApplied;
        }

        public IEnumerable<CandidateRecruitment> GetCandidateRecruitments(string candidateId)
        {
            return _jobExchangeContext.CandidateRecruitments .Include(cr => cr.Recruitment)
                .ThenInclude(cr => cr.Company).Where(cr => cr.CandidateId == candidateId).ToList();
        }
    }
}