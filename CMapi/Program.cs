using CMapi.StartUpConfig;

var builder = WebApplication.CreateBuilder(args);

builder.AddStandardServices();
//builder.AddAuthServices();
//builder.AddHealthCheckServices();
builder.AddCustomServices();

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

app.UseAuthorization();


app.MapControllers();
app.UseCors("AllowOrigin");

//app.MapHealthChecks("/health").AllowAnonymous();

app.Run();
        