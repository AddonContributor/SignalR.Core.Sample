using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

public class Chat : Hub
{
    public static ISession Session { get; set; }

    public Chat(IHttpContextAccessor httpContextAccessor)
    {
        Session = httpContextAccessor.HttpContext.Session;
    }

    public void Send(string name, string message)
    {
        Session.SetString("key", DateTime.Now.ToString());
        var someValue = Session.GetString("key");

        //return Clients.All.InvokeAsync("Send", data);
        Clients.All.SendAsync("broadcastMessage", name, "${someValue}: message");
    }
}