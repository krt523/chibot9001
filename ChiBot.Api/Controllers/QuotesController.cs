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
using ChiBot.Domain;
using ChiBot.Domain.Models;

namespace ChiBot.Api.Controllers
{
    public class QuotesController : ApiController
    {
        private BotContext db = new BotContext();

        // GET: api/Quotes
        public IQueryable<Quote> GetQuotes()
        {
            return db.Quotes;
        }

        // GET: api/5/Quotes?index=5
        [ResponseType(typeof(Quote))]
        public async Task<IHttpActionResult> GetPersonalityQuote(int personalityId, int index)
        {
            var list = await db.Quotes.Where(q => q.PersonalityId == personalityId).ToListAsync();
            try
            {
                return Ok(list[index - 1]);
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }
        
        }

        //GET: api/5/Quotes?random=true
        [ResponseType(typeof(Quote))]
        public async Task<IHttpActionResult> GetRandomPersonalityQuote(int personalityId, bool random)
        {
            if (!random)
            {
                return BadRequest();
            }
            var list = await db.Quotes.Where(q => q.PersonalityId == personalityId).ToListAsync();
            int randomIndex = new Random().Next(0, list.Count);
            return Ok(list[randomIndex]);
        }

        // GET: api/5/Quotes
        public IQueryable<Quote> GetPersonalityQuotes(int personalityId)
        {
            return db.Quotes.Where(q => q.PersonalityId == personalityId);
        }

        //POST: api/5/Quotes
        [ResponseType(typeof(Quote))]
        public async Task<IHttpActionResult> PostPersonalityQuote(int personalityId, Quote quote)
        {
            quote.PersonalityId = personalityId;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Quotes.Add(quote);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = quote.Id }, quote);
        }

        // GET: api/Quotes/5
        [ResponseType(typeof(Quote))]
        public async Task<IHttpActionResult> GetQuote(int id)
        {
            Quote quote = await db.Quotes.FindAsync(id);
            if (quote == null)
            {
                return NotFound();
            }

            return Ok(quote);
        }

        // PUT: api/Quotes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutQuote(int id, Quote quote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != quote.Id)
            {
                return BadRequest();
            }

            db.Entry(quote).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuoteExists(id))
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

        // POST: api/Quotes
        [ResponseType(typeof(Quote))]
        public async Task<IHttpActionResult> PostQuote(Quote quote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Quotes.Add(quote);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = quote.Id }, quote);
        }

        // DELETE: api/Quotes/5
        [ResponseType(typeof(Quote))]
        public async Task<IHttpActionResult> DeleteQuote(int id)
        {
            Quote quote = await db.Quotes.FindAsync(id);
            if (quote == null)
            {
                return NotFound();
            }

            db.Quotes.Remove(quote);
            await db.SaveChangesAsync();

            return Ok(quote);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuoteExists(int id)
        {
            return db.Quotes.Count(e => e.Id == id) > 0;
        }
    }
}