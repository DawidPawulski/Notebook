using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NotebookAPI.Application.Common.Behaviors;

namespace NotebookAPI.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSwaggerGen();
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddValidatorsFromAssembly(typeof(Startup).Assembly);
            services.AddAutoMapper(typeof(Startup));

            return services;
        }
    }
}