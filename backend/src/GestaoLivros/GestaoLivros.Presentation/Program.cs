using GestaoLivros.Application.Extensions;
using GestaoLivros.Infra.Data;
using GestaoLivros.Presentation.Conventions;
using GestaoLivros.Presentation.Extensions;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddOpenApi();
builder.Services.AddApplication(configuration);
builder.Services.AddAuthentication(configuration);

builder.Services.AddControllers(options => { options.Conventions.Add(new RoutePrefixConvention("api/v1")); });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddCors();

builder.Services.AddInfra(configuration);

var app = builder.Build();

QuestPDF.Settings.License = LicenseType.Community;

app.UseSwagger();
app.UseSwaggerUI();
app.MapOpenApi();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.MapControllers();

app.UseHttpsRedirection();

app.Run();