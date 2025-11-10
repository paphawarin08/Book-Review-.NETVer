using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookDemo8.Models
{
    public class UserWithReviewsViewModel
    {
        public AspNetUsers User { get; set; }
        public List<BookReviews> Reviews { get; set; }
        // เพิ่มข้อมูลสำหรับกราฟ
        public List<MonthlyVisits> MonthlyVisits { get; set; }

    }

    public class MonthlyVisits
    {
        public string Month { get; set; }
        public int VisitCount { get; set; }
    }
}