namespace BL.Models
{
    public static class Constants
    {
        public const string PasswordRegex = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";

        public const string WrongPasswordValidation = "Please enter valid password";

        public const int PageSize = 100;
    }
}
