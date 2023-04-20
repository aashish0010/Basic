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
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public IEnumerable<string> Interests { get; set; }
    }
    public class Uservalidate : CommonResponse
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? IsAdmin { get; set; }
        public string? Isactive { get; set; }

    }
    public class User
    {
        public int Userid { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
    }
    public class DashBoardWithCommon
    {
        public DashBoard? userdetail { get; set; }
        public CommonResponse? common { get; set; }
    }
    public class DashBoard
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? IsAdmin { get; set; }
        public string? Isactive { get; set; }
        public int UserCount { get; set; }
    }
    public class VerfiyForgetPassword
    {
        public string? Processid { get; set; }
        public string? Otp { get; set; }
        public string? Password { get; set; }
    }


}
