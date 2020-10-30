using Discord;
using Discord.WebSocket;
using Guppy;
using Guppy.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.Extensions.Collections;
using System.Threading.Tasks;
using System.Linq;
using Discord.Net;
using System.Runtime.CompilerServices;
using DungeonMaster.Library.ReactMenus.Contexts;

namespace DungeonMaster.Library.ReactMenus
{
    public class ReactMenu : Service
    {
        #region Private Fields
        private ReactMenuContext _context;
        private Boolean _ready;
        private ServiceProvider _provider;
        private Dictionary<String, ReactMenuItem> _items;
        private AuditLog _log;
        private DiscordSocketClient _client;
        private Config _config;

        private IUserMessage _message;
        private ITextChannel _channel;
        #endregion

        #region Public Properties
        public UInt64 MessageId => _message.Id;

        public String Name => _context.Name;

        public String Description => _context.Description;

        public IReadOnlyDictionary<String, ReactMenuItem> Items => _items;
        public Boolean Ready => _ready;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _provider = provider;
            _items = new Dictionary<String, ReactMenuItem>();

            provider.Service(out _log);
            provider.Service(out _client);
            provider.Service(out _config);
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.TryFlushMenu();
        }

        protected override void Release()
        {
            base.Release();

            _message?.DeleteAsync();
            _config.ReactMenus.Remove(_context);
            _config.Flush();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Load the recieved context data into the current react menu instance.
        /// </summary>
        /// <param name="restored">Defines a pre-exising react menu.</param>
        /// <param name="creator">Creates a brand new react menu if one is not given.</param>
        public Boolean TryLoadContext(ReactMenuContext context, ITextChannel fallbackChannel = null)
        {
            _context = context;
            _channel = _client.GetChannel(_context.ChannelId) as ITextChannel ?? fallbackChannel;
            if (_channel == null)
                return false;

            try
            {
                _message = _channel.GetMessageAsync(_context.MessageId).GetAwaiter().GetResult() as IUserMessage ?? _channel.SendMessageAsync(text: "...").GetAwaiter().GetResult();
            }
            catch(Exception e)
            {
                _message = _channel.SendMessageAsync(text: "...").GetAwaiter().GetResult();
            }
            _context.ChannelId = _channel.Id;
            _context.MessageId = _message.Id;
            _context.Id = this.Id = _context.Id ?? this.Id;

            // Add all menu items.
            _context.Items.ForEach(rmic =>
            {
                this.AddOrUpdateReaction(rmic);
            });

            if (_config.ReactMenus.Add(_context))
                _config.Flush();

            return _ready = true;
        }

        private Embed GetEmbed()
        {
            var builder = new EmbedBuilder();
            builder.Title = this.Name;
            builder.Description = this.Description;
            
            this.Items.Values.ForEach(rmi =>
            {
                var field = new EmbedFieldBuilder();
                field.Name = "\u200B";
                field.Value = $"{rmi.Icon.ToString()} - **{rmi.Name}**{(rmi.Description == null ? "" : $": *{rmi.Description}*")}";
            
                builder.AddField(field);
            });
            
            return builder.Build();
        }

        public void AddOrUpdateReaction(String name, String description, IEmote icon)
            => this.AddOrUpdateReaction(new ReactMenuItemContext()
            {
                Name = name,
                Description = description,
                Icon = icon
            });

        public void AddOrUpdateReaction(ReactMenuItemContext context)
        {
            if(_items.ContainsKey(context.Name))
                _items[context.Name].LoadContext(context);
            else 
                _items.Add(context.Name, _provider.GetService<ReactMenuItem>((rmi, p, d) =>
                {
                    rmi.LoadContext(context);
                }));

            this.TryFlushMenu();
        }

        private Boolean TryFlushMenu()
        {
            if (_ready)
            {
                _config.Flush();

                _message.ModifyAsync(mp =>
                {
                    mp.Content = null;
                    mp.Embed = this.GetEmbed();
                }).GetAwaiter().GetResult();
                
                _message.AddReactionsAsync(this.Items.Values.Select(rmi => rmi.Icon).ToArray()).GetAwaiter().GetResult();

                return true;
            }

            return false;
        }

        internal void ToggleReaction(SocketReaction reaction)
            => this.Items.Values.First(rmi => rmi.Icon.Equals(reaction.Emote)).Toggle(this, reaction);
        #endregion
    }
}
