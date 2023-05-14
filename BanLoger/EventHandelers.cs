using BanLogger.Features;
using BanLogger.Features.Enums;
using BanLogger.Features.Structs;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System;
using System.Globalization;

namespace BanLogger
{
    public class EventHandlers
    {
        public void OnBanning(BanningEventArgs ev)
        {
            if (Plugin.Instance.Config.PrivateWebhooks.ContainsKey(MessageType.Ban) && Plugin.Instance.Config.PrivateWebhooks[MessageType.Ban] != DiscordHandler.DefaultUrl)
                Utils.CreateEmbed(new BanInfo(new UserInfo(ev.Target.Nickname, ev.Target.UserId), new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"), ev.Reason, ev.Duration), MessageType.Ban, WebhookType.Private);
            if (!Plugin.Instance.Config.PublicWebhooks.ContainsKey(MessageType.Ban) || !(Plugin.Instance.Config.PublicWebhooks[MessageType.Ban] != DiscordHandler.DefaultUrl))
                return;
            Utils.CreateEmbed(new BanInfo(new UserInfo(ev.Target.Nickname, ev.Target.UserId), new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"), ev.Reason, ev.Duration), MessageType.Ban, WebhookType.Public);
        }

        public void OnKicking(KickingEventArgs ev)
        {
            if (Plugin.Instance.Config.PrivateWebhooks.ContainsKey(MessageType.Kick) && Plugin.Instance.Config.PrivateWebhooks[MessageType.Kick] != DiscordHandler.DefaultUrl)
                Utils.CreateEmbed(new BanInfo(new UserInfo(ev.Target.Nickname, ev.Target.UserId), new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"), ev.Reason, -1L), MessageType.Kick, WebhookType.Private);
            if (!Plugin.Instance.Config.PublicWebhooks.ContainsKey(MessageType.Kick) || !(Plugin.Instance.Config.PublicWebhooks[MessageType.Kick] != DiscordHandler.DefaultUrl))
                return;
            Utils.CreateEmbed(new BanInfo(new UserInfo(ev.Target.Nickname, ev.Target.UserId), new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"), ev.Reason, -1L), MessageType.Kick, WebhookType.Public);
        }

        public void OnMuted(ChangingMuteStatusEventArgs ev)
        {
            if (!ev.IsMuted)
                return;
            if (Plugin.Instance.Config.PrivateWebhooks.ContainsKey(MessageType.Mute) && Plugin.Instance.Config.PrivateWebhooks[MessageType.Mute] != DiscordHandler.DefaultUrl)
                Utils.CreateEmbed(new BanInfo(new UserInfo(ev.Player.Nickname, ev.Player.UserId), new UserInfo("n/a", "n/a"), "mute", -1L), MessageType.Mute, WebhookType.Private);
            if (Plugin.Instance.Config.PublicWebhooks.ContainsKey(MessageType.Mute) && Plugin.Instance.Config.PublicWebhooks[MessageType.Mute] != DiscordHandler.DefaultUrl)
                Utils.CreateEmbed(new BanInfo(new UserInfo(ev.Player.Nickname, ev.Player.UserId), new UserInfo("n/a", "n/a"), "mute", -1L), MessageType.Mute, WebhookType.Public);
        }

        public void OnPlayerOBan(BannedEventArgs ev)
        {
            if (ev.Type != 0 || !(ev.Details.OriginalName == "Offline Ban - Unknown"))
                return;
            long result;
            long duration = long.TryParse(TimeSpan.FromTicks(ev.Details.Expires - ev.Details.IssuanceTime).TotalSeconds.ToString((IFormatProvider)CultureInfo.InvariantCulture), out result) ? result : -1L;
            if (ev.Details.Id.Contains("@steam") && !string.IsNullOrEmpty(Plugin.Instance.Config.SteamApiKey))
            {
                try
                {
                    string userName = Utils.GetUserName(ev.Details.Id);
                    if (Plugin.Instance.Config.PrivateWebhooks.ContainsKey(MessageType.OBan) && Plugin.Instance.Config.PrivateWebhooks[MessageType.OBan] != DiscordHandler.DefaultUrl)
                        Utils.CreateEmbed(new BanInfo(new UserInfo(userName, ev.Details.Id), new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"), ev.Details.Reason, duration), MessageType.OBan, WebhookType.Private);
                    if (Plugin.Instance.Config.PublicWebhooks.ContainsKey(MessageType.OBan) && Plugin.Instance.Config.PublicWebhooks[MessageType.OBan] != DiscordHandler.DefaultUrl)
                        Utils.CreateEmbed(new BanInfo(new UserInfo(userName, ev.Details.Id), new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"), ev.Details.Reason, duration), MessageType.OBan, WebhookType.Public);
                }
                catch (Exception ex)
                {
                    Log.Error("An error has ocurred trying to get the username of an obaned user.");
                    if (Plugin.Instance.Config.PrivateWebhooks.ContainsKey(MessageType.OBan) && Plugin.Instance.Config.PrivateWebhooks[MessageType.OBan] != DiscordHandler.DefaultUrl)
                        Utils.CreateEmbed(new BanInfo(new UserInfo("Unknown (Incorrect API Key)", ev.Details.Id), new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"), ev.Details.Reason, duration), MessageType.OBan, WebhookType.Private);
                    if (Plugin.Instance.Config.PublicWebhooks.ContainsKey(MessageType.OBan) && Plugin.Instance.Config.PublicWebhooks[MessageType.OBan] != DiscordHandler.DefaultUrl)
                        Utils.CreateEmbed(new BanInfo(new UserInfo("Unknown (Incorrect API Key)", ev.Details.Id), new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"), ev.Details.Reason, duration), MessageType.OBan, WebhookType.Public);
                }
            }
            else
            {
                if (Plugin.Instance.Config.PrivateWebhooks.ContainsKey(MessageType.OBan) && Plugin.Instance.Config.PrivateWebhooks[MessageType.OBan] != DiscordHandler.DefaultUrl)
                    Utils.CreateEmbed(new BanInfo(new UserInfo("Unknown (OBan)", ev.Details.Id), new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"), ev.Details.Reason, duration), MessageType.OBan, WebhookType.Private);
                if (Plugin.Instance.Config.PublicWebhooks.ContainsKey(MessageType.OBan) && Plugin.Instance.Config.PublicWebhooks[MessageType.OBan] != DiscordHandler.DefaultUrl)
                    Utils.CreateEmbed(new BanInfo(new UserInfo("Unknown (OBan)", ev.Details.Id), new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"), ev.Details.Reason, duration), MessageType.OBan, WebhookType.Public);
            }
        }
    }
}