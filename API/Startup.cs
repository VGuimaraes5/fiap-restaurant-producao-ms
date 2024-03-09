using Microsoft.AspNetCore.Mvc.Authorization;
using Infrastructure.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Application.Consumers;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        private readonly IWebHostEnvironment Environment;

        public void ConfigureServices(IServiceCollection services)
        {
            RegisterServiceExtensions.RegisterService(services);

            services.RegisterAutoMapper();
            services.AddRazorPages();
            services.AddSwaggerGen();
            services.AddCognitoIdentity();
            services.AuthenticationConfigurations(Configuration);
            services.AddHttpContextAccessor();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AllowAnonymousFilter());
            }).AddNewtonsoftJson(options => ConfigureJsonOptionsSerializer(options.SerializerSettings));

            services.AddMemoryCache();
            services.AddControllers();

            services.AddCors();

            services.AddHostedService<PedidoCreateConsumer>();
            services.AddHostedService<PagamentoSuccessConsumer>();
            services.AddHostedService<PagamentoErrorConsumer>();
        }

        private void ConfigureJsonOptionsSerializer(JsonSerializerSettings serializerSettings)
        {
            serializerSettings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            serializerSettings.Converters.Add(new StringEnumConverter());
            serializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
            });
            app.UseReDoc(c =>
            {
                c.DocumentTitle = "Documento REDOC API";
                c.SpecUrl = "/swagger/v1/swagger.json";
                c.RoutePrefix = "";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
