using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class CardCreateDto
    {
        public int UserId { get; set; }
        public string CardNumber { get; set; }

        public int CardBalance { get; set; }

        public User User { get; set; }
    }
}
