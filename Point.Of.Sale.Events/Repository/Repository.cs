using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Persistence.Repository;

namespace Point.Of.Sale.Events.Repository;

public class Repository : GenericRepository<AuditLog>, IRepository
{
    private readonly PosDbContext _dbContext;

    public Repository(PosDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
