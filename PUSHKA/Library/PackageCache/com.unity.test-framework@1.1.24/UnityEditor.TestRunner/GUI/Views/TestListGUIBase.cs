using System;
using System.Linq.Expressions;
using UnityEditor;

namespace Cinemachine.Editor
{
    /// <summary>
    /// Helpers for the editor
    /// </summary>
    public static class SerializedPropertyHelper
    {
        /// This is a way to get a field name string in such a manner that the compiler will
        /// generate errors for invalid fields.  Much better than directly using strings.
        /// Usage: instead of
        /// <example>
        /// "m_MyField";
        /// </example>
        /// do this:
        /// <example>
        /// MyClass myclass = null;
        /// SerializedPropertyHelper.PropertyName( () => myClass.m_MyField);
        /// </example>
        public static string PropertyName(Expression<Func<object>> exp)
        {
            var body = exp.Body as MemberExpression;
            if (body == null)
            {
                var ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }
            return body.Member.Name;
        }

        /// Usage: instead of
        /// <example>
        /// mySerializedObject.FindProperty("m_MyField");
        /// </example>
        /// do this:
        /// <example>
        /// MyClass myclass = null;
        /// mySerializedObject.FindProperty( () => myClass.m_MyField);
        /// </example>
        public static SerializedProperty FindProperty(this SerializedObject obj, Expression<Func<object>> exp)
        {
            return obj.FindProperty(PropertyName(exp));
        }

        /// Usage: instead of
        /// <example>
        /// mySerializedProperty.FindPropertyRelative("m_MyField");
        /// </example>
        /// do this:
        /// <example>
        /// MyClass myclass = null;
        /// mySerializedProperty.FindPropertyRelative( () => myClass.m_MyField);
        /// </example>
        public static SerializedProperty FindPropertyRelative(this SerializedProperty obj, Expression<Func<object>> exp)
        {
            return obj.FindPropertyRelative(PropertyName(exp));
        }

        /// <summary>Get the value of a proprty, as an object</summary>
        public static object GetPropertyValue(SerializedProperty property)
        {
            var targetObject = property.serializedObject.targetObject;
            var targetObjectClassType = targetObject.GetType();
            var field = targetObjectClassType.GetField(property.propertyPath);
            if (field != null)
                return field.GetValue(targetObject);
            return null;
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 using UnityEngine;
using UnityEditor;
using System.IO;

namespace Cinemachine.Editor
{
    [InitializeOnLoad]
    internal class AboutWindow : EditorWindow
    {
        private const string kLastVersionOpened = "CNMCN_Last_Version_Loaded";
        private const string kInvalidVersionNumber = "0.0";

        private static readonly Vector2 kMinWindowSize = new Vector2(550f, 550f);

        private static string LastVersionLoaded
        {
            get { return EditorPrefs.GetString(kLastVersionOpened, kInvalidVersionNumber); }
            set { EditorPrefs.SetString(kLastVersionOpened, value); }
        }

        private GUIStyle mButtonStyle;
        private GUIStyle mLabelStyle;
        private GUIStyle mHeaderStyle;
        private GUIStyle mNotesStyle;
        private Vector2 mReleaseNoteScrollPos = Vector2.zero;

        string mReleaseNotes;

        private void OnEnable()
        {
            string path = ScriptableObjectUtility.CinemachineInstallPath + "/Extras~/ReleaseNotes.txt";
            try
            {
                StreamReader reader = new StreamReader(path); 
                mReleaseNotes = reader.ReadToEnd();
                reader.Close();
            }
            catch (System.Exception)
            {
                mReleaseNotes = path + " not found";
            }
        }

        private void OnGUI()
        {
            if (EditorApplication.isCompiling)
            {
                Close();
            }

            if (mButtonStyle == null)
            {
                mButtonStyle = new GUIStyle(GUI.skin.button);
                mButtonStyle.richText = true;
            }

            if (mLabelStyle == null)
            {
                mLabelStyle = new GUIStyle(EditorStyles.label);
                mLabelStyle.wordWrap = true;
                mLabelStyle.richText = true;
            }

            if (mHeaderStyle == null)
            {
                mHeaderStyle = new GUIStyle(EditorStyles.boldLabel);
                mHeaderStyle.wordWrap = true;
            }

            if (mNotesStyle == null)
            {
                mNotesStyle = new GUIStyle(EditorStyles.textArea);
                mNotesStyle.richText = true;
                mNotesStyle.wordWrap = true;
            }

            using (var vertScope = new EditorGUILayout.VerticalScope())
            {
                if (CinemachineSettings.CinemachineHeader != null)
                {
                    float headerWidth = position.width;
                    float aspectRatio = (float)CinemachineSettings.CinemachineHeader.height / (float)CinemachineSettings.CinemachineHeader.width;
                    GUILayout.BeginScrollView(Vector2.zero, false, false, GUILayout.Width(headerWidth), GUILayout.Height(headerWidth * aspectRatio));
                    Rect texRect = new Rect(0f, 0f, headerWidth, headerWidth * aspectRatio);

                    GUILayout.FlexibleSpace();
                    GUILayout.BeginArea(texRect);
                    GUI.DrawTexture(texRect, CinemachineSettings.CinemachineHeader, ScaleMode.ScaleToFit);
                    GUILayout.EndArea();
                    GUILayout.FlexibleSpace();

                    GUILayout.EndScrollView();
                }

                EditorGUILayout.LabelField("Welcome to Cinemachine!", mLabelStyle);
                EditorGUILayout.LabelField("Smart camera tools for passionate creators.", mLabelStyle);
                EditorGUILayout.LabelField("Below are links to the forums, please reach out if you have any questions or feedback", mLabelStyle);

                if (GUILayout.Button("<b>Forum</b>\n<i>Discuss</i>", mButtonStyle))
                {
                    Application.OpenURL("https://forum.unity3d.com/forums/cinemachine.136/");
                }

                if (GUILayout.Button("<b>Rate it!</b>\nUnity Asset Store", mButtonStyle))
                {
                    Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/79898");
                }
            }

            EditorGUILayout.LabelField("Release Notes", mHeaderStyle);
            using (var scrollScope = new EditorGUILayout.ScrollViewScope(mReleaseNoteScrollPos, GUI.skin.box))
            {
                mReleaseNoteScrollPos = scrollScope.scrollPosition;
                EditorGUILayout.LabelField(mReleaseNotes, mNotesStyle);
            }
        }

        [MenuItem("Cinemachine/About")]
        private static void OpenWindow()
        {
            EditorApplication.update += ShowWindowDeferred;
        }

        private static void ShowWindowDeferred()
        {
            string loadedVersion = LastVersionLoaded;
            if (loadedVersion != CinemachineCore.kVersionString)
                LastVersionLoaded = CinemachineCore.kVersionString;

            AboutWindow window = EditorWindow.GetWindow<AboutWindow>();

            window.titleContent = new UnityEngine.GUIContent(
                "About", CinemachineSettings.CinemachineLogoTexture);
            window.minSize = kMinWindowSize;
            window.Show(true);

            EditorApplication.update -= ShowWindowDeferred;
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 using UnityEngine;
using UnityEditor;

using Cinemachine.Editor;

namespace Cinemachine
{
    [InitializeOnLoad]
    internal static class CinemachineColliderPrefs
    {
        private static bool SettingsFoldedOut
        {
            get { return EditorPrefs.GetBool(kColliderSettingsFoldoutKey, false); }
            set
            {
                if (value != SettingsFoldedOut)
                {
                    EditorPrefs.SetBool(kColliderSettingsFoldoutKey, value);
                }
            }
        }

        public static Color FeelerHitColor
        {
            get
            {
                return CinemachineSettings.UnpackColour(EditorPrefs.GetString(kFeelerHitColourKey, CinemachineSettings.PackColor(Color.yellow)));
            }

            set
            {
                if (value != FeelerHitColor)
                {
                    EditorPrefs.SetString(kFeelerHitColourKey, CinemachineSettings.PackColor(value));
                }
            }
        }

        public static Color FeelerColor
        {
            get
            {
                return CinemachineSettings.UnpackColour(EditorPrefs.GetString(kFeelerColourKey, CinemachineSettings.PackColor(Color.gray)));
            }

            set
            {
                if (value != FeelerColor)
                {
                    EditorPrefs.SetString(kFeelerColourKey, CinemachineSettings.PackColor(value));
                }
            }
        }

        private const string kColliderSettingsFoldoutKey  = "CNMCN_Collider_Foldout";
        private const string kFeelerHitColourKey          = "CNMCN_Collider_FeelerHit_Colour";
        private const string kFeelerColourKey             = "CNMCN_Collider_Feeler_Colour";

        static CinemachineColliderPrefs()
        {
            Cinemachine.Editor.CinemachineSettings.AdditionalCategories += DrawColliderSettings;
        }

        private static void DrawColliderSettings()
        {
            SettingsFoldedOut = EditorGUILayout.Foldout(SettingsFoldedOut, "Collider Settings", true);
            if (SettingsFoldedOut)
            {
                EditorGUI.indentLevel++;

                EditorGUI.BeginChangeCheck();

                FeelerHitColor   = EditorGUILayout.ColorField("Feeler Hit", FeelerHitColor);
                FeelerColor = EditorGUILayout.ColorField("Feeler", FeelerColor);

                if (EditorGUI.EndChangeCheck())
                {
                    UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                }

                EditorGUI.indentLevel--;
            }
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              using UnityEngine;
using UnityEditor;
using System;

namespace Cinemachine.Editor
{
    [InitializeOnLoad]
    internal sealed class CinemachineSettings
    {
        public static class CinemachineCoreSettings
        {
            private static readonly string hShowInGameGuidesKey = "CNMCN_Core_ShowInGameGuides";
            public static bool ShowInGameGuides
            {
                get { return EditorPrefs.GetBool(hShowInGameGuidesKey, true); }
                set
                {
                    if (ShowInGameGuides != value)
                    {
                        EditorPrefs.SetBool(hShowInGameGuidesKey, value);
                        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                    }
                }
            }


            static string kUseTimelneScrubbingCache = "CNMCN_Timeeline_UseTimelineScrubbingCache";
            public static bool UseTimelneScrubbingCache
            {
                get { return EditorPrefs.GetBool(kUseTimelneScrubbingCache, true); }
                set
                {
                    if (UseTimelneScrubbingCache != value)
                        EditorPrefs.SetBool(kUseTimelneScrubbingCache, value);
                }
            }
        
            private static readonly string kCoreActiveGizmoColourKey = "CNMCN_Core_Active_Gizmo_Colour";
            public static readonly Color kDefaultActiveColour = new Color32(255, 0, 0, 100);
            public static Color ActiveGizmoColour
            {
                get
                {
                    string packedColour = EditorPrefs.GetString(kCoreActiveGizmoColourKey, PackColor(kDefaultActiveColour));
                    return UnpackColour(packedColour);
                }

                set
                {
                    if (ActiveGizmoColour != value)
                    {
                        string packedColour = PackColor(value);
                        EditorPrefs.SetString(kCoreActiveGizmoColourKey, packedColour);
                    }
                }
            }

            private static readonly string kCoreInactiveGizmoColourKey = "CNMCN_Core_Inactive_Gizmo_Colour";
            public static readonly Color kDefaultInactiveColour = new Color32(9, 54, 87, 100);
            public static Color InactiveGizmoColour
            {
                get
                {
                    string packedColour = EditorPrefs.GetString(kCoreInactiveGizmoColourKey, PackColor(kDefaultInactiveColour));
                    return UnpackColour(packedColour);
                }

                set
                {
                    if (InactiveGizmoColour != value)
                    {
                        string packedColour = PackColor(value);
                        EditorPrefs.SetString(kCoreInactiveGizmoColourKey, packedColour);
                    }
                }
            }
        }

        public static class ComposerSettings
        {
            private static readonly string kOverlayOpacityKey           = "CNMCN_Overlay_Opacity";
            private static readonly string kComposerHardBoundsColourKey = "CNMCN_Composer_HardBounds_Colour";
            private static readonly string kComposerSoftBoundsColourKey = "CNMCN_Composer_SoftBounds_Colour";
            private static readonly string kComposerTargetColourKey     = "CNMCN_Composer_Target_Colour";
            private static readonly string kComposerTargetSizeKey       = "CNMCN_Composer_Target_Size";

            public const float kDefaultOverlayOpacity = 0.15f;
            public static readonly Color kDefaultHardBoundsColour = new Color32(255, 0, 72, 255);
            public static readonly Color kDefaultSoftBoundsColour = new Color32(0, 194, 255, 255);
            public static readonly Color kDefaultTargetColour = new Color32(255, 254, 25, 255);

            public static float OverlayOpacity
            {
                get { return EditorPrefs.GetFloat(kOverlayOpacityKey, kDefaultOverlayOpacity); }
                set
                {
                    if (value != OverlayOpacity)
                    {
                        EditorPrefs.SetFloat(kOverlayOpacityKey, value);
                    }
                }
            }

            public static Color HardBoundsOverlayColour
            {
                get
                {
                    string packedColour = EditorPrefs.GetString(kComposerHardBoundsColourKey, PackColor(kDefaultHardBoundsColour));
                    return UnpackColour(packedColour);
                }

                set
                {
                    if (HardBoundsOverlayColour != value)
                    {
                        string packedColour = PackColor(value);
                        EditorPref