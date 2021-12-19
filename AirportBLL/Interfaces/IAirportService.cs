using AirportModels.DataStructure;
using AirportModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBLL.Interfaces
{
    public interface IAirportService
    {
        List<Station> Stations { get; }
        void LandingRequest(Plane plane);
        void DeportRequest(Plane plane);
        void NextStation(List<NextStation> path, int index, Station station, Station prevStation, Plane plane, TaskCompletionSource tcs);
        void EndPath(List<NextStation> path, Plane plane);
    }
}
