# LichFix
This mod is designed to fix / balance some of the problems facing by Lich players in Pathfinder: WotR. 

**Version 1.3.0 Rework** ( EVB Curation version, Permission granted by theDD as shown on the NexusMods page for the OG Mod )

This is a purely compatability rework at the moment to allow players to have the main parts of LichFix in a 1.3 compliant format. It takes the original versions of patch 1.1.x and the partial rework from patch 1.2.0+ ( by Kurufivne ) and builds on them with some slight changes to produce an updated version of the mod for patch 1.3.x and hopefully onwards. It also removed the Blessing of Unlife Functionality that 1.1/1.2 had due it it being neither Tabletop legal or indeed matching current Patch Game State ( neither allow Lord Beyond The Grave to work with BoU or indeed self buff, which the way it had been coded to work it was causing both to happen ).
 
This version is produced with the full permission of theDD ( see comment to this effect at https://www.nexusmods.com/pathfinderwrathoftherighteous/mods/210 ) and will be published onwards with a GPL Licence in effect to allow others to take over if required with a full trackable source to work with and/or a rotating team of modders to curate and upkeep the mod when needed without Licence/Permission issues as long as proper credit is stated.

The Mod itself is provided in an "as seen, use at own choice and risk" format, but has been checked for both general function and also no conflict with the major QOL mods like BubbleBuffs/Tweaks, Visual Adjustments, Tabletop Tweaks and Toybox.

While there should be no issues, please note that it's a mod and the game itself bugs out never mind when you start changing things. It's also my first mod on top of it being basedon theDDs' first mod so "mibbes good, mibbes arggggh" is a possibility. ;)


**Version 1.3.0 Rework changes and patching**
 
- **Lord Beyond The Grave**
  1. Stat Toggles disabled till code fully cleaned and working. LBTG/BOU interaction disabled as it no longer matches either TT, RAI or 1.3 Game Balance.
- **Eye of the Bodak**
  1. Works as Intended ( 1.3 update for broken GUID ).
  2. GFX again displays on cast/duration ( 1.3 update for broken GUID ).
- **Blessing of Unlife**
  1. All mod patching/adaptation of the ability has been removed currently as it relied on forcing UndeadType to function and this in turn broke intended game functionality on Lord Beyond The Grave in combination with it allowing both "Live" Companions and the MC to gain/regain bonuses from said ability against Balance/RAW/RAI/TT rulings/intentions.

**Credits**

- theDD : Original Author and Creator, coder of versions 1.0.0 through 1.2.0.
- KuruFivne : Modder who stepped in when I was stuck and created the patch 1.2.0+ working "bodge" version most of us used in 1.2.1 and was the basis for the 1.3.0+ compliant version.
- Evilvonbek ( aka Madlampy on the server ) : Author and mod-botherer for version 1.3.0.
- Sparhawk : massive help with sorting the Localization code which kept blowing up on me.
- AlterAsc/Bubbles/Vek : for teaching me about casts and types without me even realising and how to get the mod to pass values properly.
- The three above plus WraithJT, Balkoth, Wolfie and many others in the Tech Channel in the discord for helping me get this from "Argh, it's borked" to "heh, this actually works" without shouting at me too many times.

Owlcat for the game ofc...
 
**Patch Notes for previous versions and also comments from theDD**

*This is my first mod, so it may have bugs on updates on the game. Try remove it from the mod folder if your game crashed.

*Download: https://www.nexusmods.com/pathfinderwrathoftherighteous/mods/210* **this is the link for the OG Mod which has issues for 1.3 fucntion and balance but will be left in for credit to theDD as the original author**

**version 1.2.0**
- Lord beyond the Grave
  1. You can now choose the abilities affected by Lord beyond the Grave in the mod menu.
  *Please be reminded that you need to restart after you make changes in the mod menu.

**version 1.1.1a**
- Blessing of Unlife
  1. Add an option to fix the Double Saving Bonus if WorldCrawl is also installed.

**version 1.1**

- Option menu UI improved, press Ctrl + F10 to adjust setting in game

- Eye of the Bodak:
  1. Works as intended now (-1 level per round)
  2. Removed Gaze Attack / Death Descriptor, so it works when the Lich have such immunities. It will however, pass through the immunities of enemies.

- Eclipse Chill:
  1. DC now equal to 10 + Character Level + Mythic Level

- Tainted Sneak Attack:
  1. DC now equal to 10 + Character Level + Mythic Level

- Negative Eruption:
  1. Maximum Damage set to 250 when Caster Level is 25.
  2. Please be noted that there are no limitation on the maximum damage originally (even it is Max. 150 in description), so this is for balance only

**version 1.0.0**

As a lich player, I found that some of the core abilities / spells have bugs or have to be balanced. It shall allow you to have more fun.

- corrupted blood: 
  1. now have save on cast / trigger
  2. damage reduced (but progress with caster level)
  3. adjustable range (5 feet is recommended)
  4. metamagic selective now works on the corrupted blood trigger

- Lord beyond the grave:
  1. work on LIVING companions and main character if the have the buff of Blessing of Unlife. (you may want to turn it off if you think it is too OP).

- Eye of the Bodak:
  1. It will now show FX effect (same as the death gaze)
  2. floating number will now shows when negative level is applied on enemies.

- Insightful Contemplation (Court Poet Skill):
  1. As my Ember is a court poet, I found that something is going wrong if there is no FX effect when she play a song.
