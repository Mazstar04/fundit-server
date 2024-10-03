using System.Reflection;
using fundit_server.Contexts;
using fundit_server.Implementations.Repositories;
using fundit_server.Implementations.Services;
using fundit_server.Interfaces.Repositories;
using fundit_server.Interfaces.Services;
using fundit_server.MappingProfiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace fundit_server.Extensions
{

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICampaignRepository, CampaignRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IWithdrawalRepository, WithdrawalRepository>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserIdentityService, UserIdentityService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IImageUploadService, ImageUploadService>();
            services.AddScoped<IAuthService, AuthService>(); 
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IWithdrawalService, WithdrawalService>();

            return services;
        }

        public static IServiceCollection AutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserProfile));
            services.AddAutoMapper(typeof(CampaignProfile));
            services.AddAutoMapper(typeof(PaymentProfile));
            services.AddAutoMapper(typeof(WithdrawalProfile));
            return services;
        }
    }
}