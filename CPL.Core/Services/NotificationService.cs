using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class NotificationService : CoreBase<Notification>, INotificationService
    {
        private readonly IRepositoryAsync<Notification> _repository;

        public NotificationService(IRepositoryAsync<Notification> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
