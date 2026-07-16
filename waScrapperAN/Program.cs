using waScrapperAN.Configuration;
using waScrapperAN.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace waScrapperAN
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Validate configuration
            var options = new AduanaNacionalOptions();
            configuration.GetSection(AduanaNacionalOptions.SectionName).Bind(options);
            if (!options.IsValid())
            {
                MessageBox.Show(
                    "La configuración del servicio de Aduana Nacional es inválida. Verifique el archivo appsettings.json.",
                    "Error de configuración",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configuration);

            services.Configure<AduanaNacionalOptions>(
                configuration.GetSection(AduanaNacionalOptions.SectionName));

            services.AddHttpClient("AduanaNacional");

            services.AddSingleton<IDimDuiEvidenceWriter, DimDuiEvidenceWriter>();
            services.AddTransient<IDimDuiTrackingService, DimDuiTrackingService>();
            services.AddTransient<Form1>();

            using var serviceProvider = services.BuildServiceProvider();

            Application.Run(serviceProvider.GetRequiredService<Form1>());
        }
    }
}