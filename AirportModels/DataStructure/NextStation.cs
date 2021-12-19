using AirportModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportModels.DataStructure
{
    public class NextStation
    {
        public List<Station> Stations { get; private set; }
        public NextStation(params Station[] stations) => Stations = stations.ToList();
    }
}
