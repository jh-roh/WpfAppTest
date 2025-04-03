using Microsoft.AspNetCore.SignalR;

namespace SignalRTest
{
    public class MessageHub : Hub
    {
        public async Task Send(string message, string userId)
        {
            if(userId != null && userId.Length > 0)
            {
                await Clients.All.SendAsync("receive", message, userId);

            }
            else
            {
                if (userId != null)
                    await Clients.Others.SendAsync("notify", "채팅방에서 1명이 퇴장했습니다.");
            }
        }
    }
}
