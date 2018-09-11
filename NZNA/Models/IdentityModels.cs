using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NZNA.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<NZNA.Areas.Admin.Models.Siteset> Sitesets { get; set; }
        public System.Data.Entity.DbSet<NZNA.Areas.Admin.Models.Album> Albums { get; set; }

        public System.Data.Entity.DbSet<NZNA.Areas.Admin.Models.Banner> Banners { get; set; }

        public System.Data.Entity.DbSet<NZNA.Areas.Admin.Models.Aboutpage> Aboutpages { get; set; }
        public System.Data.Entity.DbSet<NZNA.Areas.Admin.Models.Saugat> Saugats { get; set; }
        public System.Data.Entity.DbSet<NZNA.Areas.Admin.Models.Member> Members { get; set; }
        public System.Data.Entity.DbSet<NZNA.Areas.Admin.Models.PastExComMember> PastExComMembers { get; set; }
        public System.Data.Entity.DbSet<NZNA.Areas.Admin.Models.RelatedLink> RelatedLinks { get; set; }
        public System.Data.Entity.DbSet<NZNA.Areas.Admin.Models.Event> Events { get; set; }
        public System.Data.Entity.DbSet<NZNA.Areas.Admin.Models.News> News { get; set; }
        public System.Data.Entity.DbSet<NZNA.Areas.Admin.Models.Contact> Contacts { get; set; }
        public System.Data.Entity.DbSet<NZNA.Areas.Admin.Models.Sponsor> Sponsors { get; set; }
        public System.Data.Entity.DbSet<NZNA.Areas.Admin.Models.Gallery> Galleries { get; set; }



    }
}