namespace DLMS_MODELS.Bases
{
    public class ResponseBase<T>
    {
        public ResponseBase()
        {
            IsSuccess = true;
            Message = "";
        }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }


    }
}
