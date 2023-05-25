using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// This model inherits from IdentityUser in order to add a custom collumn
// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-6.0#custom-user-data

namespace Eob_Web.Frontend.Areas.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public int UserId { get; set; }
    }
}
