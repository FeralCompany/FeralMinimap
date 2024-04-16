using FeralCommon.Config;

namespace FeralMinimap;

public static class Config
{
    public static class Minimap
    {
        public static readonly IntConfig Size = new IntConfig("Minimap", "Size")
            .WithDescription("The size (in px) of the minimap.")
            .WithMin(100)
            .WithMax(500)
            .WithDefaultValue(200)
            .NoSlider();

        public static readonly IntConfig Brightness = new IntConfig("Minimap", "Brightness")
            .WithDescription("The brightness of the minimap.")
            .WithMin(0)
            .WithMax(100)
            .WithDefaultValue(16)
            .NoSlider();
    }

    public static class Fixes
    {
        public static readonly BoolConfig AlwaysShowRadarBoosters = new BoolConfig("Fixes", "Always Show Radar Boosters")
            .WithDescription("""
                             Always show radar boosters on the minimap, even when they're not active.

                             The default behavior is to only show radar boosters on the minimap when they're active or when they have never been
                             activated (because they're visible by default).

                             Enabling this option will make radar boosters always visible on the minimap, regardless of their state.
                             """)
            .WithDefaultValue(true);
    }
}
