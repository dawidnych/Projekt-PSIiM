using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LastMinuteWebApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class ApplicationUser : IdentityUser<int, UserLoginIntPk, UserRoleIntPk, UserClaimIntPk>
    {
        public Nullable<long> idClientBusiness { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    

    public class UserLoginIntPk : IdentityUserLogin<int>
    { }

    public class UserRoleIntPk : IdentityUserRole<int>
    { }

    public class UserClaimIntPk : IdentityUserClaim<int>
    { }

    public class RoleIntPk : IdentityRole<int, UserRoleIntPk>
    {
        public RoleIntPk() { }
        public RoleIntPk(string name) { Name = name; }
    }

    public class UserStoreIntPk : UserStore<ApplicationUser, RoleIntPk, int,
    UserLoginIntPk, UserRoleIntPk, UserClaimIntPk>
    {
        public UserStoreIntPk(ApplicationDbContext context)
          : base(context)
        {
        }
    }

    public class RoleStoreIntPk : RoleStore<RoleIntPk, int, UserRoleIntPk>
    {
        public RoleStoreIntPk(ApplicationDbContext context)
           : base(context)
        {
        }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, RoleIntPk, int, UserLoginIntPk, UserRoleIntPk, UserClaimIntPk>
    {
        public ApplicationDbContext()
           : base("MyContext")
        {
        }

        public static ApplicationDbContext Create()
        {
            var context = new ApplicationDbContext();
            context.Database.CreateIfNotExists();
            return context;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoleIntPk>()
               .Property(c => c.Name)
               .HasMaxLength(128)
               .IsRequired();
            modelBuilder.Entity<ApplicationUser>()
               .ToTable("ClientPrivate")
               .Property(c => c.UserName)
               .HasMaxLength(128)
               .IsRequired();
            modelBuilder.Entity<ApplicationUser>().ToTable("ClientPrivate").Property(p => p.Id).HasColumnName("id");
            modelBuilder.Entity<ApplicationUser>().ToTable("ClientPrivate").Property(p => p.Email).HasColumnName("email");
            modelBuilder.Entity<ApplicationUser>().ToTable("ClientPrivate").Property(p => p.PasswordHash).HasColumnName("haslo");
            modelBuilder.Entity<UserLoginIntPk>().ToTable("UsersLog");
            modelBuilder.Entity<RoleIntPk>().ToTable("Roles");
            modelBuilder.Entity<UserRoleIntPk>().ToTable("UserRoles");
    }
    }


}