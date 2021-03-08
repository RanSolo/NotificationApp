using Microsoft.EntityFrameworkCore;
using MessageLibrary;
using MessageLibrary.Models;
namespace NotificationAppAPI.Data
{
	public class NotificationAppDbContext : DbContext
	{
		public NotificationAppDbContext(DbContextOptions <NotificationAppDbContext>options) : base(options) { }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder
				.Entity<Messenger>()
				.ToTable("messengers");
			builder
				.Entity<Message>()
				.ToTable("messages");
			builder
				.Entity<User>()
				.ToTable("users");
			builder
				.Entity<MessengerUserGroup>()
				.ToTable("messenger_user_groups");
			builder
				.Entity<MessageRecipient>()
				.ToTable("message_recipients");
		}
		public DbSet<MessageRecipient> MessageRecipients { get; set; }
		public DbSet<MessengerUserGroup> MessengerUserGroups { get; set; }
		public DbSet<Messenger> Messengers { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<User> Users { get; set; }
	}
}