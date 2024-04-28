using BepInEx;
using FeralCommon;
using FeralCommon.Utils.Extensions;
using LethalCompanyInputUtils.Api;

namespace FeralMinimap;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(FeralCommon.MyPluginInfo.PLUGIN_GUID)]
public class FeralMinimapPlugin : FeralPlugin
{
    protected override void Load()
    {
        RegisterConfigs(typeof(Config));
        RegisterButtons(typeof(Buttons));

        var binder = new BinderWorkAround(this);
        CompleteWorkAroundPartTwo(binder);

        Harmony.PatchNamespace("FeralMinimap.Patches");
    }

    private class BinderWorkAround(FeralPlugin plugin) : LcInputActions
    {
        public override void CreateInputActions(in InputActionMapBuilder builder)
        {
            plugin.CompleteWorkAroundPartOne(in builder);
        }
    }
}
