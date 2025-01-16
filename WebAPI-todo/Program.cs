using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// middleware to redirect URL   
app.UseRewriter(new RewriteOptions().AddRedirect("jobs/(.*)", "todos/$1"));

var todos = new List<Todo>();

// get ALL
app.MapGet("/todos", () => todos); // mappget ALL method and handler

// get by ID
app.MapGet("/todos/{id}", Results<Ok<Todo>, NotFound> (int id) => // URL string id and parameter
{
    var targetTodo = todos.SingleOrDefault(t => id ==t.Id);
    return targetTodo is null
    ? TypedResults.NotFound()
    : TypedResults.Ok(targetTodo);
});

// post
app.MapPost("/todos", (Todo task) => // mappost method and handler
{
    todos.Add(task);
    return TypedResults.Created("/todos/{id}", task); // outputs status code 201 (created)
});

// delete
app.MapDelete("/todos/{id}", (int id) =>
{
    todos.RemoveAll(t => id == t.Id);
    return TypedResults.NoContent();
});

app.Run();

public record Todo(int Id, string Name, DateTime DueDate, bool inCompleted); // record type