namespace Management.Dashboard.Services.Interfaces
{
    public interface IAiService
    {
        Task<string> GenerateAsync(string inputText);
    }
}
