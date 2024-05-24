using Discord;
using Discord.WebSocket;
using System;
using System.Timers;

namespace FriendsNotify;

public class Program
{
    private DiscordSocketClient _client;

    public static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        DiscordSocketConfig config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent | GatewayIntents.Guilds | GatewayIntents.GuildMembers | GatewayIntents.GuildPresences,
            AlwaysDownloadUsers = true
        };
        _client = new DiscordSocketClient(config);

        _client.Log += Log;
        _client.MessageReceived += HandleMessageReceived;
        _client.Ready += ReadyAsync;

        var token = "";

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private async Task ReadyAsync()
    {
        Console.WriteLine("Bot is ready.");
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private async Task HandleMessageReceived(SocketMessage message)
    {
        if (message.Author.Id == _client.CurrentUser.Id)
            return;

        Console.WriteLine($"Повідомлення від {message.Author.Username}: {message.Content}");
    }
}