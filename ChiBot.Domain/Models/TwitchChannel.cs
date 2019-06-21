using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChiBot.Domain.Models
{
     public class TwitchChannel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(30)]
        [Index(IsClustered = false, IsUnique = true)]
        public string Name { get; set; }

        public int PersonalityId { get; set; }
        public virtual Personality Personality{ get; set; }
        public virtual List<User> Users { get; set; }
    }
}
