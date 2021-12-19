using AirportBLL.Connection;
using AirportBLL.Interfaces;
using AirportDAL.Interfaces;
using AirportDAL.Repositories;
using AirportModels.DataStructure;
using AirportModels.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AirportBLL.Services
{
    public class AirportService : IAirportService
    {
        private ServerToClient connection;
        public List<Station> Stations { get; private set; }
        public List<NextStation> LandingPath;
        public List<NextStation> DeportPath;
        private IAirplaneRepo repo;

        public AirportService(IAirplaneRepo repo)
        {
            connection = new ServerToClient();
            this.repo = repo;
            CreateStations(8);
            CreateLandingPath();
            CreateDeportPath();
        }
        #region Create stations
        private void CreateDeportPath()
        {
            DeportPath = new List<NextStation>();
            DeportPath.Add(new NextStation(Stations[5], Stations[6]));
            DeportPath.Add(new NextStation(Stations[7]));
            DeportPath.Add(new NextStation(Stations[3]));
        }

        private void CreateLandingPath()
        {
            LandingPath = new List<NextStation>();
            for (int i = 0; i < 5; i++)
                LandingPath.Add(new NextStation(Stations[i]));
            LandingPath.Add(new NextStation(Stations[5], Stations[6]));
        }

        private void CreateStations(int stationsAmount)
        {
            Stations = new List<Station>();
            for (int i = 0; i < stationsAmount; i++)
                Stations.Add(new Station { StationId = i + 1, Plane = null, DurationInStation = new TimeSpan(5 * 1000000) });
        }

        #endregion

        public void LandingRequest(Plane plane)
        {
            if (Stations[0].Plane == default)
            {
                var tcs = new TaskCompletionSource();
                LandingPath[0].Stations.ForEach(s => new Task(() => NextStation(LandingPath, 0, s, new Station(), plane, tcs)).Start());
            }
        }

        public void DeportRequest(Plane plane)
        {
            var tcs = new TaskCompletionSource();
            DeportPath[0].Stations.ForEach(station => new Task(() => NextStation(DeportPath, 0, station, new Station(), plane, tcs)).Start());
            repo.Remove(plane);
        }

        public void NextStation(List<NextStation> path, int index, Station station, Station prevStation, Plane plane, TaskCompletionSource tcs)
        {
            do
            {
                if (tcs.Task.IsCompleted)
                    return;
            } while (station.Plane != default);
            lock (station.Locker)
            {
                lock (tcs)
                {
                    if (tcs.Task.IsCompleted)
                        return;
                    tcs.SetResult();
                }

                UpdateStation(prevStation);
                UpdateStation(station, plane);

                connection.CurrentConnection.InvokeAsync("StationsStatus",Stations);
                Thread.Sleep(station.DurationInStation);
            }

            var tcs2 = new TaskCompletionSource();
            if (path.Count > index + 1)
                if (path == LandingPath)
                    LandingPath[index + 1].Stations.ForEach(s => new Task(() => NextStation(LandingPath, index + 1, s, station, plane, tcs2)).Start());
                else
                    DeportPath[index + 1].Stations.ForEach(s => new Task(() => NextStation(DeportPath, index + 1, s, station, plane, tcs2)).Start());

            EndPath(path, plane);
        }

        private void UpdateStation(Station station, Plane plane = null)
        {
            station.Plane = plane;
        }

        public void EndPath(List<NextStation> path, Plane plane)
        {
            if (path == LandingPath)
                repo.Add(plane);
        }
    }
}
