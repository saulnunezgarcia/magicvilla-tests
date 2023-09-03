using MagicVilla_API;
using MagicVilla_API.Datos;
using MagicVilla_API.Repositorio;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Cada vez que se instala un paquete este debe de añadirse aqui como servicio

builder.Services.AddControllers().AddNewtonsoftJson(); //Aqui se añadio para poder trabajar con httppatch
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); //Asi se indica que utilice el defaultconnection 
    //definido en appsettings.json despues de haber creado la clase ApplicationDbcontext 
});

//Agregamos el servicio de los mapeos 

builder.Services.AddAutoMapper(typeof(MappingConfig)); //Se escribe el nombre de la clase que tiene los mapeos

builder.Services.AddScoped<IVillaRepositorio, VillaRepositorio>();

builder.Services.AddScoped<INumeroVillaRepositorio, NumeroVillaRepositorio>();

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
