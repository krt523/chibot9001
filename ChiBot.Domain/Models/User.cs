using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChiBot.Domain.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [StringLength(30)]
        [Index(IsClustered = false, IsUnique = true)]
        public string TwitchUsername { get; set; }

        public long DiscordSnowflake { get; set; }
        public virtual List<TwitchChannel> TwitchChannels{ get; set;}
    }
}
