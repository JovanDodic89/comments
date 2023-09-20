using Comments.API.Filters;
using Comments.Application;
using Comments.Domain.Interfaces;
using Comments.Persistance;
using Comments.Persistance.Repositories;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddMvc(opt => { opt.Filters.Add<ApiExceptionFilterAttribute>(); });

builder.Services.AddDbContext<CommentsDbContext>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();

builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
