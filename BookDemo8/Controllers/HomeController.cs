using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookDemo8.Models;

namespace BookDemo8.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();
        public ActionResult Index(string searchString, int? rating)
        {
            // Fetch all book reviews from the database
            var bookReviews = db.BookReviews.AsQueryable();

            // Filter book reviews based on the search string
            if (!string.IsNullOrEmpty(searchString))
            {
                bookReviews = bookReviews.Where(b => b.BookTitle.Contains(searchString) ||
                                                     b.Category.Contains(searchString));
            }

            if (rating > 0)
            {
                bookReviews = bookReviews.Where(r => r.Rating == rating);
            }

            // Convert to a list and send it to the view
            return View(bookReviews.ToList());
        }

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
    }
     
       
    }
