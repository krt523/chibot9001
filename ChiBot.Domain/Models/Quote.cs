using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ChiBot.Domain.Models
{
    public class Quote
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }

        public int PersonalityId { get; set; }
        public virtual Personality Personality { get; set; }
    }
}
