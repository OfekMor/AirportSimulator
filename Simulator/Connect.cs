using AirportModels.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Simulator
{
    public class Connect
    {
        static HubConnection connection;

        public Connect(Logic logic)
        {
            if (connection == null)
            {
                connection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:44379/airportServer", (opts) =>
                    {
                        opts.HttpMessageHandlerFactory = (message) =>
                        {
                            if (message is HttpClientHandler clientHandler)
                                // always verify the SSL certificate
                                clientHandler.ServerCertificateCustomValidationCallback +=
                                        (sender, certificate, chain, sslPolicyErrors) => { return true; };
                            return message;
                        };
                    })
                    .Build();
                connection.On("GetPlanes", (List<Plane> planes) =>
                {
                    logic.Planes = planes;
                });
                connection.StartAsync();
            }
        }
        public HubConnection CurrentConnection { get { return connection; } }
    }
}