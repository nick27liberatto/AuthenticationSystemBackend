namespace Application.Dtos.Request
{
    public class UpdatePasswordRequestDto
    {
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
