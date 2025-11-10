using BookDemo8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BookDemo8.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private Entities db = new Entities();
        public ActionResult ManageUsers()
        {
            var usersWithReviews = db.AspNetUsers
       .Select(user => new UserWithReviewsViewModel
       {
           User = user,
           Reviews = db.BookReviews.Where(r => r.UserId == user.Id).ToList()
       })
       .ToList();

            return View(usersWithReviews);
        }


        // ระงับผู้ใช้
        public ActionResult SuspendUser(string id)
        {
            var user = db.AspNetUsers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            // ตัวอย่างการระงับผู้ใช้ (อาจเพิ่มฟิลด์ IsSuspended ในตาราง AspNetUsers)
            user.LockoutEnabled = true;
            user.LockoutEndDateUtc = DateTime.UtcNow.AddYears(1);
            db.SaveChanges();

            return RedirectToAction("ManageUsers");
        }

        // ลบผู้ใช้
        public ActionResult DeleteUser(string id)
        {
            var user = db.AspNetUsers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            db.AspNetUsers.Remove(user);
            db.SaveChanges();

            return RedirectToAction("ManageUsers");
        }

        [HttpGet]
        public ActionResult EditUser(string id)
        {
            var user = db.AspNetUsers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            // ดึงรีวิวของผู้ใช้
            var userReviews = db.BookReviews.Where(r => r.UserId == id).ToList();

            // สร้าง ViewModel สำหรับแสดงข้อมูลในฟอร์ม
            var viewModel = new EditUserViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Reviews = userReviews.Select(r => new EditReviewViewModel
                {
                    ReviewId = r.ReviewId,
                    BookTitle = r.BookTitle,
                    ReviewContent = r.ReviewContent,
                    Rating = r.Rating
                }).ToList()
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // ค้นหาผู้ใช้ในฐานข้อมูล
                var user = db.AspNetUsers.Find(model.UserId);
                if (user == null)
                {
                    return HttpNotFound();
                }

                // แก้ไขข้อมูลผู้ใช้
                user.UserName = model.UserName;
                user.Email = model.Email;

                // แก้ไขข้อมูลรีวิวของผู้ใช้
                foreach (var reviewModel in model.Reviews)
                {
                    var review = db.BookReviews.Find(reviewModel.ReviewId);
                    if (review != null)
                    {
                        review.BookTitle = reviewModel.BookTitle;
                        review.ReviewContent = reviewModel.ReviewContent;
                        review.Rating = reviewModel.Rating;
                    }
                }

                // บันทึกการเปลี่ยนแปลง
                db.SaveChanges();
                return RedirectToAction("ManageUsers");
            }

            return View(model);
        }

        public ActionResult Detail(string id)
        {
            var user = db.AspNetUsers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userReviews = db.BookReviews.Where(r => r.UserId == id).ToList();

            var viewModel = new UserWithReviewsViewModel
            {
                User = user,
                Reviews = userReviews
            };

            return View(viewModel);
        }

        public ActionResult UserRatingChart(string id)
        {
            var user = db.AspNetUsers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            // ดึงรีวิวทั้งหมดของผู้ใช้
            var userReviews = db.BookReviews.Where(r => r.UserId == id).ToList();

            // คำนวณคะแนนเฉลี่ยของแต่ละหนังสือ
            var bookAverageRatings = userReviews
                .GroupBy(r => r.BookTitle)
                .Select(group => new BookAverageRating
                {
                    BookTitle = group.Key,
                    AverageRating = group.Average(r => r.Rating) // คำนวณคะแนนเฉลี่ย
        })
                .OrderByDescending(x => x.AverageRating) // สามารถจัดอันดับจากคะแนนเฉลี่ยได้
                .ToList();

            // สร้าง ViewModel สำหรับการแสดงผลกราฟ
            var viewModel = new UserRatingChartViewModel
            {
                UserName = user.UserName,
                BookAverageRatings = bookAverageRatings
            };

            return View(viewModel);
        }

    }

}