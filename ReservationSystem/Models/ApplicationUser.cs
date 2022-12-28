using Microsoft.AspNetCore.Identity;

namespace ReservationSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public class Name {
            public Name()
            {
                
            }
            public string FullName (ApplicationUser au)
            {
                string fullName = au.FirstName+ " " + au.LastName;
                return fullName;
            }
        }
    }
}
