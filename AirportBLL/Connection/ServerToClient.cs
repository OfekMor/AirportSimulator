using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AirportBLL.Connection
{
    public class ServerToClient
    {
        static HubConnection connection;

        public ServerToClient()
        {
            if (connection == null)
            {
                connection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:44379/airportServer")
                    .Build();
                connection.StartAsync();
            }
        }
        public HubConnection CurrentConnection { get { return connection; } }
    }
}
