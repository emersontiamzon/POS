namespace Point.Of.Sale.Shared.Models;

public class CrudResult<T> : ICrudResult<T> where T : class
{
    public int Count { get; set; }
    public T? Entity { get; set; }
}
