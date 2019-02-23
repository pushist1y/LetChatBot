using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetChatBot.Models
{
    [Table("phpbb_chat")]
    public partial class PhpbbChat
    {
        [Column("message_id")]
        [Key]
        public uint MessageId { get; set; }
        [Column("chat_id")]
        public uint ChatId { get; set; }
        [Column("user_id")]
        public uint UserId { get; set; }
        [Required]
        [Column("username", TypeName = "varchar(255)")]
        public string Username { get; set; }
        [Required]
        [Column("user_colour", TypeName = "varchar(6)")]
        public string UserColour { get; set; }
        [Required]
        [Column("message", TypeName = "text")]
        public string Message { get; set; }
        [Required]
        [Column("bbcode_bitfield", TypeName = "varchar(255)")]
        public string BbcodeBitfield { get; set; }
        [Required]
        [Column("bbcode_uid", TypeName = "varchar(5)")]
        public string BbcodeUid { get; set; }
        [Column("bbcode_options")]
        public byte BbcodeOptions { get; set; }
        [Column("time")]
        public uint Time { get; set; }
        [Column("processed", TypeName = "smallint(6)")]
        public short? Processed { get; set; }
        [Column("telegram_processed", TypeName = "tinyint(1)")]
        public sbyte TelegramProcessed { get; set; }
    }
}
