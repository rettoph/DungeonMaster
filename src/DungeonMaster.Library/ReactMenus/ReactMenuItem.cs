using Discord;
using Discord.WebSocket;
using DungeonMaster.Library.ReactMenus.Contexts;
using System;
using System.Threading.Tasks;

namespace DungeonMaster.Library.ReactMenus
{
    public class ReactMenuItem
    {
        private ReactMenuItemContext _context;

        public String Name => _context.Name;
        public String Description => _context.Description;
        public IEmote Icon => _context.Icon;

        public delegate void OnItemToggledDelegate(ReactMenu menu, ReactMenuItem item, SocketReaction reaction);

        public event OnItemToggledDelegate OnToggled;

        public void Toggle(ReactMenu menu, SocketReaction reaction)
            => this.OnToggled?.Invoke(menu, this, reaction);

        public void LoadContext(ReactMenuItemContext context)
            => _context = context;
    }
}