# Crossveil
A client side mod made for V Rising that allows you to fully replace the default crosshairs.

## Features
🎯 Swap default crosshairs with custom images<br>
💾 Import custom image collections<br>
🔥 Adjustable hotspot locations<br>
🎯 Scale crosshairs to your preferred size<br>
🔧 Configure options & swap collections from the ingame menu<br>
📌 Saves & reloads configured options automatically when closing and opening the game

## How It Works
Crossveil intercepts cursor rendering logic inside `SetCursorSystem.OnUpdate` and applies your chosen crosshair based on the current UI state. It uses Unity's `SetCursor()` to override visuals, and falls back to the default game crosshairs when disabled.

## Installing Crossveil

1️⃣ Navigate to the game install folder. Typically: `C:\Program Files (x86)\Steam\steamapps\common\VRising`<br>

2️⃣ Make sure you have the latest version of [BepInEx](https://thunderstore.io/c/v-rising/p/BepInEx/BepInExPack_V_Rising/) for V Rising, if installed for the first time make sure to run the game at least once and close it afterwards for the plugins & config directories to be created automatically.<br>

3️⃣ ️Place the `Crossveil.dll` file inside the `\BepInEx\plugins` folder.<br>

## Custom Collections
1️⃣ Make sure you have ran the game at least once since installing the mod.<br>

2️⃣ A folder called `\BepInEx\config\Crossveil` should have been automatically created.<br>

3️⃣ Each sub-folder inside containing **==at least one .png image==**  will be defined as and imported as its own collection of crosshairs when you start up V Rising.<br>

Example:
```
Directory of C:\Program Files (x86 \Steam\steamapps\common\VRising\BepInEx\config\Crossveil
04.05.2025  11:12    <DIR>          .
05.05.2025  08:39    <DIR>          ..
04.05.2025  11:10    <DIR>          Cats
04.05.2025  11:09    <DIR>          Memes
29.04.2025  06:11    <DIR>          Standard
```

## Included crosshairs
To get you started there will be a suite of preset crosshairs available by default.
<p align="center"><img src="https://i.ibb.co/cSC77K6J/Included-Crosshairs-Centered.png"/></p>

## Limitations
❗Software & Hardware Rendering<br>
Due to the nature of Unity Engine and how V Rising was built we primarily use hardware rendering for the cursors (default & custom). Textures are limited to the size of 32x32 pixels in this mode and cannot be overriden without using forced software rendering. This is automatically accounted for when using the **scaling** option, due to these limitations we do not scale the crosshair inside menus either because of unwanted behaviour and bugs.

✅  For best experience<br>
Use images that are even in size and has an obvious **hotspot** placement for aligning the click zone correctly to the crosshair image.

Is is recommended to keep crosshair scaling **off** due to potential negative impacts on performance and colouration of the images. Milage may vary here, so feel free to play around with it!

#### Credits
- [V Rising Modding Community](https://wiki.vrisingmods.com/)

#### License
[This project is licensed under the AGPL-3.0 license.](https://choosealicense.com/licenses/agpl-3.0/#)
