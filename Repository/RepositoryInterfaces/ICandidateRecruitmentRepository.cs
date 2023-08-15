using JobExchange.Models;

namespace JobExchange.Repository
{
    public interface ICandidateRecruitmentRepository
    {
        public CandidateRecruitment AddCandidateRecruitment(CandidateRecruitment candidateRecruitment);
        public bool checkApplication(string candidateId, string recruitmentId);
        public IEnumerable<CandidateRecruitment> GetCandidateRecruitments(string candidateId);
    }
}
