using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG430_TP
{
    public class ApplicationMessageRepository : IApplicationMessageRepository
    {
        MongoDBContext db = new MongoDBContext();
        public async Task Add(ApplicationMessage message)
        {
            await db.ApplicationMessage.InsertOneAsync(message);
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationMessage> GetApplicationMessage(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationMessage>> GetApplicationMessages()
        {
            throw new NotImplementedException();
        }

        public Task Update(ApplicationMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
