
using JobExchange.DataModel;
using JobExchange.Models;
using JobExchange.Repository.RepositoryInterfaces;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.Design;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace JobExchange.Repository
{
    public class RecruitmentRepository : IRecruitmentRepository
    {
        private readonly JobExchangeContext _jobExchangeContext;
        public RecruitmentRepository(JobExchangeContext jobExchangeContext)
        {
            _jobExchangeContext = jobExchangeContext;
        }

        public Recruitment Create(Recruitment recruitment)
        {
            _jobExchangeContext.Recruitments.Add(recruitment);
            _jobExchangeContext.SaveChanges();

            return recruitment;
        }

        public void Delete(Recruitment recruitment)
        {
            _jobExchangeContext.Recruitments.Remove(recruitment);
            _jobExchangeContext.SaveChanges();
        }

        public List<Recruitment> GetAll()
        {
            return _jobExchangeContext.Recruitments.ToList();
        }

        public List<Recruitment> GetAllByCompanyId(string companyId)
        {
            return _jobExchangeContext.Recruitments.Where(r => r.CompanyId == companyId).ToList();
        }

        public Recruitment? GetById(string id)
        {
            return _jobExchangeContext.Recruitments.Include(c => c.Industry).Include(c => c.Company).FirstOrDefault(u => u.RecruitmentId == id);
        }

        public List<Recruitment> Search(string search)
        {
            var results = _jobExchangeContext.Recruitments.Where(
        recruitment => recruitment.RecruitmentTitle.Contains(search)
            || recruitment.Slug.Contains(search)
            || recruitment.Industry.IndustryName.Contains(search));
            //.OrderByDescending(recruitment => recruitment.CreatedAt).Take(10);
            return results.ToList();
        }

        public List<Recruitment> SearchByCompanyAdmin(string companyId, string search)
        {
            var results = _jobExchangeContext.Recruitments.Where(
        recruitment => recruitment.RecruitmentTitle.Contains(search) && recruitment.CompanyId == companyId
            || recruitment.Slug.Contains(search) && recruitment.CompanyId == companyId
            || recruitment.Industry.IndustryName.Contains(search) && recruitment.CompanyId == companyId);
            //.OrderByDescending(recruitment => recruitment.CreatedAt).Take(10);
            return results.ToList();
        }

        public void Update(Recruitment recruitment)
        {
            _jobExchangeContext.Recruitments.Update(recruitment);
            _jobExchangeContext.SaveChanges();
        }

        //trả về danh sách các việc làm đáp ứng tất cả các điều kiện và cũng phải có ngày tạo trong 10 ngày qua:
        //public List<Recruitment> Search(string search)
        //{
        //    var results = _jobExchangeContext.Recruitments.Where(
        //        recruitment => recruitment.Name.Contains(search)
        //            && recruitment.Description.Contains(search)
        //            && recruitment.Category.Name.Contains(search)
        //            && recruitment.CreatedAt > DateTime.Now - TimeSpan.FromDays(10)
        //    ).OrderByDescending(recruitment => recruitment.CreatedAt).Take(10);

        //    return results;
        //}
        
        public IEnumerable<Recruitment> GetRecruitmentsByCompanyId(string recruitmentId, string companyId)
        {
            return _jobExchangeContext.Recruitments.Include(c => c.Company).Where(r => r.CompanyId == companyId && r.RecruitmentId != recruitmentId).ToList();
            
        }
        public IEnumerable<object> GetRecruitmentsByCompanyId(string recruitmentId, string companyId, string candidateId)
        {
            //return _jobExchangeContext.Recruitments.Include(c => c.Company).Where(r => r.CompanyId == companyId && r.RecruitmentId != id).ToList();
            var recruitments = _jobExchangeContext.Recruitments
            .GroupJoin(
                _jobExchangeContext.SaveRecruitments,
                r => r.RecruitmentId,
                sr => sr.RecruitmentId,
                (r, srs) => new { Recruitment = r, SaveRecruitments = srs }
            )
            .SelectMany(
                rs => rs.SaveRecruitments.DefaultIfEmpty(),
                (rs, s) => new { Recruitment = rs.Recruitment, SaveRecruitment = s }
            )
            .Where(rss => rss.Recruitment.CompanyId == companyId
                     && (rss.SaveRecruitment == null || rss.SaveRecruitment.CandidateId == candidateId)
                     && rss.Recruitment.RecruitmentId != recruitmentId)
            .Select(rss => new { Recruitment = rss.Recruitment, SaveRecruitment = rss.SaveRecruitment })
            .ToList();

            return recruitments;
        }
        public IEnumerable<object> GetRecruitmentsByCompanyId(string recruitmentId, string companyId, string candidateId, int limit)
        {
            //return _jobExchangeContext.Recruitments.Include(c => c.Company).Where(r => r.CompanyId == companyId && r.RecruitmentId != id).ToList();
            var recruitments = _jobExchangeContext.Recruitments
            .GroupJoin(
                _jobExchangeContext.SaveRecruitments,
                r => r.RecruitmentId,
                sr => sr.RecruitmentId,
                (r, srs) => new { Recruitment = r, SaveRecruitments = srs }
            )
            .SelectMany(
                rs => rs.SaveRecruitments.DefaultIfEmpty(),
                (rs, s) => new { Recruitment = rs.Recruitment, SaveRecruitment = s }
            )
            .Where(rss => rss.Recruitment.CompanyId == companyId
                     && (rss.SaveRecruitment == null || rss.SaveRecruitment.CandidateId == candidateId)
                     && rss.Recruitment.RecruitmentId != recruitmentId)
            .Select(rss => new { Recruitment = rss.Recruitment, SaveRecruitment = rss.SaveRecruitment })
            .Take(limit)
            .ToList();

            return recruitments;
        }
        public IEnumerable<Recruitment> GetRecruitmentsByIndustryId(string id, int industryId)
        {
            return _jobExchangeContext.Recruitments.Include(c => c.Company).Where(r => r.IndustryId == industryId && r.RecruitmentId != id).ToList();
        }

        public IEnumerable<Recruitment> GetRecruitmentsByName(string companyId, string name)
        {
            return _jobExchangeContext.Recruitments.Include(c => c.Company).Where(r => r.CompanyId == companyId &&  r.RecruitmentTitle.Contains(name)).ToList();
        }

        public IEnumerable<object> GetRecruitments(string candidateId, int limit, string filter = null, string value1 = null, string value2 = null)
        {
            //var recruitments = _jobExchangeContext.Recruitments.Include(r => r.Industry).Include(r => r.Company);

            var recruitments = _jobExchangeContext.Recruitments.Include(r => r.Industry).Include(r => r.Company)
            .GroupJoin(
                _jobExchangeContext.SaveRecruitments,
                r => r.RecruitmentId,
                sr => sr.RecruitmentId,
                (r, srs) => new { Recruitment = r, SaveRecruitments = srs }
            )
            .SelectMany(
                rs => rs.SaveRecruitments.DefaultIfEmpty(),
                (rs, s) => new { Recruitment = rs.Recruitment, SaveRecruitment = s }
            )
            .Where(rss => rss.SaveRecruitment == null || rss.SaveRecruitment.CandidateId == candidateId)
            .Select(rss => new { Recruitment = rss.Recruitment, SaveRecruitment = rss.SaveRecruitment })
            .OrderByDescending(c => c.Recruitment.CreatedAt)
            .Take(limit);
             


            if (!string.IsNullOrEmpty(filter))
            {
                if (filter.Equals("districts"))
                {
                    return recruitments.Where(r => r.Recruitment.District.Contains(value1)).ToList();
                }
                if (filter.Equals("salaries") && !string.IsNullOrEmpty(value1) && !string.IsNullOrEmpty(value2))
                {
                    int intValue1 = int.Parse(value1) * 1000000;
                    int intValue2 = int.Parse(value2) * 1000000;
                    return recruitments.Where(r => r.Recruitment.Salary >= intValue1 && r.Recruitment.Salary <= intValue2).ToList();
                }
                if (filter.Equals("experiences") && !string.IsNullOrEmpty(value1) && !string.IsNullOrEmpty(value2))
                {
                    int intValue1 = int.Parse(value1);
                    int intValue2 = int.Parse(value2);
                    if (intValue1 == 0)
                    {
                        return recruitments.Where(r => r.Recruitment.Experience == intValue1).ToList();

                    }
                    return recruitments.Where(r => r.Recruitment.Experience > intValue1 && r.Recruitment.Experience <= intValue2).ToList();
                }
                if (filter.Equals("industryes") && !string.IsNullOrEmpty(value1))
                {
                    int intValue1 = int.Parse(value1);
                    return recruitments.Where(r => r.Recruitment.IndustryId == intValue1).ToList();
                }
                if (filter.Equals("title") && !string.IsNullOrEmpty(value1))
                {
                    return recruitments.Where(r => r.Recruitment.RecruitmentTitle.Contains(value1)).ToList();
                }
            }

            recruitments.ToList();
            return recruitments;


        }
        public IEnumerable<object> GetRecruitments(int limit, string filter = null, string value1 = null, string value2 = null)
        {
            var recruitments = _jobExchangeContext.Recruitments.Include(r => r.Industry).Include(r => r.Company)
            .GroupJoin(
                _jobExchangeContext.SaveRecruitments,
                r => r.RecruitmentId,
                sr => sr.RecruitmentId,
                (r, srs) => new { Recruitment = r, SaveRecruitments = srs }
            )
            .SelectMany(
                rs => rs.SaveRecruitments.DefaultIfEmpty(),
                (rs, s) => new { Recruitment = rs.Recruitment, SaveRecruitment = s }
            )
            .Where(rss => rss.SaveRecruitment == null)
            .Select(rss => new { Recruitment = rss.Recruitment, SaveRecruitment = rss.SaveRecruitment })
            .Take(limit);




            if (!string.IsNullOrEmpty(filter))
            {
                if (filter.Equals("districts"))
                {
                    return recruitments.Where(r => r.Recruitment.District.Contains(value1)).ToList();
                }
                if (filter.Equals("salaries") && !string.IsNullOrEmpty(value1) && !string.IsNullOrEmpty(value2))
                {
                    int intValue1 = int.Parse(value1) * 1000000;
                    int intValue2 = int.Parse(value2) * 1000000;
                    return recruitments.Where(r => r.Recruitment.Salary >= intValue1 && r.Recruitment.Salary <= intValue2).ToList();
                }
                if (filter.Equals("experiences") && !string.IsNullOrEmpty(value1) && !string.IsNullOrEmpty(value2))
                {
                    int intValue1 = int.Parse(value1);
                    int intValue2 = int.Parse(value2);
                    if (intValue1 == 0)
                    {
                        return recruitments.Where(r => r.Recruitment.Experience == intValue1).ToList();

                    }
                    return recruitments.Where(r => r.Recruitment.Experience > intValue1 && r.Recruitment.Experience <= intValue2).ToList();
                }
                if (filter.Equals("industryes") && !string.IsNullOrEmpty(value1))
                {
                    int intValue1 = int.Parse(value1);
                    return recruitments.Where(r => r.Recruitment.IndustryId == intValue1).ToList();
                }
                if (filter.Equals("title") && !string.IsNullOrEmpty(value1))
                {
                    return recruitments.Where(r => r.Recruitment.RecruitmentTitle.Contains(value1)).ToList();
                }
            }

            recruitments.ToList();
            return recruitments;


        }
    }
}

