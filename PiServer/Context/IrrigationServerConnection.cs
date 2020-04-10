using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using PiServer.DTOs;
using PiServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PiServer.Context
{
    public class IrrigationServerConnection : IHostedService
    {
        private HubConnection hubConnection;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44353/ws/pi")
                .WithAutomaticReconnect()
                .Build();

            hubConnection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await hubConnection.StartAsync();
            };

            hubConnection.Reconnected += async (error) =>
            {
                await PiLogin();
            };

            await hubConnection.StartAsync();
            await PiLogin();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await hubConnection.DisposeAsync();
        }

        public async Task<Tuple<bool, long>> RegisterSzenzor(string id, SzenzorTipus tipus)
        {
            SzenzorDTO szenzorDTO = new SzenzorDTO()
            {
                Nev = id,
                Tipus = tipus,
                Megjegyzes = ""
            };

            var response = await hubConnection.InvokeCoreAsync<RegisterSensorResponseDTO> ("RegisterSensor", new object[] { szenzorDTO });

            return Tuple.Create(response.Success, response.Id);
        }

        public async Task<bool> PostMeresData(long szenzorId, long meresData)
        {
            PostMeresDataDTO meresDataDTO = new PostMeresDataDTO()
            {
                SzenzorId = szenzorId,
                MertAdat = meresData,
                Mikor = DateTime.UtcNow
            };

            var response = await hubConnection.InvokeCoreAsync<PostMeresDataResponseDTO>("PostMeresData", new object[] { meresDataDTO });

            return response.Success;
        }

        private async Task PiLogin()
        {
            try
            {
                bool connected = false;
                while (!connected)
                {
                    var response = await hubConnection.InvokeCoreAsync<PiLoginResponseDTO>("PiLogin", new object[] { "12345" });
                    connected = response.Success;
                    await Task.Delay(new Random().Next(0, 5) * 5000);
                }
            }
            catch(Exception e)
            {
                
            }
        }
    }
}
