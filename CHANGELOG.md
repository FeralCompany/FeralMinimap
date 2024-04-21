# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/ 'Keep a Changelog, 1.1.0'),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html 'Semantic Versioning, 2.0.0').

## [Unreleased]

- Nothing yet.

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
