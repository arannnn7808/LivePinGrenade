using System;
using HarmonyLib;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using LivePinGrenade.Handlers;

namespace LivePinGrenade
{
    public class LivePinGrenade : Plugin<Config>
    {
        private const string HarmonyId = "es.araangarciiia.livepingrenade";

        private readonly Harmony _harmony = new(HarmonyId);
        private GrenadeHandler _handler;

        public override string Name => "LivePinGrenade";
        public override string Description => "Starts the HE grenade fuse the moment the pin is pulled.";
        public override string Author => "araangarciiia";
        public override Version Version => new(1, 0, 0);
        public override Version RequiredApiVersion => LabApiProperties.CurrentVersion;

        public override void Enable()
        {
            _handler = new GrenadeHandler(Config);
            CustomHandlersManager.RegisterEventsHandler(_handler);
            _harmony.PatchAll();
        }

        public override void Disable()
        {
            _harmony.UnpatchAll(HarmonyId);
            CustomHandlersManager.UnregisterEventsHandler(_handler);
            _handler.Dispose();
            _handler = null;
        }
    }
}
