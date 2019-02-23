using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetChatBot.Models
{
    [Table("ibot_piu")]
    public partial class IbotPiu
    {
        [Column("id")]
        [Key]
        public uint Id { get; set; }
        [Column("text", TypeName = "varchar(255)")]
        public string Text { get; set; }
        [Column("author", TypeName = "varchar(30)")]
        public string Author { get; set; }
        [Column("date_create", TypeName = "varchar(20)")]
        public string DateCreate { get; set; }
        [Column("u", TypeName = "int(1)")]
        public int? U { get; set; }
        [Column("n", TypeName = "int(1)")]
        public int? N { get; set; }
    }
}
