# Post Processing Extension

Use the Cinemachine Post Processing [extension](CinemachineVirtualCameraExtensions.html) to attach a Postprocessing V2 profile to a Virtual Camera.

**Note 1**: Unity recommends using Postprocessing V2 instead of Postprocessing V1.

**Note 2**: With HDRP and URP 7 and up, The PostProcessing package is deprecated, and is implemented natively by HDRP and URP.  In that case, please see the __CinemachineVolumeSettings__ extension.

The Cinemachine Post Processing extension holds a Post-Processing Profile asset to apply to a Virtual Camera when it is activated. If the camera is blending with another Virtual Camera, then the blend weight is applied to the Post Process effects also.

Before attaching post processing profiles to Virtual Cameras, you first need to set up your project to use post processing. 

To set up project to use Post Processing V2 with Cinemachine:

1. [Install](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@latest/index.html) the Postprocessing V2 package.

2. Select your Unity camera with Cinemachine Brain in the [Scene](https://docs.unity3d.com/Manual/UsingTheSceneView.html) view.

3. [Add the component](https://docs.unity3d.com/Manual/UsingComponents.html) named __Post-Process Layer__.  This will enable Post Process profiles to affect the Camera.

To add a Post Process profile to a Virtual Camera

4. Select your Virtual Camera in the [Scene](https://docs.unity3d.com/Manual/UsingTheSceneView.html) view or [Hierarchy](https://docs.unity3d.com/Manual/Hierarchy.html) window.

5. In the [Inspector](https://docs.unity3d.com/Manual/UsingTheInspector.html), choose __Add Extension > CinemachinePostProcessing__, then configre the Profile asset to have the effects you want when this virtual camera is live.

## Properties:

| **Property:** || **Function:** |
|:---||:---|
| __Profile__ || The Post-Processing profile to activate when this Virtual Camera is live. |
| __Focus Tracks Target__ || This is obsolete, please use __Focus Tracking__. |
| __Focus Tracking__ || If the profile has the appropriate overrides, will set the base focus distance to be the distance from the selected target to the camera.  The __Focus Offset__ field will then modify that distance. |
|| _None_ | No focus tracking |
|| _Look At Target_ | Focus offset is relative to the LookAt target |
|| _Follow Target_ | Focus offset is relative to the Follow target |
|| _Custom Target_ | Focus offset is relative to the Custom target |
|| _Camera_ | Focus offset is relative to the camera |
| __Focus Target__ || The target to use if __Focus Tracks Target__ is set to _Custom Target_ |
| __Focus Offset__ || Used when __Focus Tracking__ is not _None_.  Offsets the sharpest point away from the location of the focus target. |


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         # Cinemachine Recomposer Extension

Use the Cinemachine Recomposer [extension](CinemachineVirtualCameraExtensions.html) is an add-on module for Cinemachine Virtual Camera that adds a final tweak to the camera composition.  It is intended for use in a Timeline context, where you want to hand-adjust the output of procedural or recorded camera aiming.

All of these properties can be animated within the Timeline.

## Properties:

| **Property:** | **Function:** |
|:---|:---|
| __Apply After__ | The Volume Settings profile to activate when this Virtual Camera is live. |
| _Body_ | Camera has been positioned but not yet rotated |
|  _Aim_ | Camera has been rotated and positioned, but no noise or collision resolution applied |
|  _Noise_ | Camera has been positioned, rotated, and noise and other corrections applied |
|  _Finalize_ | Default setting.  Applied after all standard virtual camera processing has occurred |
| __Tilt__ | Add a vertical rotation to the camera's current rotation |
| __Pan__ | Add a horizontal rotation to the camera's current rotation |
| __Dutch__ | Add a tilt (local Z rotation) to the current camera's rotation |
| __Zoom Scale__ | Scale the current zoom |
| __Follow Attachment__ | When this is less than 1, damping on the Follow target is increased.  When the value is zero, damping is infinite - effectively "letting go" of the target |
| __Look At Attachment__ | When this is less than 1, damping on the Look At target is increased.  When the value is zero, damping is infinite - effectively "letting go" of the target |


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               # Saving in Play Mode

It’s often most convenient to adjust camera settings while the game is playing. But normally, Unity does not save your changes to the Scene when you exit Play Mode. Cinemachine has a special feature to preserve the tweaks you make during Play Mode.  It doesn’t save structural changes, like adding or removing a behavior. With the exception of certain properties, Cinemachine preserves most of the settings in your Virtual Cameras when you exit Play Mode.

When you exit Play Mode, Cinemachine scans the Scene to collect any changed properties in the Virtual Cameras.  Cinemachine saves these changes a second or so after exiting. Use the __Edit > Undo__ command to revert these changes.

Check __Save During Play__ on any Virtual Camera in the [Inspector](https://docs.unity3d.com/Manual/UsingTheInspector.html) to enable this feature.  This is a global property, not per-camera, so you only need to check or uncheck it once.

Cinemachine components have the special attribute `[SaveDuringPlay]` to enable this functionality. Feel free to use it on classes within your own custom scripts too if you need it. To exclude a field in a class with the `[SaveDuringPlay]` attribute, add the `[NoSaveDuringPlay]` attribute to the field.

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           # Setting up Virtual Cameras

In your project, organize your Scene Hierarchy to have a single Unity camera with a CinemachineBrain component and many Virtual Cameras.

To add a Virtual Camera to a Scene:

1. In the Unity menu, choose __Cinemachine > Create Virtual Camera__. <br/>Unity adds a new GameObject with a Cinemachine Virtual Camera component. If necessary, Unity also adds a [Cinemachine Brain](CinemachineBrainProperties.html) component to the Unity camera GameObject for you.

2. Use the __Follow__ property to specify a GameObject to follow. <br/>The Virtual Camera automatically positions the Unity camera relative to this GameObject at all times, even as you move it in the Scene.

3. Use the __Look At__ property to specify the GameObject that the Virtual Camera should aim at. <br/>The Virtual Camera automatically rotates the Unity camera to face this GameObject at all times, even as you move it in the Scene.

4. [Customize the Virtual Camera](CinemachineVirtualCamera.html) as needed. <br/>Choose the algorithm for following and looking at, and adjust settings such as the follow offset, the follow damping, the screen composition, and the damping used when re-aiming the camera.

![Adding a Virtual Camera to a Scene. Note the Cinemachine Brain icon next to the Main Camera.](images/CinemachineNewVCam.png)

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              # Cinemachine Smooth Path

A __Cinemachine Smooth Path__ is a component that defines a world-space path, consisting of an array of waypoints. Each waypoint has position and roll settings. Cinemachine uses Bezier interpolation to calculate positions between the waypoints to get a smooth and continuous path. The path passes through all waypoints. Unlike Cinemachine Path, first and second order continuity is guaranteed, which means that not only the positions but also the angular velocities of objects animated along the path will be smooth and continuous.

## Properties:

| **Property:** || **Function:** |
|:---|:---|:---|
| __Resolution__ || Path samples per waypoint. Cinemachine uses this value to calculate path distances. |
| __Looped__ || If checked, then the path ends are joined to form a continuous loop. |
| __Waypoints__ || The waypoints that define the path. They are interpolated using a bezier curve. |
| __Resolution__ || Path samples per waypoint. This is used for calculating path distances. |
| __Appearance__ || The settings that control how the path appears in the Scene view. |
| | _Path Color_ | The color of the path when it is selected. |
| | _Inactive Path Color_ | The color of the path when it is not selected. |
| | _Width_ | The width of the railroad tracks that represent the path. |
| __Looped__ || Check to join the ends of the path to form a continuous loop. |
| __Selected Waypoint__ || Properties for the waypoint you selected in the Scene view or in the Waypoints list. |
| __Waypoints__ || The list of waypoints that define the path. They are interpolated using a bezier curve. |
| | _Position_ | Position in path-local space. |
| | _Roll_ | Defines the roll of the path at this waypoint. The other orientation axes are inferred from the tangent and world up. |


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           # Cinemachine State-Driven Camera

The __Cinemachine State-Driven Camera__ component activates a child Virtual Camera when an animation target changes states. For example, consider your avatar’s local-motion system and orbit camera.  Your game feels more alive to the player when the camera shakes more as your avatar runs. When the avatar walks, blend for example to a Virtual Camera with more damping.

![State-Driven camera with three child Virtual Cameras (red)](images/CinemachineStateDrivenChildren.png)

The animation target for a State-Driven Camera is a GameObject with an [Animator](https://docs.unity3d.com/Manual/class-Animator.html) component controlled by an [Animator Controller](https://docs.unity3d.com/Manual/class-AnimatorController.html).

Assign normal __Look At__ and __Follow__ targets to each child Virtual Camera. If a child Virtual Camera has no __Look At__ or __Follow__ target, State-Driven camera uses its own, respective targets.

State-Driven Camera has a list that assigns child Virtual Cameras to animation states. You can define default and custom blends between the State-Driven children.

In the Inspector, the State-Driven camera lists its Virtual Camera children. Use this list to add and delete child Virtual Cameras, and assign priorities.

![Properties for Cinemachine State-Driven camera](images/CinemachineStateDrivenCamera.png)

To create a State-Driven camera:

1. Set up the animation target GameObject to [control it with an Animator Controller](https://docs.unity3d.com/Manual/AnimatorControllers.html).

2. In the Unity menu, choose __Cinemachine > Create State-Driven Camera__.<br/>A new State-Driven camera appears in the hierarchy with a new child Virtual Camera.

3. In the [Inspector](https://docs.unity3d.com/Manual/UsingTheInspector.html), assign the animation target you created in step 1 to the Animated Target property.

4. If needed, add more child VIrtual Cameras either by clicking + in Virtual Camera Children or dragging and dropping existing Virtual Cameras in the [Hierarchy](https://docs.unity3d.com/Manual/Hierarchy.html) window.

5. Use the __State__ to assign child Virtual Cameras to the animation states.

## Properties:

| **Property:** | **Function:** |
|:---|:---|
| __Solo__ | Toggles whether or not the Virtual Camera is temporarily live. Use this property to get immediate visual feedback in the [Game view](https://docs.unity3d.com/Manual/GameView.html) to adjust the Virtual Camera. |
| __Game Window Guides__ | Toggles the visibility of compositional guides in the Game view. These guides are available when Look At specifies a GameObject and the Aim section uses Composer or Group Composer, or when Follow specifies a target and the Body section uses Framing Composer. This property applies to all Virtual Cameras. |
| __Save During Play__ | Check to [apply the changes while in Play mode](CinemachineSavingDuringPlay.html).  Use this feature to fine-tune a Virtual Camera without having to remember which properties to copy and paste. This property applies to all Virtual Cameras. |
| __Priority__ | The importance of this State-Driven camera for choosing the next shot. A higher value indicates a higher priority. This property has no effect when using a State-Driven camera with Timeline. |
| __Look At__ | The default target GameObject that the children Virtual Camera move with. The State-Driven camera uses this target when the child does not specify this target. May be empty if all of the children define targets of their own. |
| __Follow__ | The default target GameObject to aim the Unity camera at. The State-Driven camera uses this target when the child does not specify this target. May be empty if all of the children define targets of their own. |
| __Animated Target__ | The GameObject that contains the Animator Controller. The State-Drive camera reacts to the animation state changes from this GameObject. |
| __Layer__ | The animation layer to observe in the Animated Target. |
| __Show Debug Text__ | Check to display a textual summary of the live Virtual Camera and blend in the game view. |
| __Default Blend__ | The blend which is used if you don’t explicitly define a blend between two Virtual Cameras. |
| __Custom Blends__ | The asset which contains custom settings for specific child blends. |
| __State__ | The list of animation state assignments for child Virtual Cameras.  |
| __Virtual Camera Children__ | The list of Virtual Cameras that are children of the State-Driven camera. |


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            # Storyboard

Use the Cinemachine Storyboard [extension](CinemachineVirtualCameraExtensions.html) to let artists, producers, and directors contribute to your game development. Cinemachine Storyboard places a still image in screen space over the output of the Unity camera.

Storyboard simplifies animatics for your team. Start with still images to pre-visualize terrain, layout, movement, lighting, timing, and so on. Following the intentions of the Storyboard image, developers incrementally add the assets, GameObjects, and settings that implement the Scene.

Use the properties in the Storyboard component to hide and show the image to compare it to the actual rendering of the Unity camera.

Storyboards can be muted at a global level - completely disabling them.  To toggle this, go to the Cinemachine main menu and select _Storyboard Global Mute_.

## Properties:

| **Property:** || **Function:** |
|:---|:---|:---|
| __Show Image__ || Toggle the visibility of the storyboard image. |
| __Image__ || The image to display as an overlay over the output of the Virtual Camera. |
| __Aspect__ || How to handle differences between image aspect and screen aspect. |
| | _Best Fit_ | Resize the image as large as possible on the screen without cropping. Preserve the vertical and horizontal proportions. |
| | _Crop Image To Fit_ | Resize the image to fill the screen, cropping if necessary. Preserve the vertical and horizontal proportions. |
| | _Stretch To Fit_ | Resize the image to fill the screen, adjusting the vertical or horizontal width if necessary.  |
| __Alpha__ || The opacity of the image. Use 0 for transparent, 1 for opaque. |
| __Center__ || The screen-space position of the image. Use 0 for center. |
| __Rotation__ || The screen-space rotation of the image. |
| __Scale__ || The screen-space scaling of the image. |
| __Sync Scale__ || Check to synchronize the scale of the x and y axes. |
| __Mute Camera__ || Check to prevent the Virtual Camera from updating the position, rotation, or scale of the Unity camera. Use this feature to prevent Timeline from [blending](CinemachineBlending.html) the camera to an unintended position in the Scene. |
| __Split View__ || Wipe the image on and off horizontally. |
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                # Cinemachine Target Group

Use Cinemachine Target Group to treat multiple GameObjects as a single Look At target. Use a Target Group with the [Group Composer](CinemachineAimGroupComposer.html) algorithm.

To create a Virtual Camera with a Target Group:

1. In the Unity menu, choose __Cinemachine > Create Target Group Camera__. <br/>Unity adds a new Virtual Camera and Target Group to the Scene. The __Follow__ and __Look At__ targets in the Virtual Camera refer to the new Target Group.

2. In the [Hierarchy](https://docs.unity3d.com/Manual/Hierarchy.html), select the new Target Group object.

3. In the [Inspector](https://docs.unity3d.com/Manual/UsingTheInspector.html), click the + sign to add a new item to the group.

4. In the new item, assign a GameObject, and edit the __Weight__ and __Radius__ properties.

5. To add more GameObjects to the Target Group, repeat steps 3-4.

![Cinemachine Target Group with two targets](images/CinemachineTargetGroup.png)

## Properties:

| **Property:** || **Function:** |
|:---|:---|:---|
| __Position Mode__ || How to calculate the position of the Target Group. |
| | _Group Center_ | Use the center of the axis-aligned bounding box that contains all items of the Target Group. |
| | _Group Average_ | Use the weighted average of the positions of the items in the Target Group. |
| __Rotation Mode__ || How to calculate the rotation of the Target Group.  |
| | _Manual_ | Use the values specified in the Rotation properties of the Target Group’s transform.  This is the recommended setting. |
| | _Group Average_ | Weighted average of the orientation of the items in the Target Group. |
| __Update Method__ || When to update the transform of the Target Group. |
| | _Update_ | Update in the normal MonoBehaviour Update() method. |
| | _Fixed Update_ | Updated in sync with the Physics module, in FixedUpdate(). |
| | _Late Update_ | Updated in MonoBehaviour `LateUpdate()`. |
| __Targets__ || The list of target GameObjects. |
| | _Weight_ | How much weight to give the item when averaging. Cannot be negative. |
| | _Radius_ | The radius of the item, used for calculating the bounding box. Cannot be negative. |
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         # Cinemachine and Timeline

Use [Timeline](https://docs.unity3d.com/Packages/com.unity.timeline@latest) to activate, deactivate, and blend between Virtual Cameras. In Timeline, combine Cinemachine with other GameObjects and assets to interactively implement and tune rich cutscenes, even interactive ones.

**Tip**: For simple shot sequences, use a [Cinemachine Blend List Camera](CinemachineBlendListCamera.html) instead of Timeline.

Timeline overrides the priority-based decisions made by [Cinemachine Brain](CinemachineBrainProperties.html). When the timeline finishes, control returns to the Cinemachine Brain, which chooses the Virtual Camera with the highest Priority setting.

You control Virtual Cameras in Timeline with a __Cinemachine Shot Clip__. Each shot clip points to a Virtual Camera to activate then deactivate. Use a sequence of shot clips to specify the order and duration of each shot.

To cut between two Virtual Cameras, place the clips next to each other. To blend between two Virtual Cameras, overlap the clips.

![Cinemachine Shot Clips in Timeline, with a cut (red) and a blend (blue)](images/CinemachineTimelineShotClips.png)

To create a Timeline for Cinemachine:

1. Create an empty GameObject in your Scene by choosing the __GameObject > Create Empty __menu item.

2. Give the empty GameObject a descriptive name. For example, `IntroTimeline`.

3. In your Scene, select your empty Timeline object as the focus to create a Timeline Asset and instance.

4. Click the padlock button to lock the TImeline window to make it easier to add and adjust tracks.

5. Drag a Unity camera with a CinemachineBrain component onto the Timeline Editor, then choose __Create Cinemachine Track__ from the drop-down menu.

6. Add other tracks to the Timeline for controlling the subjects of your Scene.  For example, add an Animation track to animate your main character.

**Tip**: Delete the default track that refers to your Timeline object. This track isn’t necessary for Timeline. For example, in the Timeline editor, right-click the track for IntroTimeline and choose __Delete__.

To add Cinemachine Shot Clips to a Cinemachine Track:

1. In the Cinemachine Track, right-click and choose __Add Cinemachine Shot Clip__.

2. Do one of the following:

    * To add an existing Virtual Camera to the shot clip, drag and drop it onto the Virtual Camera property in the Cinemachine Shot component.

    * To create a new Virtual Camera and add it to the shot clip, click Create in the Cinemachine Shot component.

3. In the Timeline editor, adjust the order, duration, cutting, and blending of the shot clip.

4. [Adjust the properties of the Virtual Camera](CinemachineVirtualCamera.html) to place it in the Scene and specify what to aim at or follow.

5. To animate properties of the Virtual Camera, create an Animation Track for it and animate as you would any other GameObject.

6. Organize your Timeline tracks to fine-tune your Scene.

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      # Top-down games

Cinemachine Virtual Cameras are modelled after human camera operators and how they operate real-life cameras.  As such, they have a sensitivity to the up/down axis, and always try to avoid introducing roll into the camera framing.  Because of this sensitivity, the Virtual Camera avoid looking straight up or down for extended periods.  They may do it in passing, but if the __Look At__ target is straight up or down for extended periods, they will not always give the desired result.

**Tip:** You can deliberately roll by animating properties like __Dutch__ in a Virtual Camera.

If you are building a top-down game where the cameras look straight down, the best practice is to redefine the up direction, for the purposes of the camera.  You do this by setting the __World Up Override__ in the [Cinemachine Brain](CinemachineBrainProperties.html) to a GameObject whose local up points in the direction that you want the Virtual Camera’s up to normally be.   This is applied to all Virtual Cameras controlled by that Brain.

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  # Using Cinemachine

Using Cinemachine requires a new way of thinking about working with cameras. For example, you might have invested heavily in carefully scripted camera behaviors. However, Cinemachine can give the same results, if not better, in less time.


## Virtual Cameras

Cinemachine does not create new cameras. Instead, it directs a single Unity camera for multiple shots. You compose these shots with __Virtual Cameras__. Virtual Cameras move and rotate the Unity camera and control its settings.

The Virtual Cameras are separate GameObjects from the Unity Camera, and behave independently. They are not nested within each other. For example, a Scene might look like this:

![A Scene containing a Unity camera with Cinemachine Brain (blue) and multiple Virtual Cameras (red)](images/CinemachineSceneHierarchy.png)

The main tasks that the Virtual Camera does for you:

* Positions the Unity camera in the Scene.
* Aims the Unity camera at something.
* Adds procedural noise to the Unity camera. Noise simulates things like handheld effects or vehicle shakes.

Cinemachine encourages you to create many Virtual Cameras. The Virtual Camera is designed to consume little processing power. If your Scene is performance-sensitive, deactivate all but the essential Virtual Cameras at any given moment for best performance.

It is recommended that you use a single Virtual Camera for a single shot. Take advantage of this to create dramatic or subtle cuts or blends. Examples:

* For a cutscene where two characters exchange dialog, use three Virtual Cameras: one camera for a mid-shot of both characters, and separate Virtual Cameras for a close-up of each character. Use Timeline to synchronize audio with the Virtual Cameras.

* Duplicate an existing Virtual Camera so that both Virtual Cameras are in the same position in the Scene. For the second Virtual Camera, change the FOV or composition. When a player enters a trigger volume, Cinemachine blends from the first to the second Virtual Camera to emphasize a change in action.

One Virtual Camera has control of the Unity camera at any point in time. This is the __live__ Virtual Camera. The exception to this rule is when a blend occurs from one Virtual Camera to the next. During the blend, both Virtual Cameras are live.

## Cinemachine Brain

The Cinemachine Brain is a component in the Unity Camera itself. The Cinemachine Brain monitors all active Virtual Cameras in the Scene. To specify the next live Virtual Camera, you [activate or deactivate](https://docs.unity3d.com/Manual/DeactivatingGameObjects.html) the desired Virtual Camera's game object. Cinemachine Brain then chooses the most recently activated Virtual Camera with the same or higher priority as the live Virtual Camera.  It performs a cut or blend between the previous and new Virtual Cameras.

**Tip**: Use Cinemachine Brain to respond to dynamic game events in real time. It allows your game logic to control the camera by manipulating priorities. This is particularly useful for live gameplay, where action isn’t always predictable. Use [Timeline](CinemachineTimeline.html) to choreograph cameras in predictable situations, like cutscenes. Timeline overrides the Cinemachine Brain priority system to give you precise, to-the-frame camera control.

## Moving and aiming

Use the [__Body__ properties](CinemachineVirtualCameraBody.html) in a Virtual Camera to specify how to move it in the Scene. Use the [__Aim__ properties](CinemachineVirtualCameraAim.html) to specify how to rotate it.

A Virtual Camera has two targets:

* The __Follow__ target specifies a GameObject for the Virtual Camera to move with.
* The __Look At__ target specifies the GameObject to aim at.

Cinemachine includes a variety of procedural algorithms to control moving and aiming. Each algorithm solves a specific problem, and has properties to customize the algorithm for your specific needs. Cinemachine implements these algorithms as `CinemachineComponent` objects. Use the `CinemachineComponent` class to implement a custom moving or aiming behavior.

The __Body__ properties offer the following procedural algorithms for moving the Virtual Camera in a Scene:

* __Transposer__: Move in a fixed relationship to the __Follow__ target, with optional damping.
* __Do Nothing__: Do not move the Virtual Camera.
* __Framing Transposer__: Move in a fixed screen-space relationship to the __Follow__ target, with optional damping.
* __Orbital Transposer__: Move in a variable relationship to the __Follow__ target, optionally accepting player input.
* __Tracked Dolly__: Move along a predefined path.
* __Hard Lock to Target__: Use the same position at the __Follow__ target.

The __Aim__ properties offer the following procedural algorithms for rotating a Virtual Camera to face the __Look At__ target:

* __Composer__: Keep the __Look At__ target in the camera frame, with compositional constraints.
* __Group Composer__: Keep multiple __Look At__ targets in the camera frame.
* __Do Nothing__: Do not rotate the Virtual Camera.
* __POV__: Rotate the Virtual Camera based on the user’s input.
* __Same As Follow Target__: Set the camera’s rotation to the rotation of the __Follow__ target.
* __Hard Look At__: Keep the __Look At__ target in the center of the camera frame.


## Composing a shot

The [__Framing Transposer__](CinemachineBodyFramingTransposer.html), [__Composer__](CinemachineAimComposer.html), and [__Group Composer__](CinemachineAimGroupComposer.html) algorithms define areas in the camera frame for you to compose a shot:

* __Dead zone__: The area of the frame that Cinemachine keeps the target in.

* __Soft zone__: If the target enters this region of the frame, the camera will re-orient to put it back in the dead zone.  It will do this slowly or quickly, according to the time specified in the Damping settings.

* __Screen__: The screen position of the center of the dead zone.  0.5 is the center of the screen.

* __Damping__: Simulates the lag that a real camera operator introduces while operating a heavy physical camera. Damping specifies quickly or slowly the camera reacts when the target enters the __soft zone__ while the camera tracks the target. Use small numbers to simulate a more responsive camera, rapidly moving or aiming the camera to keep the target in the __dead zone__. Larger numbers simulate heavier cameras, The larger the value, the more Cinemachine allows the target to enter the soft zone.

The __Game Window Guides__ gives an interactive, visual indication of these areas. The guides appear as tinted areas in the [Game view](https://docs.unity3d.com/Manual/GameView.html).

![Game Window Guides gives a visual indication of the damping, screen, soft zone, and dead zone](images/CinemachineGameWindowGuides.png)

The clear area indicates the __dead zone__. The blue-tinted area indicates the __soft zone__. The position of the soft and dead zones indicates the __screen__ position. The red-tinted area indicates the __no pass__ area, which the target never enters. The yellow square indicates the target.

Adjust these areas to get a wide range of camera behaviors. To do this, drag their edges in the Game view or edit their properties in the Inspector window. For example, use larger damping values to simulate a larger, heavier camera, or enlarge the __soft zone__ and __dead zone__ to create an area in the middle of the camera frame that is immune to target motion. Use this feature for things like animation cycles, where you don’t want the camera to track the target if it moves just a little.

## Using noise to simulate camera shake

Real-world physical cameras are often heavy and cumbersome. They are hand-held by the camera operator or mounted on unstable objects like moving vehicles. Use [Noise properties](CinemachineVirtualCameraNoise.html) to simulate these real-world qualities for cinematic effect. For example, you could add a camera shake when following a running character to immerse the player in the action.

At each frame update, Cinemachine adds noise separately from the movement of the camera to follow a target. Noise does not influence the camera’s position in future frames. This separation ensures that properties like __damping__ behave as expected.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     # Setting Virtual Camera properties

The Cinemacine Virtual Camera is a component that you add to an empty GameObject. It represents a Virtual Camera in the Unity Scene.

Use the __Aim__, __Body__, and __Noise__ properties to specify how the Virtual Camera animates position, rotation, and other properties. The Virtual Camera applies these settings to the Unity Camera when [Cinemachine Brain](CinemachineBrainProperties.html) or [Timeline](CinemachineTimeline.html) transfers control of the Unity camera to the Virtual Camera.

At any time, each Virtual Camera may be in one of these states:

* __Live__: The Virtual Camera actively controls a Unity camera that has a Cinemachine Brain. When a Cinemachine Brain blends from one Virtual Camera to the next, both Virtual Cameras are live. When the blend is complete, there is only one live Virtual Camera.

* __Standby__: The Virtual Camera doesn’t control the Unity camera. However, it still follows and aims at its targets, and updates at every frame. A Virtual Camera in this state is activated and has a priority that is the same as or lower than the live Virtual Camera.

* __Disabled__: The Virtual Camera doesn’t control the Unity camera and doesn’t actively follow or aim at its targets. A Virtual Camera in this state doesn’t consume processing power. To disable a Virtual Camera, deactivate its game object. The Virtual Camera is present but disabled in the Scene. However, even though the game object is deactivated, the virtual camera can still control the Unity camera if the virtual camera is participating in a blend, or if it is invoked by Timeline.

![Virtual Camera properties](images/CinemachineVCamProperties.png)

### Properties

| **Property:** || **Function:** |
|:---|:---|:---|
| __Solo__ || Toggles whether or not the Virtual Camera is temporarily live. Use this property to get immediate visual feedback in the [Game view](https://docs.unity3d.com/Manual/GameView.html) to adjust the Virtual Camera. |
| __Game Window Guides__ || Toggles the visibility of compositional guides in the Game view. These guides are available when Look At specifies a GameObject and the Aim section uses Composer or Group Composer, or when Follow specifies a target and the Body section uses Framing Composer. This property applies to all Virtual Cameras. |
| __Save During Play__ || Check to [apply the changes while in Play mode](CinemachineSavingDuringPlay.html).  Use this feature to fine-tune a Virtual Camera without having to remember which properties to copy and paste. This property applies to all Virtual Cameras. |
| __Priority__ || The importance of this Virtual Camera for choosing the next shot. A higher value indicates a higher priority. Cinemachine Brain chooses the next live Virtual Camera from all Virtual Cameras that are activated and have the same or higher priority as the current live Virtual Camera. This property has no effect when using a Virtual Camera with Timeline. |
| __Follow__ || The target GameObject that the Virtual Camera moves with. The [Body properties](CinemachineVirtualCameraBody.html) use this target to update the position of the Unity camera. Keep this property empty to make the Unity camera use the position of the Virtual Camera’ transform. For example, you might choose to animate the Virtual Camera in Timeline. |
| __Look At__ || The target GameObject to aim the Unity camera at. The [Aim properties](CinemachineVirtualCameraAim.html) use this target to update the rotation of the Unity camera. Keep this property empty to make the Unity camera use the orientation of the Virtual Camera. |
| __Position Blending__ || Style for blending positions to and from this Virtual Camera. |
| | _Linear_ | Standard linear position blend. |
| | _Spherical_ | Spherical blend about the Look At position, if there is a Look At target. |
| | _Cylindrical_ | Cylindrical blend about the Look At position, if there is a Look At target. Vertical coordinate is linearly interpolated. |
| __Lens__ || These properties mirror their counterparts in the properties for the [Unity camera](https://docs.unity3d.com/Manual/class-Camera.html). |
| | _Field Of View_ | The camera view in vertical degrees. For example, to specify the equivalent of a 50mm lens on a Super 35 sensor, enter a Field of View of 19.6 degrees. This property is available when the Unity camera with the Cinemachine Brain component uses a Projection of Perspective.   |
| | _Presets_ | A drop-down menu of settings for commonly-used lenses. Choose **Edit Presets** to add or edit the asset that contains a default list of lenses. |
| | _Orthographic Size_ | When using an orthographic camera, defines the half-height of the camera view, in world coordinates. Available when the Unity camera with the Cinemachine Brain component uses a Projection of Orthographic. |
| | _Near Clip Plane_ | The closest point relative to the camera where drawing occurs. |
| | _Far Clip Plane_ | The furthest point relative to the camera where drawing occurs. |
| __Dutch__ || Dutch angle. Tilts the Unity camera on the z-axis, in degrees. This property is unique to the Virtual Camera; there is no counterpart property in the Unity camera. |
| __Extensions__ || Components that add extra behaviors to the Virtual Camera.  |
| | _Add Extension_ | Choose a new [extension](CinemachineVirtualCameraExtensions.html) to add to the Virtual Camera. |


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 