using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG430_TP
{
    public class ApplicationMessageRepository : IApplicationMessageRepository
    {
        private static ApplicationMessageRepository _Instance;
        public static ApplicationMessageRepository Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ApplicationMessageRepository();

                return _Instance;
            }
        }

        private MongoDBContext _DataBase;

        private ApplicationMessageRepository() 
        {
            _DataBase = new MongoDBContext();
        }

        public void Add(ApplicationMessage message)
        {
            _DataBase.ApplicationMessage.InsertOne(message);
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationMessage> GetApplicationMessage(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ApplicationMessage>> GetApplicationMessages()
        {
            return _DataBase.ApplicationMessage.Find(_ => true).ToListAsync();
        }

        public Task Update(ApplicationMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
