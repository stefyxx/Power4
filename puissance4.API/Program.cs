using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using puissance4.API.Hubs;
using puissance4.API.Services;
using pussance4.Security;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

JwtManager.JwtConfig config = builder.Configuration.GetSection("Jwt").Get<JwtManager.JwtConfig>(); 

builder.Services.AddSingleton(config);
builder.Services.AddScoped<JwtSecurityTokenHandler>();
builder.Services.AddScoped<JwtManager>();

builder.Services.AddScoped<GameService>();


//Hub
builder.Services.AddSignalR();

//preso dalla docs : https://learn.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-8.0
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    //options.Authority = "Authority URL";
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // If the request is for our hub...
            //var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken))  //&& (path.StartsWithSegments("/hubs/chat"))
            {
                // Read the token out of the query string
                //context.Token = accessToken;

                //qui' non valido realmente il token, perché é tutto false, lo valido alla mano: asccessToken é quello dell'user connesso
                context.HttpContext.User = new JwtSecurityTokenHandler().ValidateToken(accessToken, new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(builder.Configuration["Jwt:Signature"]),
                    ValidateLifetime = false,
                }, out SecurityToken t);
            }
            return Task.CompletedTask;
        }
    };

});





var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) //if you are into state Develop --> not into API builded --> so I put in coment
{
    //entra sempre in swagger.io --> perché ho commentato 'if' che mi faceva entrare Solo se Developement
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//Hub --> definisco l'url x accedere al mio Hub
app.MapHub<GameHub>("/ws/game");

app.Run();
