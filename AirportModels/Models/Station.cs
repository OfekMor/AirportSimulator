using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportModels.Models
{
    public class Station
    {
        public int StationId { get; set; }
        public TimeSpan DurationInStation { get; set; }
        public Plane Plane { get; set; }

        public object Locker = new object();
    }
}
