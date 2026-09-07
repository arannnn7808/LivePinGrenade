# LivePinGrenade

[![LabAPI](https://img.shields.io/badge/LabAPI-1.1.7+-blue)](https://github.com/northwood-studios/LabAPI)
[![GitHub all releases](https://img.shields.io/github/downloads/arannnn7808/LivePinGrenade-LabApi/total)](https://github.com/arannnn7808/LivePinGrenade-LabApi/releases)
[![GitHub forks](https://img.shields.io/github/forks/arannnn7808/LivePinGrenade-LabApi)](https://github.com/arannnn7808/LivePinGrenade-LabApi/network/members)
[![GitHub](https://img.shields.io/github/license/arannnn7808/LivePinGrenade-LabApi)](https://github.com/arannnn7808/LivePinGrenade-LabApi/blob/master/LICENSE)

Starts the HE grenade fuse the moment the pin is pulled instead of when the projectile leaves your hand. Hold it too long and it cooks off in your hand; throw it and it flies with only the time that is left.

## Features

-   **Live Pin:** The fuse starts as soon as the pin is pulled, not when the grenade is thrown.
-   **Cook In Hand:** Holding the pin past the fuse time detonates the grenade on the holder.
-   **Remaining-Fuse Throws:** A thrown grenade only keeps the fuse time that was left when it was released.
-   **Configurable Fuse:** Set a custom fuse length or keep the game's default HE timing.
-   **Safe Cleanup:** Cancelling the throw, dying, disconnecting or a round restart clears any pending grenade.

## Dependencies

-   **LabAPI v1.1.7** or newer is required for this plugin.
-   **0Harmony**

## Installation

1.  Make sure you have **LabAPI v1.1.7** or a compatible version installed on your server.
2.  Download the latest release of `LivePinGrenade.dll` from the [**Releases Page**](https://github.com/arannnn7808/LivePinGrenade-LabApi/releases/latest).
3.  Place the downloaded `.dll` file into your server's plugin directory (`.config/SCP Secret Laboratory/LabApi/plugins/(server-port/global)`).
4.  Download dependencies zip, extract it and place the content into (`.config/SCP Secret Laboratory/LabApi/dependencies/(server-port/global)`).
5.  Restart the server. The configuration file will be generated on the first run.

## Configuration

After the first launch, `config.yml` will be created in your LabApi configuration folder.

| Key             | Default | Description                                                                                             |
|-----------------|---------|-------------------------------------------------------------------------------------------------------- |
| `fuse_duration` | `0`     | Fuse length in seconds, measured from the moment the pin is pulled. `0` keeps the game's default HE fuse. |

## Showcase
https://github.com/user-attachments/assets/1174416f-9cfa-4edc-b2ab-39ccf3efb7f7

## License

This project is licensed under the GNU General Public License v3.0. See the [LICENSE](https://github.com/arannnn7808/LivePinGrenade-LabApi/blob/master/LICENSE) file for details.
