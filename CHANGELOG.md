# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/ 'Keep a Changelog, 1.1.0'),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html 'Semantic Versioning, 2.0.0').

## [Unreleased]

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
