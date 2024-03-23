using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Passwords
{
    public static class HashPassword
    {
        public static string DoHashPassword(string password)
            => BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        public static bool CheckPassword(string inputPassword, string passwordFromDb)
          => BCrypt.Net.BCrypt.EnhancedVerify(inputPassword, passwordFromDb);


    }
}
