using GetRequiredSectionSample.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GetRequiredSectionSample
{
    public class Startup
    {
        private IServiceCollection _services;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _services = services;
            // raise exception on startup
            var configSection = Configuration.GetRequiredSection(SampleOptions.ConfigurationName);
            services.Configure<SampleOptions>(configSection);

            //// raise exception on fisrt usage
            //services.Configure<SampleOptions>(options =>
            //{
            //    Configuration.GetRequiredSection(SampleOptions.ConfigurationName).Bind(options);
            //});

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GetRequiredSectionSample", Version = "v1" });
            });
        }
        private void CheckConfiguration(IApplicationBuilder app, IServiceCollection services)
        {
            var optionsServiceDescriptors = services.Where(s => s.ServiceType.Name.Contains("IOptionsChangeTokenSource"));

            foreach (var service in optionsServiceDescriptors)
            {
                var genericTypes = service.ServiceType.GenericTypeArguments;

                if (genericTypes.Length > 0)
                {
                    var optionsType = genericTypes[0];
                    var genericOptions = typeof(IOptions<>).MakeGenericType(optionsType);

                    dynamic instance = app.ApplicationServices.GetService(genericOptions);
                    var options = instance.Value;
                    var results = new List<ValidationResult>();
                   
                    var isValid = Validator.TryValidateObject(options, new ValidationContext(options), results, true);
                    if (!isValid)
                    {
                        var messages = new List<string> { "Configuration issues" };
                        messages.AddRange(results.Select(r => r.ErrorMessage));
                        throw new Exception(string.Join("\n", messages));
                    }
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            CheckConfiguration(app, _services);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GetRequiredSectionSample v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

