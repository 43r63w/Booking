﻿using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IVillaRoomService:IRepository<VillaNumber>
    {
        Task<bool> IsExistAsync(int roomsNumber);
    }
}