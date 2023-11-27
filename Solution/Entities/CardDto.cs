using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class CardDto
    {
        public int Id { get; set; }
        [Column("CardNumber")]
        public int UserId { get; set; }
        public string CardNumber { get; set; }

        public int CardBalance { get; set; }

        public User User { get; set; }
    }
}
