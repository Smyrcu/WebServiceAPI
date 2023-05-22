namespace WebServiceAPI.Model.ResponseModels
{
    public class UserResponse
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public List<string> ErrorMessages { get; set; }

    }
}
