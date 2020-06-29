using iTextSharp.text;
using iTextSharp.text.pdf;
using MVCProject.Data.Models;
using MVCProject.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MVCProject.Filters
{
    public class PDFGenerator
    {
        private readonly IMemberDetailsRepository _memberDetailsRepository;
        private readonly IRelationshipRepository _relationshipRepository;

        public PDFGenerator(IMemberDetailsRepository memberDetailsRepository, IRelationshipRepository relationshipRepository)
        {
            _memberDetailsRepository = memberDetailsRepository;
            _relationshipRepository = relationshipRepository;
         }
          }
}