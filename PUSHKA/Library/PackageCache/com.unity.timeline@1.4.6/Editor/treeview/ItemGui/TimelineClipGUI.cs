# Clip Edit modes and the Clips view

Use the Clips view to add, position, and manipulate clips on each track in the Track list. The selected Clip Edit mode determines how clips interact when you add, move, or delete them.

![The Clip Edit modes (green) and the Clips view (red)](images/timeline_clips_view.png)

_The Clip Edit modes (green) and the Clips view (red)_

## Clips and the Clips view

In the Clips view, each clip has a colored accent line that identifies the type of clip:

* Activation clips are green.
* Animation clips are blue.
* Audio clips are orange.
* Control clips are turquoise.
* Playable clips are white.

A clip based on data, such as an Animation clip or an Audio clip, displays arrows that indicate when the clip has been trimmed to exclude part of its source animation, waveform, or other data. For example, if an Animation clip uses only part of its full key animation, white arrows indicate that key animation exists before the start or after the end of the clip.

![Small arrows (circled) indicate that data exists before the start or after the end of the area defined by the clip](images/timeline_clips_arrows.png)

_Small arrows (circled) indicate that data exists before the start or after the end of the area defined by the clip_

To resize a clip and view its hidden data, either right-click the clip and select **Match Content** from the context menu, or select the clip and modify its clip timing properties in the Inspector window. When you resize a clip, the selected Clip Edit mode determines how the surrounding clips are affected.

## Clip Edit modes

Select a Clip Edit mode to choose how clips are added, positioned, and trimmed within the Clips view, or when modifying clip timing properties in the Inspector window. There are three Clip Edit modes that affect most clip editing features: Mix mode (default), Ripple mode, and Replace mode.

![Clip Edit modes are Mix (default and selected), Ripple, and Replace mode](images/timeline_zoomed_clip_edit_modes.png)

_Clip Edit modes are Mix (default and selected), Ripple, and Replace mode_

You can also temporarily switch between Clip Edit modes. This is useful if, for example, you want to temporarily use Ripple mode to offset the content of a track while you position clips. To temporarily switch between Clip Edit modes, hold down the following keyboard keys:

* Hold 1 to temporarily switch to Mix mode.
* Hold 2 to temporarily switch to Ripple mode.
* Hold 3 to temporarily switch to Replace mode.

### Mix mode

Use Mix mode to add, position, and trim clips without moving or replacing adjacent clips. Mix mode creates blends between intersecting clips. Mix mode is the default Clip Edit mode.

![Timeline window with Mix mode as the selected Clip Edit mode. The position cursor (circled) indicates where you drag to position the clip.](images/timeline_mix_mode_position_cursor.png)

_Timeline window with Mix mode as the selected Clip Edit mode. The position cursor (circled) indicates where you drag to position the clip._

In Mix mode, when you hover over a selected clip in the Clips view, the cursor changes to indicate the action that you can perform. The action depends on the part of the clip that you hover over:

* When you hover over the start of a selected clip, the cursor changes to a trim cursor. The trim cursor indicates the area to drag to trim the start of the clip.
* When you hover over the middle of a selected clip, the cursor changes to a position cursor and indicates the area to drag to position the clip.
* When you hover over the end of a selected clip, the cursor changes to a trim cursor. The trim cursor indicates the area to drag to trim the end of the clip.

In Mix mode, if you drag to trim or position a clip and it intersects another clip, the cursor changes to a white arrow that points towards the blend being created. There are three possible cursors depending on whether the blend is created at the beginning of the clip, at the end of the clip, or at both the beginning and end of the clip.

![The white arrow cursor indicates that dragging Clip 2A to the right creates a blend, at the end of the clip, between Clip 2A and Clip 2B.](images/timeline_mix_mode_blend_arrow.png)

_The white arrow cursor indicates that dragging Clip 2A to the right creates a blend, at the end of the clip, between Clip 2A and Clip 2B._

### Ripple mode

Use Ripple mode to add, position, and trim a clip while affecting the subsequent clips on the same track. Positioning or trimming clips in Ripple mode preserves the gaps between subsequent clips.

![Timeline window with Ripple mode as the selected Clip Edit mode. The position cursor (circled) indicates where you drag to position the clip.](images/timeline_ripple_mode.png)

_Timeline window with Ripple mode as the selected Clip Edit mode. The position cursor (circled) indicates where you drag to position the clip._

In Ripple mode, when you hover over a selected clip in the Clips view, the cursor changes to indicate the action that you can perform. The actions and areas are similar to Mix mode:

* When you hover over the start of a selected clip, the cursor changes to a trim cursor. The trim cursor indicates the area to drag to trim the clip relative to its start.
* When you hover over the middle of a clip, the cursor changes to a position cursor and indicates the area to drag to position the clip.
* When you hover over the end of a clip, the cursor changes to a trim cursor. The trim cursor indicates the area to drag to trim the clip relative to its end.

In Ripple mode, when you click and drag to trim or position a clip, the cursor switches to a yellow arrow that points towards the affected clips and gaps. A yellow line indicates the ripple point. When you drag to trim a clip, dragging left and right changes the duration of the selected clip and repositions subsequent clips and gaps after the ripple point.

![For example, the yellow arrow cursor indicates that trimming the start of Clip 2A in Ripple mode changes the clip duration and affects the clips and gaps after the ripple point: Clip 2B and Clip 2C.](images/timeline_ripple_mode_yellow_arrow.png)

_For example, the yellow arrow cursor indicates that trimming the start of Clip 2A in Ripple mode changes the clip duration and affects the clips and gaps after the ripple point: Clip 2B and Clip 2C.]_

### Replace mode

Use Replace mode to add, position, and trim a clip while cutting or replacing intersecting clips.

![Timeline window with Replace mode as the selected Clip Edit mode. The position cursor (circled) indicates where you drag to position the clip.](images/timeline_replace_mode.png)

_Timeline window with Replace mode as the selected Clip Edit mode. The position cursor (circled) indicates where you drag to position the clip._

In Replace mode, when you hover over a selected clip in the Clips view, the cursor changes to indicate the action that you can perform. The actions and areas are similar to Mix mode:

* When you hover over the start of a selected clip, the cursor changes to a trim cursor. The trim cursor indicates the area to drag to trim the clip relative to its start.
* When you hover over the middle of a clip, the cursor changes to a position cursor and indicates the area to drag to position the clip.
* When you hover over the end of a clip, the cursor changes to a trim cursor. The trim cursor indicates the area to drag to trim the clip relative to its end.

In Replace mode, when you drag to position a clip, the clip becomes translucent so that you can view overlapping clips. If the clip being positioned overlaps other clips, the cursor changes to a red arrow and red replacement lines indicate where each overlap occurs. Releasing the clip cuts the underlying clip at each red overlap.

![For example, the red arrow cursor indicates that dragging Clip 2A to the right overlaps Clip 2B. Releasing the clip cuts Clip 2B at the point where the overlap occurs.](images/timeline_replace_mode_red_cut.png)

_For example, the red arrow cursor indicates that dragging Clip 2A to the right overlaps Clip 2B. Releasing the clip cuts Clip 2B at the point where the overlap occurs._

In Replace mode, trimming a clip is similar to positioning a clip. When you drag to trim a clip and it intersects another clip, the cursor changes to a red arrow and a red replacement line indicates where the overlap occurs. Releasing the trim cuts the intersecting clip at the red replacement line.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             # Adding clips

The Timeline window supports different methods of adding clips to tracks, depending on the type of track, where you click, and whether a clip or track is already selected.

The quickest method to add a clip is to right-click on an empty area within a track and select the appropriate Add option from the context menu. Depending on the track, the options for adding a clip change.

![Context menu for adding an Activation clip](images/timeline_clips_view_adding.png)

_Context menu for adding an Activation clip._

There are other ways to add clips:

* Select a clip option from the Track menu in the Track Header to add a clip at the location of the Timeline Playhead.
* Drag an animation Source Asset from the Project window to an empty area in the Timeline window to automatically create an Animation track and add an Animation clip.
* Drag an animation Source Asset from the Project window to an existing track in the Timeline window to add an Animation clip to the same track.
* Drag an audio Source Asset from the Project window to an empty area in the Timeline window to automatically create an Audio track and add an Audio clip.
* Drag a GameObject with a PlayableDirector component to create a nested Timeline instance. This automatically creates a Control track and adds a Control clip for the nested Timeline instance.
* Drag a Prefab from the Project window to an empty area in the Timeline window to add a Prefab instance to your Timeline instance. This automatically creates a Control track and adds a Control clip for the Prefab instance.
* Drag a GameObject with a Particle component to add a particle effect to your Timeline instance. This automatically creates a Control track and adds a Control clip for the duration of the Particle effect.

When you add a clip, the [selected Clip Edit mode](clp_about.md) determines how the added clip interacts with surrounding clips. For example, if you add an Animation clip or an Audio clip in Mix mode and the added clip intersects a clip on the same track, Timeline [creates a blend](clp_blend.md).
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 # Blending clips

Blend two clips on the same track to create a smooth transition between two Animation clips, two Audio clips, or two Playable clips. To blend two clips, select the Mix Clip Edit mode and position or trim one clip until it overlaps an adjacent clip.

In a blend, the first clip is referred to as the **outgoing clip** and the second clip is referred to as the **incoming clip**. The area where the outgoing clip transitions to the incoming clip is referred to as the **blend area**. The blend area sets the duration of the transition.

![The blend area shows the transition between the outgoing clip and incoming clip](images/timeline_clip_blend_area.png)

_The blend area shows the transition between the outgoing clip and incoming clip_

Although the Clips view represents a blend area as a single linear curve, the transition between clips is actually comprised of two blend curves. The blend curve for the outgoing clip is referred to as the **Blend Out** curve. The blend curve for the incoming clip is referred to as the **Blend In** curve. By default, each blend curve is automatically set to an ease-in and ease-out curve.

![Use Blend Curves to customize the blend area](images/timeline_inspector_blend_curves.png)

_Use Blend Curves to customize the blend area_

Use the **Blend Curves** in the Inspector window to change the shape for either the Blend In or Blend Out curve of the selected clip. However, the Inspector window only allows you to edit the properties of one clip at a time. You cannot simultaneously customize both blend curves from the same blend area.

To customize the **Blend Curves** for the transition between two clips:

1. Select the outgoing clip to customize its Blend Out curve (labelled **Out**).
2. Select the incoming clip to customize its Blend In curve (labelled **In**).

To customize either the Blend Out curve or Blend In curve, use the drop-down menu to switch from **Auto** to **Manual**. With **Manual** selected, the Inspector window shows a preview of the blend curve. Click the curve preview to open the Curve Editor below the Inspector window.

![Select Manual and click the curve preview to open the Curve Editor](images/timeline_inspector_curve_editor.png)

_Select Manual and click the curve preview to open the Curve Editor_

Use the Curve Editor to customize the shape of the blend curve. By default, the blend curve includes a key at the beginning of the curve and a key at the end of the curve. The Curve Editor provides the following different methods of modifying the blend curve:

* Select the key at the start or end of the blend curve and use the tangent handles to adjust the interpolation between keys.
* Add additional keys to change the shape of the blend curve by adding more interpolation points. Adding keys in the Curve Editor is the same as [adding keys in the Curves view](crv_keys_add.md).
* Right-click a key to delete or edit the key. Editing keys in the Curve Editor is the same as [editing keys in the Curves view](crv_keys_edit.md). Note that you cannot delete the first and last keys.
* Select a shape template from the bottom of the Curve Editor.

The Curve Editor also includes shape templates based on whether you are modifying the Blend In curve or the Blend Out curve. Select a shape template to change the blend curve to the selected shape template.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  # Duplicating clips

There are many ways to duplicate clips in the Clips view:

* Select a clip or multiple clips. Right-click in the Clips view and select **Duplicate** from the context menu.
* Select a clip or multiple clips. Hold Command/Control and press D.
* Right-click an unselected clip and choose **Duplicate** from the context menu.

Duplicating clips copies each selected clip and places the duplicates after the last clip on the same track. If you duplicate clips used in a blend or clips separated by a gap, the blend or gap is also duplicated.

If you duplicate an Animation clip that uses a recorded clip as its Source Asset, the recorded clip is also duplicated. The duplicate of the recorded clip only appears in your Project after you save the Scene or Project. For example, the following images demonstrates what happens if you duplicate an Animation clip named "Clip 2B" that uses the recorded clip named "Recorded (3)".

![Select the"Clip 2B", hold Command/Control and press D to duplicate](images/timeline_clip_duplicate_clip_before.png)

_Select the"Clip 2B", hold Command/Control and press D to duplicate_

![A duplicate Animation clip is placed at the end of the same track. The recorded clip associated with "Clip 2B" is also duplicated.](images/timeline_clip_duplicate_clip_after.png)

_A duplicate Animation clip is placed at the end of the same track. The recorded clip associated with "Clip 2B" is also duplicated._

![The new "Recorded (6)" recorded clip appears in the Project window after you save the Scene or Project](images/timeline_clip_duplicate_project.png)

_The new "Recorded (6)" recorded clip appears in the Project window after you save the Scene or Project_
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  # Easing-in and easing-out clips

Ease-in and ease-out a clip to create a smooth transition between a clip and its surrounding gaps. To create an ease-in or ease-out transition, select a clip and, in the Inspector window, set either the **Ease In Duration** or the **Ease Out Duration**.

![Use Ease In Duration and Ease Out Duration to smoothly transition into and out of the selected clip.](images/timeline_inspector_ease_in_out.png)

_Use Ease In Duration and Ease Out Duration to smoothly transition into and out of the selected clip._

Ease-in and ease-out transitions create different effects, depending on the track:

* On an Animation track or an Animation Override track, ease-in to an Animation clip to create a smooth transition between the animation in the gap before the clip and the Animation clip. Ease-out of an Animation clip to create a smooth transition between the Animation clip and the animation in the gap after the clip. For information on the factors that determine what animation occurs in the gap before and after an Animation clip, see [Setting gap extrapolation](clp_gap_extrap.md).

* On an Audio track, ease-in to an Audio clip to fade in the volume of the audio waveform. Ease-out of an Audio clip to fade out the volume of the audio waveform specified by the Audio clip.

* On a Playable track, ease-In to a Playable clip to fade in the effect or script in the Playable clip. Ease-out of a Playable clip to fade out the effect or script in the Playable clip.

![Ease-in and ease-out an Animation clip to transition between its animation and its gaps. Timeline represents ease-in and ease-out transitions as a linear curve.](images/timeline_clip_ease_in_out.png)

_Ease-in and ease-out an Animation clip to transition between its animation and its gaps. Timeline represents ease-in and ease-out transitions as a linear curve._

Although the Clips view represents an ease-in or ease-out transition as a single linear curve, every ease-in or ease-out transition is actually set to a gradually easing-in or easing-out curve by default. To change the shape of either the ease-in curve (labelled **In**) or the ease-out (labelled **Out**) curve, use the **Blend Curves** in the Inspector window.

![Use the Blend Curves to customize ease-in or ease-out transitions](images/timeline_inspector_blend_curves.png)

_Use the Blend Curves to customize ease-in or ease-out transitions_

Note that the **Blend Curves** might affect the blend area used for blending between two clips. The **Ease In Duration** and **Ease Out Duration** properties indicate whether the **Blend Curves** affect an ease-in or ease-out transition, or a blend. For example, If the **Ease Out Duration** is editable, then the Blend Out curve (labelled **Out**) affects the curve used by an ease-out transition. If the **Ease Out Duration** is not editable, then the Blend Out curve (labelled **Out**) affects the outgoing clip in a blend between two clips.

![Ease Out Duration is not editable, therefore the **Out** curve affects the blend area between two clips](images/timeline_inspector_ease_in_blend_out.png)

_Ease Out Duration is not editable, therefore the **Out** curve affects the blend area between two clips_

To customize either the ease-in or ease-out transition, use the drop-down menu to switch from **Auto** to **Manual**. With **Manual** selected, the Inspector window shows a preview of the blend curve. Click the curve preview to open the Curve Editor below the Inspector 