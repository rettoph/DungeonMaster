using Discord;
using Discord.Commands;
using DungeonMaster.Library.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Library.Modules
{
    public class RoleModule : BaseDungeonMasterCommandModule
    {
        public RoleModule(DungeonContext dungeon) : base(dungeon)
        {
        }

        [Command("role toggle")]
        public async Task Toggle(IRole role, IUser user = null)
        {
            user ??= this.Context.User;
            var guildUser = await this.Context.Guild.GetUserAsync(user.Id);

            if (guildUser.RoleIds.Contains(role.Id))
            { // Remove the role...
                await guildUser.RemoveRoleAsync(role);
                this.GuildMaster.AuditLogger.Log(message: $"Removed role {role.Mention} from {user.Mention}", color: Color.Green);
            }
            else
            { // Add the role...
                await guildUser.AddRoleAsync(role);
                this.GuildMaster.AuditLogger.Log(message: $"Added role {role.Mention} to {user.Mention}", color: Color.Green);
            }

            this.Context.Message?.AddReactionAsync(new Emoji("👍"));
        }
    }
}
