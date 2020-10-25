using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG430_TP
{
    public interface IApplicationMessageRepository
    {
        void Add(ApplicationMessage message);
        Task Update(ApplicationMessage message);
        Task Delete(string id);
        Task<ApplicationMessage> GetApplicationMessage(string id);
        Task<List<ApplicationMessage>> GetApplicationMessages();
    }
}
