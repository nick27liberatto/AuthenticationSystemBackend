namespace Application.Constants.Response
{
    public class MessageError
    {
        public static string InvalidCredentials => "Invalid credentials.";
        public static string UserAlreadyExists => "User with this email already exists.";
        public static string UserNotFound => "User not found.";
        public static string InvalidResetToken => "Invalid or expired password reset token.";
        public static string PasswordsDoNotMatch => "Passwords do not match.";
        public static string WeakPassword => "The password does not meet the security requirements.";
        public static string EmailSendFailure => "Failed to send email. Please try again later.";
        public static string UnauthorizedAccess => "You do not have permission to perform this action.";
        public static string InactiveUser => "User account is inactive.";
        public static string UserRegistrationValidationFailed => "User registration validation failed.";
        public static string UserRegistrationFailed => "User registration failed.";
        public static string ResetPasswordValidationFailed => "Reset password validation failed.";
        public static string ResetPasswordFailed => "Password reset failed.";
        public static string ResetPasswordFailedUser => "An error occurred during password reset for email {Email}.";
        public static string ForgotPasswordValidationFailed => "Forgot password validation failed.";
        public static string ForgotPasswordFailed => "Forgot password process failed.";
        public static string ForgotPasswordFailedUser => "An error occurred during forgot password for email {Email}.";
        public static string EmailAlreadyRegistered => "Email is already registered.";
        public static string ErrorProcessingRequest => "An error occurred while processing your request.";
        public static string ErrorProcessingRequestUser => "An error occurred while processing request for user {Email}.";
        public static string SocialLoginFailed => "Social login failed.";
        public static string SocialLoginFailedUser => "An error occurred during social login for provider {Provider}.";
        public static string SocialRegistrationFailed => "Social registration failed.";
        public static string SocialEmailNotRetrieved => "Could not retrieve email from social provider.";
        public static string InvalidSocialToken => "Invalid social login token.";
        public static string UnsuportedSocialProvider => "Unsupported social login provider.";
        public static string ErrorGeneratingToken => "An error occurred while generating authentication token.";
        public static string UserLoggingValidationFailed => "User login validation failed.";
        public static string ErrorLoggingIn => "An error occurred while logging in.";
        public static string ErrorLoggingUser => "An error occurred while logging in for user {Email}.";
    }
}
