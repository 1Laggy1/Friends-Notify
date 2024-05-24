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
        //_client.GuildMemberUpdated += GuildMemberUpdatedAsync;
        _client.PresenceUpdated += GuildMemberUpdatedAsync;

        var token = "";

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private async Task GuildMemberUpdatedAsync(SocketUser user, SocketPresence oldPresence, SocketPresence newPresence)
    {
        Console.WriteLine("Bot is ready.");
        ulong guildId = 1;
        IGuild guild = _client.GetGuild(guildId);

        ulong userId = 1;
        IGuildUser userToSend = await guild.GetUserAsync(userId);
        Console.WriteLine("Finding user...");
        if (userToSend != null)
        {
            IActivity oldActivity = null;
            IActivity newActivity = null;
            if (oldPresence.Status != newPresence.Status)
            {
                await userToSend.SendMessageAsync($"{user.Username} is now {newPresence.Status}");
            }
            foreach (var activity in oldPresence.Activities)
            {
                if (activity.Type == ActivityType.Playing)
                {
                    oldActivity = activity;
                }
            }
            foreach (var activity in newPresence.Activities)
            {
                if (activity.Type == ActivityType.Playing)
                {
                    newActivity = activity;
                }
            }
            if (oldActivity != newActivity && newActivity != null)
            {
                await userToSend.SendMessageAsync($"{user.Username} is playing {newActivity.Name}");
            }
        }
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