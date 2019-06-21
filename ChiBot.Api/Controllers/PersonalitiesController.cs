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
    public class PersonalitiesController : ApiController
    {
        private BotContext db = new BotContext();

        // GET: api/Personalities
        public IQueryable<Personality> GetPersonalities()
        {
            return db.Personalities;
        }

        // GET: api/Personalities/5
        [ResponseType(typeof(Personality))]
        public async Task<IHttpActionResult> GetPersonality(int id)
        {
            Personality personality = await db.Personalities.FindAsync(id);
            if (personality == null)
            {
                return NotFound();
            }

            return Ok(personality);
        }

        // PUT: api/Personalities/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPersonality(int id, Personality personality)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != personality.Id)
            {
                return BadRequest();
            }

            db.Entry(personality).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonalityExists(id))
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

        // POST: api/Personalities
        [ResponseType(typeof(Personality))]
        public async Task<IHttpActionResult> PostPersonality(Personality personality)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Personalities.Add(personality);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = personality.Id }, personality);
        }

        // DELETE: api/Personalities/5
        [ResponseType(typeof(Personality))]
        public async Task<IHttpActionResult> DeletePersonality(int id)
        {
            Personality personality = await db.Personalities.FindAsync(id);
            if (personality == null)
            {
                return NotFound();
            }

            db.Personalities.Remove(personality);
            await db.SaveChangesAsync();

            return Ok(personality);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonalityExists(int id)
        {
            return db.Personalities.Count(e => e.Id == id) > 0;
        }
    }
}