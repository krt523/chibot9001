using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChiBot.Domain.Models
{
    public class DiscordGuild
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Snowflake { get; set; }

        public int PersonalityId { get; set; }
        public virtual Personality Personality { get; set; }
    }
}
