using Application.JWT;
using Application.Passwords;
using Application.Services;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations
{
    public class UserService : Repository<User>, IUserService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly JWTService _jWTService;

       
        public UserService(ApplicationDbContext context,
            JWTService jWTService
           ) : base(context)
        {
            _applicationDbContext = context;
            _jWTService = jWTService;
            
        }

        public async Task<string> Login(User model)
        {
            var userFromDb = await _applicationDbContext.Users.FirstOrDefaultAsync(e => e.Email == model.Email);

            if (userFromDb != null)
            {
                return _jWTService.GenerateToken(userFromDb);
            }

            return "User isn`t find";

        }

        public bool Logout(string token,HttpContext httpContext)
        {
            
            if(token == null)
            {
                throw new Exception("Token is null");
            }

            httpContext.Session.Remove("Token");

            if (httpContext.Request.Cookies["Token"] == null)
            {
                return true;
            }

            return false;


        }

        public async Task<bool> Register(User model)
        {
            bool isExist = await EmailExist(model.Email);

            if (isExist)
            {
                return false;
            }

            var newUser = new User
            {
                Email = model.Email,
                LastName = model.LastName,
                Name = model.Name,
                SurName = model.SurName,
                PhoneNumber = model.PhoneNumber,
                Password = HashPassword.DoHashPassword(model.Password),
                Role = model.Role,
            };



            _applicationDbContext.Users.Add(newUser);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }










        #region      --- Helper Method ---
        private async Task<bool> EmailExist(string email)
        {
            return await _applicationDbContext.Users.AnyAsync(e => e.Email == email);
        }
        #endregion
    }
}
