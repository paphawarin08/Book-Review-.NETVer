using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookDemo8;
using Microsoft.AspNet.Identity;

namespace BookDemo8.Controllers
{
    public class ReviewsController : Controller
    {
        private Entities db = new Entities();

        // GET: Reviews
        public ActionResult Index(string searchString)
        {

            var books = db.BookReviews.Include(b => b.AspNetUsers).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.BookTitle.Contains(searchString) || b.Category.Contains(searchString));
            }

            return View(books.ToList());
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookReviews bookReviews = db.BookReviews.Find(id);
            if (bookReviews == null)
            {
                return HttpNotFound();
            }
            return View(bookReviews);
        }

        // GET: Reviews/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId(); // ดึง UserId ของผู้ใช้ที่ล็อกอิน
            var currentUser = db.AspNetUsers.FirstOrDefault(u => u.Id == userId);

            // สร้าง SelectList ที่มีแค่ผู้ใช้ที่ล็อกอิน
            ViewBag.UserId = new SelectList(db.AspNetUsers.Where(u => u.Id == userId), "Id", "Email");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReviewId,UserId,BookTitle,ReviewContent,Rating,Category,ImageUrl,PurchaseLink,CreatedAt,UpdatedAt")] BookReviews bookReviews)
        {
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    file.SaveAs(path);
                    bookReviews.ImageUrl = fileName;
                }

                // กำหนด CreatedAt ให้เป็นวันที่ปัจจุบัน
                bookReviews.CreatedAt = DateTime.Now;

                db.BookReviews.Add(bookReviews);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookReviews bookReviews = db.BookReviews.Find(id);
            if (bookReviews == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", bookReviews.UserId);
            return View(bookReviews);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReviewId,UserId,BookTitle,ReviewContent,Rating,Category,ImageUrl,PurchaseLink,CreatedAt,UpdatedAt")] BookReviews bookReviews)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookReviews).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", bookReviews.UserId);
            return View(bookReviews);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookReviews bookReviews = db.BookReviews.Find(id);
            if (bookReviews == null)
            {
                return HttpNotFound();
            }
            return View(bookReviews);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookReviews bookReviews = db.BookReviews.Find(id);
            db.BookReviews.Remove(bookReviews);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
