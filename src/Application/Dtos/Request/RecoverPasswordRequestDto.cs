namespace Application.Dtos.Request
{
    public class RecoverPasswordRequestDto
    {
        public required string Email { get;set; }
        public required string NewPassword { get;set; }
    }
}
