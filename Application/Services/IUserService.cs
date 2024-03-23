using Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Services
{

    public interface IUserService : IRepository<User>
    {

        Task<string> Login(User model);

        Task<bool> Register(User model);

        bool Logout(string token,HttpContext? httpContext = null);


    }

}
