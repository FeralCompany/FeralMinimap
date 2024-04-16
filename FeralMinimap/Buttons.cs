using FeralCommon.Input;

namespace FeralMinimap;

public static class Buttons
{
    public static readonly ButtonToggle MinimapEnabled = new("MinimapEnabled", "Minimap Enabled", "<keyboard>/m", true);
    public static readonly ButtonPress SwitchTarget = new("SwitchTarget", "Switch Target", "<keyboard>/l");
}
