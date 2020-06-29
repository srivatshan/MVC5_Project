using MVCProject.Data.Models;
using MVCProject.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject.Data.Repository.Repo
{
    public class MemberDetailsRepository : IMemberDetailsRepository
    {
        private readonly ApplicationDBContext _db;
        public MemberDetailsRepository(ApplicationDBContext caseStudyDBContext)
        {
            _db = caseStudyDBContext;
        }
        public int ApplicationId()
        {
            return (_db.MemberDetails.Select(p => p.ApplicationId).DefaultIfEmpty(0).Max())+1;
        }

        public int ApplicationId(int UserId)
        {
            return _db.MemberDetails.Where(p=>p.UserId==UserId).Select(p => p.ApplicationId).FirstOrDefault();
        }

        public List<MemberDetails> GetAllMembers()
        {
            return _db.MemberDetails.ToList();
        }

        public List<MemberDetails> GetMemberById(int userId)
        {
            return _db.MemberDetails.Where(x => x.UserId == userId).ToList();
        }

        public void AddMemberDetails(List<MemberDetails> memberDetails)
        {
            int UserId = memberDetails.Select(x => x.UserId).FirstOrDefault();
            int AppId = ApplicationId(UserId);
            if (GetMemberById(UserId).Count > 0)
            {
                var data = _db.MemberDetails.Where(x => x.UserId == UserId).ToList();
                _db.MemberDetails.RemoveRange(data);
                _db.SaveChanges();
            }

            foreach (var details in memberDetails)
            {
                details.ApplicationId = AppId != 0 ? AppId : ApplicationId();
                _db.MemberDetails.Add(details);
            }

            _db.SaveChanges();
        }

        public MemberDetails GetMemberByApplicationId(int ApplicationId,int MemberId)
        {
           return _db.MemberDetails.Where(x => x.ApplicationId == ApplicationId && x.MemberId==  MemberId).FirstOrDefault();
        }

        public void SaveMember(MemberDetails memberDetails)
        {
            var model = _db.MemberDetails.SingleOrDefault(x=> x.ApplicationId== memberDetails.ApplicationId && x.MemberId==memberDetails.MemberId);
            if(model != null)
            {
                model.FirstName = memberDetails.FirstName;
                model.LastName = memberDetails.LastName;
                model.ML = memberDetails.ML;
                model.Dob = memberDetails.Dob;
                model.Suffix = memberDetails.Suffix;
                model.Gender = memberDetails.Gender;
            }
            _db.SaveChanges();

               
        }
    }
}
