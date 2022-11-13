namespace Basic.Domain.Entity
{
    public class LoginRequest
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
    public class LoginResponse
    {
        public string? Username { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
    public class Register
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        //public string? Flag { get; set; }
        //public string? Role { get; set; }
        //public string? IsAdmin { get; set; }
    }
    public class Uservalidate : CommonResponse
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? IsAdmin { get; set; }
        public string? Isactive { get; set; }

    }


}
