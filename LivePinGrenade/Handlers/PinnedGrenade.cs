using System;
using System.Collections.Generic;
using Footprinting;
using InventorySystem;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.ThrowableProjectiles;
using LabApi.Features.Wrappers;
using MEC;
using Mirror;
using UnityEngine;
using Object = UnityEngine.Object;
using ThrowableItem = InventorySystem.Items.ThrowableProjectiles.ThrowableItem;

namespace LivePinGrenade.Handlers
{
    internal sealed class PinnedGrenade
    {
        private const float HeldScale = 0.01f;
        private const float FollowInterval = 0.1f;
        private const float FallbackFuse = 3f;

        private readonly Player _owner;
        private readonly ExplosionGrenade _grenade;
        private readonly Action _onExpired;
        private CoroutineHandle _follow;

        private PinnedGrenade(Player owner, ExplosionGrenade grenade, Action onExpired)
        {
            _owner = owner;
            _grenade = grenade;
            _onExpired = onExpired;
        }

        public static PinnedGrenade Create(Player owner, float fuseDuration, Action onExpired)
        {
            if (!InventoryItemLoader.TryGetItem(ItemType.GrenadeHE, out ThrowableItem item) || item.Projectile is not ExplosionGrenade prefab)
                return null;

            var grenade = Object.Instantiate(prefab);
            grenade.Info = new PickupSyncInfo(ItemType.GrenadeHE, item.Weight, 0, true);
            grenade.PreviousOwner = new Footprint(owner.ReferenceHub);
            grenade._fuseTime = ResolveFuse(fuseDuration, prefab._fuseTime);
            grenade.transform.position = owner.Position;
            grenade.transform.localScale = Vector3.one * HeldScale;

            NetworkServer.Spawn(grenade.gameObject);
            grenade.ServerActivate();

            if (grenade.TryGetComponent(out Rigidbody body))
            {
                body.useGravity = false;
                body.linearVelocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }

            var pinned = new PinnedGrenade(owner, grenade, onExpired);
            pinned._follow = Timing.RunCoroutine(pinned.FollowOwner());
            return pinned;
        }

        private bool Gone => _grenade == null || _grenade._alreadyDetonated || !NetworkServer.spawned.ContainsKey(_grenade.netId);

        public double RemainingFuse => Gone ? 0.0 : Math.Max(0.0, _grenade.TargetTime - NetworkTime.time);

        public void TransferFuseTo(TimeGrenade thrown)
        {
            thrown.TargetTime = NetworkTime.time + RemainingFuse;
        }

        public void Release()
        {
            Timing.KillCoroutines(_follow);

            if (!Gone)
                _grenade.DestroySelf();
        }

        private IEnumerator<float> FollowOwner()
        {
            while (_owner.IsAlive && !Gone)
            {
                _grenade.transform.position = _owner.Position;
                yield return Timing.WaitForSeconds(FollowInterval);
            }

            _onExpired?.Invoke();
        }

        private static float ResolveFuse(float configured, float prefabFuse)
        {
            if (configured > 0f)
                return configured;

            return prefabFuse > 0f ? prefabFuse : FallbackFuse;
        }
    }
}
