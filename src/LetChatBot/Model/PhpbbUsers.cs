using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbUsers
    {
        public int UserId { get; set; }
        public sbyte UserType { get; set; }
        public int GroupId { get; set; }
        public string UserPermissions { get; set; }
        public int UserPermFrom { get; set; }
        public string UserIp { get; set; }
        public int UserRegdate { get; set; }
        public string Username { get; set; }
        public string UsernameClean { get; set; }
        public string UserPassword { get; set; }
        public int UserPasschg { get; set; }
        public sbyte UserPassConvert { get; set; }
        public string UserEmail { get; set; }
        public long UserEmailHash { get; set; }
        public string UserBirthday { get; set; }
        public int UserLastvisit { get; set; }
        public int UserLastmark { get; set; }
        public int UserLastpostTime { get; set; }
        public string UserLastpage { get; set; }
        public string UserLastConfirmKey { get; set; }
        public int UserLastSearch { get; set; }
        public sbyte UserWarnings { get; set; }
        public int UserLastWarning { get; set; }
        public sbyte UserLoginAttempts { get; set; }
        public sbyte UserInactiveReason { get; set; }
        public int UserInactiveTime { get; set; }
        public int UserPosts { get; set; }
        public string UserLang { get; set; }
        public decimal UserTimezone { get; set; }
        public sbyte UserDst { get; set; }
        public string UserDateformat { get; set; }
        public int UserStyle { get; set; }
        public int UserRank { get; set; }
        public string UserColour { get; set; }
        public int UserNewPrivmsg { get; set; }
        public int UserUnreadPrivmsg { get; set; }
        public int UserLastPrivmsg { get; set; }
        public sbyte UserMessageRules { get; set; }
        public int UserFullFolder { get; set; }
        public int UserEmailtime { get; set; }
        public short UserTopicShowDays { get; set; }
        public string UserTopicSortbyType { get; set; }
        public string UserTopicSortbyDir { get; set; }
        public short UserPostShowDays { get; set; }
        public string UserPostSortbyType { get; set; }
        public string UserPostSortbyDir { get; set; }
        public sbyte UserNotify { get; set; }
        public sbyte UserNotifyPm { get; set; }
        public sbyte UserNotifyType { get; set; }
        public sbyte UserAllowPm { get; set; }
        public sbyte UserAllowViewonline { get; set; }
        public sbyte UserAllowViewemail { get; set; }
        public sbyte UserAllowMassemail { get; set; }
        public int UserOptions { get; set; }
        public string UserAvatar { get; set; }
        public sbyte UserAvatarType { get; set; }
        public short UserAvatarWidth { get; set; }
        public short UserAvatarHeight { get; set; }
        public string UserSig { get; set; }
        public string UserSigBbcodeUid { get; set; }
        public string UserSigBbcodeBitfield { get; set; }
        public string UserFrom { get; set; }
        public string UserIcq { get; set; }
        public string UserAim { get; set; }
        public string UserYim { get; set; }
        public string UserMsnm { get; set; }
        public string UserJabber { get; set; }
        public string UserWebsite { get; set; }
        public string UserOcc { get; set; }
        public string UserInterests { get; set; }
        public string UserActkey { get; set; }
        public string UserNewpasswd { get; set; }
        public string UserFormSalt { get; set; }
        public sbyte UserNew { get; set; }
        public sbyte UserReminded { get; set; }
        public int UserRemindedTime { get; set; }
        public long? UserTelegramId { get; set; }
    }
}
