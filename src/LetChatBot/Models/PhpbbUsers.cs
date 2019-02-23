using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetChatBot.Models
{
    [Table("phpbb_users")]
    public partial class PhpbbUsers
    {
        [Column("user_id", TypeName = "mediumint unsigned")]
        [Key]
        public uint UserId { get; set; }
        [Column("user_type", TypeName = "tinyint(2)")]
        public sbyte UserType { get; set; }
        [Column("group_id", TypeName = "mediumint unsigned")]
        public uint GroupId { get; set; }
        [Required]
        [Column("user_permissions", TypeName = "mediumtext")]
        public string UserPermissions { get; set; }
        [Column("user_perm_from", TypeName = "mediumint unsigned")]
        public uint UserPermFrom { get; set; }
        [Required]
        [Column("user_ip", TypeName = "varchar(40)")]
        public string UserIp { get; set; }
        [Column("user_regdate")]
        public uint UserRegdate { get; set; }
        [Required]
        [Column("username", TypeName = "varchar(255)")]
        public string Username { get; set; }
        [Required]
        [Column("username_clean", TypeName = "varchar(255)")]
        public string UsernameClean { get; set; }
        [Required]
        [Column("user_password", TypeName = "varchar(40)")]
        public string UserPassword { get; set; }
        [Column("user_passchg")]
        public uint UserPasschg { get; set; }
        [Column("user_pass_convert")]
        public byte UserPassConvert { get; set; }
        [Required]
        [Column("user_email", TypeName = "varchar(100)")]
        public string UserEmail { get; set; }
        [Column("user_email_hash", TypeName = "bigint(20)")]
        public long UserEmailHash { get; set; }
        [Required]
        [Column("user_birthday", TypeName = "varchar(10)")]
        public string UserBirthday { get; set; }
        [Column("user_lastvisit")]
        public uint UserLastvisit { get; set; }
        [Column("user_lastmark")]
        public uint UserLastmark { get; set; }
        [Column("user_lastpost_time")]
        public uint UserLastpostTime { get; set; }
        [Required]
        [Column("user_lastpage", TypeName = "varchar(200)")]
        public string UserLastpage { get; set; }
        [Required]
        [Column("user_last_confirm_key", TypeName = "varchar(10)")]
        public string UserLastConfirmKey { get; set; }
        [Column("user_last_search")]
        public uint UserLastSearch { get; set; }
        [Column("user_warnings", TypeName = "tinyint(4)")]
        public sbyte UserWarnings { get; set; }
        [Column("user_last_warning")]
        public uint UserLastWarning { get; set; }
        [Column("user_login_attempts", TypeName = "tinyint(4)")]
        public sbyte UserLoginAttempts { get; set; }
        [Column("user_inactive_reason", TypeName = "tinyint(2)")]
        public sbyte UserInactiveReason { get; set; }
        [Column("user_inactive_time")]
        public uint UserInactiveTime { get; set; }
        [Column("user_posts", TypeName = "mediumint unsigned")]
        public uint UserPosts { get; set; }
        [Required]
        [Column("user_lang", TypeName = "varchar(30)")]
        public string UserLang { get; set; }
        [Column("user_timezone", TypeName = "decimal(5,2)")]
        public decimal UserTimezone { get; set; }
        [Column("user_dst")]
        public byte UserDst { get; set; }
        [Required]
        [Column("user_dateformat", TypeName = "varchar(30)")]
        public string UserDateformat { get; set; }
        [Column("user_style", TypeName = "mediumint unsigned")]
        public uint UserStyle { get; set; }
        [Column("user_rank", TypeName = "mediumint unsigned")]
        public uint UserRank { get; set; }
        [Required]
        [Column("user_colour", TypeName = "varchar(6)")]
        public string UserColour { get; set; }
        [Column("user_new_privmsg", TypeName = "int(4)")]
        public int UserNewPrivmsg { get; set; }
        [Column("user_unread_privmsg", TypeName = "int(4)")]
        public int UserUnreadPrivmsg { get; set; }
        [Column("user_last_privmsg")]
        public uint UserLastPrivmsg { get; set; }
        [Column("user_message_rules")]
        public byte UserMessageRules { get; set; }
        [Column("user_full_folder", TypeName = "int(11)")]
        public int UserFullFolder { get; set; }
        [Column("user_emailtime")]
        public uint UserEmailtime { get; set; }
        [Column("user_topic_show_days")]
        public ushort UserTopicShowDays { get; set; }
        [Required]
        [Column("user_topic_sortby_type", TypeName = "varchar(1)")]
        public string UserTopicSortbyType { get; set; }
        [Required]
        [Column("user_topic_sortby_dir", TypeName = "varchar(1)")]
        public string UserTopicSortbyDir { get; set; }
        [Column("user_post_show_days")]
        public ushort UserPostShowDays { get; set; }
        [Required]
        [Column("user_post_sortby_type", TypeName = "varchar(1)")]
        public string UserPostSortbyType { get; set; }
        [Required]
        [Column("user_post_sortby_dir", TypeName = "varchar(1)")]
        public string UserPostSortbyDir { get; set; }
        [Column("user_notify")]
        public byte UserNotify { get; set; }
        [Column("user_notify_pm")]
        public byte UserNotifyPm { get; set; }
        [Column("user_notify_type", TypeName = "tinyint(4)")]
        public sbyte UserNotifyType { get; set; }
        [Column("user_allow_pm")]
        public byte UserAllowPm { get; set; }
        [Column("user_allow_viewonline")]
        public byte UserAllowViewonline { get; set; }
        [Column("user_allow_viewemail")]
        public byte UserAllowViewemail { get; set; }
        [Column("user_allow_massemail")]
        public byte UserAllowMassemail { get; set; }
        [Column("user_options")]
        public uint UserOptions { get; set; }
        [Required]
        [Column("user_avatar", TypeName = "varchar(255)")]
        public string UserAvatar { get; set; }
        [Column("user_avatar_type", TypeName = "tinyint(2)")]
        public sbyte UserAvatarType { get; set; }
        [Column("user_avatar_width")]
        public ushort UserAvatarWidth { get; set; }
        [Column("user_avatar_height")]
        public ushort UserAvatarHeight { get; set; }
        [Required]
        [Column("user_sig", TypeName = "mediumtext")]
        public string UserSig { get; set; }
        [Required]
        [Column("user_sig_bbcode_uid", TypeName = "varchar(8)")]
        public string UserSigBbcodeUid { get; set; }
        [Required]
        [Column("user_sig_bbcode_bitfield", TypeName = "varchar(255)")]
        public string UserSigBbcodeBitfield { get; set; }
        [Required]
        [Column("user_from", TypeName = "varchar(100)")]
        public string UserFrom { get; set; }
        [Required]
        [Column("user_icq", TypeName = "varchar(15)")]
        public string UserIcq { get; set; }
        [Required]
        [Column("user_aim", TypeName = "varchar(255)")]
        public string UserAim { get; set; }
        [Required]
        [Column("user_yim", TypeName = "varchar(255)")]
        public string UserYim { get; set; }
        [Required]
        [Column("user_msnm", TypeName = "varchar(255)")]
        public string UserMsnm { get; set; }
        [Required]
        [Column("user_jabber", TypeName = "varchar(255)")]
        public string UserJabber { get; set; }
        [Required]
        [Column("user_website", TypeName = "varchar(200)")]
        public string UserWebsite { get; set; }
        [Required]
        [Column("user_occ", TypeName = "text")]
        public string UserOcc { get; set; }
        [Required]
        [Column("user_interests", TypeName = "text")]
        public string UserInterests { get; set; }
        [Required]
        [Column("user_actkey", TypeName = "varchar(32)")]
        public string UserActkey { get; set; }
        [Required]
        [Column("user_newpasswd", TypeName = "varchar(40)")]
        public string UserNewpasswd { get; set; }
        [Required]
        [Column("user_form_salt", TypeName = "varchar(32)")]
        public string UserFormSalt { get; set; }
        [Column("user_new")]
        public byte UserNew { get; set; }
        [Column("user_reminded", TypeName = "tinyint(4)")]
        public sbyte UserReminded { get; set; }
        [Column("user_reminded_time")]
        public uint UserRemindedTime { get; set; }
        [Column("user_telegram_id", TypeName = "bigint(8)")]
        public long? UserTelegramId { get; set; }
    }
}
