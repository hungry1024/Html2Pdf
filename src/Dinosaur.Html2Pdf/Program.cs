using Dinosaur.DinkToPdf;
using Dinosaur.DinkToPdf.Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

builder.Services.AddMemoryCache();

builder.Services.AddControllers().AddNewtonsoftJson(opts =>
{
    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    opts.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
});

builder.Services.AddHealthChecks();
builder.Services.AddRouting(opts => opts.LowercaseUrls = true);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dinosaur.Html2Pdf", Version = "v1" });
    c.IncludeXmlComments("xmls/Dinosaur.Html2Pdf.xml", true);
});

Task.Run(() =>
{
    string dir = Path.Combine(AppContext.BaseDirectory, "temp");
    foreach (string var in Directory.GetDirectories(dir))
    {
        Directory.Delete(var, true);
    }
    foreach (string file in Directory.GetFiles(dir))
    {
        File.Delete(file);
    }
});

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment() || "dev".Equals(app.Configuration["Environment"], StringComparison.OrdinalIgnoreCase))
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "文档转换服务 v1"));
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/health");
});

app.Run();
