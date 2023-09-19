using Comments.Application.Comments.Commands.Add;
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
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<AddCommentCommand>());

builder.Services.AddDbContext<CommentsDbContext>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
