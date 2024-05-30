namespace Point.Of.Sale.Shared.Configuration;

public static class DbHelper
{
    public static string BuildConnectionString(this Database database)
    {
        return database.DbProvider switch
        {
            DbProvider.MsSql => ToMsSqlConnectionString(database),
            DbProvider.PostgreSql => ToPostgresConnectionString(database),
            DbProvider.MySql => ToMySqlConnectionString(database),
            _ => ToSqlLiteConnectionString(database),
        };
    }

    private static string ToMsSqlConnectionString(Database database)
    {
        return $"Server={database.Server};Database={database.Name};User Id={database.User};Password={database.Password};";
    }

    private static string ToPostgresConnectionString(Database database)
    {
        return $"Server={database.Server};Port={database.Port};Database={database.Name};User Id={database.User};Password={database.Password};";
    }

    private static string ToSqlLiteConnectionString(Database database)
    {
        return $"Data Source={database.Name}.db";
    }

    private static string ToMySqlConnectionString(Database database)
    {
        return $"Server={database.Server};Database={database.Name};Uid={database.User};Pwd={database.Password};";
    }

    private static string ToInmeoryConnectionString(Database database)
    {
        return $"Data Source={database.Name}";
    }
}
