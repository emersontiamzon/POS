using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Point.Of.Sale.Shared.Utils;

public class InternalServerErrorObjectResult : ObjectResult
{
    public InternalServerErrorObjectResult(object value) : base(value)
    {
        StatusCode = 500;
    }

    public InternalServerErrorObjectResult() : this(null)
    {
        StatusCode = 500;
    }
}
