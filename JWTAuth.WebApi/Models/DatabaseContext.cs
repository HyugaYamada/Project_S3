using khiemnguyen.WebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace JWTAuth.WebApi.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

  
        public virtual DbSet<UserInfo>? UserInfos { get; set; }
        public virtual DbSet<Menu>? Menues { get; set; }
        public virtual DbSet<Order>? Orders { get; set; }
        public virtual DbSet<Food>? Foods { get; set; }
        public virtual DbSet<Food_in_Menu>? Food_In_Menus { get; set; }
        public virtual DbSet<FeedBack>? FeedBacks { get; set; }
        public virtual DbSet<Tag_Include>? Tag_Includes { get; set; }
        public virtual DbSet<Menu_Tag>? Menu_Tags { get; set; }
        public virtual DbSet<Message>? Messages { get; set; }
        public virtual DbSet<Favor_Cater>? Favor_Caters { get; set; }
        public virtual DbSet<Category>? Categories { get; set; }
	
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>(entity =>
            {
              

                entity.ToTable("UserInfo");

                entity.Property(e => e.UserId).HasColumnName("UserId");

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .IsUnicode(false);
            })

    ;

			modelBuilder.Entity<UserInfo>().HasData(
	  new UserInfo { UserId = 1, UserName = "API", FullName = "API", Email = "abc@abc.com", Password = "abc", Role = "Admin", DisplayName = "abc", CreatedDate = DateTime.Now },
  new UserInfo { UserId = 2, UserName = "Allen", FullName = "Allen", Email = "user@abc.com", Password = "abc", Role = "Customer", DisplayName = "Allen", CreatedDate = DateTime.Now },
  new UserInfo { UserId = 3, UserName = "Taro", FullName = "Taro", Email = "admin@abc.com", Password = "abc", Role = "Admin", DisplayName = "Taro", CreatedDate = DateTime.Now },
   new UserInfo { UserId = 4, UserName = "Jery", FullName = "Jery", Email = "caterer@abc.com", Password = "abc", Role = "Caterer", DisplayName = "Jery", CreatedDate = DateTime.Now }

  );

			modelBuilder.Entity<Category>().HasData(
new Category { id = 1, Name = "Appetizer" },
new Category {id=2,Name= "Starter" },
new Category {id=3,Name= "Main course" },
new Category { id = 4, Name = "Drink" },
new Category { id=5,Name= "Desert" }

);



			modelBuilder.Entity<Food>().HasData(
new Food {id=1,Name= "Food appertise",Category= "Appetizer",Description= "Food appertise",Caterid=4 },
new Food {id=2, Name = "Food Starter", Category = "Starter", Description = "Food Starter", Caterid = 4 },
new Food {id=3, Name = "Food Main course", Category = "Main course", Description = "Food Main course", Caterid = 4 },
new Food {id=4, Name = "Food Drink", Category = "Drink", Description = "Food Drink", Caterid = 4 },
new Food {id=5, Name = "Food Desert", Category = "Desert", Description = "Food Desert", Caterid = 4 }

);



			OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
