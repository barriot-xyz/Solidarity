using Microsoft.Extensions.DependencyInjection;

namespace Solidarity
{
    public class Program
    {
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _provider;

        private readonly DiscordSocketConfig _config = new()
        {
            AlwaysDownloadUsers = true,
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers,
            LogLevel = LogSeverity.Info,
            SuppressUnknownDispatchWarnings = true,
        };

        private readonly InteractionServiceConfig _serviceConfig = new()
        {
            LogLevel = LogSeverity.Info,
            UseCompiledLambda = true,
        };

        public Program()
        {
            _provider = ConfigureServices();
            _client = _provider.GetRequiredService<DiscordSocketClient>();
        }

        static void Main()
            => new Program()
                .RunAsync()
                .GetAwaiter()
                .GetResult();

        public async Task RunAsync()
        {
            await _client.LoginAsync(TokenType.Bot, Config.Settings.Token);

            await _provider.GetRequiredService<AssemblyOperator>()
                .ConfigureAsync();

            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private IServiceProvider ConfigureServices()
            => new ServiceCollection()
                .AddSingleton(_config)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton(_serviceConfig)
                .AddSingleton<InteractionService>()
                .AddSingleton<AssemblyOperator>()
                .BuildServiceProvider();
    }
}
