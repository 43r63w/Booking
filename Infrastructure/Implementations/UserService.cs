
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
    public class UserService : Repository<ApplicationUser>, IUserService
    {
        private readonly ApplicationDbContext _applicationDbContext;
     
        public UserService(ApplicationDbContext context

           ) : base(context)
        {
            _applicationDbContext = context;


        }













    }
}
