using Microsoft.EntityFrameworkCore;

namespace Point.Of.Sale.Persistence.Models;

public record ChangesModel(
    string EntityName,
    string EntityId,
    EntityState EntityState,
    List<ChangesNewAndOld> Changes
);

public record ChangesNewAndOld(string FieldName, object? OldValue, object? NewValue);
