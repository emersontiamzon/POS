namespace Point.Of.Sale.Shared.Models;

public interface ICrudResult<T> where T : class
{
    public int Count { get; set; }
    public T? Entity { get; set; }
}
