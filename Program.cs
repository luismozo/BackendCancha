using BackendCancha.Data;
using BackendCancha.MappinClass;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BackendCancha.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BackendCancha
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1️ Configuración de CORS
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:8080", 
                            "http://n4mkmf-ip-191-95-129-96.tunnelmole.net")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            // 2️ Configuración de DbContext con MySQL
            builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 40))
                ));

            // 3️ Configuración JSON para evitar ciclos
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

            // 4️ Configuración de JWT Authentication
            var Key = builder.Configuration["Jwt:Key"];

            // Segundo, VALIDAMOS el string.
            if (string.IsNullOrEmpty(Key))
            {
                throw new InvalidOperationException("La clave JWT (Jwt:Key) no está configurada en appsettings.json");
            }

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key))
                    };
                });

            // inyecciones de dependecia.
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
            builder.Services.AddScoped<IProductoService, ProductoService>();
            builder.Services.AddScoped<IReservaService, ReservaService>();
            builder.Services.AddScoped<ICategoriaPService, CategoriaPService>();
            builder.Services.AddScoped<ICanchaService, CanchaService>();
            //

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
           

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // app.UseHttpsRedirection(); // Puedes dejarlo comentado JUANKI

            app.UseCors(MyAllowSpecificOrigins);

            // 5️ Activar autenticación y autorización
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
