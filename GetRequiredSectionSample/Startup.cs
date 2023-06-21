using GetRequiredSectionSample.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace GetRequiredSectionSample;
public class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // raise exception on startup
        var configSection = builder.Configuration.GetRequiredSection(SampleOptions.ConfigurationName);
        builder.Services.Configure<SampleOptions>(configSection);

        // raise exception on fisrt usage
        //builder.Services.Configure<SampleOptions>(options =>
        //{
        //    builder.Configuration.GetRequiredSection(SampleOptions.ConfigurationName).Bind(options);
        //});

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "GetRequiredSectionSample", Version = "v1" });
        });

        WebApplication app = builder.Build();
       
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GetRequiredSectionSample v1"));
       
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
