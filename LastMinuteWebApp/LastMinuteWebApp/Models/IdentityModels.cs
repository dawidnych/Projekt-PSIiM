using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LastMinuteWebApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationDbContext()
            : base("name=GrouponDB")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable("ClientPrivate");
            modelBuilder.Entity<ApplicationUser>().ToTable("ClientPrivate");
            modelBuilder.Entity<IdentityUser>().ToTable("ClientPrivate").Property(p => p.Id).HasColumnName("id");
            modelBuilder.Entity<ApplicationUser>().ToTable("ClientPrivate").Property(p => p.Id).HasColumnName("id");
            modelBuilder.Entity<IdentityUser>().ToTable("ClientPrivate").Property(p => p.Email).HasColumnName("email");
            modelBuilder.Entity<ApplicationUser>().ToTable("ClientPrivate").Property(p => p.Email).HasColumnName("email");
            modelBuilder.Entity<IdentityUser>().ToTable("ClientPrivate").Property(p => p.PasswordHash).HasColumnName("haslo");
            modelBuilder.Entity<ApplicationUser>().ToTable("ClientPrivate").Property(p => p.PasswordHash).HasColumnName("haslo");


            modelBuilder.Entity<CustomUserRole>().ToTable("MyUserRoles");
            modelBuilder.Entity<CustomUserLogin>().ToTable("MyUserLogins");
            modelBuilder.Entity<CustomUserClaim>().ToTable("MyUserClaims");
            modelBuilder.Entity<CustomRole>().ToTable("MyRoles");

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            /*modelBuilder.Entity<IdentityUserRole>().ToTable("MyUserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("MyUserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("MyUserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("MyRoles");*/
        }
    }

    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}