using System.IO.Compression;
using System.Text;
using Microsoft.Extensions.Options;

public class CBZService : ICBZService
{
    private readonly BaseConfigurationSettings _options;
    private readonly ILogService _logService;
    private readonly ILogger<CBZService> _logger;

    public CBZService(IOptions<BaseConfigurationSettings> options, ILogService logService, ILogger<CBZService> logger)
    {
        _options = options.Value;
        _logService = logService;
        _logger = logger;
    }

    public async Task<bool> CreateCBZ(string title, string? comicInfoXml = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            await _logService.CreateLog(new LogRequest { IsError = true, Message = "CBZService: Title cannot be null or empty" });
            _logger.LogError("Title cannot be null or empty.");

            return false;
        }

        var sourceDirectory = Path.Combine(_options.WorkingDirectory, title);
        var outputDirectory = _options.OutputDirectory;
        var sanitizedFileName = SanitizeFileName($"{title}.cbz");
        var outputPath = Path.Combine(outputDirectory, sanitizedFileName);

        if (!Directory.Exists(sourceDirectory))
        {
            await _logService.CreateLog(new LogRequest { IsError = true, Message = $"CBZService: Source directory not found: {sourceDirectory}" });
            _logger.LogError("Source directory not found: {SourceDir}", sourceDirectory);

            return false;
        }

        var maxAttempts = _options.CBZPackagerConfig.MaxAttempts;
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                Directory.CreateDirectory(outputDirectory);

                if (File.Exists(outputPath))
                {
                    await _logService.CreateLog(new LogRequest { IsWarning = true, Message = $"CBZService: Output file already exists, overwriting: {outputPath}" });
                    _logger.LogWarning("Output file already exists, overwriting: {Path}", outputPath);
                    File.Delete(outputPath);
                }

                using (var archive = ZipFile.Open(outputPath, ZipArchiveMode.Create))
                {
                    if (!string.IsNullOrEmpty(comicInfoXml))
                    {
                        var entry = archive.CreateEntry("ComicInfo.xml", CompressionLevel.Optimal);
                        using var writter = new StreamWriter(entry.Open());
                        await writter.WriteAsync(comicInfoXml);
                    }

                    var files = Directory.GetFiles(sourceDirectory, "*.*", SearchOption.TopDirectoryOnly);
                    Array.Sort(files, new NaturalStringComparer());

                    foreach (var file in files)
                    {
                        archive.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
                    }

                    await _logService.CreateLog(new LogRequest { IsInfo = true, Message = $"Successfully created CBZ for '{title}' at {outputPath}" });
                    _logger.LogInformation("Successfully created CBZ for '{Title}' at {Path}", title, outputPath);

                    return true;
                }
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(new LogRequest { IsError = true, Message = $"Attempt {attempt}/{maxAttempts} failed for '{title}" });
                _logger.LogError(ex, "Attempt {Attempt}/{Max} failed for '{Title}'", attempt, maxAttempts, title);
                if (attempt == maxAttempts)
                {
                    return false;
                }
                await Task.Delay(1000 * attempt);
            }
        }

        return false;
    }

    private static string SanitizeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sb = new StringBuilder(fileName.Length);
        foreach (char c in fileName)
        {
            sb.Append(invalidChars.Contains(c) ? '_' : c);
        }
        return sb.ToString();
    }
}
