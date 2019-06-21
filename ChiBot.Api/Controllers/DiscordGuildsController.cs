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

namespace BotApi.Controllers
{
    public class DiscordGuildsController : ApiController
    {
        private BotContext db = new BotContext();

        // GET: api/DiscordGuilds
        public IQueryable<DiscordGuild> GetDiscordGuilds()
        {
            return db.DiscordGuilds;
        }

        // GET: api/DiscordGuilds/5
        [ResponseType(typeof(DiscordGuild))]
        public async Task<IHttpActionResult> GetDiscordGuild(string id)
        {
            DiscordGuild discordGuild = await db.DiscordGuilds.FindAsync(id);
            if (discordGuild == null)
            {
                return NotFound();
            }

            return Ok(discordGuild);
        }

        // PUT: api/DiscordGuilds/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDiscordGuild(string id, DiscordGuild discordGuild)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != discordGuild.Snowflake)
            {
                return BadRequest();
            }

            db.Entry(discordGuild).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiscordGuildExists(id))
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

        // POST: api/DiscordGuilds
        [ResponseType(typeof(DiscordGuild))]
        public async Task<IHttpActionResult> PostDiscordGuild(DiscordGuild discordGuild)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DiscordGuilds.Add(discordGuild);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DiscordGuildExists(discordGuild.Snowflake))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = discordGuild.Snowflake }, discordGuild);
        }

        // DELETE: api/DiscordGuilds/5
        [ResponseType(typeof(DiscordGuild))]
        public async Task<IHttpActionResult> DeleteDiscordGuild(string id)
        {
            DiscordGuild discordGuild = await db.DiscordGuilds.FindAsync(id);
            if (discordGuild == null)
            {
                return NotFound();
            }

            db.DiscordGuilds.Remove(discordGuild);
            await db.SaveChangesAsync();

            return Ok(discordGuild);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DiscordGuildExists(string id)
        {
            return db.DiscordGuilds.Count(e => e.Snowflake == id) > 0;
        }
    }
}