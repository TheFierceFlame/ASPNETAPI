using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using NotificationsAPI.Manager;
using NotificationsAPI.Models;
using Python.Runtime;

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(
        Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "key.json"
        )
    )
});

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
String PythonDLL = @"C:\Users\lenovo\AppData\Local\Programs\Python\Python312\python312.dll";

Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", PythonDLL);

var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<FakeStoreContext>(options =>
{
    options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
});
builder.Services.AddControllers();
builder.Services.AddScoped<FireBaseNotificationsRepositoryInterface, FireBaseNotificationsRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
