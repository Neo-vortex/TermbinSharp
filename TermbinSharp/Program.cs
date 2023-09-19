using MediatR;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.ResponseCompression;
using TermbinSharp;
using TermbinSharp.Utilities;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers(options =>
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer())));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddMemoryCache();
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder => 
        builder.Expire(TimeSpan.FromSeconds(3600)));
});
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
var app = builder.Build();

app.UseResponseCompression();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed( origin => true) 
    .AllowCredentials()); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

System.IO.Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Data"));

app.Run();