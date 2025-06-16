using Microsoft.Extensions.Configuration;

namespace Vectaurant.Shared
{
    public static class ConfigurationLoader
    {
        public static IConfiguration LoadConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
        }

        public static T GetSection<T>(string sectionName) where T : new()
        {
            var configuration = LoadConfig();
            var section = new T();
            configuration.GetSection(sectionName).Bind(section);
            return section;
        }
    }
}
