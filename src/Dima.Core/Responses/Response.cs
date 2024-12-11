using System.Text.Json.Serialization;

namespace Dima.Core.Responses;

public class Response<TData>
{
    private readonly int _statusCode;

    [JsonConstructor]
    public Response()
        =>_statusCode = Configuration.DefaultStatusCode;

    public Response(TData? data, int statusCode = Configuration.DefaultStatusCode, string? message = null)
    {
        Data = data;
        Message = message;
        _statusCode = statusCode;
    }
    public TData? Data { get; set; }
    public string? Message { get; set; }
    
    [JsonIgnore]
    public bool IsSuccess => _statusCode is >= 200 and  <= 299;
}