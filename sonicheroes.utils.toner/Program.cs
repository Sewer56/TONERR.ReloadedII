using System;
using System.Diagnostics;
using Heroes.SDK;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Reloaded.Mod.Interfaces.Internal;

namespace sonicheroes.utils.toner
{
    public class Program : IMod, IExports
    {
        private IReloadedHooks _hooks; // Library cannot be unloaded, no need for weak reference.
        private IModLoader _modLoader;
        private Toner _toner;

        public static void Main() { }
        public void Start(IModLoaderV1 loader)
        {
            _modLoader = (IModLoader)loader;
            
            /* Your mod code starts here. */
            _modLoader.GetController<IReloadedHooks>().TryGetTarget(out _hooks);
            SDK.Init(_hooks);

            _toner = new Toner();
        }

        /* Mod loader actions. */
        public void Suspend() => _toner.Disable();
        public void Resume()  => _toner.Enable();
        public void Unload()  => _toner.Dispose();

        public bool CanUnload() => true;
        public bool CanSuspend() => true;

        /* Automatically called by the mod loader when the mod is about to be unloaded. */
        public Action Disposing { get; }
        public Type[] GetTypes() => new Type[0];
    }
}
