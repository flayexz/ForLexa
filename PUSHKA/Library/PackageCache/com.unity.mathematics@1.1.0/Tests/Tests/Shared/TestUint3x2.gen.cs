# Audio clip properties

Use the Inspector window to change the properties of an Audio clip. These properties include the name, timing, play speed, blend properties, audio media, and loop option.

![Inspector window when selecting an Audio clip in the Timeline window](images/timeline_inspector_audio_clip.png)

_Inspector window when selecting an Audio clip in the Timeline window_

## Display Name

The name of the Audio clip shown in the Timeline window. This is not the name of the audio file that Unity uses for the waveform. For information on audio file properties, see **Audio Playable Asset** below.

## Clip Timing properties

Use the **Clip Timing** properties to position, change the duration, change the ease-in and ease-out duration, and adjust the play speed of the Audio clip.

Most timing properties are expressed in both seconds (s) and frames (f). When specifying seconds, a **Clip Timing** property accepts decimal values. When specifying frames, a property only accepts integer values. For example, if you attempt to enter 12.5 in a frames (f) field, the Inspector window sets the value to 12 frames.

Depending on [the selected Clip Edit mode](clp_about.md), changing the **Start**, **End**, or **Duration** may blend, ripple, or replace Audio clips on the same track.

|**Property** |**Description** |
|:---|:---|
|**Start**|The frame or time (in seconds) when the clip starts. Changing the Start property changes the position of the clip on its track in the Timeline Asset.<br />Changing the Start also affects the End. Changing the Start sets the End to the new Start value plus the Duration.|
|**End**|The frame or time (in seconds) when the clip ends.<br />Changing the End also affects the Start. Changing the End sets the Start to the new End value minus the Duration.|
|**Duration**|The duration of the clip in frames or seconds.<br />Changing the Duration also affects the End. Changing the Duration sets the End to the Start value plus the new Duration.|

## Blend Curves

Use the **Blend Curves** to customize the fade-in and fade-out between the outgoing and incoming Audio clips. See [Blending clips](clp_blend.md) for details on how to blend clips and customize blend curves.

When easing-in or easing-out Audio clips, use the **Blend Curves** to customize the curve that fades-in or fades-out an Audio clip. See [Easing-in and Easing-out clips](clp_ease.md) for details.

## Audio Playable Asset

Use the **Audio Playable Asset** properties to select the Audio file used by the Audio clip and to set whether the selected Audio clip loops (**Loop** enabled) or plays once (**Loop** disabled).
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        # Control clip common properties

Use the Inspector window to change the common properties of a Control clip. You can only create a Control clip in a Control track. A Control clip is a special clip that controls a nested Timeline instance, Particle System, Prefab instance, or ITimeControl Script, depending on how you create the Control clip:

* If you create the Control clip from a GameObject with a Playable Director component associated with a Timeline Asset, then the Control clip controls a nested Timeline instance. If the GameObject parents other GameObjects associated with many Timeline Assets, then the Control clip controls multiple Timeline instances.
* If you create the Control clip from a GameObject with a Particle System component, then the Control clip controls a Particle System.
* If you create the Control clip from a GameObject linked to a Prefab, then the Control clip controls a Prefab instance.
* If you create the Control clip from a GameObject with a script that implements the ITimeControl interface, then the Control clip controls an ITimeControl Script.

The common properties of a Control clip include its name and Clip Timing properties. Not all common properties apply to all types of Control clips.

![Inspector window when selecting a Control clip in the Timeline window](images/timeline_inspector_control_clip_common.png)

_Inspector window when selecting a Control clip in the Timeline window_

## Display Name

The name of the Control clip shown in the Timeline window.

## Clip Timing properties

Use the **Clip Timing** properties to position and change the duration of the Control clip.

Most timing properties are expressed in both seconds (s) and frames (f). When specifying seconds, a **Clip Timing** property accepts decimal values. When specifying frames, a property only accepts integer values. For example, if you attempt to enter 12.5 in a frames (f) field, the Inspector window sets the value to 12 frames.

Depending on the [selected Clip Edit mode](clp_about.md), changing the **Start**, **End** or **Duration** of a Control clip may create an insert or replace clips on the same track. You cannot create a blend between Control clips.

|**Property:** |**Description:** |
|:---|:---|
|**Start**|The frame or time (in seconds) when the Control clip starts. Changing the Start changes the position of the Control clip on its track in the Timeline Asset.<br />Changing the Start also affects the End. Changing the Start sets the End to the new Start value plus the Duration.|
|**End**|The frame or time (in seconds) when the Control clip ends.<br />Changing the End also affects the Start. Changing the End sets the Start to the new End value minus the Duration.|
|**Duration**|The duration of the clip in frames or seconds.<br />Changing the Duration also affects the End. Changing the Duration sets the End to the Start value plus the new Duration.|
|**Clip In**|Sets the offset of when the Control clip starts playing. The Clip In property only affects Particle Systems and nested Timeline instances.|
|**Speed Multiplier**|A speed multiplier that affects the playback speed of the Control clip. This value must be greater than 0. The Speed Multiplier property only affects Particle Systems and nested Timeline instances.|
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 # Control clip Playable Asset properties

Use the Inspector window to change the playable asset properties of a Control clip. To view the playable asset properties for a Control clip, select a Control clip in the Timeline window and expand **Control Playable Asset** in the Inspector window.

![Inspector window showing the **Control Playable Asset** properties for the selected Control clip](images/timeline_inspector_control_clip_playable.png)

_Inspector window showing the **Control Playable Asset** properties for the selected Control clip_

## Source Game Object

Use **Source Game Object** to select the GameObject with the Particle System, nested Timeline instance, or ITimeControl Script for the selected Control clip. Changing the **Source Game Object** changes what the Control clip controls.

## Prefab

Use **Prefab** to select a Prefab to instantiate when the Timeline instance plays in Play Mode. When a Prefab is selected, the label of the **Source Game Object** property changes to **Parent Object**.

When in Play Mode, the Prefab is instantiated as a child of the **Parent Object**. Although the Prefab is instantiated at the start of the Timeline instance, the Prefab is only activated during the Control clip. When the Control clip ends, the Prefab instance is deactivated.

## Control Activation

Enable **Control Activation** to activate the **Source Game Object** while the Control clip plays. Disable this property to activate the **Source Game Object** during the entire Timeline instance.

The **Control Activation** property only affects Control clips that control a nested Timeline instance or a Particle System.

## Post Playback

When **Control Activation** is enabled, use the **Post Playback** property to set the activation state for the nested Timeline instance when the main Timeline stops playing. The **Post Playback** property only affects nested Timeline instances.

|**Post-Playback State** |**Description** |
|:---|:---|
|**Active**|Activates the Source Game Object after the nested Timeline instance finishes playing.|
|**Inactive**|Deactivates the Source Game Object after the nested Timeline instance finishes playing.|
|**Revert**|Reverts the Source Game Object to its activation state before the nested Timeline instance began playing.|


## Advanced properties

Use the Advanced properties to select additional functionality based on whether the Control clip controls a Playable Director, Particle System, or ITimeControl Script. The Advanced properties do not apply to all Control clips.

|**Property** |**Description** |
|:---|:---|
|**Control Playable Directors**|Enable this property if the Source Game Object is attached to a Playable Director and you want the Control clip to control the nested Timeline instance associated with this Playable Director.|
|**Control Particle Systems**|Enable this property when the Control clip includes a Particle System. Set the value of the Random Seed property to create a unique, repeatable effect.|
|**Control ITimeControl**|Enable this property to control ITimeControl scripts on the Source GameObject. To use this feature, the Source Game Object must have a script that implements the ITimeControl interface.|
|**Control Children**|Enable this property if the Source Game Object has a child GameObject with either a Playable Director, Particle System, or ITimeControl Script, and you want the Control clip to control this child component.<br /><br />For example, if the Source Game Object is a GameObject that parents another GameObject with a Particle System, enable this property to make the Control clip control the Particle system on the child GameObject.|
                                                                                                                                                                                                                                                                                                                                                                                                            # Setting Timeline Asset properties

Use the Inspector window to set the frame rate, the duration mode, and a fixed length for the selected Timeline Asset. From the Project window, select a Timeline Asset to view its properties.

![Inspector window when selecting a Timeline Asset in the Project window ](images/timeline_inspector_timeline.png)

_Inspector window when selecting a Timeline Asset in the Project window_

|**Property**||**Description**|
|:---|:---|:---|
|**Frame Rate**||Sets the reference frame rate for the Timeline Asset and its Timeline instances. Change the Frame Rate to align clips at precise frames but changing the Frame Rate is only visual and has no effect on play speed, keys, tracks, or clips.<br /><br/>Timeline supports the following standard frame rates: 24 (PAL), 25 (NTSC), 30, 50, and 60. Timeline also supports custom frame rates from 1e-6 to 1000.<br /><br />To set a custom frame rate, enter a non-standard frame rate for the Frame Rate property. In the [Timeline Settings](tl_settings.md) menu, the Custom menu item is enabled and automatically selected for the Timeline instance. The Custom menu item shows the custom frame rate in parentheses.|
| **Duration Mode**||Choose whether the duration of the Timeline Asset extends to the end of the last clip or ends at a specific time or frame.|
||Based On Clips|Sets the length of the Timeline Asset based on the end of the last clip.|
||Fixed Length|Sets the length of the Timeline Asset to a specific number of seconds or frames.|
| **Duration**||Shows the length of the Timeline Asset in seconds and frames when the Duration Mode is set to Based on Clips.<br /><br/>Sets the length of the Timeline Asset to a specific number of seconds or frames when the Duration Mode is set to Fixed Length.|
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           # Setting track properties

Use the Inspector window to change the name of a track and its properties. The available properties depend on the type of track selected. For example, select an Animation Track to set how track offsets are applied, to apply an avatar mask, and to select which transforms are modified when matching offsets between Animation clips.

![Inspector window when selecting an Animation track in the Timeline window](images/timeline_inspector_animation_track.png)

_Inspector window when selecting an Animation track in the Timeline window_

Not all tracks have properties. See the following sections for tracks with properties:

* [Activation Track properties](insp_trk_act.md)
* [Animation Track properties](insp_trk_anim.md)
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        # Activation track properties

Use the Inspector window to change the name of an Activation track and set the state of its bound GameObject when the Timeline Asset finishes playing.

![Inspector window when selecting an Activation track in the Timeline window](images/timeline_inspector_activation_track.png)

_Inspector window when selecting an Activation track in the Timeline window_

|**Property**||**Description**|
|:---|:---|:---|
|**Display Name**||The name of the Activation track shown in the Timeline window and Playable Director component. The Display Name applies to the Timeline Asset and all of its Timeline instances.|
|**Post-playback state**||Sets the activation state for the bound GameObject when the Timeline Asset stops playing. The Post-playback state applies to the Timeline Asset and all of its Timeline instances.|
||Active|Activates the bound GameObject when the Timeline Asset finishes playing.|
||Inactive|Deactivates the bound GameObject when the Timeline Asset finishes playing.|
||Revert|Reverts the bound GameObject to its activation state before the Timeline Asset began playing.<br /><br />For example, if the Timeline Asset finishes playing with the GameObject set to inactive, and the GameObject was active before the Timeline Asset began playing, then the GameObject reverts to active.|
||Leave As Is|Sets the activation state of the bound GameObject to the state the Timeline Asset is at when it finishes playing.<br /><br />For example, if the Timeline Asset finishes playing with the GameObject set to inactive, the GameObject remains inactive.|
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              # Animation track properties

Use the Inspector window to change the name of an Animation track, set how track offsets are applied, apply an avatar mask, and set which transforms are modified by default when you [match clip offsets](clp_match.md).

![Inspector window when selecting an Animation track in the Timeline window](images/timeline_inspector_animation_track.png)

_Inspector window when selecting an Animation track in the Timeline window_

|**Property** ||**Description** |
|:---|:---|:---|
|**Display Name**||The name of the Animation track shown in the Timeline window and in the Playable Director component. The Display Name applies to the Timeline Asset and all of its Timeline instances.|
|**Track Offsets**||Applies a position and rotation offset to the start of each Animation clip on the selected Animation track. The position and rotation offset starts from a specific position and rotation or from the position and rotation relative to a state machine or another Timeline instance.|
||Apply Transform Offsets|Starts the animation in each Animation clip from a specific position and rotation offset. Use the Move and Rotate tools, and the Position and Rotation fields, to set the starting position and rotation.|
||Apply Scene Offsets|Starts the animated GameObject from its current position and rotation in the Scene. Use this mode to build a Timeline instance that transitions to and from a state machine or to and from another Timeline instance.|
||Auto (deprecated)|If you load a Scene or Project that was built before 2018.3, Track Offsets is automatically set to Auto (deprecated). This is a special mode for backwards compatibility.<br /><br />After opening an old Project, choose another Track Offsets mode because the Auto (deprecated) offset disables key animation recording.|
||Move tool|Enable the Move tool to show the Move Gizmo in the Scene view. Use the Move Gizmo to visually position the transform offset. Positioning the Move Gizmo changes the Position properties.<br /><br />The Move tool only appears when Track Offsets is set to Apply Transform Offsets.|
||Rotate tool|Enable the Rotate tool to show the Rotate Gizmo in the Scene view. Use the Rotate Gizmo to visually rotate the track offset. Rotating the Rotate Gizmo changes the Rotation properties.<br /><br />The Rotate tool only appears when Track Offsets is set to Apply Transform Offsets.|
||Position|Sets the track position offset in X, Y, and Z coordinates. The Position fields only appears when Track Offsets is set to Apply Transform Offsets.|
||Rotation|Sets the track rotation offset in X, Y, and Z coordinates. The Rotation fields appear when Track Offsets is set to Apply Transform Offsets.|
|**Apply Avatar Mask**||Enables Avatar masking. When enabled, Timeline applies the animation of all Animation clips on the track based on the selected Avatar Mask.|
||Avatar Mask|Selects the Avatar Mask applied to all Animation clips on the Animation track.<br />An Avatar Mask defines which humanoid body parts are animated by Animation clips on the selected Animation track. The body parts that are masked are animated by other Animation tracks in the Timeline Asset.<br /><br />For example, you can use an Avatar Mask to combine the lower-body animation on an Animation track with the upper body animation on an [Override Animation track](wf_mask.md).|
|**Default Offset Match Fields**||Expand to display a series of checkboxes that choose which transforms are matched when [matching clip offsets](clp_match.md) between Animation clips.<br /><br />The Default Offset Match Fields set the default matching options for all Animation clips on the same track. Use the [Animation Playable Asset properties](insp_clp_anim_plyb.md) to override these defaults for each Animation clip.|
                                                                                                                                                                                                                                                                                                     # Playable Director component

The Playable Director component stores the link between a Timeline instance and a Timeline Asset. The Playable Director component controls when the Timeline instance plays, how the Timeline instance updates its clock, and what happens when the Timeline instance finishes playing.

![Playable Director component added to the GameObject named Ground. The GameObject is associated with the GroundCTL Timeline Asset.](images/timeline_inspector_playable_director.png)

_Playable Director component added to the GameObject named Ground. The GameObject is associated with the GroundCTL Timeline Asset._

The Playable Director component also shows the list of tracks from the associated Timeline Asset (**Playable** property) that animate GameObjects in the Scene. The link between Timeline Asset tracks and GameObjects in the Scene is referred to as **binding** or **Track binding**. For more on binding and the relationship between Timeline Assets and Timeline instances, see [Timeline overview](tl_about.md).

|**Property** ||**Description** |
|:---|:---|:---|
|**Playable**||Associates a Timeline Asset with a GameObject in the Scene.<br />When you make this association, you create a Timeline instance for the selected Timeline Asset. After you create a Timeline instance, you can use the other properties in the Playable Director component to control the instance and choose which GameObjects in the Scene are animated by the Timeline Asset.|
|**Update Method**||Sets the clock source that the Timeline instance uses to update its timing.|
||DSP|Select for sample accurate audio scheduling. When selected, the Timeline instance uses the same clock source that processes audio. DSP stands for digital signal processing.|
||Game Time|Select to use the same clock source as the game clock. This clock source is affected by [time scaling](https://docs.unity3d.com/Manual/TimeFrameManagement.html).|
||Unscaled Game Time|Select to use the same clock source as the game clock, but without being affected by time scaling.|
||Manual|Select to not use a clock source and to manually set the clock time through scripting.|
|**Play on Awake**||Whether the Timeline instance is played when game play is initiated. By default, a Timeline instance is set to begin as soon as the Scene begins playback. To disable the default behaviour, disable the Play on Awake option in the Playable Director component.|
|**Wrap Mode**||The behaviour when the Timeline instance ends playback.|
||Hold|Plays the Timeline instance once and holds on the last frame until playback is interrupted.|
||Loop|Plays the Timeline instance repeatedly until playback is interrupted.|
||None|Plays the Timeline instance once.|
|Initial Time||The time (in seconds) at which the Timeline instance begins playing. The Initial Time adds a delay in seconds before the Timeline instance actually begins. For example, when Play On Awake is enabled and Initial Time is set to five seconds, if you click the Play button in the Unity Toolbar, [Play Mode](https://docs.unity3d.com/Manual/GameView.html) starts and the Timeline instance begins five seconds later.|
|**Current Time**||Views the progression of time according to the Timeline instance in the Timeline window. The Current Time field matches the Playhead Location field.<br />Use the Current Time field when the Timeline window is hidden. The Current Time field appears in the Playable Director Component when in Timeline Playback mode or when Unity is in Game Mode.|
|**Bindings**||Shows the link between GameObjects in the Scene with tracks from the associated Timeline Asset (Playable property). The Bindings area is split into two columns:<br />The first column lists the tracks from the Timeline Asset. Each track is identified by an icon and its track type.<br />The second column lists the GameObject linked (or bound) to each track.<br />The Bindings area does not list Track groups, Track sub-groups, or tracks that do not animate GameObjects. The Timeline window shows the same bindings in the [Track list](trk_list_about.md).|
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           # Timeline overview

Use the **Timeline window** to create cut-scenes, cinematics, and game-play sequences by visually arranging tracks and clips linked to GameObjects in your Scene.

![A cinematic sequence in the Timeline window.](images/timeline_cinematic_example.png)

_A cinematic sequence in the Timeline window._

For each cut-scene, cinematic, or game-play sequence, the Timeline window saves the following:

* **Timeline Asset**: Stores the tracks, clips, and recorded animations without links to the specific GameObjects being animated. The Timeline Asset is saved to the Project.
* **Timeline instance**: Stores links to the specific GameObjects being animated or affected by the Timeline Asset. These links, referred to as **bindings**, are saved to the Scene.

## Timeline Asset

The Timeline window saves track and clip definitions as a **Timeline Asset**. If you record key animations while creating your cinematic, cut-scene, or game-play sequence, the Timeline window saves the recorded clips as children of the Timeline Asset.

![The Timeline Asset saves tracks and clips (red). Timeline saves recorded clips (blue) as children of the Timeline Asset.](images/timeline_overview_asset.png)

_The Timeline Asset saves tracks and clips (red). Timeline saves recorded clips (blue) as children of the Timeline Asset._

## Timeline instance

To animate a GameObject in your Scene with a Timeline Asset, you must create a **Timeline instance**. A **Timeline instance** associates a Timeline Asset with the GameObject in the Scene, through a [Playable Director](play_director.md) component.

When you select a GameObject in a Scene that has a Playable Director component, the Timeline instance appears in the Timeline window. The bindings appear in the Timeline window and in the Playable Director component (Inspector window).

![The Playable Director component shows the Timeline Asset (blue) with its bound GameObjects (red). The Timeline window shows the same bindings (red) in the Track list.](images/timeline_overview_instance.png)

_The Playable Director component shows the Timeline Asset (blue) with its bound GameObjects (red). The Timeline window shows the same bindings (red) in the Track list._

The Timeline window provides an automated method of creating a Timeline instance while [creating a Timeline Asset](wf_instance.md).

## Reusing Timeline Assets

Because Timeline Assets and Timeline instances are separate, you can reuse the same Timeline Asset with many Timeline instances.

For example, you could create a Timeline Asset named VictoryTL with the animation, music, and particle effects that play when the main game character (Player) wins. To reuse the VictoryTL Timeline Asset to animate another game character (Enemy) in the same Scene, you can create another Timeline instance for the secondary game character.

![The Player GameObject (red) is attached to the VictoryTL Timeline Asset](images/timeline_overview_player.png)

_The Player GameObject (red) is attached to the VictoryTL Timeline Asset]_

![The Enemy GameObject (blue) is also attached to the VictoryTL Timeline Asset](images/timeline_overview_enemy.png)

_The Enemy GameObject (blue) is also attached to the VictoryTL Timeline Asset]_

Because you are reusing the Timeline Asset, any modification to the Timeline Asset in the Timeline window results in changes to all Timeline instances.

For example, in the previous example, if you delete the Audio track while modifying the Player Timeline instance, the Timeline window removes the track from the VictoryTL Timeline Asset. The Timeline window also removes the Audio track from all instances of the VictoryTL Timeline Asset, including the Enemy Timeline instance.
                                                                                                                                                                                                                                                                                                                                           # Timeline glossary

This topic provides an alphabetical list of the terminology used throughout the Timeline documentation.

**animatable property**: A property belonging to a GameObject, or belonging to a component added to a GameObject, that can have different values over time.

**animation**: The result of adding two different keys, at two different times, for the same animatable property.

**animation curve**: The curve drawn between keys set for the same animatable property, at different frames or seconds. The position of the tangents and the selected interpolation mode for each key determines the shape of the animation curve.

**binding** or **Track binding**: Refers to the link between Timeline Asset tracks and the GameObjects in the scene. When you link a GameObject to a track, the track animates the GameObject. Bindings are stored as part of the Timeline instance.

**blend** and **blend area**: The area where two Animation clips, Audio clips, or Control clips overlap. The overlap creates a transition that is referred to as a **blend**. The duration of the overlap is referred to as the **blend area**. The blend area sets the duration of the transition.

**Blend In curve**: In a blend between two Animation clips, Audio clips, or Control clips, there are two blend curves. The blend curve for the incoming clip is referred to as the **Blend In** curve.

**Blend Out curve**: In a blend between two Animation clips, Audio clips, or Control clips, there are two blend curves. The blend curve for the out-going clip is referred to as the **Blend Out** curve.

**clip**: A generic term that refers to any clip within the Clips view of the Timeline window.

**Clips view**: The area in the Timeline window where you add, position, and manipulate clips.

**Control/Command**: This term is used when instructing the user to press or hold down the Control key on Windows, or the Command key on Mac.

**Curves view**: The area in the Timeline window that shows the animation curves for Infinite clips or for Animation clips that have been converted from Infinite clips. The Curves view is similar to [Curves mode](animeditor-AnimationCurves) in the Animation window.

**Gap extrapolation**: How an Animation track approximates animation data in the gaps before and after an Animation clip.

**field**: A generic term that describes an editable box that the user clicks and types-in a value. A field is also referred to as a **property**.

**incoming clip:** The second clip in a blend between two clips. The first clip, the out-going clip, transitions to the second clip, the **incoming clip**.

**Infinite clip**: A special animation clip that contains basic key animation recorded directly to an Animation track within the Timeline window. An Infinite clip cannot be positioned, trimmed, or split because it does not have a defined duration: it spans the entirety of an Animation track.

**interpolation**: The estimation of values that determine the shape of an animation curve between two keys.

**interpolation mode**: The interpolation algorithm that draws the animation curve between two keys. The interpolation mode also joins or breaks left and right tangents.

**key**: The value of an animatable property, set at a specific point in time. Setting at least two keys for the same property creates an animation.

**out-going clip**: The first clip in a blend between two clips. The first clip, the **out-going clip**, transitions to the second clip, the incoming clip.

**Playhead Location field**: The field that expresses the location of the Timeline Playhead in either frames or seconds, depending on the Timeline Settings.

**property**: A generic term for the editable fields, buttons, checkboxes, or menus that comprise a component. An editable field is also referred to as a **field**.

**tangent**: One of two handles that controls the shape of the animation curve before and after a key. Tangents appear when a key is selected in the Curves view, or when a key is selected in the Curve Editor.

**tangent mode**: The selected interpolation mode used by the left tangent, right tangent, or both tangents.

**Timeline** or **Unity's Timeline**: Generic terms that refer to all features, windows, editors, and components related to creating, modifying, or reusing cut-scenes, cinematics, and game-play sequences.

**Timeline Asset**: Refers to the tracks, clips, and recorded animation that comprise a cinematic, cut-scene, game-play sequence, or other effect created with the Timeline window. A Timeline Asset does not include bindings to the GameObjects animated by the Timeline Asset. The bindings to scene GameObjects are stored in the Timeline instance. The Timeline Asset is project-based.

**Timeline window**: The official name of the window where you create, modify, and preview a Timeline instance. Modifications to a Timeline instance also affects the Timeline Asset.

**Timeline instance**: Refers to the link between a Timeline Asset and the GameObjects that the Timeline Asset animates in the scene. You create a Timeline instance by associating a Timeline Asset to a GameObject through a Playable Director component. The Timeline instance is scene-based.

**Timeline Playback Controls**: The row of buttons and fields in the Timeline window that controls playback of the Timeline instance. The Timeline Playback Controls affect the location of the Timeline Playhead.

**Timeline Playback mode**: The mode that previews the Timeline instance in the Timeline window. Timeline Playback mode is a simulation of Play mode. Timeline Playback mode does not support audio playback.

**Timeline Playhead**: The white marker and line that indicates the exact point in time being previewed in the Timeline window.

**Timeline Selector**: The name of the menu in the Timeline window that selects the Timeline instance to be previewed or modified.

**track**: A generic term that refers to any track within the Track list of the Timeline window.

**Track groups**: The term for a series of tracks organized in an expandable and collapse collection of tracks.

**Track list**: The area in the Timeline window where you add, group, and modify tracks.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  # Timeline Playback Controls

To play the Timeline instance and to control the location of the Timeline Playhead, use the Timeline Playback Controls.

![Timeline Playback Controls](images/timeline_playback_controls.png)

_Timeline Playback Controls_

## Timeline Start button

![](images/timeline_start_button.png)

To move the Timeline Playhead to the start of the Timeline instance, click the Timeline Start button, or hold Shift and press Comma (,).

## Previous Frame button

![](images/timeline_previous_frame_button.png)

To move the Timeline Playhead to the previous frame, click the Previous Frame button, or press Comma (,).

<a name="playbutton"></a>
## Timeline Play button

![](images/timeline_play_button.png)

To preview the Timeline instance in Timeline Playback mode, click the Timeline Play button, or press the Spacebar. Timeline Playback mode does the following:

* Begins playback at the current location of the Timeline Playhead and continues to the end of the Timeline instance. If the Play Range button is enabled, playback is restricted to a specified time range.
* The Timeline Playhead position moves along the Timeline instance. The Playhead Location field shows the position of the Timeline Playhead in either frames or seconds, depending on the [Timeline settings](tl_settings.md).
* To pause playback, click the Timeline Play button again, or press the Spacebar.
* When playback reaches the end of the Timeline instance, the Wrap Mode determines whether playback should hold, repeat, or do nothing. The Wrap Mode setting is a [Playable Director component](play_director.md) property.

Timeline Playback mode provides a preview of the Timeline instance while in the Timeline window. Timeline Playback mode is only a simulation of [Play Mode in the Game View](https://docs.unity3d.com/Manual/GameView.html).

The Timeline Playback mode does not support audio playback. To preview a Timeline instance with audio, enable the Play on Awake option in the [Playable Director component](play_director.md) and preview game play in Play Mode.

## Next Frame button

![](images/timeline_next_frame_button.png)

To move the Timeline Playhead to the next frame, click the Next Frame button, or press Period (.).

## Timeline End button

![](images/timeline_end_button.png)

To move the Timeline Playhead to the end of the Timeline instance, click the Timeline End button, or hold Shift and press Period (.).

<a name="playrange"></a>
## Play Range button

![](images/timeline_play_range_button.png)

Enable the **Play Range** button to restrict playback to a specific range of seconds or frames. You can only set a play range when previewing a Timeline instance within the Timeline window. Unity ignores the play range in [Play Mode](https://docs.unity3d.com/Manual/GameView.html).

The Timeline ruler highlights the play range and indicates its start and end with white markers. To modify the play range, drag either marker.

![Play Range (red circle) enabled with while markers and highlighted area defining range](images/timeline_play_range.png)

_Play Range (red circle) enabled with while markers and highlighted area defining range_

Use **Play Range Mode**, in the [Timeline Settings](tl_settings.md), to set whether the play range plays once or repeatedly.

<a name="playheadlocation"></a>
## Timeline Playhead and Playhead Location field

The Timeline Playhead indicates the exact point in time being previewed in the Timeline window. The Playhead Location field expresses the location of the Timeline Playhead in either frames or seconds.

![Playhead Location field and Timeline Playhead (red). The Timeline Playhead also appears on the Zoombar (red arrow).](images/timeline_playhead_location.png)

_Playhead Location field and Timeline Playhead (red). The Timeline Playhead also appears on the Zoombar (red arrow)._

Use the [Zoombar to navigate, scroll, and zoom](clp_pan_zoom.md) the Clips view. A white line indicates the location of the Timeline Playhead in relation to the entire Timeline instance.

To jump the Timeline Playhead to a specific time, click the Timeline ruler. You can also enter the time value in the Playhead Location field and press Enter. When entering a value, frames are converted to seconds or seconds are converted to frames, based on the Timeline settings. For example, if the Timeline ruler is expressed as seconds with a frame rate of 30 frames per second, entering 180 in the Playhead Location field converts 180 frames to seconds and moves the Timeline Playhead to 6:00. To set the time format that the Timeline window uses, configure the [Timeline Settings](tl_settings.md).

## Switching between Local and Global

Use the Local or Global button to change the Timeline ruler from local time to global time. Local time and global time are only relevant when editing a nested Timeline instance.

[To create a nested Timeline instance](wf_nested.md), drag a GameObject associated with a Timeline instance into another Timeline instance. The Timeline instance you are dragging into becomes the master Timeline instance. The Timeline instance associated with the GameObject becomes a nested Timeline instance.

![A nested Timeline instance appears as a Control clip on a Control track (red arrow)](images/timeline_nesting_example.png)

_A nested Timeline instance appears as a Control clip on a Control track (red arrow)_

To edit a nested Timeline instance, double-click the Control clip that contains the nested Timeline instance. The Timeline window switches to the nested Timeline instance, indicated by the Timeline title which shows the name and GameObject of the master Timeline instance, followed by the name and GameObject of the nested Timeline instance.

![The Timeline title indicates that you are editing a nested Timeline instance (red outline). The Global button (red arrow) indicates that the nested Timeline instance is shown using global time.](images/timeline_nesting_editing.png)

_The Timeline title indicates that you are editing a nested Timeline instance (red outline). The Global button (red arrow) indicates that the nested Timeline instance is shown using global time._

When editing a nested Timeline instance, click **Global** to switch the Timeline ruler to Local time. Local time is relative to the nested Timeline. This means that the Timeline ruler starts at zero.

![A nested Timeline instance in Local time.](images/timeline_nesting_local.png)

_A nested Timeline instance in Local time._

Click **Local** to view the Timeline ruler in relation to the placement of the nested Timeline in the master Timeline instance. This means that if, for example, if the Control clip is placed at frame 70 of the master Timeline then the Timeline ruler starts at 70 at the beginning of the nested Timeline instance.

![A nested Timeline instance in Global time.](images/timeline_nesting_global.png)

_A nested Timeline instance in Global time._
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             # Timeline Preview and Timeline Selector

Use the Timeline Selector to select the Timeline instance to view, modify, or preview in the Timeline window. The Timeline **Preview** button enables or disables previewing the effect that the selected Timeline instance has on your Scene.

![Timeline **Preview** button with Timeline Selector and menu. Selecting a Timeline instance automatically enables the Timeline Preview button.](images/timeline_preview_selector.png)

_Timeline **Preview** button with Timeline Selector and menu. Selecting a Timeline instance automatically enables the Timeline Preview button._

To select a Timeline instance, click the Timeline Selector and choose from the list of Timeline instances in the current Scene.

Each menu item displays the name of the Timeline Asset and its associated GameObject in the current Scene. For example, the Timeline Asset named GroundATL that is associated with the Ground GameObject, displays as "GroundATL (Ground)."
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     # Timeline Settings

Use the Timeline Settings to choose the Timeline window and Timeline Asset settings such as the unit of measurement, the duration mode, audio waveform, and window snap settings.

![Click the Cog icon in the Timeline window to view the Timeline Settings menu](images/timeline_cog_menu.png)

_Click the Cog icon in the Timeline window to view the Timeline Settings menu_

## Seconds or Frames

Select either **Seconds** or **Frames** to set the Timeline window to display time as either seconds or frames.

## Duration Mode

Use the **Duration Mode** to set whether the duration of the Timeline Asset extends to the end of the last clip (**Based On Clips**), or ends at a specific time or frame (**Fixed Length**). When the **Duration Mode** is set to **Fixed Length**, use one of the following methods to change the length of the Timeline Asset:

* Select the Timeline Asset in the Project window and use the Inspector window to set the Duration in seconds or frames.
* In the Timeline window, drag the blue marker on the timeline. The blue marker indicates the end of the Timeline Asset. A blue line indicates the duration of the Timeline Asset.

![Timeline Asset duration (red rectangle) and end marker (green circle)](images/timeline_duration_mode.png)

_Timeline Asset duration (red rectangle) and end marker (green circle)_

## Frame Rate

Select one of the options under **Frame Rate** to set the unit of measurement for the Timeline ruler. Change the Frame Rate to align clips at precise frames but changing the Frame Rate is only visual and has no effect on play speed, keys, tracks, or clips. The following standard frame rates are listed: Film (24 fps), PAL (25 fps), NTSC (29.97 fps), 30, 50, or 60.

Timeline supports custom frame rates from 1e-6 to 1000. To set a custom frame rate, you must use the **Frame Rate** property in the [Timeline Asset settings](insp_tl.md). When the Timeline Asset is set to a custom frame rate, the Custom menu item is enabled and is automatically selected for the Timeline instance. The Custom menu item shows the custom frame rate in parentheses.

## Play Range Mode

Select one of the options under **Play Range Mode** to set the behaviour of the Timeline window when the [Play Range button](tl_play_cntrls.md#playrange) is enabled.

* When **Play Range Mode** is set to **Loop**, the Timeline instance plays the defined Play Range repeatedly, until playback is interrupted.
* When **Play Range Mode** is set to **Hold**, the Timeline instance plays the defined Play Range once and stops at the end of the Play Range.

You can only set a play range when previewing a Timeline instance within the Timeline window. Unity ignores the play range in [Play Mode](https://docs.unity3d.com/Manual/GameView.html).

## Show Audio Waveforms

Enable **Show Audio Waveforms** to draw the waveforms for all audio clips on all audio tracks. For example, use an audio waveform as a guide when manually positioning an Audio clip of footsteps with the Animation clip of a humanoid walking. Disable **Show Audio Waveform** to hide audio waveforms. **Show Audio Waveforms** is enabled by default.

## Enable Audio Scrubbing

**Enable Audio Scrubbing** to play audio while dragging the Timeline Playhead.

Disable **Enable Audio Scrubbing** to stop playing audio while dragging the Timeline Playhead. When disabled, Timeline