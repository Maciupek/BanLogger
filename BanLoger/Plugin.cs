using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System;

namespace BanLogger
{
    public class Plugin : Plugin<BanLogger.Config>
    {
        private EventHandlers _eventHandlers;

        public override string Name { get; } = "Ban Logger";

        public override string Author { get; } = "Maciupek#4307 (Original: Jesus-QC)";

        public override string Prefix { get; } = "BanLogger";

        public override Version Version { get; } = new Version(1, 0, 8);

        public override Version RequiredExiledVersion { get; } = new Version(7, 0, 0);

        public static Plugin Instance { get; private set; }

        public override void OnEnabled()
        {
            Plugin.Instance = this;
            this._eventHandlers = new EventHandlers();
            Exiled.Events.Handlers.Player.Banning += new Exiled.Events.Events.CustomEventHandler<BanningEventArgs>(this._eventHandlers.OnBanning);
            Exiled.Events.Handlers.Player.Kicking += new Exiled.Events.Events.CustomEventHandler<KickingEventArgs>(this._eventHandlers.OnKicking);
            Exiled.Events.Handlers.Player.Banned += new Exiled.Events.Events.CustomEventHandler<BannedEventArgs>(this._eventHandlers.OnPlayerOBan);
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Banning -= new Exiled.Events.Events.CustomEventHandler<BanningEventArgs>(this._eventHandlers.OnBanning);
            Exiled.Events.Handlers.Player.Kicking -= new Exiled.Events.Events.CustomEventHandler<KickingEventArgs>(this._eventHandlers.OnKicking);
            Exiled.Events.Handlers.Player.Banned -= new Exiled.Events.Events.CustomEventHandler<BannedEventArgs>(this._eventHandlers.OnPlayerOBan);
            this._eventHandlers = (EventHandlers)null;
            Plugin.Instance = (Plugin)null;
            base.OnDisabled();
        }
    }
}