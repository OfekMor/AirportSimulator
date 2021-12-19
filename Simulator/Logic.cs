using AirportModels.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Simulator
{
    public class Logic
    {
        private bool autoLandIsOn = false;
        private bool autoTakeoffIsOn = false;
        private bool isCompanyExist = false;
        private string input;
        Connect connection;
        Random random;
        Timer autoLand;
        Timer autoTakeoff;
        public List<Plane> Planes { get; set; }
        public Logic()
        {
            connection = new Connect(this);
            Planes = new List<Plane>();

            random = new Random();
            autoLand = new Timer();
            autoTakeoff = new Timer();
        }

        #region colors
        private void Cyan() => Console.ForegroundColor = ConsoleColor.Cyan;
        private void DarkCyan() => Console.ForegroundColor = ConsoleColor.DarkCyan;
        private void White() => Console.ForegroundColor = ConsoleColor.White;
        private void Red() => Console.ForegroundColor = ConsoleColor.Red;
        private void Green() => Console.ForegroundColor = ConsoleColor.Green;
        private void Yellow() => Console.ForegroundColor = ConsoleColor.Yellow;
        private void Gray() => Console.ForegroundColor = ConsoleColor.Gray;
        #endregion

        public void SimulatorRun()
        {
            Console.Clear();
            Console.WriteLine("\t\t\t\t\t\tWelcome to AirportSimulator");
            Console.WriteLine("========================================================================================================================");
            Console.WriteLine();
            Console.Write($"Connection Mode: ");

            if (connection.CurrentConnection.State == HubConnectionState.Connected)
            {
                connection.CurrentConnection.InvokeAsync("GetPlanes").Wait();

                Green();
                Console.Write("Online!\n\n");

                Yellow();
                Console.Write("Automatic Land: ");
                if (autoLandIsOn)
                {
                    Green();
                    Console.Write("Active\n");
                }
                else
                {
                    Red();
                    Console.Write("Disabled\n");
                }

                Yellow();
                Console.Write("Automatic Takeoff: ");
                if (autoTakeoffIsOn)
                {
                    Green();
                    Console.Write("Active\n\n");
                }
                else
                {
                    Red();
                    Console.Write("Disabled\n\n");
                }

                Gray();
                Console.WriteLine($"Current Planes: {Planes.Count}\n");

                DarkCyan();
                Console.Write("'Land'");
                White();
                Console.Write(" => 'Mannually land new plane'\n");

                DarkCyan();
                Console.Write("'Takeoff'");
                White();
                Console.Write(" => 'Manually Take off plane'\n");

                DarkCyan();
                Console.Write("'AutoLand'");
                White();
                Console.Write(" => 'Automaticlly land the airplanes'\n");

                DarkCyan();
                Console.Write("'Disable AutoLand'");
                White();
                Console.Write(" => 'Disable the automaticlly land of the airplanes'\n");

                DarkCyan();
                Console.Write("'AutoTakeoff'");
                White();
                Console.Write(" => 'Automaticlly takeoff the airplanes'\n");

                DarkCyan();
                Console.Write("'Disable AutoTakeoff'");
                White();
                Console.Write(" => 'Disable the automaticlly takeoff of the airplanes'\n");

                DarkCyan();
                Console.Write("'Planes'");
                White();
                Console.Write(" => 'Show list of all planes'\n");

                DarkCyan();
                Console.Write("'Close Connection'");
                White();
                Console.Write(" => 'Close the connection'");

                Console.WriteLine("\n========================================================================================================================\n");
                Console.WriteLine("Your Command:\n");

                input = Console.ReadLine().ToLower();
                if (input == "b")
                    CloseConnection();
                if (input == "al")
                {
                    autoLandIsOn = true;
                    AutoLand();
                }
                if (input == "dal")
                {
                    autoLandIsOn = false;
                    AutoLand();
                }
                if (input == "at")
                {
                    autoTakeoffIsOn = true;
                    AutoTakeOff();
                }
                if (input == "dat")
                {
                    autoTakeoffIsOn = false;
                    AutoTakeOff();
                }
                if (input == "p")
                    GetPlanes();
                if (input == "l")
                    Land();
                if (input == "t")
                {
                    Takeoff();
                }
                Console.Clear();
                SimulatorRun();
            }
            else
            {
                Red();
                Console.Write("Offline!\n\n");
                White();
                Console.Write("To begin please start the connection by typing:");
                Gray();
                Console.Write(" Start Connection\n");
                if (Console.ReadLine() == "a")
                    StartConnection();
                else
                    Console.WriteLine("Incorrect input");
                Console.Clear();
                White();
                SimulatorRun();
            }
            White();
        }

        private void Takeoff()
        {
            if (Planes.Count != 0)
            {
                Console.WriteLine("\nif you want to cancel type: back\n");
                Console.WriteLine("please type the plane name:");
                var input = Console.ReadLine();
                if (input == "back")
                    SimulatorRun();
                else
                    foreach (var plane in Planes)
                    {
                        if (input.ToUpper() == plane.PlaneName)
                        {
                            connection.CurrentConnection.InvokeAsync("Takeoff", plane);
                            Console.WriteLine("Plane tookoff successfully!");
                            SimulatorRun();
                        }
                    }
                Console.WriteLine("this plane is not at the airport\n");
                Takeoff();
            }
            else
                Console.WriteLine("no airplanes current at the airport!");

        }

        private void AutoTakeOff()
        {
            if (autoTakeoffIsOn)
            {
                autoTakeoff.Elapsed += AutoTakeoff_Elapsed;
                autoTakeoff.Start();
            }
            else
            {
                autoTakeoff.Elapsed -= AutoTakeoff_Elapsed;
                autoTakeoff.Stop();
            }
        }

        private void AutoTakeoff_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Planes.Count != 0)
            {
                autoTakeoff.Interval = random.Next(1, 20) * 1000;
                connection.CurrentConnection.InvokeAsync("Takeoff", Planes[random.Next(0, Planes.Count)]);
                Console.WriteLine("plane successfully automaticlly took off");
            }
            else
            {
                Console.WriteLine("no planes at the airport!");
                autoTakeoffIsOn = false;
                AutoTakeOff();
            }
        }

        private void AutoLand()
        {
            if (autoLandIsOn)
            {
                autoLand.Elapsed += AutoLand_Elapsed;
                autoLand.Start();
            }
            else
            {
                autoLand.Elapsed -= AutoLand_Elapsed;
                autoLand.Stop();
            }
        }

        private void AutoLand_Elapsed(object sender, ElapsedEventArgs e)
        {
            autoLand.Interval = random.Next(1, 20) * 1000;
            connection.CurrentConnection.InvokeAsync("Land", "Random", "AutoPlane", "auto");
            Console.WriteLine("successfully landed auto plane");
        }

        public void Land()
        {
            Console.WriteLine("To return at any time type: back\nInsert Plane Name: (5 letters)");
            var planeName = Console.ReadLine().ToUpper();
            if (planeName == "back") SimulatorRun();
            if (planeName.Length != 5)
                Land();
            else
            {
                isCompanyExist = false;
                var companies = (CompanyNames[])Enum.GetValues(typeof(CompanyNames));
                Console.WriteLine("Insert Company Name:\n");
                Yellow();

                foreach (var company in companies)
                    Console.Write($"{company}, ");
                White();
                var companyName = Console.ReadLine();
                if (companyName == "back") SimulatorRun();

                foreach (var company in companies)
                {
                    if (companyName.ToUpper() == company.ToString().ToUpper())
                    {
                        isCompanyExist = true;
                        Console.WriteLine("Insert Plane Type:");
                        Cyan();
                        foreach (var planeType in (PlaneType[])Enum.GetValues(typeof(PlaneType)))
                            Console.WriteLine(planeType);
                        White();

                        var type = Console.ReadLine();
                        if (type == "back") SimulatorRun();

                        connection.CurrentConnection.InvokeAsync("Land", planeName, companyName, type);
                        connection.CurrentConnection.InvokeAsync("GetPlanes");
                    }
                }
                if (!isCompanyExist)
                {
                    Console.WriteLine("Company is not exist plz try again");
                    Land();
                }
            }
        }

        private void GetPlanes()
        {
            Console.Clear();
            Console.WriteLine("Current planes at the airport");
            Console.WriteLine("\n========================================================================================================================\n");

            foreach (var plane in Planes)
            {
                White();
                Console.Write("\nPlane Name: ");
                Cyan();
                Console.Write(plane.PlaneName);
                White();
                Console.Write(" Plane id: ");
                Cyan();
                Console.Write(plane.PlaneId);
                White();
                Console.Write(" plane company name: ");
                Cyan();
                Console.Write(plane.CompanyName);
                White();
                Console.Write(" plane type: ");
                Cyan();
                Console.Write(plane.Type);
            }
            Gray();
            Console.WriteLine("\nType anything to return =>");
            Console.Read();
        }

        private void StartConnection() => connection.CurrentConnection.StartAsync();

        private void CloseConnection() => connection.CurrentConnection.StopAsync();
    }
}