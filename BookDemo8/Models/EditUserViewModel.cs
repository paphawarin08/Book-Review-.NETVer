using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookDemo8.Models
{
    public class EditUserViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        // เพิ่มข้อมูลรีวิวที่สามารถแก้ไขได้
        public List<EditReviewViewModel> Reviews { get; set; }

        public EditUserViewModel()
        {
            Reviews = new List<EditReviewViewModel>(); // กำหนดค่าเริ่มต้นให้เป็นรายการว่าง
        }
    }

    public class EditReviewViewModel
    {
        public int ReviewId { get; set; }
        public string BookTitle { get; set; }
        public string ReviewContent { get; set; }
        public int Rating { get; set; }
    }
}