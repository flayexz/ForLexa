* The [pre-extrapolate and post-extrapolate settings](clp_gap_extrap.md) for the Animation clip and for other Animation clips on the same track.
* Animation clips on other Animation tracks that are bound to the same GameObject.
* The position or animation of the GameObject in the Scene, outside the Timeline Asset.

## Gap extrapolation and easing clips

To successfully ease-in or ease-out an Animation clip, gap extrapolation must not be set based on the Animation clip being eased-in or eased-out. Gap extrapolation must either be set to **None** or set by another Animation clip.

For example, the following ease-in transition has no effect because the Pre-Extrapolate for the Victory_Dance clip is set to **Hold**. This means that the ease-in creates a transition between the first frame of the Animation clip and the rest of the Animation clip.

![The gap is set to **Hold** from the Animation clip (circled). The ease-in transition has no effect.](images/timeline_clip_ease_in_bad_gap.png)

_The gap is set to **Hold** from the Animation clip (circled). The ease-in transition has no effect._

![To ease-in from the Idle clip, set pre-extrapolate for the Victory_Dance clip to **None**. The ease-in gap uses the post-extrapolate mode from the Idle clip (circled).](images/timeline_clip_ease_in_good_gap.png)

_To ease-in from the Idle clip, set pre-extrapolate for the Victory_Dance clip to **None**. The ease-in gap uses the post-extrapolate mode from the Idle clip (circled)._

## Overriding Animation tracks with ease-in and ease-out transitions

Use two Animation tracks bound to the same GameObject to create a smooth transition between two Animation clips.

For example, if two Animation tracks are bound to the same GameObject and a clip on the second track contains an ease-in transition, the ease-in transition creates a smooth transition between the animation on the previous track and the animation on the second track.

![Example of using two Animation tracks, bound to the same GameObject, to create smooth transitions between Animation clips.](images/timeline_clip_ease_in_override_track.png)

_Example of using two Animation tracks, bound to the same GameObject, to create smooth transitions between Animation clips._

In this example, the Animation clip on the first track is a repeated idle cycle where the humanoid GameObject stands still. The Animation clip in the second track eases-in the Victory_Dance motion and eases-out to return back to the idle cycle

To successfully override animation on a previous track, th