using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ProAgil.Domain.Identity
{
    public class Role : IdentityRole<int> // espera-se um tipo Tkey
    {
        public List<UserRole> UserRoles { get; set; }
    }
}