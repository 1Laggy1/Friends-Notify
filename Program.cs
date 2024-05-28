﻿using Discord;
using Discord.WebSocket;
using Discord.Net;
using Newtonsoft.Json;
using Friends_Notify;

namespace FriendsNotify;

public class Program
{
    private ulong guildId = 1;
    private string token = "";
    private DiscordSocketClient _client;
    private Commands commands = new Commands();
    private BotConfig botConfig;

    public static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        DiscordSocketConfig config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent | GatewayIntents.Guilds | GatewayIntents.GuildMembers | GatewayIntents.GuildPresences,
            AlwaysDownloadUsers = true
        };
        _client = new DiscordSocketClient(config);
        botConfig = LoadConfig("config.json");
        token = botConfig.Token;
        guildId = botConfig.GuildID;
        _client.Log += Log;
        _client.MessageReceived += HandleMessageReceived;
        _client.Ready += ReadyAsync;
        //_client.GuildMemberUpdated += GuildMemberUpdatedAsync;
        _client.PresenceUpdated += GuildMemberUpdatedAsync;
        _client.SlashCommandExecuted += SlashCommandHandler;

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private BotConfig LoadConfig(string path)
    {
        path = AppContext.BaseDirectory + @"config\" + path;
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Could not find {path}.");
        }

        var json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<BotConfig>(json);
    }

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        await command.DeferAsync();
        switch (command.Data.Name)
        {
            case "track":
                await commands.trackCommand(command);
                break;

            case "trackid":
                await commands.trackIDCommand(command, _client, guildId);
                break;
        }
    }

    private async Task BuildCommands()
    {
        var guild = _client.GetGuild(guildId);
        var guildBuilder = new SlashCommandBuilder()
            .WithName("track")
            .WithDescription("Start tracking the user")
            .AddOption("user", ApplicationCommandOptionType.User, "The user to track");
        var commandBuilder = new SlashCommandBuilder()
            .WithName("trackid")
            .WithDescription("Start tracking the user")
            .AddOption("userid", ApplicationCommandOptionType.String, "The ID of user to track");
        try
        {
            await _client.Rest.CreateGuildCommand(guildBuilder.Build(), guildId);
            await _client.Rest.CreateGlobalCommand(commandBuilder.Build());
        }
        catch (ApplicationCommandException exception)
        {
            var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
            Console.WriteLine(json);
        }
    }

    private async Task GuildMemberUpdatedAsync(SocketUser user, SocketPresence oldPresence, SocketPresence newPresence)
    {
        Console.WriteLine("Bot is ready.");
        ulong guildId = 659740413476995073;
        IGuild guild = _client.GetGuild(guildId);

        ulong userId = 401105413703073793;
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
        await BuildCommands();
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