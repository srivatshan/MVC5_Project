using MVCProject.Data.Models;
using MVCProject.Data.Repository.Interface;
using MVCProject.Filters;
using MVCProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Controllers
{
    [AuthendicationFilter]
    public class HouseHoldController : Controller
    {
        private readonly IMemberDetailsRepository _memberDetailsRepository;

        public HouseHoldController(IMemberDetailsRepository memberDetailsRepository)
        {
            _memberDetailsRepository = memberDetailsRepository;
        }

        public ActionResult Home()
        {
            // Getting User Details from Session or Cookies

            User UserDetails = (User)Session["UserDetails"] ?? JsonConvert.DeserializeObject<User>(Request.Cookies["UserDetails"].Value);


            var StoredData = _memberDetailsRepository.GetMemberById(UserDetails.UserId);


            List<HouseHoldMemberModel> List = new List<HouseHoldMemberModel>();
            foreach (var Details in StoredData)
            {
                HouseHoldMemberModel houseHoldMemberModel = new HouseHoldMemberModel()
                {
                    FirstName = Details.FirstName,
                    LastName = Details.LastName,
                    DateOfBirth = Convert.ToDateTime(Details.Dob),
                    Gender = Details.Gender,
                    Ml = Details.ML,
                    Suffix = (SuffixList)Enum.Parse(typeof(SuffixList), Details.Suffix, true),
                    MemberID = Details.MemberId
                };
                List.Add(houseHoldMemberModel);
            }
            HouseHoldModel houseHoldModel = new HouseHoldModel()
            {
                MembersList = List,
            };
            if (Session["MemberInfo"] == null)
                Session["MemberInfo"] = houseHoldModel;
            else
                houseHoldModel = (HouseHoldModel)Session["MemberInfo"];
            return View(houseHoldModel);
        }

        [HttpPost]
        public ActionResult AddMember(HouseHoldModel houseHoldModel)
        {
            HouseHoldModel MemberObj = (HouseHoldModel)Session["MemberInfo"] ?? new HouseHoldModel();

            if (ModelState.IsValid)
            {

                List<HouseHoldMemberModel> details = MemberObj.MembersList ?? new List<HouseHoldMemberModel>();
                if (details.Count >= 5)
                {
                    ModelState.AddModelError(string.Empty, "Maximum 5 members Only allowed");
                    return View("Home", MemberObj);
                }
                if (MemberObj.MembersList.Any(x => x.DateOfBirth > houseHoldModel.HouseHoldMemberModel.DateOfBirth))
                {
                    ModelState.AddModelError(string.Empty, "Age Should be less then Existing Members Age");
                    return View("Home", MemberObj);
                }

                HouseHoldMemberModel Model = houseHoldModel.HouseHoldMemberModel;
                Model.MemberID = details.Select(x => x.MemberID).DefaultIfEmpty(0).Max() + 1;
                details.Add(houseHoldModel.HouseHoldMemberModel);
                MemberObj.HouseHoldMemberModel = new HouseHoldMemberModel();
                MemberObj.MembersList = details;
                Session["MemberInfo"] = MemberObj;
                ModelState.Clear();
                return View("Home", MemberObj);
            }
            return View("Home", MemberObj);
        }

        [HttpPost]
        public ActionResult Save(HouseHoldModel houseHoldModel)
        {
            HouseHoldModel MemberObj = (HouseHoldModel)Session["MemberInfo"] ?? new HouseHoldModel();
            List<HouseHoldMemberModel> details = MemberObj.MembersList ?? new List<HouseHoldMemberModel>();
            var model = houseHoldModel.HouseHoldMemberModel;
            bool isEmptyModel = (string.IsNullOrEmpty(model.FirstName) && string.IsNullOrEmpty(model.LastName) && string.IsNullOrEmpty(model.Gender)
                && model.Suffix == 0 && string.IsNullOrEmpty(model.Ml) && model.DateOfBirth == DateTime.MinValue) ? true : false;
            if (ModelState.IsValid || isEmptyModel)
            {

                if (isEmptyModel == false)
                {
                    houseHoldModel.HouseHoldMemberModel.MemberID = details.Select(x => x.MemberID).DefaultIfEmpty(0).Max() + 1;
                    details.Add(houseHoldModel.HouseHoldMemberModel);
                }



                List<MemberDetails> memberDetails = new List<MemberDetails>();
                User UserDetails = (User)Session["UserDetails"] ?? JsonConvert.DeserializeObject<User>(Request.Cookies["UserDetails"].Value);

                foreach (var data in details)
                {
                    memberDetails.Add(new MemberDetails()
                    {
                        MemberId = data.MemberID,
                        UserId = UserDetails.UserId,
                        Dob = data.DateOfBirth.ToString(),
                        FirstName = data.FirstName,
                        LastName = data.LastName,
                        ML = data.Ml,
                        Gender = data.Gender,
                        Suffix = data.Suffix.ToString()
                    });
                }
                if (memberDetails.Count > 0)
                {
                    _memberDetailsRepository.AddMemberDetails(memberDetails);
                    Session["MemberInfo"] = null;
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Next(HouseHoldModel houseHoldModel)
        {
            HouseHoldModel MemberObj = (HouseHoldModel)Session["MemberInfo"] ?? new HouseHoldModel();
            List<HouseHoldMemberModel> details = MemberObj.MembersList ?? new List<HouseHoldMemberModel>();
            var model = houseHoldModel.HouseHoldMemberModel;
            bool isEmptyModel = (string.IsNullOrEmpty(model.FirstName) && string.IsNullOrEmpty(model.LastName) && string.IsNullOrEmpty(model.Gender)
                && model.Suffix == 0 && string.IsNullOrEmpty(model.Ml) && model.DateOfBirth == DateTime.MinValue) ? true : false;
            if (ModelState.IsValid || isEmptyModel)
            {
                 if (isEmptyModel == false)
                {
                    houseHoldModel.HouseHoldMemberModel.MemberID = details.Select(x => x.MemberID).DefaultIfEmpty(0).Max() + 1;
                    details.Add(houseHoldModel.HouseHoldMemberModel);
                }



                List<MemberDetails> memberDetails = new List<MemberDetails>();
                User UserDetails = (User)Session["UserDetails"] ?? JsonConvert.DeserializeObject<User>(Request.Cookies["UserDetails"].Value);

                foreach (var data in details)
                {
                    memberDetails.Add(new MemberDetails()
                    {
                        MemberId = data.MemberID,
                        UserId = UserDetails.UserId,
                        Dob = data.DateOfBirth.ToString(),
                        FirstName = data.FirstName,
                        LastName = data.LastName,
                        ML = data.Ml,
                        Gender = data.Gender,
                        Suffix = data.Suffix.ToString()
                    });
                }
                if (memberDetails.Count > 0)
                {
                    _memberDetailsRepository.AddMemberDetails(memberDetails);
                    Session["MemberInfo"] = null;
                    return RedirectToAction("Home","Relationship");
                }
            }
            return View("Home");
        }



        [HttpGet]
        public ActionResult DeleteMember(int Id)
        {
            HouseHoldModel MemberObj = (HouseHoldModel)Session["MemberInfo"] ?? new HouseHoldModel();
            List<HouseHoldMemberModel> details = MemberObj.MembersList ?? new List<HouseHoldMemberModel>();
            details.RemoveAll(x => x.MemberID == Id);
            MemberObj.MembersList = details;
            Session["MemberInfo"] = MemberObj;

            return RedirectToAction("Home");
        }

        public ActionResult EditMember(int ApplicationID,int MemberId)
        {

           var result= _memberDetailsRepository.GetMemberByApplicationId(ApplicationID, MemberId);
            HouseHoldMemberModel houseHoldMemberModel = new HouseHoldMemberModel()
            {
                ApplicationID = result.ApplicationId,
                FirstName = result.FirstName,
                LastName = result.LastName,
                DateOfBirth = Convert.ToDateTime(result.Dob),
                Gender = result.Gender,
                MemberID = result.MemberId,
                Ml = result.ML,
                Suffix = (SuffixList)Enum.Parse(typeof(SuffixList), result.Suffix, true)
            };
            return View(houseHoldMemberModel);
        }
        [HttpPost]
        public ActionResult EditMember(HouseHoldMemberModel houseHoldMemberModel)
        {
            MemberDetails memberDetails = new MemberDetails()
            {
                ApplicationId = houseHoldMemberModel.ApplicationID,
                FirstName = houseHoldMemberModel.FirstName,
                LastName = houseHoldMemberModel.LastName,
                Dob = houseHoldMemberModel.DateOfBirth.ToString(),
                Gender = houseHoldMemberModel.Gender,
                MemberId = houseHoldMemberModel.MemberID,
                ML = houseHoldMemberModel.Ml,
                Suffix= houseHoldMemberModel.Suffix.ToString()
            };
            _memberDetailsRepository.SaveMember(memberDetails);
            return RedirectToAction("Home");
        }

        public ActionResult ViewMember(int ApplicationID, int MemberId)
        {
            var result = _memberDetailsRepository.GetMemberByApplicationId(ApplicationID, MemberId);
            HouseHoldMemberModel houseHoldMemberModel = new HouseHoldMemberModel()
            {
                ApplicationID = result.ApplicationId,
                FirstName = result.FirstName,
                LastName = result.LastName,
                DateOfBirth = Convert.ToDateTime(result.Dob),
                Gender = result.Gender,
                MemberID = result.MemberId,
                Ml = result.ML,
                Suffix = (SuffixList)Enum.Parse(typeof(SuffixList), result.Suffix, true)
            };
            return View(houseHoldMemberModel);
        }

    }
}