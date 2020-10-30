using Discord;
using DungeonMaster.Library.ReactMenus;
using DungeonMaster.Library.ReactMenus.Contexts;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonMaster.Library.ServiceLists
{
    public class ReactMenuList : ServiceList<ReactMenu>
    {
        #region Private Fields
        private AuditLog _log;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _log);

            this.OnAdd += this.HandleMenuAdded;
        }
        #endregion

        #region Factory Methods
        public ReactMenu GetByIdOrCreate(Guid? id, Func<ReactMenuContext> creator, ITextChannel fallbackChannel = null)
            => id == null ? this.Create(creator, fallbackChannel) : this.GetById(id.Value) ?? this.Create((rm, p, d) =>
            {
                rm.Id = id.Value;
                rm.TryLoadContext(creator(), fallbackChannel);
                _log.Log($"Created new ReactMenu<{rm.Name}>.", Color.Green);
            });

        public ReactMenu Create(Func<ReactMenuContext> creator, ITextChannel fallbackChannel = null)
            => this.Create((rm, p, d) =>
            {
                rm.TryLoadContext(creator(), fallbackChannel);
                _log.Log($"Created new ReactMenu<{rm.Name}>.", Color.Green);
            });
        #endregion

        #region Event Handlers
        private void HandleMenuAdded(ReactMenu item)
        {
            if (!item.Ready)
                item.TryRelease();

        }
        #endregion
    }
}
