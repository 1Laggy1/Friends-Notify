using Discord;
using Discord.WebSocket;
using Friends_Notify.Models;
using Friends_Notify.Services;
using System.Data;

namespace Friends_Notify
{
    internal class Commands
    {
        private readonly UserService _userService;

        public Commands(UserService userService)
        {
            _userService = userService;
        }

        public async Task trackCommand(SocketSlashCommand command)
        {
            SocketGuildUser SocketUserToTrack = (SocketGuildUser)command.Data.Options.First().Value;

            SocketUser SocketUser = command.User;
            ulong userToTrackID = SocketUserToTrack.Id;
            ulong userID = SocketUser.Id;
            string answer = "";
            
            try
            {
                if (await _userService.GetUser(userID) == null)
                {
                    User user = new User();
                    user.Id = userID;
                    await _userService.AddUser(user);
                    await SocketUser.SendMessageAsync("You are now registered, and anybody can track you now");
                }
                if (!SocketUserToTrack.IsBot)
                {
                    if (await _userService.GetUser(userToTrackID) == null)
                    {
                        await SocketUserToTrack.SendMessageAsync("Somebody has attempted to track you, but you have not allowed it yet. If you want to allow users to track you simply use /track command in guild, or /trackID command here, and start tracking somebody.");
                        answer = "User that you were trying to track is not allowed tracking yet";
                        throw new Exception("User is not registered");
                    }
                }
                else
                {
                    if (await _userService.GetUser(userToTrackID) == null)
                    {
                        User user = new User();
                        user.Id = userToTrackID;
                        await _userService.AddUser(user);
                    }
                }
                await _userService.StartTrackingUser(userID, userToTrackID);
                answer = "You are now tracking the user " + "**" + SocketUserToTrack.GlobalName + "**";
                //SQLite.AddUserToTrack()
            }
            catch (Exception ex)
            {
                if (answer == "")
                {
                    answer = "Sorry, can't start tracking the user " + "**" + SocketUserToTrack.GlobalName + "**" + ". Check if provided user it's correct, or contact developers team.";
                }
                Console.WriteLine(ex);
            }
            await command.ModifyOriginalResponseAsync(properties =>
            {
                properties.Content = answer;
            });
        }

        public async Task trackIDCommand(SocketSlashCommand command, DiscordSocketClient _client, ulong guildID)
        {
            SocketUser SocketUser = command.User;
            ulong userToTrackID = ulong.Parse((string)command.Data.Options.First().Value);
            IGuildUser SocketUserToTrack;
            ulong userID = SocketUser.Id;
            string answer = "";

            try
            {
                IGuild guild = _client.GetGuild(guildID);
                SocketUserToTrack = await guild.GetUserAsync(userToTrackID);
                if (SocketUserToTrack == null)
                {
                    answer = "Provided DiscordID is not correct. Notice that you can use /track in guild instead of /trackID";
                    throw new Exception("UserID is not correct");
                }
                if (await _userService.GetUser(userID) == null)
                {
                    User user = new User();
                    user.Id = userID;
                    await _userService.AddUser(user);
                    await SocketUser.SendMessageAsync("You are now registered, and anybody can track you now");
                }
                if (!SocketUserToTrack.IsBot)
                {
                    if (await _userService.GetUser(userToTrackID) == null)
                    {
                        await SocketUserToTrack.SendMessageAsync("Somebody has attempted to track you, but you have not allowed it yet. If you want to allow users to track you simply use /track command in guild, or /trackID command here, and start tracking somebody.");
                        answer = "User that you were trying to track is not allowed tracking yet";
                        throw new Exception("User is not registered");
                    }
                }
                else
                {
                    if (await _userService.GetUser(userToTrackID) == null)
                    {
                        User user = new User();
                        user.Id = userToTrackID;
                        await _userService.AddUser(user);
                    }
                }
                await _userService.StartTrackingUser(userID, userToTrackID);
                answer = "You are now tracking the user " + "**" + SocketUserToTrack.GlobalName + "**";
                //SQLite.AddUserToTrack()
            }
            catch (Exception ex)
            {
                if (answer == "")
                {
                    answer = "Sorry, can't start tracking the user " + "**" + userToTrackID + "**" + ". Check if provided user it's correct, or contact developers team.";
                }
                Console.WriteLine(ex);
            }
            await command.ModifyOriginalResponseAsync(properties =>
            {
                properties.Content = answer;
            });
        }
    }
}