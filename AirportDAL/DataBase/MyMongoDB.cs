using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDAL.DataBase
{
    public class MyMongoDB
    {
        public IMongoDatabase Client;

        public MyMongoDB(string database)
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://ofeking8:ofeking8@airportsimulationdb.lq4qn.mongodb.net/AirportSimulationDB?retryWrites=true&w=majority");
            var client = new MongoClient(settings);
            Client = client.GetDatabase(database);
        }
    }
}
