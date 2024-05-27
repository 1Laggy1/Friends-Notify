using Discord;
using Discord.WebSocket;

namespace Friends_Notify
{
    internal class Commands
    {
        public async Task trackCommand(SocketSlashCommand command)
        {
            SocketGuildUser user = (SocketGuildUser)command.Data.Options.First().Value;
            ulong userdID;
            string answer;

            try
            {
                userdID = user.Id;
                answer = "Tracking the user " + "**" + user.GlobalName + "**";
                //SQLite.AddUserToTrack()
            }
            catch (Exception ex)
            {
                answer = "Sorry, can't start tracking the user " + "**" + user.GlobalName + "**" + ". Check if provided user it's correct, or contact developers team";
                Console.WriteLine(ex);
            }
            await command.ModifyOriginalResponseAsync(properties =>
            {
                properties.Content = answer;
            });
        }

        public async Task trackIDCommand(SocketSlashCommand command, DiscordSocketClient _client, ulong guildID)
        {
            string user = "", answer = "";
            ulong userID;
            IGuild guild = _client.GetGuild(guildID);
            try
            {
                user = (String)command.Data.Options.First().Value;
                userID = ulong.Parse(user);
                IGuildUser userToSend = await guild.GetUserAsync(userID);
                if (userToSend == null)
                {
                    throw new Exception("Provided user was incorrect");
                }
                answer = "Tracking the user " + "**" + userToSend.GlobalName + "**";
                //SQLite.AddUserToTrack()
            }
            catch (Exception ex)
            {
                answer = "Sorry, can't start tracking the user " + "**" + user + "**" + ". Notice that this command using DiscordID instead of actual user, if you want to track with user, you need to use **/track** instead";
                Console.WriteLine(ex);
            }
            await command.ModifyOriginalResponseAsync(properties =>
            {
                properties.Content = answer;
            });
        }
    }
}