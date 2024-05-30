using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Persistence.Trackers;
using Point.Of.Sale.Persistence.UnitOfWork;

namespace Point.Of.Sale.Persistence.Context;

public class PosDbContext : DbContext, IPosDbContext, IUnitOfWork //, IdentityDbContext<ServiceUser>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PosDbContext()
    {
    }

    public PosDbContext(DbContextOptions<PosDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // public virtual DbSet<Person> Persons { get; set; }

    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Inventory> Inventories { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Models.Sale> Sales { get; set; }
    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public virtual DbSet<Supplier> Suppliers { get; set; }
    public virtual DbSet<Tenant> Tenants { get; set; }
    public virtual DbSet<AuditLog> AuditLogs { get; set; }


    public async Task Initialize()
    {
        await Database.MigrateAsync();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();


        var changedTracker = TrackerHelpers.ChangedTracker(this);

        if (changedTracker.Any())
        {
            var auditLogs = changedTracker.ToAuditLogs(_httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? string.Empty);
            await AuditLogs.AddRangeAsync(auditLogs, cancellationToken);
        }

        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => new
            {
                e.Id, e.TenantId,
            });
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => new
            {
                e.Id, e.TenantId,
            });
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => new
            {
                e.Id, e.TenantId,
            });
        });

        // modelBuilder.Entity<Person>(entity =>
        // {
        //     entity.HasKey(e => e.Id);
        //     entity.Property(e => e.Id).ValueGeneratedOnAdd();
        //     entity.Property(e => e.UserDetails).HasColumnType("jsonb");
        //     entity.HasIndex(e => new
        //     {
        //         e.Id, e.TenantId, e.LastName,
        //     });
        // });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => new
            {
                e.Id, e.TenantId,
            });
        });

        modelBuilder.Entity<Models.Sale>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LineItems).HasColumnType("jsonb");
            entity.HasIndex(e => new
            {
                e.Id, e.TenantId,
            });
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LineItems).HasColumnType("jsonb");
            entity.HasIndex(e => new
            {
                e.Id, e.TenantId,
            });
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => new
            {
                e.Id, e.TenantId,
            });
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => new
            {
                e.Id, e.Code, e.Name,
            });
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(e => new
                {
                    e.CreatedOn, e.Id,
                })
                .IsDescending(true, false);
        });

        modelBuilder.Entity<OutBoxedEvents>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.BatchErrors).HasColumnType("jsonb");
            entity.HasIndex(e => new
            {
                e.Id, e.PublishedOn, e.Status,
            });
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to postgres with connection string from app settings during migrations
        options.UseNpgsql("User Id=postgres;Password=xqdOSyXTk69227f5;Server=db.ykoorfkswtiuzwokviis.supabase.co;Port=5432;Database=postgres");

        //
        // var builder = new ConfigurationBuilder()
        //     .SetBasePath(Directory.GetCurrentDirectory())
        //     .AddJsonFile("appsettings.json");
        // var config = builder.Build();
        // var connectionString = config.GetConnectionString("DBConnectionString");
        //
        // if (!optionsBuilder.IsConfigured)
        // {
        //     //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //     optionsBuilder.UseSqlServer(connectionString);
        // }
        //
        // var connStr = _configuration.Value.Database.BuildConnectionString() ?? string.Empty;
        //
        // switch (_configuration.Value.Database.DbProvider)
        // {
        //     case DbProvider.PostgreSql:
        //         options.UseNpgsql(_configuration.Value.Database.BuildConnectionString() ?? string.Empty);
        //         break;
        //     case DbProvider.MsSql:
        //         options.UseSqlServer(_configuration.Value.Database.BuildConnectionString() ?? string.Empty);
        //         break;
        //     case DbProvider.MySql:
        //         options.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
        //         break;
        //     case DbProvider.SqLlite:
        //         options.UseSqlite(connStr);
        //         break;
        //     default:
        //         options.UseInMemoryDatabase(connStr);
        //         break;
        // }
    }
}
