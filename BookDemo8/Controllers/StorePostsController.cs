using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookDemo8;

namespace BookDemo8.Controllers
{
    public class StorePostsController : Controller
    {
        private Entities db = new Entities();

        // GET: StorePosts
        public ActionResult Index()
        {
            var storePosts = db.StorePosts.Include(s => s.Stores);
            return View(storePosts.ToList());
        }

        // GET: StorePosts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StorePosts storePosts = db.StorePosts.Find(id);
            if (storePosts == null)
            {
                return HttpNotFound();
            }
            return View(storePosts);
        }

        // GET: StorePosts/Create
        public ActionResult Create()
        {
            ViewBag.StoreId = new SelectList(db.Stores, "StoreId", "StoreName");
            return View();
        }

        // POST: StorePosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,StoreId,BookTitle,Description,Price,CreatedAt")] StorePosts storePosts)
        {
            if (ModelState.IsValid)
            {
                db.StorePosts.Add(storePosts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StoreId = new SelectList(db.Stores, "StoreId", "StoreName", storePosts.StoreId);
            return View(storePosts);
        }

        // GET: StorePosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StorePosts storePosts = db.StorePosts.Find(id);
            if (storePosts == null)
            {
                return HttpNotFound();
            }
            ViewBag.StoreId = new SelectList(db.Stores, "StoreId", "StoreName", storePosts.StoreId);
            return View(storePosts);
        }

        // POST: StorePosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,StoreId,BookTitle,Description,Price,CreatedAt")] StorePosts storePosts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(storePosts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StoreId = new SelectList(db.Stores, "StoreId", "StoreName", storePosts.StoreId);
            return View(storePosts);
        }

        // GET: StorePosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StorePosts storePosts = db.StorePosts.Find(id);
            if (storePosts == null)
            {
                return HttpNotFound();
            }
            return View(storePosts);
        }

        // POST: StorePosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StorePosts storePosts = db.StorePosts.Find(id);
            db.StorePosts.Remove(storePosts);
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
