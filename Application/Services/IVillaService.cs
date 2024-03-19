using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IVillaService :IRepository<Villa>
    {
        Task<bool> UpdateWithParameterAsync(Villa model);
    }
}
