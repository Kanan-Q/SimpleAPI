using Microsoft.EntityFrameworkCore;
using SimpleAPI.BL.Cache;
using SimpleAPI.Core.Repository;
using SimpleAPI.DAL.Context;
using SimpleAPI.DataAccess.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddStackExchangeRedisCache(options => options.Configuration = builder.Configuration.GetConnectionString("Redis"));
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("AzureSql"));
    opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseExceptionHandler();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
