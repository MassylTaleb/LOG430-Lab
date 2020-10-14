using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG430_TP
{
    public interface IApplicationMessageRepository
    {
        Task Add(ApplicationMessage message);
        Task Update(ApplicationMessage message);
        Task Delete(string id);
        Task<ApplicationMessage> GetApplicationMessage(string id);
        Task<IEnumerable<ApplicationMessage>> GetApplicationMessages();
    }
}
