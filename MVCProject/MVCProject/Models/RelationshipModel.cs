using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Models
{
    public class RelationshipModel
    {
        public int UserId { get; set; }
        public int FromMemberId { get; set; }
        public string FromMemberName { get; set; }
        public int Relationship { get; set; }
        public int Tomemberid { get; set; }
        public string ToMemberName { get; set; }
    }
    public class RelationShipData
    {
        public int CurrentMemberId { get; set; }
        public List<RelationshipModel> RelationshipsList { get; set; }
        public IEnumerable<Relation> RelationDropDown { get; set; }
    }
    public class Relation
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}