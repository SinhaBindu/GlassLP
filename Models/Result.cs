using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace GlassLP.Models
{
    public class Result
    {
        public bool IsSuccess { get; private set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? ValidationErrors { get; private set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Message { get; private set; }

        public object? Data { get; private set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Exception? Exception { get; private set; }

        public static Result Success(string message) => new() { IsSuccess = true, Message = message };
        public static Result Success(object data, string message) => new() { IsSuccess = true, Data = data, Message = message };
        public static Result Success(object data) => new() { IsSuccess = true, Data = data };
        public static Result Failure(string message) => new() { IsSuccess = false, Message = message };
        public static Result Failure(string message, Exception exception) => new() { IsSuccess = false, Message = message, Exception = exception };
        public static Result ValidationFailure(List<string> errors) => new() { IsSuccess = false, ValidationErrors = errors };
    }

    public class Result<T>
    {
        public bool IsSuccess { get; private set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T? Data { get; private set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Message { get; private set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? ValidationErrors { get; private set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalCount { get; private set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Page { get; private set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PageSize { get; private set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Exception? Exception { get; private set; }

        // Factory methods remain the same
        public static Result<T> Success(T data) => new() { IsSuccess = true, Data = data };
        public static Result<T> Success(T data, string? message = null) => new() { IsSuccess = true, Data = data, Message = message };
        public static Result<T> Success(T data, int totalCount, int page, int pageSize, string? message = null) => new()
        {
            IsSuccess = true,
            Data = data,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            Message = message
        };
        public static Result<T> Failure(string error) => new() { IsSuccess = false, Message = error };
        public static Result<T> Failure(string message, Exception exception) => new() { IsSuccess = false, Message = message, Exception = exception };
        public static Result<T> ValidationFailure(List<string> errors) => new() { IsSuccess = false, ValidationErrors = errors };
        public static Result<T> ValidationFailure(ModelStateDictionary modelState) => new() { IsSuccess = false, ValidationErrors = modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() };
    }
}
