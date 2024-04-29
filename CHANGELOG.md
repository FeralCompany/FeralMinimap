# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/ 'Keep a Changelog, 1.1.0'),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html 'Semantic Versioning, 2.0.0').

## [Unreleased]

### Added

- Added `Feed Quality` as a config option, allowing you to increase the size of the minimap feed, at the cost of performance.
  - Note: I have not noticed significant performance impact, but it likely depends on your system.

### Changes

- Lower maximum font size for `MinimapView` to 20.
- Removed render depth option. It was better replaced by the new Feed Quality option.

### Fixes

- Fixed some lifecycle issue, potential (often) re-registration of event handlers.
- Minimap text now scales with the minimap size properly.

## Version [v0.0.8] (2024-04-28)

### Added

- Added option to configure Aspect Ratio of the map.
- Added option to configure RenderDepth of the view. Higher values will increase the quality of the view, but may cause performance issues.

### Changed

- Lowered minimum font size for `MinimapView`

## Version [v0.0.7] (2024-04-23)

### Added

- Added a new config option to determine the camera's zoom level. Request [#2](https://github.com/FeralCompany/FeralMinimap/issues/2).

### Changed

- "North" is kind of whatever you want it to be, but I've decided to adopt GeneralImprovement's definition of "North" being 90 degrees.
    - I'm not sure what their logic was behind the decision, but I trust that Shaosil has a good reason for it.

## Version [v0.0.6] (2024-04-23)

### Fixed

- Use proper `FeralCommon` version in `thunderstore.toml`

### Changed

- Updated FeralCommon to `v0.1.3`

## Version [v0.0.5] (2024-04-23)

### Changed

- Better looking mod icon.
- Updated FeralCommon to `v0.1.1`

## Version [v0.0.4] (2024-04-23)

### Added

- Configs
    - Minimap Rotation - Allows you to rotate the minimap NESW, Default, or Target.
    - Minimap X/Y Offset - Allows you to move the minimap around the screen.
    - Minimap Inside Brightness - Allows you to change the brightness of the minimap when inside.
        - This defaults to 15, which is a decent compromise for Facility and Mansion tiles. Honestly, though, you may want to adjust this
          depending on what you get.

### Fixes

- Fixed a bug that could crash the game if you were not the host.
- Fixed a bug where target selection would not consider certain targets as valid, or skip them entirely.
- Fixed several lighting issues.
- Fixed several lifecycle bugs that caused issues when entering and exiting lobbies.

### Changed

- Actually everything. This is a complete rewrite of the mod now that I didn't feel so rushed.

## Version [v0.0.3] (2024-04-21)

### Fixes

- Fix a bug where you could not view the camera of a dead player
- Also fix an recursive issue when all players died.

## Version [v0.0.2] (2024-04-21)

### Fixes

- A (bad) lifecycle fix, where exiting and re-entering a lobby would cause the mod to stop working.
    - A better fix is in the works, but this should be a good band-aid for now.

## Version [v0.0.1] (2024-04-21)

### Added

1. No longer hijacks the ship monitor. No need to "unlock" your minimap before teleporting players.
2. Integration with [InputUtils][InputUtils] for button handling.
3. Integration with [LethalConfig][LethalConfig] for configuration.
4. View text indicates if you're viewing a Radar Booster or another player.
