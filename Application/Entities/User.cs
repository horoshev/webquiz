using Microsoft.AspNetCore.Identity;

namespace Application.Entities
{
    /// <summary>
    /// User in application.
    /// </summary>
    public class User: IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}