using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChronoTask
{
    public interface IProjectRepository
    {
        Task<List<Project>> LoadAsync();
        Task SaveAsync(IEnumerable<Project> projects);
    }

    public sealed class JsonProjectRepository : IProjectRepository
    {
        private readonly string _path;
        private readonly JsonSerializerOptions _opts = new() { WriteIndented = true };

        public JsonProjectRepository(string path) => _path = path;

        public async Task<List<Project>> LoadAsync()
        {
            if (!File.Exists(_path))
                return new();
            await using var s = File.OpenRead(_path);
            return await JsonSerializer.DeserializeAsync<List<Project>>(s, _opts) ?? new();
        }

        public async Task SaveAsync(IEnumerable<Project> projects)
        {
            await using var s = File.Create(_path);
            await JsonSerializer.SerializeAsync(s, projects, _opts);
        }
    }
}
