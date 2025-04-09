namespace practise.DTO.Authentication
{
    public class GetUserDto
    {
        public Guid UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsBlocked { get; set; } = false;

    }
}