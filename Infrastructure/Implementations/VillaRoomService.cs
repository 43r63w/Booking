using Application.Services;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations
{
    public class VillaRoomService : Repository<VillaNumber>, IVillaRoomService
    {
        private readonly ApplicationDbContext _context;
        public VillaRoomService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsExistAsync(int roomsNumber) => await _context.VillaNumbers.AnyAsync(e => e.Villa_Number == roomsNumber);



    }
}
