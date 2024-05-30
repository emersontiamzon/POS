using System.ComponentModel;

namespace Point.Of.Sale.Shared.Enums;

public enum DomainEvents
{
    [Description("None")] None = 0,
    [Description("UserCreated")] UserCreated,
    [Description("UserUpdated")] UserUpdated,
    [Description("UserDeleted")] UserDeleted,
    [Description("UserSignedIn")] UserSignedIn,
    [Description("TenantCreated")] TenantCreated,
    [Description("TenantUpdated")] TenantUpdated,
    [Description("TenantDeleted")] TenantDeleted,
    [Description("SupplierCreated")] SupplierCreated,
    [Description("SupplierUpdated")] SupplierUpdated,
    [Description("SupplierDeleted")] SupplierDeleted,
    [Description("ShoppingCartCreated")] ShoppingCartCreated,
    [Description("ShoppingCartUpdated")] ShoppingCartUpdated,
    [Description("ShoppingCartDeleted")] ShoppingCartDeleted,
    [Description("SalesCreated")] SalesCreated,
    [Description("SalesUpdated")] SalesUpdated,
    [Description("SalesDeleted")] SalesDeleted,
    [Description("ProductCreated")] ProductCreated,
    [Description("ProductUpdated")] ProductUpdated,
    [Description("ProductDeleted")] ProductDeleted,
    [Description("PersonCreated")] PersonCreated,
    [Description("PersonUpdated")] PersonUpdated,
    [Description("PersonDeleted")] PersonDeleted,
    [Description("InventoryCreated")] InventoryCreated,
    [Description("InventoryUpdated")] InventoryUpdated,
    [Description("InventoryDeleted")] InventoryDeleted,
    [Description("CustomerCreated")] CustomerCreated,
    [Description("CustomerUpdated")] CustomerUpdated,
    [Description("CustomerDeleted")] CustomerDeleted,
    [Description("CategoryCreated")] CategoryCreated,
    [Description("CategoryUpdated")] CategoryUpdated,
    [Description("CategoryDeleted")] CategoryDeleted,
}
