using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChiBot.Domain.Models
{
    public class Personality
    {
        [Key]
        public int Id { get; set; }

        [StringLength(70)]
        [Index(IsClustered = false, IsUnique = true)]
        public string Name { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }
    }
}
