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
    public class VillaRoomService : Repository<VillaNumber>, IVillaRoomService
    {
        public VillaRoomService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
