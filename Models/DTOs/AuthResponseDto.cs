namespace TaskManagementSystem.Models.DTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiry { get; set; }
        public DateTime Expiration { get; set; }

    }
}