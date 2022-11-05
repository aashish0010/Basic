using Microsoft.AspNetCore.SignalR;

namespace Basic.Application.Function
{
    public static class Data
    {
        public static List<string> DataList = new List<string>();
    }
    public class NotificationHub : Hub
    {

        public override Task OnConnectedAsync()
        {

            //Data.DataList.Add(Context.ConnectionId);
            //Context.User.Identity.Name = "aashish";
            Groups.AddToGroupAsync(Context.ConnectionId, "aashish");
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Data.DataList.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
