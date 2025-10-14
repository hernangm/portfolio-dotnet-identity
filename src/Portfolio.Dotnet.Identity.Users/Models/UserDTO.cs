namespace Portfolio.Dotnet.Identity.Users.Models
{
    public class UserDTO
    {
        public IEnumerable<ClaimDTO> Claims { get; set; } = [];
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public int Id { get; set; }
        public string? LastName { get; set; }
        public string? Name { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string SubjectId => Id.ToString();
        public string UserName { get; set; } = string.Empty;
    }
}
