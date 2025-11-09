namespace Application.Constants
{
    public class ResetPasswordEmail
    {
        public static string Title => "Reset Password";
        public static string Body =>
            "<p>Hello {userName},</p><p>To reset your password, click in the link below:</p><p><a href='{resetLink}'>Reset Password</a></p>";
    }
}
