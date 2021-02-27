using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Com.Trakindo.TSICS.Data.Model;
using Com.Trakindo.TSICS.Data.Context;
// ReSharper disable IdentifierTypo

namespace TSICS.Controllers.MobileApi
{
    public class MobileTicketCategoriesController : ApiController
    {
        private readonly TsicsContext _db = new TsicsContext();

        // GET: api/MobileTicketCategories
        public IQueryable<TicketCategory> GetTicketCategory()
        {
            return _db.TicketCategory;
        }

        // GET: api/MobileTicketCategories/5
        [ResponseType(typeof(TicketCategory))]
        public IHttpActionResult GetTicketCategory(int id)
        {
            TicketCategory ticketCategory = _db.TicketCategory.Find(id);
            if (ticketCategory == null)
            {
                return NotFound();
            }

            return Ok(ticketCategory);
        }


        // PUT: api/MobileTicketCategories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTicketCategory(int id, TicketCategory ticketCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticketCategory.TicketCategoryId)
            {
                return BadRequest();
            }

            _db.Entry(ticketCategory).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketCategoryExists(id))
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

        // POST: api/MobileTicketCategories
        [ResponseType(typeof(TicketCategory))]
        public IHttpActionResult PostTicketCategory(TicketCategory ticketCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.TicketCategory.Add(ticketCategory);
            _db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ticketCategory.TicketCategoryId }, ticketCategory);
        }

        // DELETE: api/MobileTicketCategories/5
        [ResponseType(typeof(TicketCategory))]
        public IHttpActionResult DeleteTicketCategory(int id)
        {
            TicketCategory ticketCategory = _db.TicketCategory.Find(id);
            if (ticketCategory == null)
            {
                return NotFound();
            }

            _db.TicketCategory.Remove(ticketCategory);
            _db.SaveChanges();

            return Ok(ticketCategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TicketCategoryExists(int id)
        {
            return _db.TicketCategory.Count(e => e.TicketCategoryId == id) > 0;
        }
    }
}