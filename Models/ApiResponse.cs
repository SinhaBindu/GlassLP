namespace GlassLP.Models
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public string Reason { get; set; }
        public string Msg { get; set; }
        public T Data { get; set; }

        public ApiResponse(bool status, string reason, string msg, T data)
        {
            Status = status;
            Reason = reason;
            Msg = msg;
            Data = data;
        }
    }
}
