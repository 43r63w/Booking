using Domain.Models;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Endpoint.ViewModels
{
    public class LoginVM
    {
        public string? Email { get; set; }

        public string? Password { get; set; }

        public bool RememberMe { get; set; }


        public string? RedirectUrl { get; set; }

    }


}
