using System.Text;
using EventBookingSystem.BsonFilter;
using EventBookingSystem.Configurations;
using EventBookingSystem.Configurations.Identity;
using EventBookingSystem.Model;
using EventBookingSystem.Model.MongoDbConfig;
using EventBookingSystem.Repository;
using EventBookingSystem.Service;
using EventBookingSystem.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IRepositoryBase, RepositoryBase>();
builder.Services.AddSingleton<IEventService, EventService>();
builder.Services.AddSingleton<IReservaService, ReservaService>();
builder.Services.AddSingleton<IBsonFilter<Evento>, BsonFilter<Evento>>();
builder.Services.AddSingleton<IBsonFilter<Reserva>, BsonFilter<Reserva>>();
builder.Services.AddSingleton<IPDFGenerator,PDFGenerator>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
    (
        mongoDbSettings.ConnectionString, mongoDbSettings.DatabaseName
    );
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer( jwt =>
    {
        var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value);
        jwt.SaveToken = true;
        jwt.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateTokenReplay = false,
            ValidateLifetime = true,
            
        };
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