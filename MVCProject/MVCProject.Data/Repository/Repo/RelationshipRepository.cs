using MVCProject.Data.Models;
using MVCProject.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject.Data.Repository.Repo
{
    public class RelationshipRepository : IRelationshipRepository
    {
        private readonly ApplicationDBContext _db;
        public RelationshipRepository(ApplicationDBContext caseStudyDBContext)
        {
            _db = caseStudyDBContext;
        }
        public int SaveDetails(List<RelationshipDetails> relationshipDetails)
        {
            int UserId = relationshipDetails.Select(x => x.UserId).FirstOrDefault();
            
            if (GetRelationById(UserId).Count > 0)
            {
                var data = _db.RelationshipDetails.Where(x => x.UserId == UserId).ToList();
                _db.RelationshipDetails.RemoveRange(data);
                _db.SaveChanges();
            }
            int AppId = 0;
            foreach (var details in relationshipDetails)
            {
                details.ApplicationId = ApplicationId(details.UserId);
                _db.RelationshipDetails.Add(details);
                AppId = details.ApplicationId;
            }

            _db.SaveChanges();
            return AppId;
        }

        public int ApplicationId(int UserId)
        {
            return _db.MemberDetails.Where(p => p.UserId == UserId).Select(p => p.ApplicationId).FirstOrDefault();
        }


        public List<RelationshipDetails> GetRelationById(int userId)
        {
            return _db.RelationshipDetails.Where(x => x.UserId == userId).ToList();
        }
    }
}
