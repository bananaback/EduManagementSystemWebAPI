using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Common.Interfaces.Services
{
    public interface IMessageSender
    {
        public Task AckMessageReceivedAsync(Guid messageId, CancellationToken cancellationToken);
    }
}
