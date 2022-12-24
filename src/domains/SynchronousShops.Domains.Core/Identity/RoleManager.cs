using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SynchronousShops.Domains.Core.Identity.Entities;
using SynchronousShops.Libraries.EntityFramework.Repositories;
using SynchronousShops.Libraries.EntityFramework.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynchronousShops.Domains.Core.Identity
{
    public class RoleManager : IRoleManager
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IRepository<Role> _roleRepository;

        public RoleManager(
            RoleManager<Role> roleManager,
            IUnitOfWork unitOfWork
        )
        {
            _roleManager = roleManager;
            _roleRepository = unitOfWork.GetRepository<Role>();
        }

        public async Task<Role> CreateAsync(Role role)
        {
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            return await FindByNameAsync(role.Name);
        }

        public Task<Role> FindByIdAsync(Guid id)
        {
            return _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task<Role> FindByNameAsync(string name)
        {
            return _roleManager.FindByNameAsync(name);
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _roleRepository.GetAllListAsync();
        }
    }
}
