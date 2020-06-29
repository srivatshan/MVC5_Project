using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Models
{
    public class SearchDetails
    {
        public SearchModel SearchModel { get; set; }
        public List<HouseHoldMemberModel> MembersList { get; set; }
    }
    public class SearchModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public int ApplicationId { get; set; }
    }
}