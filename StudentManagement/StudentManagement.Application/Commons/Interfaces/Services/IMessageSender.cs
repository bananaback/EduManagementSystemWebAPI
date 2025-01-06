using StudentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Commons.Interfaces.Services
{
    public interface IMessageSender
    {
        public Task<bool> SendAsync(OutboxMessage message);
    }
}
