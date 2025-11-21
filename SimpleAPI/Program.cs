using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SimpleAPI.Core.Cache;
using SimpleAPI.Core.Repository;
using SimpleAPI.DAL.Context;
using SimpleAPI.DataAccess.Repository;
using SimpleAPI.Infrastructure.Service;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
    serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(2);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssembly(Assembly.Load("SimpleAPI.BL"));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddStackExchangeRedisCache(opt => opt.Configuration = builder.Configuration.GetConnectionString("Redis"));
builder.Services.AddHostedService<RedisWarmupService>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("AzureSql"));
    opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    opt.EnableDetailedErrors(false);
    opt.EnableSensitiveDataLogging(false);
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();

//app.UseExceptionHandler();
//using (var scope = app.Services.CreateScope())
//{
//    var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
//    await cache.GetStringAsync("warmup-key");
//}

app.MapControllers();
app.Run();
