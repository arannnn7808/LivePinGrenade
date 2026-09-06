using System.Collections.Generic;
using InventorySystem.Items.ThrowableProjectiles;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;

namespace LivePinGrenade.Handlers
{
    internal sealed class GrenadeHandler : CustomEventsHandler
    {
        public static GrenadeHandler Instance { get; private set; }

        private readonly Config _config;
        private readonly Dictionary<Player, PinnedGrenade> _grenades = new();

        public GrenadeHandler(Config config)
        {
            _config = config;
            Instance = this;
        }

        public void Dispose()
        {
            ClearAll();
            Instance = null;
        }

        public void PullPin(Player player)
        {
            if (_grenades.ContainsKey(player))
                return;

            var grenade = PinnedGrenade.Create(player, _config.FuseDuration, () => Discard(player));
            if (grenade != null)
                _grenades[player] = grenade;
        }

        public void ReinsertPin(Player player) => Discard(player);

        public override void OnPlayerThrewProjectile(PlayerThrewProjectileEventArgs ev)
        {
            if (!_grenades.TryGetValue(ev.Player, out var grenade))
                return;

            _grenades.Remove(ev.Player);

            if (ev.Projectile?.Base is TimeGrenade thrown)
                grenade.TransferFuseTo(thrown);

            grenade.Release();
        }

        public override void OnPlayerDying(PlayerDyingEventArgs ev) => Discard(ev.Player);

        public override void OnPlayerLeft(PlayerLeftEventArgs ev) => Discard(ev.Player);

        public override void OnServerRoundRestarted() => ClearAll();

        private void Discard(Player player)
        {
            if (!_grenades.TryGetValue(player, out var grenade))
                return;

            _grenades.Remove(player);
            grenade.Release();
        }

        private void ClearAll()
        {
            foreach (var grenade in _grenades.Values)
                grenade.Release();

            _grenades.Clear();
        }
    }
}
