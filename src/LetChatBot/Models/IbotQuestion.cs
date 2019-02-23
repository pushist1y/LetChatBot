using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetChatBot.Models
{
    [Table("ibot_question")]
    public partial class IbotQuestion
    {
        [Column("id")]
        [Key]
        public uint Id { get; set; }
        [Column("q", TypeName = "varchar(300)")]
        public string Q { get; set; }
        [Column("a", TypeName = "varchar(100)")]
        public string A { get; set; }
    }
}
