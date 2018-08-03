using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class ContactService : CoreBase<Contact>, IContactService
    {
        private readonly IRepositoryAsync<Contact> _repository;

        public ContactService(IRepositoryAsync<Contact> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
