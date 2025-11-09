namespace Application.Constants.Validation
{
    public class ValidationRules
    {
        public static string DtoRequired = "Data transfer object is required.";
        public static string NameRequired = "Name is required.";
        public static string MaxNameLengthExceeded = "Maximum number of characters exceeded.";
        public static string EmailRequired = "Email is required.";
        public static string EmailValidRequired = "A valid email is required.";
        public static string TokenRequired = "Token is required.";
        public static string PasswordRequired = "Password";
        public static string PasswordMinLengthRequired = "Password must be longer than 8 characters.";
        public static string PasswordUppercaseRequired = "Password must contain at least one uppercase letter.";
        public static string PasswordLowercaseRequired = "Password must contain at least one lowercase letter.";
        public static string PasswordNumberRequired = "Password must contain at least one number.";
        public static string PasswordSpecialCharacterRequired = "Password must contain at least one special character.";
    }
}
