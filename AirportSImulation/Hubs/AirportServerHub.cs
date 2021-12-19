using AirportBLL.Interfaces;
using AirportBLL.Services;
using AirportDAL.Interfaces;
using AirportDAL.Repositories;
using AirportModels.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSImulation.Hubs
{
    public class AirportServerHub : Hub
    {
        IAirplaneRepo repo;
        IAirportService service;

        public AirportServerHub(IAirplaneRepo repo,IAirportService service)
        {
            this.repo = repo;
            this.service = service;
        }
        public async Task StationsStatus() => await Clients.All.SendAsync("StationsStatus", service.Stations);
        public void Land(string name, string company, string type) => service.LandingRequest(new Plane { PlaneName = name, CompanyName = company, Type = type });
        public void GetPlanes() => Clients.All.SendAsync("GetPlanes", repo.GetAirplanes());
        public void Takeoff(Plane airplane) => service.DeportRequest(airplane);
    }
}
