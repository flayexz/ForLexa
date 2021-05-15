etDatabase.LoadAssetAtPath<Texture2D>(
                        ScriptableObjectUtility.CinemachineRealativeInstallPath
                            + "/Editor/EditorResources/cinemachine_header.tif");
                ;
                if (sCinemachineHeader != null)
                    sCinemachineHeader.hideFlags = HideFlags.DontSaveInEditor;
                return sCinemachineHeader;
            }
        }

        private static readonly string kCoreSettingsFoldKey     = "CNMCN_Core_Folded";
        private static readonly string kComposerSettingsFoldKey = "CNMCN_Composer_Folded";

        internal static event Action AdditionalCategories = null;

        static CinemachineSettings()
        {
            if (CinemachineLogoTexture != null)
            {
                EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
            }
        }

        class Styles {
            //private static readonly GUIContent sCoreShowHiddenObjectsToggle = new GUIContent("Show Hidden Objects", "If checked, Cinemachine hidden objects will be shown in the inspector.  This might be necessary to repair broken script mappings when upgrading from a pre-release version");
            public static readonly GUIContent sCoreActiveGizmosColour = new GUIContent("Active Virtual Camera", "The colour for the active virtual camera's gizmos");
            public static readonly GUIContent sCoreInactiveGizmosColour = new GUIContent("Inactive Virtual Camera", "The colour for all inactive virtual camera gizmos");
            public static readonly GUIContent sComposerOverlayOpacity = new GUIContent("Overlay Opacity", "The alpha of the composer's overlay when a virtual camera is selected with composer module enabled");
            public static readonly GUIContent sComposerHardBoundsOverlay = new GUIContent("Hard Bounds Overlay", "The colour of the composer overlay's hard bounds region");
            public static readonly GUIContent sComposerSoftBoundsOverlay = new GUIContent("Soft Bounds Overlay", "The colour of the composer overlay's soft bounds region");
            public static readonly GUIContent sComposerTargetOverlay = new GUIContent("Composer Target", "The colour of the composer overlay's target");
            public static readonly GUIContent sComposerTargetOverlayPixels = new GUIContent("Target Size (px)", "The size of the composer overlay's target box in pixels");
        }

        private const string kCinemachineHeaderPath = "cinemachine_header.tif";
        private const string kCinemachineDocURL = @"http://www.cinemachineimagery.com/documentation/";

        private static Vector2 sScrollPosition = Vector2.zero;

#if UNITY_2019_1_OR_NEWER
        [SettingsProvider]
        static SettingsProvider CreateProjectSettingsProvider()
        {
            var provider = new SettingsProvider("Preferences/Cinemachine", SettingsScope.User, SettingsProvider.GetSearchKeywordsFromGUIContentProperties<Styles>());
            provider.guiHandler = (sarchContext) => OnGUI();
            return provider;
        }
#else
        [PreferenceItem("Cinemachine")]
#endif
        private static void OnGUI()
        {
            if (CinemachineHeader != null)
            {
                const float kWidth = 350f;
                float aspectRatio = (float)CinemachineHeader.height / (float)CinemachineHeader.width;
                GUILayout.BeginScrollView(Vector2.zero, false, false, GUILayout.Width(kWidth), GUILayout.Height(kWidth * aspectRatio));
                Rect texRect = new Rect(0f, 0f, kWidth, kWidth * aspectRatio);

                GUILayout.BeginArea(texRect);
                GUI.DrawTexture(texRect, CinemachineHeader, ScaleMode.ScaleToFit);
                GUILayout.EndArea();

                GUILayout.EndScrollView();
            }

            sScrollPosition = GUILayout.BeginScrollView(sScrollPosition);

            //CinemachineCore.sShowHiddenObjects
            //    = EditorGUILayout.Toggle("Show Hidden Objects", CinemachineCore.sShowHiddenObjects);

            ShowCoreSettings = EditorGUILayout.Foldout(ShowCoreSettings, "Runtime Settings", true);
            if (ShowCoreSettings)
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                Color newActiveGizmoColour = EditorGUILayout.ColorField(Styles.sCoreActiveGizmosColour, CinemachineCoreSettings.ActiveGizmoColour);

                if (EditorGUI.EndChangeCheck())
                {
                    CinemachineCoreSettings.ActiveGizmoColour = newActiveGizmoColour;
                    UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                }

                if (GUILayout.Button("Reset"))
                {
                    CinemachineCoreSettings.ActiveGizmoColour = CinemachineCoreSettings.kDefaultActiveColour;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                Color newInactiveGizmoColour = EditorGUILayout.ColorField(Styles.sCoreInactiveGizmosColour, CinemachineCoreSettings.InactiveGizmoColour);

                if (EditorGUI.EndChangeCheck())
                {
                    CinemachineCoreSettings.InactiveGizmoColour = newInactiveGizmoColour;
                    UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                }

                if (GUILayout.Button("Reset"))
                {
                    CinemachineCoreSettings.InactiveGizmoColour = CinemachineCoreSettings.kDefaultInactiveColour;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }

            ShowComposerSettings = EditorGUILayout.Foldout(ShowComposerSettings, "Composer Settings", true);
            if (ShowComposerSettings)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();

                float overlayOpacity = EditorGUILayout.Slider(Styles.sComposerOverlayOpacity, ComposerSettings.OverlayOpacity, 0f, 1f);

                if (EditorGUI.EndChangeCheck())
                {
                    ComposerSettings.OverlayOpacity = overlayOpacity;
                }

                if (GUILayout.Button("Reset"))
                {
                    ComposerSettings.OverlayOpacity = ComposerSettings.kDefaultOverlayOpacity;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                Color newHardEdgeColor = EditorGUILayout.ColorField(Styles.sComposerHardBoundsOverlay, ComposerSettings.HardBoundsOverlayColour);

                if (EditorGUI.EndChangeCheck())
                {
                    ComposerSettings.HardBoundsOverlayColour = newHardEdgeColor;
                }

                if (GUILayout.Button("Reset"))
                {
                    ComposerSettings.HardBoundsOverlayColour = ComposerSettings.kDefaultHardBoundsColour;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                Color newSoftEdgeColor = EditorGUILayout.ColorField(Styles.sComposerSoftBoundsOverlay, ComposerSettings.SoftBoundsOverlayColour);

                if (EditorGUI.EndChangeCheck())
                {
                    ComposerSettings.SoftBoundsOverlayColour = newSoftEdgeColor;
                }

                if (GUILayout.Button("Reset"))
                {
                    ComposerSettings.SoftBoundsOverlayColour = ComposerSettings.kDefaultSoftBoundsColour;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                Color newTargetColour = EditorGUILayout.ColorField(Styles.sComposerTargetOverlay, ComposerSettings.TargetColour);

                if (EditorGUI.EndChangeCheck())
                {
                    ComposerSettings.TargetColour = newTargetColour;
                }

                if (GUILayout.Button("Reset"))
                {
                    ComposerSettings.TargetColour = ComposerSettings.kDefaultTargetColour;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUI.BeginChangeCheck();
                float targetSide = EditorGUILayout.FloatField(Styles.sComposerTargetOverlayPixels, ComposerSettings.TargetSize);

                if (EditorGUI.EndChangeCheck())
                {
                    ComposerSettings.TargetSize = targetSide;
                }
                EditorGUI.indentLevel--;
            }

            if (AdditionalCategories != null)
            {
                AdditionalCategories();
            }

            GUILayout.EndScrollView();

            //if (GUILayout.Button("Open Documentation"))
            //{
            //    Application.OpenURL(kCinemachineDocURL);
            //}
        }

        private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            GameObject instance = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (instance == null)
            {
                // Object in process of being deleted?
                return;
            }

            if (instance.GetComponent<CinemachineBrain>() != null)
            {
                Rect texRect = new Rect(selectionRect.xMax - selectionRect.height, selectionRect.yMin, selectionRect.height, selectionRect.height);
                GUI.DrawTexture(texRect, CinemachineLogoTexture, ScaleMode.ScaleAndCrop);
            }
        }

        internal static Color UnpackColour(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                byte[] bytes = Base64Decode(str);

                if ((bytes != null) && bytes.Length == 16)
                {
                    float r = BitConverter.ToSingle(bytes, 0);
                 