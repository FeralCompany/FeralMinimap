using FeralCommon.Config;
using FeralMinimap.Util;

namespace FeralMinimap;

public static class Config
{
    public static class Minimap
    {
        private const float AspectRatio = 4F / 3F;

        public static readonly EnumConfig<RotationStrategy> Rotation = new EnumConfig<RotationStrategy>("Minimap", "Rotation")
            .WithDescription("""
                             Determines in which orientation the minimap is rotated.

                             Default is a north-west direction at 315 degrees.

                             Target will rotate the minimap to match the target's current orientation.

                             The cardinal directions (NESW) will rotate the minimap to face the corresponding direction.
                             """
            )
            .WithDefaultValue(RotationStrategy.Default);

        public static readonly IntConfig Size = new IntConfig("Minimap", "Size")
            .WithDescription("The size (in px) of the minimap.")
            .WithMin(100)
            .WithMax(500)
            .WithDefaultValue(200)
            .NoSlider();

        public static readonly IntConfig XPosOffset = new IntConfig("Minimap", "X Position Offset")
            .WithDescription("""
                             The horizontal offset of the minimap from the right side of the screen.

                             0 represents being flush with the right side of the screen. Positive values move the minimap to the left.
                             """
            );

        public static readonly IntConfig YPosOffset = new IntConfig("Minimap", "Y Position Offset")
            .WithDescription("""
                             The vertical offset of the minimap from the top of the screen.

                             0 represents being flush with the top of the screen. Positive values move the minimap down.
                             """
            );

        public static readonly IntConfig InsideBrightness = new IntConfig("Minimap", "Facility Brightness")
            .WithDescription("Sets the brightness of the minimap when the current view is within the facility.")
            .WithMin(0)
            .WithMax(100)
            .WithDefaultValue(15)
            .NoSlider();

        public static int Height => Size;
        public static int Width => (int)(Size * AspectRatio);
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
