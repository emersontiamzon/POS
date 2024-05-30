using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Point.Of.Sale.Persistence.Models;

namespace Point.Of.Sale.Persistence.Trackers;

public static class TrackerHelpers
{
    private static List<EntityEntry> GetChangedEntries(DbContext context)
    {
        var entries = context.ChangeTracker?.Entries()
            .Where(e => e.Entity.GetType().Name.ToLower() != "auditlog")
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();

        return entries ?? new List<EntityEntry>();
    }

    private static ChangesModel HandleAdded(EntityEntry entry)
    {
        var addedIds = entry?.Metadata.FindPrimaryKey()?.Properties?.Select(p => entry.Property(p.Name).CurrentValue)?.ToArray() ?? Array.Empty<object>();
        return new ChangesModel(entry?.Metadata?.GetTableName() ?? string.Empty, string.Join(", ", addedIds), EntityState.Added, new List<ChangesNewAndOld>());
    }

    private static ChangesModel? HandleModified(EntityEntry entry, List<ChangesModel> list)
    {
        List<ChangesModel> toLog = new();
        var modifiedIds = entry?.Metadata.FindPrimaryKey()?.Properties?.Select(p => entry.Property(p.Name).CurrentValue)?.ToArray() ?? Array.Empty<object>();

        var logs = entry?.OriginalValues.Properties
            .Where(e => entry?.Property(e.Name).IsModified ?? false)
            .Select(p => new ChangesNewAndOld
            (
                p.Name,
                entry?.GetDatabaseValues()?.GetValue<object>(p.Name).ToString() ?? string.Empty,
                entry?.OriginalValues[p]?.ToString() ?? string.Empty
            ))
            .Where(p => !Equals(p.NewValue, p.OldValue))
            .ToList();

        return logs != null && logs.Any() ? new ChangesModel(entry?.Metadata?.GetTableName() ?? string.Empty, string.Join(", ", modifiedIds), EntityState.Modified, logs) : null;
    }

    public static IEnumerable<AuditLog> ToAuditLogs(this List<ChangesModel>? changes, string user)
    {
        if (changes != null && changes.Any())
        {
            return new List<AuditLog>();
        }

        return changes!.Distinct().Select(change => new AuditLog
            {
                EntityName = change.EntityName,
                EntityId = change.EntityId,
                Action = change.EntityState switch
                {
                    EntityState.Added => "Added",
                    EntityState.Modified => "Modified",
                    _ => "Deleted",
                },
                Changes = JsonSerializer.Serialize(change.Changes, new JsonSerializerOptions {WriteIndented = true}),
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = user,
            })
            .ToList();
    }

    public static List<ChangesModel> ChangedTracker(DbContext context)
    {
        List<ChangesModel> changes = new();

        foreach (var entry in GetChangedEntries(context))
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    changes.Add(HandleAdded(entry));
                    break;
                case EntityState.Modified:
                    var modified = HandleModified(entry, changes);

                    if (modified != null)
                    {
                        changes.Add(modified);
                    }

                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    break;
            }
        }

        return changes;
    }
}
