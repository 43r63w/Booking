﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IUnitOfWork
    {

        public IVillaService IVillaService { get; }
        
        public IVillaRoomService IVillaRoomService { get; }


        public IAmenityService IAmenityService { get; }

        Task<bool> SaveAsync();
       
    }
}
