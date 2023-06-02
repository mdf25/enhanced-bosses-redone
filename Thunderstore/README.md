# Enhanced Bosses Redone
## An overhaul of Blumaye's Enhanced Bosses
This mod expands the current abilities of existing bosses and adds some new and unique attacks to most of them.

## Configuration
All bosses have their main attack settings configurable in a JSON file. You can enable or disable any attack, control their frequency, or set them to trigger when the boss reaches a certain threshold to add phases to the fight if desired.

Additionally all bosses except The Queen have a special summoning attack that can be configured. She wanted it all, but I gave her nothin'. My bad... You can choose what type and how many monsters get summoned to aid the boss.

Some bosses can recover health or generate shields. These values of health gain, shield absorption and shield time are all adjustable.

Attack frequency can optionally be tuned to increase or decrease throughout the fight, depending on how much health the boss has. Make Bonemass strike you faster, or have The Elder summon Greydwarves and roots more often, or go the other way and make the attacks less frequent as the health changes. The flexibility is yours.

More configuration options have been made available in the .cfg file for certain paramters, see the rest of the readme for details on those. Wait, who READS these days? Biatch, I got audiobooks for that...

## JSON Configurations for attacks ##

**Important Note:** If you intend to change the JSON configuration files in any way, it is *highly recommended* that you copy the default `eb_settings.json` file, paste it into the same directory, and rename it to something else. Then in the main config file, change the name of where the config is loaded from. This will prevent updates from reverting changes to the original JSON file.

All attacks can specify the following parameters:
- `Desc`: A description of the attack - only really needed to tell you what the attack does, but serves no purpose in game.
- `Enabled`: Set to true to enable the attack, or false to completely disable it.
- `Cooldown`: After the attack finishes, this is how long the boss must wait before using that attack again. A number greater than 0.
- `HpThreshold`: The health percentage the boss needs to reach before the attack can be selected. A number between 0 and 1. Example: 0.6 means the boss cannot use this attack until their health is at 60% or lower.
- `AttackCoolDownMultiplier`: As the boss HP lowers from the HP Threshold, the cooldown will be affected by multiplying the cooldown by this value. Settings between 0 and 1 make the boss use this attack faster as their health lowers. 1 leaves the cooldown unchanged. Values above 1 will make the attack happen less as their HP lowers.
- `Stars`: If **Creature Level & Loot Control** is installed, this will limit the attack to only be usable on bosses that have at least the specified number of stars or better.

Summoning attacks can specify these parameters:
- `maxMinionsCount`: The maximum number of summons in a single player game. If there are already this number of summons, the boss cannot summon more.
- `extraMaxMinionsCountPerPlayer`: For every additional player, this value is added to maxMinionsCount. 
- `spawnMinionsCount`: How many minions get spawned in a single player game when the boss uses this attack.
- `extraSpawnMinionsPerPlayer`: For every additional player, this value is added to spawnMinionsCount.
- `Creatures`: This is an array of creatures spawned with their minimum and maximum thresholds. See Creature Array for further details.
- `SummonMaxStars`: The highest number of stars a creature can have when summoned by the boss. Max 2 stars, unless CLLC is installed, then it's 5 stars. If not defined, creatures won't be summoned with stars at all. If -1 is set, the creature stars will be the same as the boss stars.

Shield attacks can specify these parameters:
- `ShieldHp`: How much HP the shield has when it is generated.
- `ShieldDuration`: The time in seconds that the shield lasts.

Finally, some attacks have special parameters:
- `Heal`: The amount of health the boss receives when using this attack.
- `Dynamic`: Some attacks change slightly over the course of a boss battle. You can enable or disable this behaviour.

### Creature Arrays ###

The `Creatures` entry requires an array of entries. These entries look like this:
```"{Prefab Name}:{Min HP Threshold}:{Max HP Threshold}"```
An example entry of the array would be
```"Greydwarf:0.3:1"```
Here is how you read this:
- `Prefab Name`: The name of the prefab to spawn in - in this case, it's a `Greydwarf`.
- `Min HP Threshold`: If the boss HP percentage multiplied by the HpThreshold is lower than this value, this creature will not be summoned. 
- `Nax HP Threshold`: If the boss HP percentage multiplied by the HpThreshold is higher than this value, the creature will not be summoned.

### Full config entry example ###

Here's an example of The Elder's summoning attack and how it translates in game.

```
"gd_king_summon": {
	"Enabled": true,
	"Cooldown": 90,
	"HpThreshold": 0.8
	"maxMinionsCount": 6,
	"extraMaxMinionsCountPerPlayer": 3,
	"spawnMinionsCount": 3,
	"extraSpawnMinionsPerPlayer": 3,
	"Creatures": [ "Greydwarf:0.3:1", "Greydwarf_Shaman:0.3:0.8", "Greydwarf_Elite:0:0.5" ],
	"AttackCoolDownMultiplier": 0.5,
	"Stars": 2
	"SummonMaxStars": 1
}
```

In this example:
- If CLLC is installed, The Elder can only use this attack when having 2 stars or more.
- The Elder can only use this attack every 90 seconds when its HP falls below 80%, but as its HP gets lower it can attack faster, only having to wait 45 seconds at very low HP.
- Every time the Elder summons, it will try spawning 3 creatures, up to a maximum of 6 in a single player game. In multiplayer, it will spawn 3 additional creatures per player.
- The Elder can summon Greydwarf, Greydwarf Shaman and Greydwarf Brute during the fight depending on its HP.
- Greydwarf can be summoned when the boss HP is between 24% (0.3 x 0.8) and 80% (1 x 0.8).
- Greydwarf Shaman can be summoned when the boss HP is between 24% (0.3 x 0.8) and 64% (0.8 x 0.8).
- Greydwarf Brute can be summoned when the boss HP is between 0% (0 x 0.8) and 40% (0.5 x 0.8).
- So at less than a quarter health, The Elder can summon in 3 brutes in a single player game - be careful! :D
- The highest level creature that can be summoned is 1 star.

## Boss Changes and Attacks (Spoiler Warning)
Enough about the overview and config, let's get into the tweaks! Skip if you wish to be surprised and amazed (well, probably the former, less so of the latter...)

### Eikthyr (Eikthyr)

- Summons Helsvin and Helneck to assist him during the fight. These are stronger than regular Boar and Neck, so be wary!
- Summons smaller clones of himself called Heldyr to aid him in battle when his health gets lower. He is literally making hellspawn of himself.
- Creates an electric vortex that will suck you in before dealing damage. Getting too close might not be a good idea. Or maybe it'll just tingle.
- Fires electrically charged lightning projectiles at you from a distance if you allow him to get far enough away. They deal AOE damage on impact with the environment.
- Brings forth storms to strike you from above (or behind, but who's really watching?)
- Can be configured to teleport when attacking to add more surprise to his patterns. This part is still a bit of a WIP and may be removed.

### The Elder (gd_king)

- Summons Greydwarf and Greydwarf Shaman to help heal him and push you back. As if we needed MORE of those guys...
- Can command trees nearby you to uproot and fall onto you. Don't be cursing the gods when on this spot a tree fell on your head!
- Will absorb trees to restore his maximum HP. Like literally. Eats. Trees. And logs, he likes a few lil' snacks when stomping his enemies.
- Generates a shield when his health gets lower. Dem legz don't grow in a day...
- Teleports if you insist on getting in melee range too much. He is simply dying to show you his vines.

### Bonemass (Bonemass)

- Getting close to Bonemass will cause you to hallucinate, blurring your vision and slowing your movement slightly. Duuuuuude...
- Can additionally summon Draugr, Skeletons and Blobs to aid him during the fight, outside of his usual throw attack. 
- Can summon the power of Ancient Oozers, granting him temporary invulnerability whilst they are alive. And they're pretty, in deep blue!
- Cam slam the ground and cause rock spikes to head towards the player to do damage.
- The poison AOE attack now does more durability damage to your equipped armor. So try not to get caught in the cloud too often... Unless you're into that.

### Moder (Dragon)

- Moder's flight patterns change slightly depending on her health. At high HP she will fly more often, but become grounded more at low HP. Almost like you broke a wing, you monster...
- Drakes will be summoned at points whilst Moder is in the air. Yes. More dragons make more good.
- Wyverns will be summoned at points when Moder is grounded. They have a more accurate ice breath than Drakes or Moder herself. Literally like mini-Moders, but NOT the momma!
- Moder can unleash an Awakened ability which will turn her from an ice dragon into a fire dragon for a while. No more sleepy dragon...

### Yagluth (GoblinKing)

- Frequencies of the meteor strike and AOE fire fist attack have been adjusted and will increase slightly as the battle progresses. Yag likes two things - Slipknot, and burning you to a crisp.
- Occasionally summons a Fuling or Fuling Berserker to assist him in the fight. Can they make fire like he can? Nah, they prefer smashy smashy.
- Fire breath can now be shot when you are at a closer range. Cheese him, you say? I think NOT!
- Lightning Quake attack that will deal a large amount of Shock damage on impact. Why? He needed to compensate for having his legs ripped off by Odin.
- Rock Formation attack that will raise Yagluth up on a rock plinth, which you'll need to climb up to reach him. He can stand tall above his subordinates once more!

### The Queen (SeekerQueen)
    
- Increase the frequency of her regular attacks slightly as the fight goes on to make it more difficult to land full combos.
- ... I mean, there's not much extra I REALLY needed to add to her... But ideas are very welcome.

## Compatibility
This mod should be compatible with most other mods, aside from those that adjust any boss AI. Other than that, everything else should work just fine.
This mod seems to work just fine with Creature Level & Loot Control.
This mod should work with Boss Despawn.

## Known Bugs
- Some entities move strangely on server installs.

## For the Future
I plan to adjust the config more and allow for more fine tuning of certain parameters.
Refactor Bonemass' rock attack a bit to make collision detection more reliable.
More attacks to certain bosses that have a limited roster currently, mainly Moder and Yagluth.
Make The Elder able to launch logs and trees towards players.
Add an Ancient Awakening ability to Moder, swapping her powers from frost to fire for a short duration.

## Contact Me (Bug Reports & Suggestions)
If you have any suggestions, or experience bugs / problems using this mod please get in touch with me on my [Discord](https://discord.gg/K2gnt7ZMHX).

## Changelog

### Changelog v1.10.2

**Additions**
- Added Creature Level & Loot Control as a soft dependency for this mod.
- Added `Stars` attribute to the JSON config. If CLLC is installed, attacks with a Stars value can only be used if the boss has that number of stars or more.
- Added speed altering behaviour to Eikthyr. He now moves faster as his health decreases. Configurable base and max speeds.
- Added two new attacks to The Elder: Poison Spit and Log Throw
- Added tornado vfx to The Elder's Log Throw and Eat Trees attacks.

**Changes**
- Creatures now have the option to be summoned with stars. Specifying the `SummonMaxStars` property will result in each creature being summoned with a star level between the boss' current level and the maximum.
- Creatures `SummonMaxStars` can be set to -1 to force the star level of summoned creatures to be the same as the boss summoning them.
- ServerSync now *requires* this mod on both client and server. Players using an outdated version will not be able to join.

**Fixes**
- Added some ZNetView checks on boss attacks to (hopefully) improve performance on servers and stop random desyncs for some motions in the case of multiple players in a zone.
- Added boss states such as Awakened and Ancient Oozers to the boss' ZDO object to help synchronise behaviour changes to clients.
- Death or logging out during a Bonemass fight should no longer leave the screen effects active.

### Changelog v1.10.1

**Fixes**
- Fixed a nullref on boss attacks when logging out and logging back into a game without quitting first.
- Fized Eikthyr's electric projectile sound playing in your ear rather than at the position of the orb.

### Changelog v1.10.0

**Additions**
- Mod now has ServerSync added. Most configurations are now synced on the servers apart from visual effects which are client loaded.
- Added config option to specify the JSON file name so that users can override the settings without them being reverted whenever the mod updates.
- Added a `Desc` field in the JSON file to make it clear what attacks do for users wanting to configure them.
- All summon attacks now give you the flexibility to control what creatures are summoned at what HP level the boss has. 
- Added new attack to Eikthyr - Lightning Projectile.
- Added new ability to Moder: Ancient Awakening. This changes Moder's abilities from ice to fire. Moder becomes weaker to Pierce and neutral to Frost, but very resistant to Poison whilst in Ancient form.
- Added fire variants of Moder's ground breath and air projectiles that leave a burning effect where they land for several seconds.

**Changes**
- Config file changed from `blumaye.enhancedbossesredone` to `maxfoxgaming.enhancedbossesredone`. 
- Changed when boss attacks get loaded (now a postfix on `ZNetScene.Awake()` instead of `ObjectDB.Awake()`) to get server JSON settings instead of client ones.
- Default JSON file name changed from `settings.json` to `eb_settings.json` to help protect against other potential settings.json files in the case of server installs not putting mods in their own directories.
- Config file stores the JSON needed before ZNetScene triggers, so the server JSON will get pushed to clients.
- Renamed some entries in the JSON file to make them more clear on what they do.
- Lowered Eikthyr's default summons from 4 to 3 in singleplayer.
- Raised Eikthyr's default cooldown when summoning Heldyr.
- Changed Eikthyr to initially only summon Helneck, but Helsvin will be summoned as well later.
- Eikthyr will now attempt to summon Helneck, Helsvin and Heldyr when further away as well as when close to the player.
- Eikthyr will now attempt to escape the player to fire projectiles.
- Improved VFX of Eikthyr's Lightning Vortex - it's now actually a proper tornado! Yay!
- Lowered The Elder's default summons from 6 to 3 in singleplayer.
- Changed The Elder to initially only summon Greydwarf, but Shaman and Brute will be spawned later.
- Bonemass now spawns minions less frequently.
- Changed Bonemass to initially only summon Skeleton, but Blob and Draugr will be spawned later.
- Changed Yagluth to initially only summon Fuling, but Fuling Berserker will be spawned later.

**Fixes**
- Fixed issue where logging out and logging in again does not result in bosses having their new attacks.

### Changelog v1.9.2

**Additions**
- Heldyr, Helneck and Helsvin now have configurable health and damage output.
- Drakes and Wyverns have configurable health and damage output.

**Changes**
- Reduced health and damage of all of Eikthyr's summons a bit.
- Heldyr summoned by Eikthyr will disappear after a certain amount of time.
- Drakes and Wyverns summoned by Moder will now disappear after a configurable amount of time. This will prevent impossible situations where your bow breaks and you're surrounded by creatures that don't land.


### Changelog v1.9.1

**Fixes**
- Added Harmony patch to remap the boss prefabs on summoning altars to ensure the new attacks are properly called. Should stop the bosses defaulting to normal now. Whoops!

### Changelog v1.9.0

**Additions**
- Yagluth can now cause a rock formation to rise from the ground, raising him up and requiring the player to climb to reach him.

**Fixes**
- Fixed issue that could cause a nullref if one of the JSON entries were missing. It will now just render that attack disabled.

### Changelog v1.8.2

**Changes**
- Small code refactor for loading custom attacks and prefabs to make adding additional attacks much easier in future.
- Bonemass will now give a blue cloud when summoning Ancient Oozers.
- Bonemass will now give a yellow cloud when summoning additional creatures.
- Improved rock spawn attack to give rocks of different sizes and rotations.

**Fixes**
- Fixed bug causing ItemDrop prefabs not being instantiated correctly when loaded on Awake().
- Removed bandaid Harmony patch for Humanoid.SetVisEquipment() as prefabs are now no longer null.

### Changelog v1.8.1

**Fixes**
- Eikthyr Clones attack no longer fires a nullref and will spawn clones properly.

### Changelog v1.8.0

**Additions**
- Bonemass can now slam the ground to send rocks in the direction of the player. These will deal blunt damage on impact but can be dodged.

**Changes**
- Boss spawns from summon attacks no longer drop items in order to prevent entity buildup and lag in case of many summons.

**Fixes**
- Fixed a nullref that could occur during Assetbundle loading when logging out of a game and logging back in.
- Fixed a loading screen freeze caused from Bonemass rocks and Eikthyr clouds.
- Fixed Ancient Oozers sometimes flying away from Bonemass instead of moving towards him and circling.

### Changelog v1.7.2

**Additions**
- Added configuration options for amount of Eikthyr spawn clouds summoned.
- Added configuration options for amount of Ancient Oozers that can spawn.

**Changes**
- Config values will now clamp to the suggested min/max values if set outside these ranges.

**Fixes**
- All bosses now read from HP cooldown threshold config correctly even if one is specified already in original attacks.

### Changelog v1.7.1

**Additions**
- Eikthyr can now summon storm clouds to float nearby and deal damage at random to targets below.

**Changes**
- Adjusted particlesystem of the clouds to follow local position instead of world position. They now correctly follow the intended flight paths.
- Reduced calculation cost on Ancient Oozer movement.
- Removed soft dependency for MonsterLabZ being required for the storm attack to become available.

**Fixes**
- Fixed a nullref that could occasionally happen if Eikthyr was killed before storm clouds had disappeared. 
- Fixed a nullref that could occasionally happen if Bonemass was killed before Ancient Oozers had disappeared.
- Fixed endless coroutine loading on storm clouds. Should properly clean up when storm clouds are not needed.

### Changelog v1.7.0

**Additions**
- Added Storm Cloud prefab (eb_thundercloud) from kitbashed vanilla files, to be used by Eikthyr later.

**Changes**
- Removed dependency of JotunnLib and completely reworked all code to handle better loading of boss attacks.
- Removed unnecessary methods in much of the core library and helper classes.
- All parts of the mod are now properly namespaced and organised.
- Adjusted Ancient Oozer movement logic and now the movement is controlled via an attached script instead of a convoluted coroutine.

**Fixes**
- Updated to work with Valheim 0.214.2 build.


### Changelog v1.6.1

**Changes**
- Adjusted Elder teleport logic and effect. He will now teleport away occasionally when in melee range to give him a chance to use some more ranged attacks.
- Adjusted Elder tree healing. He will now summon logs to him and heal up if they are absorbed.
- Heal for Elder changed to be a per-second tick.
- Added better VFX to Eikthyr's electric vortex.

**Fixes**
- Fixed nullref caused by Eikthyr clones.
- Improved summon spawn logic for bosses to stop summons going into the ground.

### Changelog v1.6.0

**Additions**
- Yagluth now has a new attack that can spew lightning from the ground around the player.
- Bonemass now has a new attack that will summon Ancient Slimes. They will float above Bonemass and make him impervious to damage until they are destroyed, and after some time will terminate themselves heal Bonemass.
- Bonemass turns blue whilst in an impervioius state.
- Boss attack frequency will now change according to how much health they have left. Configurable in the JSON file.

**Changes**
- Refactored summon attack code to allow for easier addition of custom summon fx. Will be available in config in the future.

### Changelog v1.5.0

**Additions**
- Eikthyr's clone ability is back. It will now summon smaller Heldyr to assist him in the fight instead of making direct clones.
- Config option to enable or disable Eikthyr teleporting during attacks has been added. Defaults to false.
- Config option to increase or decrease the number of Heldyr he summons.
- Added config option to adjust Bonemass' AOE attack doing damage to armor's remaining durability. Significantly reduced from 50% to 3%.
- Added config option to adjust Bonemass' hallucination intensity, so you can still enable it with less intensity if desired. Reduced default intensity of screen shear.
- Added config option to adjust Bonemass' hallucination slowdown effect, and reduced the default speed penalty from 60% to 20%.
- Added config options for Moder's takeoff and landing behaviour when passing the HP threshold, meaning she will not always be in the air before reaching that threshold. By default she will still land infrequently before her HP hits 75%.

**Changes**
- Mod name is now Enhanced Bosses Redone.
- Removed probability that creatures summoned would have stars in order to make it compatible with Creature Level & Loot Control.

**Fixes**
- Fixed a bug that would prevent the last creature from summon lists being selected. All entries should now have an equal probability of getting selected.
- All attacks should now use the HP threshold correctly and enable / disable correctly when read from configs, as only a few of these were working before.
- Eikthyr's clone ability will no longer instakill players when triggered during a player's attack animation.
- The Elder's root spawn attack now works properly again.

### Changelog v1.4.3

**Fixes**
- Game no longer nullrefs on loading.

### Changelog v1.4.2

**Changes**
- Removed RRRCore and RRRMonsters dependencies.
- Removed Eikthyr clones ability for now (will be added back later).

**Fixes**
- Updated to work with Valheim 0.212.7 build.

### Changelog v1.4.0

**Additions**
- Added new attack to Eikthyr - Electric Vortex.
- Added the ability to disable attacks or change cooldowns via JSON.

**Changes**
- Improved performance.

### Changelog v1.3.0

**Additions**
- Bosses now show their location on the map.
- Added compatibility with ValheimFPSBoost.

**Changes**
- The Elder hsa improved teleport logic.
- Bonemass hallucinations now act as an aura.

### Changelog v1.2.0

**Additions**
- Eikthyr can now summon clones.
- The Elder can now teleport to the player. 
- The Elder can now cast a shield to protect himself.
- Added detailed setting for each boss.

**Changes**
- Minor changes to Eikthyr's attacks.
- Bonemass AOE attack may cause hallucinations, be careful! (inspired by shrooms)
- Mod now depends on RRRCore and RRRMonsters.
- The Elder's tree eating ability has been improved. 

## Installation
Move the 'plugins' and 'config' folders from the archive to your BepInEx folder.
If you are updating and get the note of missing JSON configuration, delete the mod and redownload to get the most recent one.

## Credits
### Blumaye
The original author of this mod.
### MrModdieMax
Initial reuploader of this mod and updating the mod to also include The Queen for the Mistlands update.

## Disclaimer
I will continue working on this mod unless requested by Blumaye to remove it from Thunderstore, in which case I will do so, but probably be very sad.
As Blumaye is the original author, I grant them permission to upload my maintained version in their name and account if desired. Keep it ALIVE!
If my right to work on this version of the mod is withdrawn, Blumaye is also welcome to rename this maintained version back to its original, Enhanced Bosses.
If you wish to donate to support development, please message Blumaye directly - this was their mod so I won't be taking donations for this.