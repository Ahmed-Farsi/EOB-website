using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eob_Web.Core.Services
{
    public interface IBulkEmailSender
    {
        public Task SendBulkEmailAsync(List<string> emails, string subject, string message);
    }
}
