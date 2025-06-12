using System.Data;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<BaseConfigurationSettings>()
    .BindConfiguration("BaseConfigurationSettings")
    .ValidateDataAnnotations()
    .Validate(settings =>
    {
        if (string.IsNullOrEmpty(settings.WorkingDirectory))
        {
            return false;
        }

        if (string.IsNullOrEmpty(settings.OutputDirectory))
        {
            return false;
        }

        if (!settings.Proxies.Any())
        {
            return false;
        }

        if (!Directory.Exists(settings.WorkingDirectory))
        {
            return false;
        }

        if (!Directory.Exists(settings.OutputDirectory))
        {
            return false;
        }

        return true;
    })
    .ValidateOnStart();

builder.Services.AddScoped<IDbConnection>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    return new NpgsqlConnection(connectionString);
});

builder.WebHost.UseUrls("http://localhost:5000");


// Data Workers
builder.Services.AddScoped<IMangaWorker, MangaWorker>();
builder.Services.AddScoped<IVolumeWorker, VolumeWorker>();
builder.Services.AddScoped<IChapterWorker, ChapterWorker>();
builder.Services.AddScoped<IJobQueueWorker, JobQueueWorker>();
builder.Services.AddScoped<ILogWorker, LogWorker>();

// Services
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IMangaService, MangaService>();
builder.Services.AddScoped<IVolumeService, VolumeService>();
builder.Services.AddScoped<IChapterService, ChapterService>();
builder.Services.AddScoped<IJobQueueService, JobQueueService>();
builder.Services.AddScoped<IMetadataService, MetadataService>();
builder.Services.AddScoped<ICBZService, CBZService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.MapControllers();

app.Run();
