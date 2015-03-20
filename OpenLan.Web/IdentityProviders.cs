using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace OpenLan.Web
{
    public class EmailMessageProvider : IIdentityMessageProvider
    {
        public string Name
        {
            get
            {
                return "Email";
            }
        }

        public Task SendAsync(IdentityMessage message, CancellationToken cancellationToken = default(CancellationToken))
        {
            // TODO: Plug in your service
            return Task.FromResult(0);
        }
    }

    public class SmsMessageProvider : IIdentityMessageProvider
    {
        public string Name
        {
            get
            {
                return "SMS";
            }
        }

        public Task SendAsync(IdentityMessage message, CancellationToken cancellationToken = default(CancellationToken))
        {
            // TODO: Plug in your service
            return Task.FromResult(0);

        }
    }
}