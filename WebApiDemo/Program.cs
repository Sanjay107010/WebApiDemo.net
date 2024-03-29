using Microsoft.EntityFrameworkCore;
using WebApiDemo.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//builder.Services.AddDbContext<ContactApiDbContext>(options => options.UseInMemoryDatabase("contactsDb"));





builder.Services.AddDbContext<ContactApiDbContext>(Options =>
{
    Options.UseSqlServer(builder.Configuration.GetConnectionString("ContactsApiConnectionStrings"));
});
builder.Services.AddDbContext<RegistrationApiDbContext>(Options =>
{
    Options.UseSqlServer(builder.Configuration.GetConnectionString("ContactsApiConnectionStrings"));
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

app.Run();
