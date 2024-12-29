using CMapi.Authentication;
using CMLibrary.DataAccess;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

namespace CMapi.StartUpConfig;

public static class DependencyInjectionExtensions
{
    public static void AddStandardServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.AddSwaggerServices();
    }
    private static void AddSwaggerServices(this WebApplicationBuilder builder)
    {
        var securityScheme = new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Description = "JWT Authorization header info using bearer tokens",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        };

        var securityRequirement = new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "bearerAuth"
                    }
                },
                new string[] { }
            }
        };

        builder.Services.AddSwaggerGen(opts =>
        {
            opts.AddSecurityDefinition("bearerAuth", securityScheme);
            opts.AddSecurityRequirement(securityRequirement);
        });
    }

    public static void AddCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess>();
        builder.Services.AddScoped<IUserAccountData, UserAccountData>();
        builder.Services.AddScoped<ICrewDataAccess, CrewDataAccess>();
        builder.Services.AddScoped<IUserContractDataAccess, UserContractDataAccess>();
        builder.Services.AddScoped<IShipDataAccess, ShipDataAccess>();
    }

    public static void AddAuthServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(opts =>
        {
            opts.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "Firebase";
            options.DefaultChallengeScheme = "Firebase";
        })
            .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>("Firebase", null);
    }

    public static void AddFirebaseServices(this WebApplicationBuilder builder)
    {
        var firebaseApp = FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromJson($@"
            {{
                ""type"": ""{builder.Configuration.GetValue<string>("Firebase:Type")}"",
                ""project_id"": ""{builder.Configuration.GetValue<string>("Firebase:ProjectId")}"",
                ""private_key_id"": ""{builder.Configuration.GetValue<string>("Firebase:PrivateKeyId")}"",
                ""private_key"": ""{builder.Configuration.GetValue<string>("Firebase:PrivateKey")}"",
                ""client_email"": ""{builder.Configuration.GetValue<string>("Firebase:ClientEmail")}"",
                ""client_id"": ""{builder.Configuration.GetValue<string>("Firebase:ClientId")}"",
                ""auth_uri"": ""{builder.Configuration.GetValue<string>("Firebase:AuthUri")}"",
                ""token_uri"": ""{builder.Configuration.GetValue<string>("Firebase:TokenUri")}"",
                ""auth_provider_x509_cert_url"": ""{builder.Configuration.GetValue<string>("Firebase:AuthProviderX509CertUrl")}"",
                ""client_x509_cert_url"": ""{builder.Configuration.GetValue<string>("Firebase:ClientX509CertUrl")}"",
                ""universe_domain"": ""{builder.Configuration.GetValue<string>("Firebase:UniverseDomain")}""
            }}")
        });

        builder.Services.AddSingleton(firebaseApp);
    }
}
