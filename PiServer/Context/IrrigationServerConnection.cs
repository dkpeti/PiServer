using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
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
        private string irrigationServerUrl;
        private string piAzonosito;

        public IrrigationServerConnection(IConfiguration configuration)
        {
            irrigationServerUrl = configuration["ConnectionString:IrrigationServerUrl"];
            piAzonosito = configuration["PI_ID"];
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl($"{irrigationServerUrl}/ws/pi")
                .WithAutomaticReconnect()
                .Build();

            hubConnection.Closed += async (error) =>
            {
                await Connect();
            };

            hubConnection.Reconnected += async (error) =>
            {
                await PiLogin();
            };

            hubConnection.On<string>("RegisterPi", async (azonosito) => await RegisterPi(azonosito));
            hubConnection.On<OntozesDTO>("Ontozes", async (ontozesDTO) => await Ontozes(ontozesDTO));

            Connect();
        }

        public async Task Connect()
        {
            try
            {
                await hubConnection.StartAsync();
                await PiLogin();
            }
            catch (Exception e) {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                Connect();
            }
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
                    var response = await hubConnection.InvokeCoreAsync<PiLoginResponseDTO>("PiLogin", new object[] { piAzonosito });
                    connected = response.Success;
                    await Task.Delay(new Random().Next(0, 5) * 5000);
                }
            }
            catch(Exception e)
            {
                
            }
        }

        private async Task RegisterPi(string azonosito)
        {
            try
            {
                var response = new PiRegisterDTO()
                {
                    IsSuccessful = true
                };
                await hubConnection.InvokeCoreAsync("PiRegister", new object[] { response });
            }
            catch(Exception e)
            {

            }
        }

        private async Task Ontozes(OntozesDTO ontozesDTO)
        {
            try
            {
                var response = new OntozesResponseDTO()
                {
                    IsSuccessful = true
                };
                await hubConnection.InvokeCoreAsync("Ontozes", new object[] { response });
            }
            catch (Exception e)
            {

            }
        }
    }
}
