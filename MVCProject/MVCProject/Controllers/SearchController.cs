using MVCProject.Data.Models;
using MVCProject.Data.Repository.Interface;
using MVCProject.Filters;
using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Controllers
{
    [AuthendicationFilter]
    public class SearchController : Controller
    {
        private readonly IMemberDetailsRepository _memberDetailsRepository;
        public SearchController(IMemberDetailsRepository memberDetailsRepository)
        {
            _memberDetailsRepository = memberDetailsRepository;
        }

        public ActionResult Home()
        {
            return View(new SearchDetails());
        }

        [HttpPost]
        public ActionResult Home(SearchDetails searchDetails)
        {
            var model = searchDetails.SearchModel;
            bool isEmptyModel = (string.IsNullOrEmpty(model.FirstName) && string.IsNullOrEmpty(model.LastName) && string.IsNullOrEmpty(model.DateOfBirth) && model.ApplicationId <= 0) ? true : false;
            var result = new List<MemberDetails>();
            if (isEmptyModel)
            {
                ModelState.AddModelError("", "Please enter any search details");
                return View(searchDetails);
            }
            else
            {
                var query = from memberdatails in _memberDetailsRepository.GetAllMembers()
                            select memberdatails;
                if (!string.IsNullOrEmpty(model.FirstName))
                    query = query.Where(x => x.FirstName == model.FirstName);
                if (!string.IsNullOrEmpty(model.LastName))
                    query = query.Where(x => x.LastName == model.LastName);
                if (!string.IsNullOrEmpty(model.DateOfBirth))
                    query = query.Where(x => x.Dob == model.DateOfBirth);
                if (model.ApplicationId > 0)
                    query = query.Where(x => x.ApplicationId == model.ApplicationId);
                result = query.Select(x => x).ToList();

            }
            List<HouseHoldMemberModel> HouseHoldList = new List<HouseHoldMemberModel>();
            foreach (var Details in result)
            {
                HouseHoldList.Add(new HouseHoldMemberModel()
                {
                    FirstName = Details.FirstName,
                    LastName = Details.LastName,
                    Ml = Details.ML,
                    DateOfBirth = Convert.ToDateTime(Details.Dob),
                    Gender = Details.Gender,
                    Suffix = (SuffixList)Enum.Parse(typeof(SuffixList), Details.Suffix, true),
                    MemberID = Details.MemberId,
                    ApplicationID=Details.ApplicationId
                });
            }
             return View(new SearchDetails() { MembersList=HouseHoldList});
        }


      

    }
}