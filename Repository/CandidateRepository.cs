using JobExchange.Models;
using JobExchange.Repository.RepositoryInterfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;
using System.Linq;

namespace JobExchange.Repository
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly JobExchangeContext _jobExchangeContext;
        public CandidateRepository(JobExchangeContext jobExchangeContext)
        {
            _jobExchangeContext = jobExchangeContext;
        }
        public Candidate Create(Candidate candidate)
        {
            _jobExchangeContext.Candidates.Add(candidate);
            _jobExchangeContext.SaveChangesAsync();

            return candidate;
        }
        public Candidate? GetCandidate(string candidateId)
        {
            try
            {
                Candidate? candidate = _jobExchangeContext.Candidates.SingleOrDefault(c => c.CandidateId == candidateId);
                return candidate;
            } 
            catch(SqlNullValueException e)
            {
                return null;
            }
            
        }
        public Candidate UpdateInfoPersonal(Candidate candidate)
        {
            _jobExchangeContext.Candidates.Update(candidate);
            _jobExchangeContext.SaveChanges();

            return candidate;
        }

        public bool UpdateAvatar(string candidateId, string filename)
        {
            var existingCandidate = _jobExchangeContext.Candidates.Find(candidateId);

            if (existingCandidate != null && filename != null)
            {
                existingCandidate.Avatar = filename;

                _jobExchangeContext.SaveChanges();
                return true;
            }
            return false;
        }

        //education
        public List<Education> GetAllEducation(string candidateId)
        {
            var filteredJobs = _jobExchangeContext.Educations.Where(e => e.CandidateId == candidateId);
            return filteredJobs.ToList();
        }
        public Education AddEdudation(Education education)
        {
            _jobExchangeContext.Educations.Add(education);
            _jobExchangeContext.SaveChanges();

            return education;
        }
        public Education UpdateEdudation(Education education)
        {
            _jobExchangeContext.Educations.Update(education);
            _jobExchangeContext.SaveChanges();

            return education;
        }
        public Education DeleteEdudation(int educationId)
        {
      
            Education education = _jobExchangeContext.Educations.SingleOrDefault(e => e.EducationId.Equals(educationId));
            _jobExchangeContext.Educations.Remove(education);
            _jobExchangeContext.SaveChanges();
            return education;

            
        }
    }
}
