using CMapi.StartUpConfig;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);



builder.AddStandardServices();
//builder.AddHealthCheckServices();
builder.AddAuthServices();
builder.AddCustomServices();
builder.AddFirebaseServices();

// check cors requiremnts for in production. REMOVE IN PRODUCTION
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowOrigin",
//        builder => builder.WithOrigins()
//                          .AllowAnyHeader()
//                          .AllowAnyMethod());
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.UseCors("AllowOrigin");

//app.MapHealthChecks("/health").AllowAnonymous();

app.Run();
        