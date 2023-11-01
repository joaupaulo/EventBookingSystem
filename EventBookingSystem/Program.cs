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
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IRepositoryBase, RepositoryBase>();
builder.Services.AddSingleton<IEventService, EventService>();
builder.Services.AddSingleton<IBookingService, BookingService>();
builder.Services.AddSingleton<IBsonFilter<Event>, BsonFilter<Event>>();
builder.Services.AddSingleton<IBsonFilter<Booking>, BsonFilter<Booking>>();
builder.Services.AddSingleton<IPDFGenerator, PDFGenerator>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
var ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
var DataBaseIdentity = Environment.GetEnvironmentVariable("USER_REGISTER");
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
    (
       ConnectionString, DataBaseIdentity
    );
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(jwt =>
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
    Console.WriteLine("Deployment done in develop");
}

if (app.Environment.IsProduction())
{
    Console.WriteLine("Production ok, compiled!");
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();