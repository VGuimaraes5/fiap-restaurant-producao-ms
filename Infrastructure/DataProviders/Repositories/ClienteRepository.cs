﻿using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Gateways;
using Domain.ValueObjects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DataProviders.Repositories
{
    public class ClienteRepository : RepositoryBase<Cliente, Guid>, IClienteGateway
    {
        private readonly DbSet<Cliente> _clienteDbSet;

        public ClienteRepository(DBContext dbContext) : base(dbContext)
        {
            _clienteDbSet = dbContext.Set<Cliente>();
        }

        public async Task<Cliente> GetByCPFAsync(Cpf cpf)
        {
            var result = await _clienteDbSet.Where(x => x.Cpf.Equals(cpf)).FirstOrDefaultAsync();
            if (result != null)
                return result;

            return null;
        }

        public async Task<Cliente> GetByUserIdAsync(string userId)
        {
            var result = await _clienteDbSet.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (result != null)
                return result;

            return null;
        }
    }
}
