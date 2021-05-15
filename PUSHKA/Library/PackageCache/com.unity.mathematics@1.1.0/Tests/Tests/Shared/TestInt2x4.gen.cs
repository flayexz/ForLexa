# Impulse

Cinemachine Impulse generates and manages camera shake in response to game events. For example, you can use Impulse to make a Cinemachine Virtual Camera shake when one GameObject collides with another, or when something in your Scene explodes.

Impulse has three parts: 

- **[Raw vibration signal](CinemachineImpulseRawSignal.md):** a vibration curve in up to 6 dimensions: X, Y, Z, pitch, roll, yaw.

- **[Impulse Source](CinemachineImpulseSourceOverview.md):** a component that emits the raw vibration signal from a point in Scene space, and defines signal characteristics such as duration, intensity, and range.

- **[Impulse Listener](CinemachineImpulseListener.md):** a Cinemachine extension that allows a Virtual Camera to “hear” an impulse, and react to it by shaking.

It’s useful to think about this in terms of individual “impulses.” An impulse is a single occurrence of an Impulse Source emitting a raw vibration signal. Collisions and events in your Scenes _trigger_ impulses, Impulse Sources _generate_ impulses, and Impulse Listeners _react_ to impulses.

## Getting started with Impulse

To set up and use Impulse in a Scene, do the following: 

- Add **[Cinemachine Impulse Source](CinemachineImpulseSource.md)** or **[Cinemachine Collision Impulse Source](CinemachineCollisionImpulseSource.md)** components to one or more GameObjects that you want to trigger camera shake.

- Connect Raw Signals to the Impulse Sources. These can be **[6D Noise Profiles](CinemachineImpulseNoiseProfiles.md)**, **[3D Fixed Signals](CinemachineImpulseFixedSignals.md)**, or custom signal types that you create yourself.

- Add a **[Cinemachine Impulse Listener](CinemachineImpulseListener.md)** extension to one or more Cinemachine virtual cameras so they can detect and react to impulses.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        # Filtering impulses

Filtering lets you fine-tune how and when an Impulse Source generates impulses. Cinemachine Impulse allows two types of filtering:

- Use channel filtering to set things up so that an Impulse Listener reacts to certain Impulse Sources and ignores others. See [Filtering with channels](#ChannelFiltering) below for details.

- Use Trigger Object Filtering with Collision Impulse Sources to set things up so that only certain GameObjects trigger an impulse. See [Filtering with layers and tags](#TriggerObjectFiltering) for details.

<a name="ChannelFiltering"></a>
## Filtering with channels

By default, every Impulse Listener reacts to every Impulse Source within range. Channels allow you to more precisely control which Impulse Sources an Impulse Listener reacts to. To set up channel filtering, you need to do three things:

- Set up your channels
- Set your Impulse Sources to broadcast on one or more channels
- Set your Impulse Listeners to listen to one or more channels

When an Impulse Listener is listening to specific channels, it only reacts to Impulse Sources that broadcast on those channels.

### Adding channels

The **CinemachineImpulseChannels** script creates channels in your Scene. It has one channel by default, and you can add as many new channels as you need, up to a maximum of 31.

To add new channels:

1. Inspect the **CinemachineImpulseChannels** script by doing one of the following:

    - In the Cinemachine Impulse Listener inspector, navigate to the **Channel Mask** drop-down and click the **Edit** button next to it.
    
    - In the Cinemachine Impulse Source or Cinemachine Collision Impulse Source inspector, navigate to the **Impulse Channel** drop-down and click the **Edit** button next to it.

2. Expand the **Impulse Channels** property group and set the **Size** property to the number of channels you want. A new entry appears for each channel.

3. Rename your new channels.

    ![](images/InspectorImpulseChannelsScript.png)

    Channels are available from the channel drop-down in the Inspector as soon as you add them. 

### Setting listen / broadcast channels

After setting up your channels, you need to define how your Impulse Listeners and Impulse Sources use them.

- Inspect each Impulse Listener, and choose the channels you want it to listen to from the **Channel Mask** drop-down.

    ![](images/InspectorImpulseListenerChannelsMenu.png)

- Inspect each Impulse Source or Collision Impulse Source, and choose the channels you want it to broadcast on from the **Impulse Channel** drop-down.

    ![](images/InspectorImpulseSourceChannelsMenu.png)

    You can select multiple filters from the drop down. You can also choose **Everything** to use all filters, or **Nothing** to use none of them.

<a name="TriggerObjectFiltering"></a>
## Filtering with layers and tags

You can use Unity’s [Layers](https://docs.unity3d.com/Manual/Layers.html) and [Tags](<https://docs.unity3d.com/Manual/Tags.html>) to specify which GameObjects trigger an impulse when they collide with a Collision Impulse Source, or enter a trigger zone. This is called **Trigger Object Filtering**.

The Cinemachine Collision Impulse Source component has two **Trigger Object Filter** properties:

- The **Layer Mask** drop-down lists all of the Scene’s layers. When you select one or more layers, GameObjects in those layers trigger impulses when they collide with the Impulse Source. The Impulse Source ignores collisions with GameObjects on other layers.

- The **Ignore Tag** drop-down lists all of the Scene’s tags. When you select a tag, GameObjects with that tag do not trigger impulses when they collide with the Impulse Source, even if they are in a layer specified in Layer Mask.

For example, in a Scene where a large animal is lumbering through a forest, you might want the camera to shake when it collides with large trees, but not small saplings. 

One way to set that up would be to make the animal a Collision Impulse Source, put all of the large trees on their own layer, and select that as the Layer Mask.

If all of the trees, large ones and saplings alike, are already on the same layer, you could assign a special tag to the saplings, and use the **Ignore Tag** property to filter them out.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              # Using Fixed Signals

The **CinemachineFixedSignal** Asset class is a simple 3D curve definition that lets you draw the curves yourself. Cinemachine Impulse either stretches or loops the curve to fit it in the attack-sustain-decay envelope, according to your specification in the Impulse Source.

## Connecting a fixed signal to an Impulse Source

To connect a fixed signal Asset to an Impulse Source, locate it in the Project view and drag it onto the **Raw Signal** field in the Impulse Source Inspector.

## Creating a new fixed signal

To create a new fixed signal Asset:

1. In the Impulse Source Inspector, click the gear icon next to the **Raw Signal** field and choose **New Fixed Signal**.

2. When prompted, give the new Asset a name and choose a location to save it to.

3. Locate the new Asset in the Project view and select it to open the Fixed Signal Inspector.

   ![](images/InspectorFixedSignalTiles.png)

   The Inspector displays a preview of the curve for each axis. Click a preview pane to define your own curve for that axis. If you leave it blank, there is no movement along that axis.

4. Click a preview curve to open the Curve editor, where you can edit the curve for that axis.

   ![](images/FixedSignalEditor.png)

5. Click one of the thumbnails at the bottom of the window to apply a curve preset. You can then edit the curve:

    - Drag the control points and Bezier handles to change the shape of the curve.
    - Click the gear icon next to the preset thumbnails to open the **Presets** window, where you can click **New** to add your current curve to the curve presets library.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      # Cinemachine Impulse Listener

Impulse [signals](CinemachineImpulseRawSignal.md) and [sources](CinemachineImpulseSourceOverview.md) don’t do anything on their own. An **Impulse Listener** is a Cinemachine extension that allows a virtual camera to “hear” impulse vibration signals and react to them.  

When you add an **Impulse Listener** extension to a virtual camera, it makes the camera shake in response to the signals emitted from Impulse Sources. In the simplest case, the Impulse Listener applies the signal verbatim to the camera’s Transform, causing it to shake.

In the image below, the figure’s feet are Impulse Sources. When they collide with the floor (A) they generate impulses. The camera is an Impulse Listener and reacts to the impulses by shaking (B), which shakes the resulting image in the Game view (C). 

![In this Scene, the figure’s feet are Impulse Sources. When they collide with the floor (A) they generate impulses. The camera is an Impulse Listener and reacts to the impulses by shaking (B), which shakes the resulting image in the Game view (C). ](images/ImpulseOverview.png)

To add an Impulse Listener to a Cinemachine virtual camera:

1. Select the virtual camera, navigate to the Inspector window and expand the **Cinemachine Virtual Camera** script.

2. Go to **Extensions > Add Extension**, and select **CinemachineImpulseListener**.

![](images/InspectorImpulseListener.png)

In the real world, some cameras are mounted less rigidly than others, and tend to shake more as a result. The Impulse Listener’s **Gain** property emulates this behavior by amplifying or attenuating impulse vibration signals. Higher values cause the camera to shake more.

>**TIP:** You can create your own Impulse Listener to interpret vibration signals any way you like.

By default, an Impulse Listener reacts to every Impulse Source in range, but you can apply [channel filtering](CinemachineImpulseFiltering.md#ChannelFiltering) to make a Listener respond to some Sources and ignore others.

##Properties:

| Property:           | Function:                                                    |
| ------------------- | ------------------------------------------------------------ |
| **Channel Mask**    | Specify the Impulse channels to react to. For details, see [Filtering with channels](CinemachineImpulseFiltering.md#ChannelFiltering).                   |
| **Gain**            | Amplify or attenuate the impulse signal. This property enables you to configure different Impulse Listeners to react differently to the same impulse signal. |
| **Use 2D Distance** | Enable this setting to ignore the z axis when calculating camera distance from the Impulse Source. Use this property for 2D games. |
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    # Using Noise Profiles

Cinemachine uses a **[Noise Profile](CinemachineNoiseProfiles.md)** Asset to generate Basic Multi-Channel Perlin noise for Cinemachine Virtual Cameras. You can use the same Asset to create a procedural raw signal for Impulse.

The **Noise Profile** Asset allows you to add noise in up to six dimensions: three rotational axes and three positional axes. Typical camera noise profiles use only rotational noise, but for Impulse you often want positional noise as well.

Cinemachine ships with several **Noise Profile** presets, which you can use as they are, or clone and customize. **6D Shake** is a good all-around profile that you can use for a wide variety of situations. 

## Connecting a noise profile to an Impulse Source

To connect a noise profile Asset to an Impulse Source or a Collision Impulse Source, do one of the following:

- In the Impulse Source Inspector, click the gear icon next to the **Raw Signal** field and choose a preset from the **Presets** section of the menu.

- In the Project view, locate the noise profile you want to connect and drag it onto the **Raw Signal** field in the Impulse Source Inspector.

## Creating a new noise profile

You can create your own noise profiles from scratch in the Noise Profile Inspector; however, you might find it easier to clone an existing preset and edit it.

To create a new noise profile Asset:

1. In the Impulse Source Inspector, click the gear icon next to the **Raw Signal** field and choose **New Noise Settings**.

2. When prompted, give the new Asset a name and choose a location to save it to.

3. Locate the new Asset in the Project view and select it to open the Noise Profile Inspector.

   ![img](images/InspectorNoiseProfile.png)

   The Inspector displays a compound waveform for rotational noise and positional noise. You can expand any rotational or positional axis to view and edit its waveforms individually.
   
   For details about creating custom noise profiles, see [Working with noise profiles](CinemachineNoiseProfiles.md)
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       # Raw Vibration Signals

Raw vibration signals are curves that vibrate as a function of time. Cinemachine provides two types of raw signal Asset:

- A 6D (x, y, z, pitch, roll, and yaw) noise profile. See [Using Noise Profiles](CinemachineImpulseNoiseProfiles.md) for details.

- A 3D Fixed Signal. See [Using Fixed Signals](CinemachineImpulseFixedSignals.md) for details.

Usually these raw signals are Assets in the Project.

In the Impulse Source Inspector window, use the **Raw Signal** field to see which signal is connected to the Impulse Source. The Settings menu (indicated with a gear icon) next to the field provides options for working with raw signals:

![img](images/InspectorImpulseSourceRawSignalMenu.png)

- **Edit** opens the signal in either the Noise Profile Inspector or the Fixed Signal Inspector.
- **Clone** duplicates the signal Asset. This is useful when you want to use a preset or an existing Asset as a base for a new signal.
- **Locate** finds the signal in the Project view.
- **Presets** provides a menu of preset Noise Profile Assets.
- **New Noise Settings** creates a new Noise Profile Asset from scratch. See [Creating a new noise profile](CinemachineImpulseNoiseProfiles.md#CreateNoiseProfile) for details.
- **New Fixed Signal** creates a new Fixed Signal Asset from scratch. See [Creating a new fixed signal](CinemachineImpulseFixedSignals.md#CreateFixedSignal) for details.


## Creating Custom Signals

You can create your own custom signals. If you derive them from the **[CinemachineSignalSource](../api/Cinemachine.SignalSourceAsset.html)** base class, they appear automatically in the Inspector menu alongside the built-in ones.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              # Cinemachine Impulse Source

Use the **Cinemachine Impulse Source** component to generate impulses on events that are not collisions or Collider triggers. This is a generic Impulse Source that exposes a family of `GenerateImpulse()` API methods. These methods generate impulses at the specified locations and with the specified velocities and strengths. Call these methods directly from your game logic, or use them with [UnityEvents](https://docs.unity3d.com/Manual/UnityEvents.html). 

>**TIP:** You can use the script for this component as an example to reference when creating your own custom impulse-generating classes.

To add a Cinemachine Impulse Source to your Scene:

1. Select the GameObject that you want to trigger camera shake, navigate to its Inspector, and click the **Add Component** button.

2. Go to **Scripts > Cinemachine**, and select **Cinemachine Impulse Source**.

By default, an Impulse Source affects every [Impulse Listener](CinemachineImpulseListener.md) in range, but you can apply [channel filtering](CinemachineImpulseFiltering.md#ChannelFiltering) to make Impulse Sources affect some Impulse Listeners and not others. 

##Properties:

The properties in the Cinemachine Impulse Source Inspector window are divided into the following sections.

- [Impulse Channel](#ImpulseChannel) (A)
- [Signal Shape](#SignalShape) (B)
- [Time Envelope](#TimeEnvelope) (C)
- [Spatial Range](#SpatialRange) (D)

![](images/InspectorImpulseSource.png)

<a name="ImpulseChannel"></a>
### Impulse Channel

Impulse Listeners filter impulses based on channels to control which Impulse Sources they react to. Channels work like Camera Layers, but are distinct from them. These properties control the channels that the Impulse Source broadcasts impulse signals on. For details, see documentation on [Filtering](CinemachineImpulseFiltering.md).

![](images/InspectorImpulseSourceChannel.png)

| **Property:**       | **Function:**                                                |
| ------------------- | ------------------------------------------------------------ |
| **Impulse Channel** | Choose one or more channels from the drop-down.<br /><br />Click **Edit** to modify existing channels or add new ones. |

<a name="SignalShape"></a>
### Signal Shape

These properties control the basic characteristics of the raw signal when it is emitted by the Impulse Source.

![](images/InspectorImpulseSourceSignal.png)

| **Property:**  | **Function:**                                                |
| -------------- | ------------------------------------------------------------ |
| **Raw Signal** | The raw signal form. <br /><br />Drag a signal Asset onto the signal name to connect the signal to the Impulse Source.<br /><br />Click the “gear” icon for additional signal options, including options for creating new signals.<br /><br />See [Raw Vibration Signals](CinemachineImpulseRawSignal.md) for details. |
|**Amplitude Gain** | Set a value by which to multiply the amplitude of **Raw Signal**. This controls the strength of the vibrations. This is set to 1 by default.<br /><br />Use values greater than 1 to amplify the signal, 1 for the original signal, values less than 1 and greater than 0 to attenuate the signal, and 0 to mute the signal. |
|**Frequency Gain**|Set a value by which to multiply the frequency of **Raw Signal**. This controls the speed of the vibrations. This is set to 1 by default.<br /><br />Use values greater than one to increase the frequency, 1 for the original signal, values less than 1 and greater than 0 to reduce the frequency of the signal. A frequency of 0 holds a single noise value indefinitely, as though time were frozen.|
|**Randomize**|Enable this to randomize the **Raw Signal’s** start time.<br /><br />This property is available when the **Raw Signal** is a noise profile Asset. See [Raw Vibration Signals](CinemachineImpulseRawSignal.md) for details.|
|**Repeat Mode**|Specify whether to loop the **Raw Signal** over the duration of the time envelope, or stretch it to fit the time envelope.<br /><br />This property is available when the **Raw Signal** is a fixed signal. See [Raw Vibration Signals](CinemachineImpulseRawSignal.md) for details.|

<a name="TimeEnvelope"></a>
### Time Envelope

These properties control the duration of the impulse and the intensity of the **Raw Signal** over that duration.

![](images/InspectorImpulseTimeEnvelope.png)

| Property:             | Function:                                                    |
| --------------------- | ------------------------------------------------------------ |
| **Attack**            | Define how the signal reaches full amplitude at the beginning of the waveform. Click the preview pane (grey bar) to set the shape of the curve, and use the numerical field to set the time (in seconds) to reach the full amplitude. Leave the preview pane blank to use a default curve that is suitable for most purposes. |
| **Sustain Time**      | Set the time to maintain the full amplitude of the waveform after the attack. |
| **Decay**             | Define how the signal goes from full amplitude to zero the end of the waveform. Click the preview pane (grey bar) to set the shape of the curve, and use the numerical field to set the time (in seconds) to mute the signal. Leave the preview pane blank to use a default curve that is suitable for most purposes. |
| **Scale With Impact** | Enable this to apply signal amplitude scaling to **Time Envelope**. If checked, the Cinemachine Impulse Source component scales the time envelope according to the strength of the impact: stronger impacts last longer, and milder impacts have a shorter duration. |


<a name="SpatialRange"></a>
### Spatial Range

These properties define a zone in the Scene. The impulses from this Impulse Source only affect Impulse Listeners in this zone.

![](images/InspectorImpulseSourceSpatialRange.png)

| Property:|| Function:|
|--|--|--|
|**Impact Radius**||Set the radius of the space in the Scene around the impact point where the signal stays at full amplitude. Beyond that the signal strength fades out over the dissipation distance. After that, the strength is zero. In other words, the total effect radius of the signal is **Impact Radius** + **Dissipation Distance**.|
|**Direction Mode**||Define how the Cinemachine Impulse Source component should apply the direction of the signal as the Impulse Listener moves away from the Impulse Source.|
||Fixed|Use **Fixed** to use a constant signal direction.|
||Rotate Toward Source|Use **Rotate Toward Source** to rotate the signal in the direction of the Impulse Source, giving a subtle visual clue about the source’s location. **Rotate Toward Source** has no effect on radially-symmetric signals.|
|**Dissipation Mode**||Define how the signal dissipates when the listener is outside the **Impact Radius**.|
||Linear Decay|The signal dissipates evenly over the **Dissipation Distance**.|
||Soft Decay|The signal dissipates slowly at the beginning of the **Dissipation Distance**, more quickly in the middle, and slowly again as it reaches the end.|
|| Exponential Decay|The signal dissipates very quickly at the beginning of the **Dissipation Distance**, then more and more slowly as it reaches the end.|
|**Dissipation Distance**||Set the distance beyond the impact radius over which the signal decays from full strength to nothing.|
|**Propagation Speed**||The speed (m/s) at which the impulse propagates through space.  High speeds allow listeners to react instantaneously, while slower speeds allow listeners in the scene to react as if to a wave spreading from the source. |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       # Cinemachine Impulse Sources

An Impulse Source is a component that emits a vibration signal from a point in Scene space. Game events can cause an Impulse Source to emit a signal from the place where the event occurs. The event _triggers_ impulses, and the source _generates_ impulses. Virtual cameras with an Impulse Listener extension _react_ to impulses by shaking.

In the image below, the figure's feet are Impulse Sources. When they collide with the floor (A) they generate impulses. The camera is an Impulse Listener and reacts to the impulses by shaking (B), which shakes the resulting image in the game view (C). 

![In this Scene, the figure's feet are Impulse Sources. When they collide with the floor (A) they generate impulses. The camera is an Impulse Listener and reacts to the impulses by shaking (B), which shakes the resulting image in the game view (C). ](images/ImpulseOverview.png)

Cinemachine ships with two types of Impulse Source component.

- **[Cinemachine Collision Impulse Source](CinemachineCollisionImpulseSource.md)** generates impulses in reaction to collisions and trigger zones.

- **[Cinemachine Impulse Source](CinemachineImpulseSource.md)** generates impulses in reaction to events other than collisions.  

Your Scene can have as many Impulse Sources as you want. Here are a few examples of where you might use Impulse Source components in a Scene:

- On each of a giant’s feet, so that the ground shakes when the giant walks.

- On a projectile that explodes when it hits a target.

- On the surface of a gelatin planet that wobbles when something touches it.

By default, an Impulse Source affects every [Impulse Listener](CinemachineImpulseListener.md) in range, but you can apply [channel filtering](CinemachineImpulseFiltering.md#ChannelFiltering) to make Sources affect some Listeners and not others. 

## Key Impulse Source properties

While the raw vibration signal defines the basic “shape” of the camera shake, the Impulse Source controls several other important properties that define the impulses it generates.

Understanding some of these properties in detail can help you create more realistic camera shake setups. 

Below you'll find detailed descriptions of the following key properties:

- **[Amplitude](#Amplitude):** controls the strength of the vibration.

- **[Orientation and direction](#Orientation):** Impulse can transform the signal so that the vibrations are consistent with the direction of the impact that produces them. 

- **[Time envelope](#TimeEnvelope):** controls the signal’s attack, sustain, and decay so that the signal fades in and out to the appropriate intensity and has a finite duration.

- **[Spatial range](#SpatialRange):** controls how far the signal travels in the Scene before it fades out completely

For descriptions of all Impulse Source properties, as well as instructions for adding Impulse Sources to your scene, see documentation on the [Impulse Source](CinemachineImpulseSource.md) and [Collision Impulse Source](CinemachineCollisionImpulseSource.md) components.

<a name="Amplitude"></a>
### Amplitude

The amplitude of the raw impulse signal controls the strength of the vibration for each impact. There are two ways to adjust the amplitude for a given Impulse Source.

The **Amplitude Gain** property amplifies or attenuates the raw impulse signal. It affects all impacts, all the time. Think of it as a global “volume” setting for turning the strength of an Impulse Source’s vibrations up or down.

Changing the magnitude of the **Velocity** vector when generating the signal also scales the signal amplitude, but the effect is per-impact rather than global. By adjusting the velocity of individual Impulse events, you can set things up so that light impacts make smaller vibrations, and heavy impacts make bigger ones. 

- For the **Cinemachine Impulse Source** component you must set the velocity vector yourself via a script.  The signal’s amplitude scales by the magnitude of this vector.

- The **Cinemachine Collision Impulse Source** component calculates the velocity vector automatically according to rules defined by the three properties in the **[How To Generate The Impulse](CinemachineCollisionImpulseSource.md#GenerateImpulse)** section. 

These global and per-impact adjustments are multiplied to calculate the actual amplitude of each impact.


<a name="Orientation"></a>
### Orientation and Direction

To create realistic vibrations, an impulse signal should be strongest along the axis of impact, and its amplitude (or strength) should be proportional to the force of the impact. For example, if you strike a wall with a hammer, the wall vibrates primarily along the axis of the hammer’s path. For the hammer’s impulse signal to be realistic, it should have the most vibration along that axis.

In the image below, the main axis for vibration (A) matches the direction the hammer is traveling when it hits the wall (B).

![The main axis for vibration (A) matches the direction the hammer is traveling when it hits the wall (B).](images/ImpulseHammerStrike.png)

Rather than requiring separate signal definitions for every possible impact direction and strength, Impulse uses the concept of a “local space” for defining the raw signal. You can rotate and scale the raw signal in its local space to produce a “final” signal that matches the actual impact.

Impulse assumes that the main direction for an impact is “down,” so as a general rule, your signals should put more vibration along the Y axis (the 6D shake noise preset does this). You can then rely on local-space rotation and scaling to produce the correct vibrations for each impact occurrence.

#### Controlling orientation and direction

The **Cinemachine Impulse Source** and **Cinemachine Collision Impulse Source** components have properties that control the orientation of the raw signal. Use these properties to mimic real-world vibration.

- For the **Cinemachine Impulse Source** component, the [GenerateImpulse()](../api/Cinemachine.CinemachineImpulseSource.html#Cinemachine_CinemachineImpulseSource_GenerateImpulse_) method takes a velocity vector as a parameter.  This vector defines the direction and strength of the impact.  The Impulse system uses this to calculate the final signal based on the Raw signal definition, rotating and scaling it appropriately.

- For the **Cinemachine Collision Impulse Source** component, the velocity vector is generated automatically based on the directions and masses of the GameObjects involved.

  To control how this is done, use the properties in the **[How To Generate The Impulse](CinemachineCollisionImpulseSource.md#GenerateImpulse)** section of the Inspector window. The **Use Impact Direction** property controls whether the signal is rotated to correspond to the impact direction.

#### Direction mode

The **[Spatial Range](CinemachineImpulseSource.md#SpatialRange) >  Direction Mode** property allows you to add a subtle tweak to the signal orientation. When you set it to **Rotate Towards Source**, the impulse signal is further rotated so that vibrations point a little more prominently in the direction of the Impulse Source.

The effect isn’t noticeable for radially symmetric vibrations, but for signals that emphasize a direction, like 6D shake, it gives a subconscious indication of where the vibration is coming from. This can be quite effective when you generate impacts in multiple locations and you don't want them to all feel the same.

The default **Direction Mode** setting of **Fixed** turns the effect off.


<a name="TimeEnvelope"></a>
### Time envelope

Vibrations from a real-world impact get stronger until they reach their peak strength, then weaken until the vibration stops. How long this cycle takes depends on the strength of the impact, and the characteristics of the GameObjects involved.

For instance, striking a concrete wall with a hammer produces a short, sharp impact. The vibrations reach their peak strength almost instantly and stop almost instantly. Striking a large sheet of thin metal, on the other hand, produces sustained vibration, which starts suddenly, stays at peak intensity for a while, and gradually softens. 

Use an Impulse Source’s **[Time Envelope](CinemachineImpulseSource.md#TimeEnvelope)** properties to control this cycle when impacts in the Scene shake the camera. The time envelope has three properties:

- **Attack** controls the Impulse signal’s transition to peak intensity.
- **Sustain Time** specifies how long the signal stays at peak intensity.
- **Decay** controls the signal’s transition from peak intensity to zero.

Both **Attack** and **Decay** consist of a duration value that specifies how long the transition takes, and a curve that defines how it happens (for example, whether it happens gradually or suddenly). The curves are optional; if you leave them blank in the Inspector window, Impulse uses its default curves, which are suitable for most purposes. **Sustain Time** is a duration value only.

Taken together, these properties control how long vibrations from an impact occurrence last, and how they fade in and fade out. However, they don’t account for the strength of the impact. To do that, enable the **Scale with Impact** property. When it’s enabled, the time envelope scales according the strength of an impact. Stronger impacts make the envelope longer, and weaker ones make it shorter. This does not affect the envelope’s proportions.

<a name="SpatialRange"></a>
### Spatial Range

An Impulse Source’s **[Spatial Range](CinemachineImpulseSource.md#SpatialRange)** properties define the zone in the Scene that the Impulse Source affects. Impulse Listeners in this zone react to the Impulse source (unless they are [filtered out](CinemachineImpulseFiltering.md)), while Listeners outside of it do not.

The zone consists of two parts: the **Impact Radius** and the **Dissipation Distance**. When the Impulse Source generates an impulse, the vibration signal stays at full strength until it reaches the end of the **Impact Radius**. Its strength then fades to zero over the **Dissipation Distance**. Together, these two properties define the signal’s total range.

In the image below, the vibration signal stays at full strength from the time it’s emitted from impact point until it reaches the Impact Radius (A), then fades out over the Dissipation Distance (B).

![The vibration signal stays at full strength from the time it’s emitted from impact point until it reaches the Impact Radius (A), then fades out over the Dissipation Distance (B).](images/ImpulseSpatialRange.png)

The **Dissipation Mode**  property controls _how_ the signal fades out over the **Dissipation Distance**. 

* **Exponential Decay** creates a fade-out that starts fast and slows down as it nears the end of the dissipation distance.

* **Soft Decay** creates a fade-out that starts slow, speeds up, and slows down again as it nears the end of the dissipation distance.

* **Linear Decay** creates an even fade-out over the dissipation distance.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     # Managing and grouping Virtual Cameras

A __manager__ camera oversees many Virtual Cameras but acts as a single Virtual Camera from the point of view of Cinemachine Brain and Timeline.

Cinemachine includes these manager cameras:

* [Free Look Camera](CinemachineFreeLook.html): an enhanced [Orbital Transposer](CinemachineBodyOrbitalTransposer.html). It manages three horizontal orbits, arranged vertically to surround an avatar.

* [Mixing Camera](CinemachineMixingCamera.html): uses the weighted average of up to eight child Virtual Cameras.

* [Blend List Camera](CinemachineBlendListCamera.html): executes a sequence of blends or cuts of its child Virtual Cameras.

* [Clear Shot Camera](CinemachineClearShot.html): picks the child Virtual Camera with the best view of the target.

* [State-Driven Camera](CinemachineStateDrivenCamera.html): picks a child Virtual Camera in reaction to changes in animation state.

Because manager cameras act like normal Virtual Cameras, you can nest them. In other words, create arbitrarily complex camera rigs that combine regular Virtual Cameras and manager cameras.

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       # Cinemachine Mixing Camera

The __Cinemachine Mixing Camera__ component uses the weighted average of its child Virtual Cameras to compute the position and other properties of the Unity camera.

![Cinemachine Mixing Camera with two child Virtual Cameras (red)](images/CinemachineMixingCamera.png)

Mixing Camera manages up to eight child Virtual Cameras. In the Mixing Camera component, these Virtual Cameras are fixed slots, not a dynamic array. Mixing Camera uses this implementation to support weight animation in Timeline. Timeline cannot animate array elements.

To create a Mixing Camera:

1. In the Unity menu, choose __Cinemachine > Create Mixing Camera__.
A new Mixing Camera appears in the [Hierarchy](https://docs.unity3d.com/Manual/Hierarchy.html) window. By default, Unity also adds two Virtual Cameras as children of the Mixing Camera.

2. Adjust the children Virtual Cameras.

3. Add up to six more child cameras.

4. Select the Mixing Camera in the Hierarchy window then adjust the Child Camera Weights in the [Inspector](https://docs.unity3d.com/Manual/UsingTheInspector.html) window.

![Child Camera Weights (red) and their contributions to the final position (blue)](images/CinemachineMixingCameraChildren.png)

## Properties:

| **Property:** | **Function:** |
|:---|:---|
| __Solo__ | Toggles whether or not the Mixing Camera is temporarily live. Use this property to get immediate visual feedback in the [Game view](https://docs.unity3d.com/Manual/GameView.html) to adjust the Virtual Camera. |
| __Game Window Guides__ | Toggles the visibility of compositional guides in the Game view. This property applies to all Virtual Cameras. |
| __Save During Play__ | Check to [apply the changes while in Play mode](CinemachineSavingDuringPlay.html).  Use this feature to fine-tune a Virtual Camera without having to remember which properties to copy and paste. This property applies to all Virtual Cameras. |
| __Priority__ | The importance of this Mixing Camera for choosing the next shot. A higher value indicates a higher priority. Cinemachine Brain chooses the next live Virtual Camera from all Virtual Cameras that are activated and have the same or higher priority as the current live Virtual Camera. This property has no effect when using a Virtual Camera with Timeline. |
| __Child Camera Weights__ | The weight of the Virtual Camera. Each child Virtual Camera has a corresponding Weight property. Note that setting one camera's weight to 1 does not put the other weights to zero.  The contribution of any individual camera is its weight divided by the sum of all the child weights. |
| __Mix Result__ | A graphical representation of the weights of the child Virtual Cameras. The light part of the bar of each child camera represents the proportion of its contribution to the final position of the Mixing Camera. When the bar is completely dark, the camera makes no contribution to the position of the Mixing Camera. |


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               # Multiple Unity cameras

Split-screen and picture-in-picture effects require the use of more than one Unity camera. Each Unity camera presents its own view on the player’s screen.

To use a multi-camera split-screen for two players:

1. For each player, [create a layer](https://docs.unity3d.com/Manual/Layers.html). For example, for two players, create layers named P1 and P2.

2. Add two Unity cameras to your Scene, set up their viewports, and give each one its own Cinemachine Brain component.

3. For each Unity camera, set the __Culling Mask__ to the appropriate layer while excluding the other layer. For example, set the first Unity camera to include layer P1 while excluding P2.

4. Add 2 Virtual Cameras, one to follow each player to follow the players. Assign each Virtual Camera to a player layer.

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    # Working with noise profiles

A __noise profile__ is an asset that defines a procedural curve for camera noise. The __Basic Multi Channel Perlin__ component applies a noise profile to the movement of the camera. Cinemachine applies noise movement after computing the position of the camera. This way, camera noise does not affect the computation of camera movement in future updates.

Cinemachine includes some predefined profile assets. Choose a predefined noise profile in the Noise component. Create your own noise profile asset by choosing __Create > Cinemachine > NoiseSettings__ in the [Project window](https://docs.unity3d.com/Manual/ProjectView.html).

The properties in the Inspector show graphs that give a visual representation of the noise profile. There are properties for the x, y, and z axes for position and rotation. Each axis may have more than one layer.

![Editing the first noise layer for Position X](images/CinemachineNoiseProfile.png)

For realistic procedural noise, choose frequencies and amplitudes with care to ensure an interesting noise quality that is not obviously repetitive. The most convincing camera shakes use __Rotation__ noise because that’s where the camera is aiming. Handheld camera operators tend to shake more rotationally than they do positionally. After specifying __Rotation__ noise, add __Position__ noise.

Convincing noise profiles typically mix low, medium, and high frequencies together. When creating a new noise profile, start with these three layers of noise for each axis.

For amplitude, use larger values for wider lenses to shake the camera noticeably. For telephoto lenses, use smaller amplitude values because the narrower FOV amplifies the effect.

For frequency, a typical low range is 0.1-0.5 Hz, the mid range 0.8-1.5, and the high 3-4. The highest useful frequency depends on the frame rate of your game. A game typically runs at 30 or 60Hz. Noise frequencies higher than the frame rate of your game fall between the cracks of the Nyquist rate. In other words, they will not be directly tracked.

For example, if your game runs at 60 frames/second and you set a frequency to 100, you will get choppy camera noise. This is because your game can’t render something that moves faster than the frame rate.

## Properties:

| **Property:** || **Function:** |
|:---|:---|:---|
| __Preview Time__ || The number of seconds to display in the graphs in the Inspector. This property is for editing in the Inspector; it does not affect the content of the noise profile asset that you are editing. |
| __Preview Height__ || The vertical height of the graphs of the noise profile in the Inspector. This property is for editing noise profiles; it does not affect the noise profile asset. |
| __Animated__ || Check to show a moving representation of an example of the noise profile in the graph. This property is for editing noise profiles; it does not affect the noise profile asset. |
| __Position Noise__ || A graphical representation of all noise layers for all axes for camera movement.  |
| __Position X__, __Position Y__, __Position Z__ || The layers of noise for each axis to apply to camera movement. Each axis has a graphical representation of its layers. Each layer has properties for Frequency, Amplitude, and optional Perlin noise. Click + or - to add and remove layers, respectively.  |
| | _Frequency_ | The frequency of the wave in the noise layer, in Hz. |
| | _Amplitude_ | The amplitude (height) of the wave in the noise layer, in distance units. |
| | _Non-random wave if checked_ | Check to remove the Perlin noise from the noise layer. Without Perlin noise, Cinemachine uses a regular sine wave. Uncheck to apply Perlin noise to the layer, randomizing both the frequency and the amplitude while remaining in the neighbourhood of the selected values. |
| __Rotation Noise__ || A graphical representation of all noise layers for all axes for camera rotation. |
| __Rotation X__, __Rotation Y__, __Rotation Z__ || The layers of noise for each axis to apply to camera rotation. Each layer has properties for Frequency, Amplitude, and optional Perlin Noise. Click + or - to add and remove layers, respectively.  |
| | _Frequency_ | The frequency of the wave in the noise layer, in Hz. |
| | _Amplitude_ | The amplitude (height) of the wave in the noise layer, in degrees. |
| | _Non-random wave if checked_ | Check to remove the Perlin noise from the noise layer. Without Perlin noise, Cinemachine uses a regular sine wave. Uncheck to include random Perlin noise variation, randomizing both the frequency and the amplitude while remaining in the neighbourhood of the selected values. |


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     # Cinemachine Path

__Cinemachine Path__ is a component that defines a world-space path, consisting of an array of waypoints. Each waypoint specifies position, tangent, and roll settings. Bezier interpolation is performed between the waypoints, to get a smooth and continuous path.

**Tip**: While the path position will always be smooth and continuous, it is still possible to get jarring movement when animating along the path. This happens when tangents aren’t set to ensure continuity of both the first and second order derivatives. It is not easy to get this right.  To avoid this potentially jarring movement, use Cinemachine Smooth Path. CinemachineSmoothPath sets the tangents automatically to ensure complete smoothness.

## Properties:

| **Property:** || **Function:** |
|:---|:---|:---|
| __Resolution__ || Path samples per waypoint. Cinemachine uses this value to limit the granularity when calculating path distances. The crosshatches on the path gizmo in the scene view reflect this value|
| __Appearance__ || The settings that control how the path appears in the Scene view. |
| | _Path Color_ | The color of the path when it is selected. |
| | _Inactive Path Color_ | The color of the path when it is not selected. |
| | _Width_ | The width of the railroad tracks that represent the path. |
| __Looped__ || Check to join the ends of the path to form a continuous loop. |
| __Selected Waypoint__ || Properties for the waypoint you selected in the Scene view or in the Waypoints list. |
| __Prefer Tangent Drag__ || Check to use the Gizmo for the tangent of a waypoint when the Gizmos for the tangent and position coincide in the Scene view.  |
| __Waypoints__ || The list of waypoints that define the path. |
| | _Position_ | Position in path-local space (i.e. relative to the transform of the path object itself) |
| | _Tangent_ | Offset from the position, which defines the tangent of the curve at the waypoint. The length of the tangent encodes the strength of the bezier handle. The same handle is used symmetrically on both sides of the waypoint, to ensure smoothness. |
| | _Roll_ | The roll of the path at this waypoint. The other orientation axes are inferred from the tangent and world up. |


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           # Using the Cinemachine Pixel Perfect extension

Both the __Pixel Perfect Camera__ and Cinem