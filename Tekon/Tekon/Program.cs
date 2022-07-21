using Microsoft.EntityFrameworkCore;
using Tekon;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Controller>(opt => opt.UseInMemoryDatabase("TekonAPI"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

Dictionary<int, Item> Dict = new();

app.MapGet("/", () => "Launched successfully!");

app.MapGet("/TekonAPI", async (Controller controller) =>
    await Task.Run(() => controller.ToList(Dict)));

app.MapGet("/TekonAPI/{id}", async (int id, Controller controller) =>
{
    if (await Task.Run(() => Dict.ContainsKey(id)) == false) return Results.Text("�������� � ����� ID �� ����������!");

    return Results.Ok(Dict[id]);
});

app.MapPost("/TekonAPI", async (Item todo, Controller controller) =>
{
    if (await Task.Run(() => controller.AddItem(todo, Dict)) == false) return Results.Text("������� �� ��� ��������!");

    return Results.Text("������� ������� ��������!");
});

app.MapPut("/TekonAPI", async (Item inputTodo, Controller controller) =>
{
    if (await Task.Run(() => controller.UpdateItem(inputTodo, Dict)) == false) return Results.Text("������� �� ��� �������!");

    return Results.Text("������� ������� �������!");
});

app.MapDelete("/TekonAPI/{id}", async (int id, Controller controller) =>
{
    if (await Task.Run(() => controller.RemoveItem(id, Dict)) == false) return Results.Text("�������� � ����� ID �� ����������!");

    return Results.Text("������� ������� �����!");
});

app.Run();

// https://localhost:7192/TekonAPI