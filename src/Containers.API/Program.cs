using Containers.Application;
using Containers.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("UniversityDatabase");
if (!string.IsNullOrWhiteSpace(connectionString))
    builder.Services.AddSingleton<IContainerService, ContainerService>(containerService => new ContainerService(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/containers", (IContainerService containerService) =>
{
    try
    {
        return Results.Ok(containerService.GetAllContainers());
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapPost("/api/containers", (IContainerService containerService, Container container) =>
    {
        try
        {
            var result = containerService.AddContainer(container);
            if (result)
            {
                return Results.Created("/api/containers", result);
            }
            else
            {
                return Results.BadRequest();
            }
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }
);

app.Run();

