namespace JiufenPackages.SceneFlow.Logic
{
    public class DataResponseModel
    {
        public bool Success;
        public string Message;
        public int Code;

        public DataResponseModel(bool success, string message, int code)
        {
            Success = success;
            Message = message;
            Code = code;
        }
    }
}