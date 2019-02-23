using LetChatBot.Models;
using Microsoft.EntityFrameworkCore;

namespace LetChatBot.Data
{
    public partial class ForumContext : DbContext
    {
        public ForumContext()
        {
        }

        public ForumContext(DbContextOptions<ForumContext> options)
            : base(options)
        {
        }

        public virtual DbSet<IbotPiu> IbotPiu { get; set; }
        public virtual DbSet<IbotQuestion> IbotQuestion { get; set; }
        public virtual DbSet<PhpbbUsers> PhpbbUsers { get; set; }
        public virtual DbSet<PhpbbChat> PhpbbChat { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhpbbUsers>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.UserBirthday)
                    .HasName("user_birthday");

                entity.HasIndex(e => e.UserEmailHash)
                    .HasName("user_email_hash");

                entity.HasIndex(e => e.UserTelegramId)
                    .HasName("user_telegram_index");

                entity.HasIndex(e => e.UserType)
                    .HasName("user_type");

                entity.HasIndex(e => e.UsernameClean)
                    .HasName("username_clean")
                    .IsUnique();

                entity.Property(e => e.GroupId).HasDefaultValueSql("'3'");

                entity.Property(e => e.UserActkey).HasDefaultValueSql("''");

                entity.Property(e => e.UserAim).HasDefaultValueSql("''");

                entity.Property(e => e.UserAllowMassemail).HasDefaultValueSql("'1'");

                entity.Property(e => e.UserAllowPm).HasDefaultValueSql("'1'");

                entity.Property(e => e.UserAllowViewemail).HasDefaultValueSql("'1'");

                entity.Property(e => e.UserAllowViewonline).HasDefaultValueSql("'1'");

                entity.Property(e => e.UserAvatar).HasDefaultValueSql("''");

                entity.Property(e => e.UserAvatarHeight).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserAvatarType).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserAvatarWidth).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserBirthday).HasDefaultValueSql("''");

                entity.Property(e => e.UserColour).HasDefaultValueSql("''");

                entity.Property(e => e.UserDateformat).HasDefaultValueSql("'d M Y H:i'");

                entity.Property(e => e.UserDst).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserEmail).HasDefaultValueSql("''");

                entity.Property(e => e.UserEmailHash).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserEmailtime).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserFormSalt).HasDefaultValueSql("''");

                entity.Property(e => e.UserFrom).HasDefaultValueSql("''");

                entity.Property(e => e.UserFullFolder).HasDefaultValueSql("'-3'");

                entity.Property(e => e.UserIcq).HasDefaultValueSql("''");

                entity.Property(e => e.UserInactiveReason).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserInactiveTime).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserIp).HasDefaultValueSql("''");

                entity.Property(e => e.UserJabber).HasDefaultValueSql("''");

                entity.Property(e => e.UserLang).HasDefaultValueSql("''");

                entity.Property(e => e.UserLastConfirmKey).HasDefaultValueSql("''");

                entity.Property(e => e.UserLastPrivmsg).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserLastSearch).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserLastWarning).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserLastmark).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserLastpage).HasDefaultValueSql("''");

                entity.Property(e => e.UserLastpostTime).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserLastvisit).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserLoginAttempts).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserMessageRules).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserMsnm).HasDefaultValueSql("''");

                entity.Property(e => e.UserNew).HasDefaultValueSql("'1'");

                entity.Property(e => e.UserNewPrivmsg).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserNewpasswd).HasDefaultValueSql("''");

                entity.Property(e => e.UserNotify).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserNotifyPm).HasDefaultValueSql("'1'");

                entity.Property(e => e.UserNotifyType).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserOptions).HasDefaultValueSql("'230271'");

                entity.Property(e => e.UserPassConvert).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserPasschg).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserPassword).HasDefaultValueSql("''");

                entity.Property(e => e.UserPermFrom).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserPostShowDays).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserPostSortbyDir).HasDefaultValueSql("'a'");

                entity.Property(e => e.UserPostSortbyType).HasDefaultValueSql("'t'");

                entity.Property(e => e.UserPosts).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserRank).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserRegdate).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserReminded).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserRemindedTime).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserSigBbcodeBitfield).HasDefaultValueSql("''");

                entity.Property(e => e.UserSigBbcodeUid).HasDefaultValueSql("''");

                entity.Property(e => e.UserStyle).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserTimezone).HasDefaultValueSql("'0.00'");

                entity.Property(e => e.UserTopicShowDays).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserTopicSortbyDir).HasDefaultValueSql("'d'");

                entity.Property(e => e.UserTopicSortbyType).HasDefaultValueSql("'t'");

                entity.Property(e => e.UserType).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserUnreadPrivmsg).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserWarnings).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserWebsite).HasDefaultValueSql("''");

                entity.Property(e => e.UserYim).HasDefaultValueSql("''");

                entity.Property(e => e.Username).HasDefaultValueSql("''");

                entity.Property(e => e.UsernameClean).HasDefaultValueSql("''");
            });
        }
    }
}
