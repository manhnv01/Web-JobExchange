using JobExchange.Models;

namespace JobExchange.Repository.RepositoryInterfaces
{
    public interface ICandidateRepository
    {
        public Candidate Create(Candidate candidate);
        public Candidate? GetCandidate(string id);
        public Candidate UpdateInfoPersonal(Candidate candidate);
        public bool UpdateAvatar(string candidateId, string filename);

        public Education AddEdudation(Education education);
        public Education UpdateEdudation(Education education);
        public Education DeleteEdudation(int educationId);
        public List<Education> GetAllEducation(string candidateId);
    }
}
