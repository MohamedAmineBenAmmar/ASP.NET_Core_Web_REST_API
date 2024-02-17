using Application.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registering services (this operation must be written before `var app = builder.Build();`)
// ------------------
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDBContext>(options => {
    // Here we configure what database we are going to use
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    // The operation that we did is going to seach in the appsettings.json file for a connection string with the name "DefaultConnection"
});

// ------------------
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Check to add this before the Run method. Without it, it will produce an HTTPS redirect error
app.MapControllers();

app.Run();

