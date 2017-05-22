using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LastMinuteWebApp.Models
{
   

    public class ClientBusinessAuthorizeAttribute : AuthorizeAttribute
    {

        private readonly ApplicationDbContext context = new ApplicationDbContext(); // my entity  
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            base.AuthorizeCore(httpContext);
            bool authorize = false;
            var userId = HttpContext.Current.User.Identity.GetUserId<int>();

            var user = context.Users.Where(m => m.Id == userId && m.idClientBusiness != null); // checking active users with allowed roles. 
            if (user.Any())
            {
                authorize = true; /* return true if Entity has current user(active) with specific role */
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class ApplicationUser : IdentityUser<int, UserLoginIntPk, UserRoleIntPk, UserClaimIntPk>
    {
        public long? idClientBusiness { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here => this.OrganizationId is a value stored in database against the user
            userIdentity.AddClaim(new Claim("idClientBusiness", this.idClientBusiness.ToString()));

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
            modelBuilder.Entity<ApplicationUser>().ToTable("ClientPrivate").Property(p => p.idClientBusiness).HasColumnName("idClientBusiness");
            modelBuilder.Entity<UserLoginIntPk>().ToTable("userslog");
            modelBuilder.Entity<RoleIntPk>().ToTable("roles");
            modelBuilder.Entity<UserRoleIntPk>().ToTable("userroles");
            modelBuilder.Entity<UserClaimIntPk>().ToTable("aspnetuserclaims");

            
    }
    }


}