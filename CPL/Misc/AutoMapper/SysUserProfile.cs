using AutoMapper;
using CPL.Domain;
using CPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.MapperCreate
{
    public class SysUserProfile : Profile
    {
        public SysUserProfile()
        {
            CreateMap<SysUser, SysUserViewModel>();
            CreateMap<SysUserViewModel, SysUser>();
            CreateMap<SysUser, ActivateEmailTemplateViewModel>();
            CreateMap<SysUser, MemberEmailTemplateViewModel>();
        }
    }
}
