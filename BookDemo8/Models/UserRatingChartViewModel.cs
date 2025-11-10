using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookDemo8.Models
{
    public class UserRatingChartViewModel
    {
        // ชื่อผู้ใช้
        public string UserName { get; set; }

        // ข้อมูลคะแนนเฉลี่ยของหนังสือ
        public List<BookAverageRating> BookAverageRatings { get; set; }
    }

    public class BookAverageRating
    {
        public string BookTitle { get; set; }
        public double AverageRating { get; set; } // คะแนนเฉลี่ย
    }
}