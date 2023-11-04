using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Iserv.Models;

namespace Iserv.Controllers
{
    public class MessagesController : Controller
    {
        private InternetServicesEntities db = new InternetServicesEntities();

        // GET: Messages
        public async Task<ActionResult> Index()
        {
            var messages = db.Messages;
            await messages.ToListAsync();

            return View(messages);
        }

        // GET: Messages/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = await db.Messages.FindAsync(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }


        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "DateSent,TimeSent,MessageBody,MesseageLocation,SentFrom,MessageSentTo")] Message message, string MessageBody, string requestby, string forservice)
        {
            if (ModelState.IsValid)
            {
                if (message.MessageSentTo != null && requestby != null && forservice != null)
                {
                    message.MessageBody = "Service Request Removal from |" + requestby + "|-" + "-|" + forservice + "|";
                    db.Messages.Add(message);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    //try find user 
                    User finduser = db.Users.Where(x => x.UserName == message.MessageSentTo).FirstOrDefault();
                    message.MessageSentTo = finduser.UserName;

                    db.Messages.Add(message);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

            }

            return View(message);
        }

        // GET: Messages/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = await db.Messages.FindAsync(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MessageID,DateSent,TimeSent,MessageBody,SendTo,SentFrom,UserID,SentTo")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = await db.Messages.FindAsync(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Message message = await db.Messages.FindAsync(id);
            db.Messages.Remove(message);
            await db.SaveChangesAsync();
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
