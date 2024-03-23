﻿using Application.JWT;
using Application.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infrastructure.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly JWTService _jWTService;

    
        public UnitOfWork(ApplicationDbContext context,
            JWTService jWTService
            )
        {
            _context = context;
            IVillaService = new VillaService(context);
            IVillaRoomService = new VillaRoomService(context);
            IAmenityService = new AmenityService(context);
            IUserService = new UserService(context, jWTService);
          
        }

        public IVillaService IVillaService { get; private set; }

        public IVillaRoomService IVillaRoomService { get; private set; }

        public IAmenityService IAmenityService { get; private set; }

        public IUserService IUserService { get; private set; }

        public async Task<bool> SaveAsync()
        {
            var result = await _context.SaveChangesAsync();

            if (result == 1)
            {
                return true;
            }
            return false;
        }
    }
}
