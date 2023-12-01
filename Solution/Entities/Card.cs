using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Card")]
    public class Card
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column("CardNumber")]
        public int UserId { get; set; }
        public string CardNumber { get; set; }

        public int CardBalance { get; set; }

        public virtual User User { get; set; }
    }
   
}
