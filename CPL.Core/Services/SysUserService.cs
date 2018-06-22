using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class SysUserService : CoreBase<SysUser>, ISysUserService
    {
        private readonly IRepositoryAsync<SysUser> _repository;

        public SysUserService(IRepositoryAsync<SysUser> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
