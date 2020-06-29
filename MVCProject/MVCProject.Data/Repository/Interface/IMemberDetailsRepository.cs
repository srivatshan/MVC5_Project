using MVCProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject.Data.Repository.Interface
{
   public interface IMemberDetailsRepository
    {
        List<MemberDetails> GetMemberById(int userId);

        void AddMemberDetails(List<MemberDetails> memberDetails);

        List<MemberDetails> GetAllMembers();

        int ApplicationId();

        int ApplicationId(int UserId);

        MemberDetails GetMemberByApplicationId(int ApplicationId,int MemberId);

        void SaveMember(MemberDetails memberDetails);
    }
}
