using System;
using csharp_prs_interfaces;
using Heroes.SDK;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Reloaded.Mod.Interfaces.Internal;

namespace sonicheroes.utils.toner;

public class Program : IMod
{
    private IReloadedHooks _hooks; // Library cannot be unloaded, no need for weak reference.
    private IModLoader _modLoader;
    private Toner _toner;

    public void Start(IModLoaderV1 loader)
    {
        _modLoader = (IModLoader)loader;
            
        /* Your mod code starts here. */
        _modLoader.GetController<IReloadedHooks>().TryGetTarget(out _hooks);
        _modLoader.GetController<IPrsInstance>().TryGetTarget(out var _prsInstance);
        SDK.Init(_hooks, _prsInstance);
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

    public static void Main() { }
}