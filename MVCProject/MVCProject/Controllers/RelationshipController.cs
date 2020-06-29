using iTextSharp.text;
using iTextSharp.text.pdf;
using MVCProject.Data.Models;
using MVCProject.Data.Repository.Interface;
using MVCProject.Filters;
using MVCProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Controllers
{
    [AuthendicationFilter]
    public class RelationshipController : Controller
    {
        private readonly IMemberDetailsRepository _memberDetailsRepository;
        private readonly IRelationshipRepository _relationshipRepository;
        public RelationshipController(IMemberDetailsRepository memberDetailsRepository,IRelationshipRepository relationshipRepository)
        {
            _memberDetailsRepository = memberDetailsRepository;
            _relationshipRepository = relationshipRepository;
        }

        public ActionResult Home()
        {
            Session["UserDetails"] = (User)Session["UserDetails"] ?? JsonConvert.DeserializeObject<User>(Request.Cookies["UserDetails"].Value);
            User UserDetails = (User)Session["UserDetails"];
            List<MemberDetails> MemberDetailsList = (List<MemberDetails>)Session["ApplicationDetails"] ?? _memberDetailsRepository.GetMemberById(((User)Session["UserDetails"]).UserId);

            List<RelationshipModel> RelationshipsList = new List<RelationshipModel>();
            for (int i = 0; i < MemberDetailsList.Count; i++)
            {
                for (int j = 0; j < (MemberDetailsList.Count); j++)
                {
                    if (MemberDetailsList[i].Id != MemberDetailsList[j].Id)
                    {
                        RelationshipModel relationshipModel = new RelationshipModel()
                        {
                            // MemberId = MemberDetailsList[i].MemberId,
                            UserId = UserDetails.UserId,
                            FromMemberId = MemberDetailsList[i].MemberId,
                            FromMemberName = MemberDetailsList[i].FirstName,
                            Relationship = 0,
                            Tomemberid = MemberDetailsList[j].MemberId,
                            ToMemberName = MemberDetailsList[j].FirstName
                        };
                        RelationshipsList.Add(relationshipModel);
                    }

                }
            }
            List<Relation> Relationlist = new List<Relation>();
            Relationlist.Add(new Relation() { Id = 1, Name = "Father" });
            Relationlist.Add(new Relation() { Id = 2, Name = "Mother" });
            Relationlist.Add(new Relation() { Id = 3, Name = "Son" });
            Relationlist.Add(new Relation() { Id = 4, Name = "Daughter" });

            RelationShipData RelationData = new RelationShipData()
            {
                CurrentMemberId = MemberDetailsList.Min(x => x.MemberId),
                RelationshipsList = RelationshipsList,
                RelationDropDown = Relationlist
            };
            Session["RelationDetails"] = RelationData;
            return View(RelationData);
        }

        [HttpPost]
        public ActionResult Home(RelationShipData RelationDetails, int id)
        {
            RelationShipData RelationData = (RelationShipData)Session["RelationDetails"];
            var SelectedData = RelationDetails.RelationshipsList.Where(x => x.Relationship != 0 ).Select(x => x);
            if (SelectedData != null)
            {
                foreach (var Item in SelectedData)
                {
                    RelationData.RelationshipsList.Where(x => x.FromMemberId == Item.FromMemberId && x.Tomemberid == Item.Tomemberid).FirstOrDefault().Relationship = Item.Relationship;
                }
            }
            //if user didnt select all the dropdown list
            if (RelationData.RelationshipsList.Any(x => x.FromMemberId == RelationDetails.CurrentMemberId && (x.Relationship == 0)))
            {
                ModelState.AddModelError(string.Empty, "Select All Mandatory Fields");
                return View(RelationData);
            }


            if (id != RelationDetails.CurrentMemberId && id > RelationDetails.CurrentMemberId)
            {
                var CurMemberID = ((RelationShipData)Session["RelationDetails"]).RelationshipsList.Where(x => x.Relationship == 0).FirstOrDefault();
                RelationData.CurrentMemberId = CurMemberID == null ? id : CurMemberID.FromMemberId;
            }
            else if (id < RelationDetails.CurrentMemberId)
            {
                RelationData.CurrentMemberId = id;
            }
            Session["RelationDetails"] = RelationData;
            ModelState.Clear();
            return View(RelationData);
         }
        [HttpPost]
        public ActionResult Submit(RelationShipData RelationDetails)
        {
            RelationShipData RelationData = (RelationShipData)Session["RelationDetails"];
            var SelectedData = RelationDetails.RelationshipsList.Where(x => x.Relationship != 0).Select(x => x);
            if (SelectedData != null)
            {
                foreach (var Item in SelectedData)
                {
                    RelationData.RelationshipsList.Where(x => x.FromMemberId == Item.FromMemberId && x.Tomemberid == Item.Tomemberid).FirstOrDefault().Relationship = Item.Relationship;
                }
            }
            int ApplicationId = 0;
            //if user didnt select all the dropdown list
            if (RelationData.RelationshipsList.Any(x => x.Relationship == 0))
            {
                ModelState.AddModelError(string.Empty, "Select All Mandatory Fields");
                return View("Home",RelationData);
            }
            else
            {
                List<RelationshipDetails> relationshipDetails = new List<RelationshipDetails>();
                foreach(var data in RelationData.RelationshipsList)
                {
                    relationshipDetails.Add(new RelationshipDetails()
                    {
                        FromMemberId= data.FromMemberId,
                        FromMemberName=data.FromMemberName,
                        Relationship=data.Relationship,
                        Tomemberid=data.Tomemberid,
                        ToMemberName=data.ToMemberName,
                        UserId = ((User)Session["UserDetails"]).UserId
                });
                }
              _relationshipRepository.SaveDetails(relationshipDetails);
            }


            return RedirectToAction("Confirm");
        }

        public ActionResult Confirm()
        {
           
             return View();
        }
        public ActionResult DownloadReceipt()
        {

            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");

            string strPDFFileName = string.Format("Receipt" + DateTime.Now.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 5 columns  
            PdfPTable tableLayout = new PdfPTable(5);
            doc.SetMargins(0f, 0f, 0f, 0f);
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
            //Add Content to PDF   
            doc.Add(Add_Member_Content_To_PDF(tableLayout));
            PdfPTable tableLayout1 = new PdfPTable(2);
            doc.SetMargins(0f, 0f, 0f, 0f);

            doc.Add(Add_Relation_Content_To_PDF(tableLayout1));
            // Closing the document  
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;
            return File(workStream, "application/pdf", strPDFFileName);
        }

        protected PdfPTable Add_Member_Content_To_PDF(PdfPTable tableLayout)
        {
            float[] headers = { 50, 24, 45, 35, 50 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;


            List<MemberDetails> Members = _memberDetailsRepository.GetMemberById(((User)Session["UserDetails"]).UserId);
            


            tableLayout.AddCell(new PdfPCell(new Phrase("Member Details", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });


            ////Add header  
            AddCellToHeader(tableLayout, "Member ID");
            AddCellToHeader(tableLayout, "FirstName");
            AddCellToHeader(tableLayout, "LastName");
            AddCellToHeader(tableLayout, "Date Of Birth");
            AddCellToHeader(tableLayout, "Gender");

            ////Add body  
            foreach (var data in Members)
            {
                AddCellToBody(tableLayout, data.MemberId.ToString());
                AddCellToBody(tableLayout, data.FirstName);
                AddCellToBody(tableLayout, data.LastName);
                AddCellToBody(tableLayout, data.Dob);
                AddCellToBody(tableLayout, data.Gender);
            }



            return tableLayout;
        }

        protected PdfPTable Add_Relation_Content_To_PDF(PdfPTable tableLayout)
        {
            float[] headers = { 50, 24 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            List<RelationshipDetails> relationshipDetails = _relationshipRepository.GetRelationById(((User)Session["UserDetails"]).UserId);
            tableLayout.AddCell(new PdfPCell(new Phrase("Relationship Details", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });


            ////Add header  
            AddCellToHeader(tableLayout, "Member Relationship");
            AddCellToHeader(tableLayout, "Relation");

            List<Relation> Relationlist = new List<Relation>();
            Relationlist.Add(new Relation() { Id = 1, Name = "Father" });
            Relationlist.Add(new Relation() { Id = 2, Name = "Mother" });
            Relationlist.Add(new Relation() { Id = 3, Name = "Son" });
            Relationlist.Add(new Relation() { Id = 4, Name = "Daughter" });

            ////Add body  
            foreach (var data in relationshipDetails)
            {
                AddCellToBody(tableLayout, data.FromMemberName + " Relationship to " + data.ToMemberName);
                AddCellToBody(tableLayout, Relationlist.Where(x=> x.Id==Convert.ToInt32(data.Relationship)).SingleOrDefault()?.Name );
            }
            return tableLayout;
        }
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.YELLOW)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0)
            });
        }

        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }

    }
}