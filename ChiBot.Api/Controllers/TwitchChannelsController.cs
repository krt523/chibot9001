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
    public class TwitchChannelsController : ApiController
    {
        private BotContext db = new BotContext();

        // Post: api/{controller}/{channelId}/Administrators
        [Route("api/TwitchChannel/{channelId}/Administrators")]
        [ResponseType(typeof(void))]
        [HttpPut]
        public async Task<IHttpActionResult> PutTwitchChannelAdmin(int channelId, User userFromRequest)
        {
            //get the objects we want to modify from DB.
            TwitchChannel channel = await db.TwitchChannels.FindAsync(channelId);
            User userFromDB = await db.Users.FindAsync(userFromRequest.Id);
            if(channel == null || userFromDB == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            //Add the objects pulled from the DB to each other, then tell EF to track the changes.
            channel.Users.Add(userFromDB);
            userFromDB.TwitchChannels.Add(channel);
            db.TwitchChannels.Attach(channel);
            db.Users.Attach(userFromDB);

            //Save.
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // delete: api/{controller}/{channelId}/Administrators/{userId}
        [Route("api/TwitchChannel/{channelId}/Administrators/{userId}")]
        [ResponseType(typeof(void))]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteTwitchChannelAdmin(int channelId, int userId)
        {
            TwitchChannel channel = await db.TwitchChannels.FindAsync(channelId);
            db.TwitchChannels.Attach(channel);

            int index = channel.Users.FindIndex(u => u.Id == userId);
            if(index < 0)
            {
                return NotFound();
            }


            channel.Users.RemoveAt(index);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TwitchChannelExists(userId))
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

        #region Auto-generated controller functions
        // GET: api/TwitchChannels
        public IQueryable<TwitchChannel> GetTwitchChannels()
        {
            return db.TwitchChannels;
        }

        // GET: api/TwitchChannels/5
        [ResponseType(typeof(TwitchChannel))]
        public async Task<IHttpActionResult> GetTwitchChannel(int id)
        {
            TwitchChannel twitchChannel = await db.TwitchChannels.FindAsync(id);
            if (twitchChannel == null)
            {
                return NotFound();
            }

            return Ok(twitchChannel);
        }

        // PUT: api/TwitchChannels/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTwitchChannel(int id, TwitchChannel twitchChannel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != twitchChannel.Id)
            {
                return BadRequest();
            }

            db.Entry(twitchChannel).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TwitchChannelExists(id))
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

        // POST: api/TwitchChannels
        [ResponseType(typeof(TwitchChannel))]
        public async Task<IHttpActionResult> PostTwitchChannel(TwitchChannel twitchChannel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TwitchChannels.Add(twitchChannel);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = twitchChannel.Id }, twitchChannel);
        }

        // DELETE: api/TwitchChannels/5
        [ResponseType(typeof(TwitchChannel))]
        public async Task<IHttpActionResult> DeleteTwitchChannel(int id)
        {
            TwitchChannel twitchChannel = await db.TwitchChannels.FindAsync(id);
            if (twitchChannel == null)
            {
                return NotFound();
            }

            db.TwitchChannels.Remove(twitchChannel);
            await db.SaveChangesAsync();

            return Ok(twitchChannel);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TwitchChannelExists(int id)
        {
            return db.TwitchChannels.Count(e => e.Id == id) > 0;
        }
    }
}