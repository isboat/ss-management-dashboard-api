using Microsoft.AspNetCore.SignalR;

namespace Management.Dashboard.Services.SignalR
{
    public sealed class SignalRServiceHub: Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveMessage", $"new client - {Context.ConnectionId} joined");
            //return base.OnConnectedAsync();
        }

        public async Task SendMessage(string inputText)
        {
            await Clients.All.SendAsync("ReceiveMessage", inputText);
        }
    }
}
