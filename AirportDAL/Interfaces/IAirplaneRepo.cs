using AirportModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDAL.Interfaces
{
    public interface IAirplaneRepo
    {
        void Add(Plane airplane);
        void Remove(Plane plane);
        List<Plane> GetAirplanes();
    }
}
