using PeanutTradeTestTask.API.Attributes;
using PeanutTradeTestTask.BLL.Interfaces;
using PeanutTradeTestTask.BLL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICryptoExchangeSevice, CryptoExchangeSevice>();
builder.Services.Scan(scan => scan.FromAssemblyOf<IExchange>()
    .AddClasses(classes => classes.AssignableTo<IExchange>())
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(CurrencyAttribute));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
