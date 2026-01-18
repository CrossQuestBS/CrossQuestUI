# CrossQuestUI

UI for an alternative Beat Saber Quest modding ecosystem.

## Are you interested to help out with development, create mods or to test it out?
Join CrossQuest Discord server!

https://discord.gg/2vAFccFBsu

## Requirements to run
* Beat Saber Quest APK 1.42.0
* Beat Saber PC game (Available through cross-buy)
* [Unity Hub](https://unity.com/download)
* [Unity Editor 6000.0.40f1](https://unity.com/releases/editor/whats-new/6000.0.40f1#installs)
    * Android Build Support component
* [UnityBaseProject](https://github.com/CrossQuestBS/UnityBaseProject/tree/1-42-0)
* [apktool](https://apktool.org/)

## Mods

### OculusPatcher
**REQUIRED**

This mod fixes up DataModels.dll to reference correct OculusPlatformExtensions.

https://github.com/CrossQuestBS/OculusPatcher

### QuestPatcher
**REQUIRED**

This mod updates the game assemblies to have Quest specific code.

https://github.com/CrossQuestBS/QuestPatcher

### CustomSongEnabler

This mod patches the game assemblies to force enable CustomSongs, and to fetch the CustomLevel folder from OBB path.

https://github.com/CrossQuestBS/CustomSongEnabler

### ImageCoverExpander
First port of PC mod to test CrossQuest POC

https://github.com/CrossQuestBS/ImageCoverExpander