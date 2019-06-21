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
    public class PersonalityCommandsController : ApiController
    {
        private BotContext db = new BotContext();
        
        // GET: api/5/PersonalityCommands
        public IQueryable<PersonalityCommand> GetPersonalityCommandsForPersonalty(int personalityId)
        {
            return db.PersonalityCommands.Where(c => c.PersonalityId == personalityId);
        }

        //GET: api/5/PersonalityCommands/5
        [ResponseType(typeof(PersonalityCommand))]
        public async Task<IHttpActionResult> GetPersonalityCommand(int personalityId, string id)
        {
            var result = await db.PersonalityCommands.Where(c => c.PersonalityId == personalityId && c.Trigger == id.ToLower()).FirstOrDefaultAsync();

            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        //POST: api/5/PersonalityCommands/
        [ResponseType(typeof(PersonalityCommand))]
        public async Task<IHttpActionResult> PostPersonalityCommand(int personalityId, PersonalityCommand personalityCommand)
        {

            personalityCommand.PersonalityId = personalityId;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //We want all command triggers to be in lower case
            personalityCommand.Trigger = personalityCommand.Trigger.ToLower();

            //Check if command exists; if it does return 409
            var result = await db.PersonalityCommands
                .Where(c => c.PersonalityId == personalityId && c.Trigger == personalityCommand.Trigger)
                .FirstOrDefaultAsync();
            if (result != null)
            {
                return Conflict();
            }

            db.PersonalityCommands.Add(personalityCommand);
            await db.SaveChangesAsync();
            return CreatedAtRoute("Personality", new { personalityId = personalityCommand.PersonalityId, controller = "Personalities", id = personalityCommand.Trigger }, personalityCommand);
        }

        // PUT: api/5/PersonalityCommands/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPersonalityCommand(int personalityId, string id, [FromBody]string response)
        {
            
            //Check if command exists; if it doesn't return 404
            var result = await db.PersonalityCommands
                .Where(c => c.PersonalityId == personalityId && c.Trigger == id)
                .FirstOrDefaultAsync();
            if(result == null)
            {
                return NotFound();
            }

            result.Response = id.ToLower();
            db.Entry(result).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/5/PersonalityCommands/5
        [ResponseType(typeof(PersonalityCommand))]
        public async Task<IHttpActionResult> DeletePersonalityCommand(int personalityId, string id)
        {
            PersonalityCommand personalityCommand = await db.PersonalityCommands
                .Where(c => c.PersonalityId == personalityId && c.Trigger == id.ToLower())
                .FirstOrDefaultAsync();

            if (personalityCommand == null)
            {
                return NotFound();
            }

            db.PersonalityCommands.Remove(personalityCommand);
            await db.SaveChangesAsync();

            return Ok(personalityCommand);
        }



        //MVC/Entity framework scaffolding
        // GET: api/PersonalityCommands
        public IQueryable<PersonalityCommand> GetPersonalityCommands()
        {
            return db.PersonalityCommands;
        }

        // GET: api/PersonalityCommands/5
        [ResponseType(typeof(PersonalityCommand))]
        public async Task<IHttpActionResult> GetPersonalityCommand(int id)
        {
            PersonalityCommand personalityCommand = await db.PersonalityCommands.FindAsync(id);
            if (personalityCommand == null)
            {
                return NotFound();
            }

            return Ok(personalityCommand);
        }

        // PUT: api/PersonalityCommands/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPersonalityCommand(int id, PersonalityCommand personalityCommand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != personalityCommand.Id)
            {
                return BadRequest();
            }

            db.Entry(personalityCommand).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonalityCommandExists(id))
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

        // POST: api/PersonalityCommands
        [ResponseType(typeof(PersonalityCommand))]
        public async Task<IHttpActionResult> PostPersonalityCommand(PersonalityCommand personalityCommand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PersonalityCommands.Add(personalityCommand);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = personalityCommand.Id }, personalityCommand);
        }

        // DELETE: api/PersonalityCommands/5
        [ResponseType(typeof(PersonalityCommand))]
        public async Task<IHttpActionResult> DeletePersonalityCommand(int id)
        {
            PersonalityCommand personalityCommand = await db.PersonalityCommands.FindAsync(id);
            if (personalityCommand == null)
            {
                return NotFound();
            }

            db.PersonalityCommands.Remove(personalityCommand);
            await db.SaveChangesAsync();

            return Ok(personalityCommand);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonalityCommandExists(int id)
        {
            return db.PersonalityCommands.Count(e => e.Id == id) > 0;
        }
    }
}