using MVCProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject.Data.Repository.Interface
{
    public interface IRelationshipRepository
    {
        int SaveDetails(List<RelationshipDetails> relationshipDetails);

        List<RelationshipDetails> GetRelationById(int userId);
    }
}
