var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "This is going to call the list of todos that are in the database");

app.Run();
