using Application.Services;
using Domain.Models;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations
{
    public class AmenityService : Repository<Amenity>, IAmenityService
    {
        public AmenityService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
