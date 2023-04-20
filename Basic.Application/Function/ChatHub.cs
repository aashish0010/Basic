using Microsoft.AspNetCore.SignalR;

namespace Basic.Application.Function
{
    public class ChatHub : Hub
    {


        public override Task OnConnectedAsync()
        {
            //string name = user;
            Groups.AddToGroupAsync(Context.ConnectionId, "aashish");
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendToUser(string user, string receiverConnectionId, string message)
        {

            await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", user, message);
        }

        //here the name is included
        public List<string> GetConnectionId() => new List<string> { Context.ConnectionId, "" };


        //Concept of Group

        //public Task JoinRoom(string roomName)
        //{
        //    return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        //}

        //public Task LeaveRoom(string roomName)
        //{
        //    return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        //}
        //public async Task JoinRoom(String name, string roomName, string message)
        //{

        //    await Clients.Group(roomName).SendAsync("ReceiveMessage", name, message);
        //}
    }
}
