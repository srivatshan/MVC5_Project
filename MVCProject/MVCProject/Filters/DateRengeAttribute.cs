using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCProject.Filters
{
    public class DateRengeAttribute : RangeAttribute
    {
        public DateRengeAttribute() : base(typeof(DateTime), DateTime.Now.AddYears(-125).ToShortDateString(), DateTime.Now.ToShortDateString())
        { }
    }
}