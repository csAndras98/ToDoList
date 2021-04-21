using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class toDoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: toDoes
        public ActionResult Index()
        {
            
            return View();
        }

        private IEnumerable<toDo> GetToDoes()
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.FirstOrDefault(
                x => x.Id == userId);
            IEnumerable<toDo> myToDos = db.toDos.ToList().Where(x => x.User == user);
            
            int completeCount = 0;

            foreach(toDo toDo in myToDos) 
            {
                if (toDo.Done)
                {
                    completeCount++;
                }
            }

            ViewBag.Percent = 100f * ((float)completeCount / (float)myToDos.Count());

            return myToDos;

        }
        public ActionResult BuildToDoTable()
        {
            
            return PartialView("_ToDoTable", GetToDoes());
        }

        // GET: toDoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            toDo toDo = db.toDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            return View(toDo);
        }

        // GET: toDoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: toDoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Desc,Done")] toDo toDo)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                ApplicationUser user = db.Users.FirstOrDefault(
                    x => x.Id == userId);
                toDo.User = user;
                db.toDos.Add(toDo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(toDo);
        }

        public ActionResult AjaxCreate([Bind(Include = "Id,Desc")] toDo toDo)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                ApplicationUser user = db.Users.FirstOrDefault(
                    x => x.Id == userId);
                toDo.User = user;
                toDo.Done = false;
                db.toDos.Add(toDo);
                db.SaveChanges();
                return PartialView("_ToDoTable", GetToDoes());
            }

            return View(toDo);
        }

        // GET: toDoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            toDo toDo = db.toDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            string userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.FirstOrDefault(
                x => x.Id == userId);
            if (toDo.User != user)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(toDo);
        }

        // POST: toDoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Desc,Done")] toDo toDo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toDo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(toDo);
        }

        [HttpPost]
        public ActionResult AjaxEdit(int? id, bool value)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            toDo toDo = db.toDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            else
            {
                toDo.Done = value;
                db.Entry(toDo).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("_ToDoTable", GetToDoes());
            }
        }

        // GET: toDoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            toDo toDo = db.toDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            return View(toDo);
        }

        // POST: toDoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            toDo toDo = db.toDos.Find(id);
            db.toDos.Remove(toDo);
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
