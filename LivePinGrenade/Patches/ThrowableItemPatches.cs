using HarmonyLib;
using InventorySystem.Items.Pickups;
using LabApi.Features.Wrappers;
using LivePinGrenade.Handlers;
using Mirror;
using ThrowableItem = InventorySystem.Items.ThrowableProjectiles.ThrowableItem;

namespace LivePinGrenade.Patches
{
    [HarmonyPatch(typeof(ThrowableItem), nameof(ThrowableItem.ServerProcessInitiation))]
    internal static class ThrowableItemInitiationPatch
    {
        private static void Postfix(ThrowableItem __instance)
        {
            if (__instance.ItemTypeId != ItemType.GrenadeHE || !__instance.ThrowStopwatch.IsRunning)
                return;

            var owner = Player.Get(__instance.Owner);
            if (owner != null)
                GrenadeHandler.Instance?.PullPin(owner);
        }
    }

    [HarmonyPatch(typeof(ThrowableItem), nameof(ThrowableItem.ServerProcessCancellation))]
    internal static class ThrowableItemCancellationPatch
    {
        private static void Postfix(ThrowableItem __instance)
        {
            if (__instance.ItemTypeId != ItemType.GrenadeHE || !__instance.CancelStopwatch.IsRunning)
                return;

            var owner = Player.Get(__instance.Owner);
            if (owner != null)
                GrenadeHandler.Instance?.ReinsertPin(owner);
        }
    }

    [HarmonyPatch(typeof(ThrowableItem), nameof(ThrowableItem.OnRemoved))]
    internal static class ThrowableItemRemovedPatch
    {
        private static bool Prefix(ThrowableItem __instance, ItemPickupBase pickup)
        {
            if (__instance.ItemTypeId != ItemType.GrenadeHE || !NetworkServer.active || pickup == null || __instance._alreadyFired)
                return true;

            if (__instance.ScaledThrowElapsed < __instance._pinPullTime)
                return true;

            pickup.Info.Locked = true;
            pickup.DestroySelf();
            return false;
        }
    }
}
