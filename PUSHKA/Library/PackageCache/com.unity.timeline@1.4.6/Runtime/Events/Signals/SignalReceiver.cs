using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

#if CINEMACHINE_HDRP || CINEMACHINE_LWRP_7_0_0
    #if CINEMACHINE_HDRP_7_0_0
    using UnityEngine.Rendering.HighDefinition;
    #else
        #if CINEMACHINE_LWRP_7_0_0
        using UnityEngine.Rendering.Universal;
        #else
        using UnityEngine.Experimental.Rendering.HDPipeline;
        #endif
    #endif
#endif

namespace Cinemachine
{
    /// <summary>
    /// CinemachineBrain is the link between the Unity Camera and the Cinemachine Virtual
    /// Cameras in the scene.  It monitors the priority stack to choose the current
    /// Virtual Camera, and blend with another if necessary.  Finally and most importantly,
    /// it applies the Virtual Camera state to the attached Unity Camera.
    ///
    /// The CinemachineBrain is also the place where rules for blending between virtual cameras
    /// are defined.  Camera blending is an interpolation over time of one virtual camera
    /// position and state to another. If you think of virtual cameras as cameramen, then
    /// blending is a little like one cameraman smoothly passing the camera to another cameraman.
    /// You can specify the time over which to blend, as well as the blend curve shape.
    /// Note that a camera cut is just a zero-time blend.
    /// </summary>
    [DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
//    [RequireComponent(typeof(Camera))] // strange but true: we can live without it
    [DisallowMultipleComponent]
#if UNITY_2018_3_OR_NEWER
    [ExecuteAlways]
#else
    [ExecuteInEditMode]
#endif
    [AddComponentMenu("Cinemachine/CinemachineBrain")]
    [SaveDuringPlay]
    [HelpURL(Documentation.BaseURL + "manual/CinemachineBrainProperties.html")]
    public class CinemachineBrain : MonoBehaviour
    {
        /// <summary>
        /// When enabled, the current camera and blend will be indicated in the 
        /// game window, for debugging.
        /// </summary>
        [Tooltip("When enabled, the current camera and blend will be indicated in "
            + "the game window, for debugging")]
        public bool m_ShowDebugText = false;

        /// <summary>
        /// When enabled, shows the camera's frustum in the scene view.
        /// </summary>
        [Tooltip("When enabled, the camera's frustum will be shown at all times "
            + "in the scene view")]
        public bool m_ShowCameraFrustum = true;

        /// <summary>
        /// When enabled, the cameras will always respond in real-time to user input and damping,
        /// even if the game is running in slow motion
        /// </summary>
        [Tooltip("When enabled, the cameras will always respond in real-time to user input "
            + "and damping, even if the game is running in slow motion")]
        public bool m_IgnoreTimeScale = false;

        /// <summary>
        /// If set, this object's Y axis will define the worldspace Up vector for all the
        /// virtual cameras.  This is useful in top-down game environments.  If not set, Up is worldspace Y.
        /// </summary>
        [Tooltip("If set, this object's Y axis will define the worldspace Up vector for all the "
            + "virtual cameras.  This is useful for instance in top-down game environments.  "
            + "If not set, Up is worldspace Y.  Setting this appropriately is important, "
            + "because Virtual Cameras don't like looking straight up or straight down.")]
        public Transform m_WorldUpOverride;

        /// <summary>This enum defines the options available for the update method.</summary>
        [DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
        public enum UpdateMethod
        {
            /// <summary>Virtual cameras are updated in sync with the Physics module, in FixedUpdate</summary>
            FixedUpdate,
            /// <summary>Virtual cameras are updated in MonoBehaviour LateUpdate.</summary>
            LateUpdate,
            /// <summary>Virtual cameras are updated according to how the target is updated.</summary>
            SmartUpdate,
            /// <summary>Virtual cameras are not automatically updated, client must explicitly call 
            /// the CinemachineBrain's ManualUpdate() method.</summary>
            ManualUpdate
        };

        /// <summary>Depending on how the target objects are animated, adjust the update method to
        /// minimize the potential jitter.  Use FixedUpdate if all your targets are animated with for RigidBody animation.
        /// SmartUpdate will choose the best method for each virtual camera, depending
        /// on how the target is animated.</summary>
        [Tooltip("The update time for the vcams.  Use FixedUpdate if all your targets are animated "
            + "during FixedUpdate (e.g. RigidBodies), LateUpdate if all your targets are animated "
            + "during the normal Update loop, and SmartUpdate if you want Cinemachine to do the "
            + "appropriate thing on a per-target basis.  SmartUpdate is the recommended setting")]
        public UpdateMethod m_UpdateMethod = UpdateMethod.SmartUpdate;

        /// <summary>This enum defines the options available for the update method.</summary>
        [DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
        public enum BrainUpdateMethod
        {
            /// <summary>Camera is updated in sync with the Physics module, in FixedUpdate</summary>
            FixedUpdate,
            /// <summary>Camera is updated in MonoBehaviour LateUpdate (or when ManualUpdate is called).</summary>
            LateUpdate
        };

        /// <summary>The update time for the Brain, i.e. when the blends are evaluated and the
        /// brain's transform is updated.</summary>
        [Tooltip("The update time for the Brain, i.e. when the blends are evaluated and "
            + "the brain's transform is updated")]
        public BrainUpdateMethod m_BlendUpdateMethod = BrainUpdateMethod.LateUpdate;

        /// <summary>
        /// The blend which is used if you don't explicitly define a blend between two Virtual Cameras.
        /// </summary>
        [CinemachineBlendDefinitionProperty]
        [Tooltip("The blend that is used in cases where you haven't explicitly defined a "
            + "blend between two Virtual Cameras")]
        public CinemachineBlendDefinition m_DefaultBlend
            = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2f);

        /// <summary>
        /// This is the asset which contains custom settings for specific blends.
        /// </summary>
        [Tooltip("This is the asset that contains custom settings for blends between "
            + "specific virtual cameras in your scene")]
        public CinemachineBlenderSettings m_CustomBlends = null;

        /// <summary>
        /// Get the Unity Camera that is attached to this GameObject.  This is the camera
        /// that will be controlled by the brain.
        /// </summary>
        public Camera OutputCamera
        {
            get
            {
                if (m_OutputCamera == null && !Application.isPlaying)
#if UNITY_2019_2_OR_NEWER
                    TryGetComponent(out m_OutputCamera);
#else
                    m_OutputCamera = GetComponent<Camera>();
#endif
                return m_OutputCamera;
            }
        }
        private Camera m_OutputCamera = null; // never use directly - use accessor

        /// <summary>Event with a CinemachineBrain parameter</summary>
        [Serializable] public class BrainEvent : UnityEvent<CinemachineBrain> {}

        /// <summary>
        /// Event that is fired when a virtual camera is activated.
        /// The parameters are (incoming_vcam, outgoing_vcam), in that order.
        /// </summary>
        [Serializable] public class VcamActivatedEvent : UnityEvent<ICinemachineCamera, ICinemachineCamera> {}

        /// <summary>This event will fire whenever a virtual camera goes live and there is no blend</summary>
        [Tooltip("This event will fire whenever a virtual camera goes live and there is no blend")]
        public BrainEvent m_CameraCutEvent = new BrainEvent();

        /// <summary>This event will fire whenever a virtual camera goes live.  If a blend is involved,
        /// then the event will fire on the first frame of the blend.
        /// 
        /// The Parameters are (incoming_vcam, outgoing_vcam), in that order.</summary>
        [Tooltip("This event will fire whenever a virtual camera goes live.  If a blend is "
            + "involved, then the event will fire on the first frame of the blend.")]
        public VcamActivatedEvent m_CameraActivatedEvent = new VcamActivatedEvent();

        /// <summary>
        /// API for the Unity Editor.
        /// Show this camera no matter what.  This is static, and so affects all Cinemachine brains.
        /// </summary>
        public static ICinemachineCamera SoloCamera
        {
            get { return mSoloCamera; }
            set
            {
                if (value != null && !CinemachineCore.Instance.IsLive(val