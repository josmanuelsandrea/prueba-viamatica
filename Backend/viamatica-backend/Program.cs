using Microsoft.EntityFrameworkCore;
using viamatica_backend.Configuration;
using viamatica_backend.DBModels;
using viamatica_backend.Repository;
using viamatica_backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ViamaticaContext>(options => options.UseNpgsql((new ConfigurationBuilder()).AddJsonFile("appsettings.json").Build().GetSection("DB").GetValue<string>("connection")));
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<PersonaRepository>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<RolOpcionesRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<HistorialSesioneRepository>();
builder.Services.AddScoped<SesionesActivaRepository>();
builder.Services.AddScoped<RolUsuarioRepository>();
builder.Services.AddScoped<SesionesActivasService>();
builder.Services.AddScoped<HistorialSesionesService>();
builder.Services.AddScoped<RolRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Agregar origen permitido
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

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
