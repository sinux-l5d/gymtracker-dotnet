using GymTracker;
using GymTracker.Services;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// This provide a instance of InMemorySessionRepo to every controller that needs ISessionRepo
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<ISessionRepo, InMemorySessionRepo>();
}
else
{
    //builder.Services.AddScoped<ISessionRepo, SqlSessionRepo>();
}

/* FOR LATER TO UNDERSTAND
builder.Services.AddDbContext<GymTrackerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GymTrackerContext")));
    */

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