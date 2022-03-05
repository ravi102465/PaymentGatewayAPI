using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Newtonsoft.Json.Converters;
using PaymentGatewayAPI;
using PaymentGatewayAPI.Helpers;
using PaymentGatewayAPI.Repositories;
using PaymentGatewayAPI.Services;
using PaymentGatewayAPI.Validations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson( options => { options.SerializerSettings.Converters.Add(new StringEnumConverter()); });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning();

builder.Services.AddScoped<IPaymentValidator, PaymentValidator>();
builder.Services.AddSingleton<IPaymentRepository, MemoryPaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddHttpClient<IBankSimulatorClient, BankSimulatorClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BankSimulator:BaseUrl"]);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseErrorLogging(app.Logger);

app.Run();
