using AirportDAL.DataBase;
using AirportDAL.Interfaces;
using AirportModels.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDAL.Repositories
{
    public class AirplaneRepo : IAirplaneRepo
    {
        static string table = "Airplanes";
        static MyMongoDB db = new MyMongoDB("Planes");

        public void Add(Plane airplane)
        {
            var collection = db.Client.GetCollection<Plane>(table);
            collection.InsertOneAsync(airplane);
        }

        public List<Plane> GetAirplanes() => db.Client.GetCollection<Plane>(table).Find(new BsonDocument()).ToList();

        public void Remove(Plane airplane) => db.Client.GetCollection<Plane>(table).DeleteOne(Builders<Plane>.Filter.Eq(x => x.PlaneName, airplane.PlaneName));
    }
}
