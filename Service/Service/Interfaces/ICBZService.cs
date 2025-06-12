public interface ICBZService
{
    Task<bool> CreateCBZ(string title, string? comicInfoXml = null);
}
