using SignalRTest;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR().AddHubOptions<MessageHub>(options =>
{
    options.EnableDetailedErrors = true;
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
});

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();



app.MapGet("/", () => "Hello World!");


app.MapHub<MessageHub>("/mychat");
app.Run();
