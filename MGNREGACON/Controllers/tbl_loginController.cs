using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MLAFund.Models.Data;

namespace MLAFund.Controllers
{
    public class tbl_loginController : ApiController
    {
        private RuralWorksData db = new RuralWorksData();

        // GET: api/tbl_login
        public IQueryable<tbl_Users> Gettbl_Users()
        {
            return db.tbl_Users;
        }

        // GET: api/tbl_login/5
        [ResponseType(typeof(tbl_Users))]
        public async Task<IHttpActionResult> Gettbl_Users(int id)
        {
            tbl_Users tbl_Users = await db.tbl_Users.FindAsync(id);
            if (tbl_Users == null)
            {
                return NotFound();
            }

            return Ok(tbl_Users);
        }

        // PUT: api/tbl_login/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puttbl_Users(int id, tbl_Users tbl_Users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_Users.id)
            {
                return BadRequest();
            }

            db.Entry(tbl_Users).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_UsersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/tbl_login
        [ResponseType(typeof(tbl_Users))]
        public async Task<IHttpActionResult> Posttbl_Users(tbl_Users tbl_Users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_Users.Add(tbl_Users);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tbl_Users.id }, tbl_Users);
        }

        // DELETE: api/tbl_login/5
        [ResponseType(typeof(tbl_Users))]
        public async Task<IHttpActionResult> Deletetbl_Users(int id)
        {
            tbl_Users tbl_Users = await db.tbl_Users.FindAsync(id);
            if (tbl_Users == null)
            {
                return NotFound();
            }

            db.tbl_Users.Remove(tbl_Users);
            await db.SaveChangesAsync();

            return Ok(tbl_Users);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_UsersExists(int id)
        {
            return db.tbl_Users.Count(e => e.id == id) > 0;
        }
    }
}