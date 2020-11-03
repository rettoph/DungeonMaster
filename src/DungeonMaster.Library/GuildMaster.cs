using Discord;
using Discord.WebSocket;
using DungeonMaster.Library.Database;
using DungeonMaster.Library.Utilities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Library
{
    public class GuildMaster
    {
        #region Private Fields
        [NotMapped]
        private DungeonContext _context;

        [NotMapped]
        private ILogger<GuildMaster> _logger;
        #endregion

        #region Internal Properties
        [NotMapped]
        internal AuditLogger auditLogger { get; private set; }
        #endregion

        #region Public Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Int32 Id { get; set; }

        public SocketGuild Guild { get; set; }

        public ICategoryChannel AdminitrationCategoryChannel { get; set; }

        public ITextChannel AuditLogChannel { get; set; }

        public ITextChannel TogglePowerChannel { get; set; }

        public ICollection<ReactMenu> ReactMenus { get; set; }

        public AuditLogger AuditLogger => this.auditLogger;
        #endregion

        #region Constructors
        public GuildMaster(
            DungeonContext context)
        {
            _context = context;
            _logger = context.GetService<ILogger<GuildMaster>>();

            this.ReactMenus = new List<ReactMenu>();
        }
        #endregion

        #region Helper Methods
        public async Task InitializeAsync()
        {
            _logger.LogInformation($"Validating GuildMaster<{this.Guild.Id}>('{this.Guild.Name}')...");

            // First check if the Audit Channel exists. This is the only special initilaization process as everything else utilizes this channel...
            this.AuditLogChannel ??= await this.Guild.CreateTextChannelAsync("audit-log");
            this.auditLogger = new AuditLogger(this);
            
            // Ensure all other required administration objects exist...
            this.AdminitrationCategoryChannel ??= await this.CreateCategoryChannel("administration");
            await this.AuditLogChannel.ModifyAsync(gcp => gcp.CategoryId = this.AdminitrationCategoryChannel.Id);

            this.TogglePowerChannel ??= await this.CreateTextChannel("toggle-power", tcp =>
            {
                tcp.CategoryId = this.AdminitrationCategoryChannel.Id;
            });

            var menu = await this.CreateReactMenu(this.TogglePowerChannel);
            menu.CreateItem(new Emoji("👎"), "Admin", "Toggle Admin powers.", "!role toggle @Admin");
        }
        #endregion

        #region Helper Methods
        private async Task<T> DoAndLog<T>(Func<Task<T>> task, Func<T, String> log, IGuildUser sender = null, Color? color = null)
        {
            T result = await task();

            this.auditLogger.Log(log(result), sender, color);

            return result;
        }

        public async Task<ICategoryChannel> CreateCategoryChannel(String name, Action<GuildChannelProperties> func = null)
            => await this.DoAndLog<ICategoryChannel>(async () => await this.Guild.CreateCategoryChannelAsync(name, func), c => $"Created `<{c.Name}>` category channel.");

        public async Task<ITextChannel> CreateTextChannel(String name, Action<TextChannelProperties> func = null)
            => await this.DoAndLog<ITextChannel>(async () => await this.Guild.CreateTextChannelAsync(name, func), c => $"Created <#{c.Id}> text channel.");

        public async Task<ReactMenu> CreateReactMenu(ITextChannel channel)
            => await this.DoAndLog<ReactMenu>(
                task: async () =>
                {
                    var rm = _context.GetService<ReactMenu>();
                    rm.GuildMaster = this;
                    rm.Message = channel.SendMessageAsync(text: "...").Result;
                    this.ReactMenus.Add(rm);

                    return rm;
                },
                log: rm => $"Created new ReactMenu<{rm.Id}>()");
        #endregion
    }
}
