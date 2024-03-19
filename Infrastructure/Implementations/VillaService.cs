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
    public class VillaService : Repository<Villa>, IVillaService
    {
        private readonly ApplicationDbContext _context;
        public VillaService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> UpdateWithParameterAsync(Villa model)
        {
            var result = await _context.Villas.ExecuteUpdateAsync(
                   i => i.SetProperty(e => e.Name, model.Name)
                   .SetProperty(e => e.Description, model.Description)
                   .SetProperty(e => e.Sqft, model.Sqft)
                   .SetProperty(e => e.Price, model.Price)
                   .SetProperty(e => e.Occupancy, model.Occupancy)
                   );
            if (result >= 1) return true;
            return false;
        }
    }
}
