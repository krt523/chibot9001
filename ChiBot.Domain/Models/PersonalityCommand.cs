using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiBot.Domain.Models
{
    public class PersonalityCommand
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Trigger { get; set; }
        public string Response { get; set; }

        public int PersonalityId { get; set; }
        public virtual Personality Personality { get; set; }

        public PersonalityCommand()
        {

        }

        public PersonalityCommand(string trigger, string response)
        {
            Trigger = trigger;
            Response = response;
        }
    }
}
