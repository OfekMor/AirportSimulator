using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportModels.Models
{
    public class Plane
    {
        [BsonId]
        public Guid PlaneId { get; set; }
        public string PlaneName { get; set; }
        public string CompanyName { get; set; }
        public string Type { get; set; }
    }
}
