using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Models;

namespace Point.Of.Sale.Auth.IdentityContext;

public class UsersDbContext : IdentityDbContext<ServiceUser>, IUsersDbContext
{
    public UsersDbContext()
    {
    }

    // public UserDbContext(DbContextOptions<UserDbContext> options, ISaveDatabaseHelper saveDatabaseHelper)
    //     : this(options)
    // {
    //     _saveDatabaseHelper = saveDatabaseHelper;
    // }

    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ServiceUser> ServiceUsers { get; set; }

    public async Task Initialize()
    {
        await Database.MigrateAsync();
    }

    public async Task Init()
    {
        await Database.MigrateAsync();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var adminId = "02174cf0–9412–4cfe-afbf-59f706d72cf6";
        var roleId = "341743f0-asd2–42de-afbf-59kmkkmk72cf6";

        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Name = "SuperAdmin",
            NormalizedName = "SUPERADMIN",
            Id = roleId,
            ConcurrencyStamp = roleId,
        });

        var appUser = new ServiceUser
        {
            Id = adminId,
            Email = "admin@kodelev8.com",
            EmailConfirmed = true,
            FirstName = "Super",
            LastName = "Admin",
            MiddleName = string.Empty,
            UserName = "SuperAdmin",
            NormalizedUserName = "SuperAdmin",
            ApiToken = "JH+C1Fnv72VIXbmM8aS8+UXJ6ci8Bgtn5R1MeOksvdWz11qmVKNvVQrSsbYivtzBkBikwz6s3ycyY4nyf34i/Q==",
            RefreshToken = "dMQa7YJBXc0rgNQeBeeJnabu+mpChoi4NAkO+1WnhqS+A+fRESDU2svYGdWPTH+1OkpzeHeVBPw8TbJ9p/LKXg==",
            TenantId = 0,
            Active = true,
        };

        //set user password
        var ph = new PasswordHasher<ServiceUser>();
        appUser.PasswordHash = ph.HashPassword(appUser, "SUPERADMIN");

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ServiceUser>(entity => { entity.HasData(appUser); });

        //set user role to admin
        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.HasKey(e => new
            {
                e.RoleId, e.UserId,
            });
            entity.HasData(new IdentityUserRole<string>
            {
                RoleId = roleId,
                UserId = adminId,
            });
        });

        // modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        // {
        //     RoleId = roleId,
        //     UserId = adminId,
        // });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to postgres with connection string from app settings during migrations
        options.UseNpgsql("User Id=postgres;Password=xqdOSyXTk69227f5;Server=db.ykoorfkswtiuzwokviis.supabase.co;Port=5432;Database=postgres");
    }
}