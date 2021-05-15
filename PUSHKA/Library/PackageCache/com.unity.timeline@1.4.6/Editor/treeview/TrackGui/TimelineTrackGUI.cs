;
                    w = (int)ti.wrapModeW;
                }
                u = Mathf.Max(0, u);
                v = Mathf.Max(0, v);
                w = Mathf.Max(0, w);
                if (u != v)
                {
                    return true;
                }
                if (isVolumeTexture)
                {
                    if (u != w || v != w)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // showPerAxisWrapModes is state of whether "Per-Axis" mode should be active in the main dropdown.
        // It is set automatically if wrap modes in UVW are different, or if user explicitly picks "Per-Axis" option -- when that one is picked,
        // then it should stay true even if UVW wrap modes will initially be the same.
        //
        // Note: W wrapping mode is only shown when isVolumeTexture is true.
        internal static void WrapModePopup(SerializedProperty wrapU, SerializedProperty wrapV, SerializedProperty wrapW, bool isVolumeTexture, ref bool showPerAxisWrapModes)
        {
            if (s_Styles == null)
                s_Styles = new Styles();

            // In texture importer settings, serialized properties for things like wrap modes can contain -1;
            // that seems to indicate "use defaults, user has not changed them to anything" but not totally sure.
            // Show them as Repeat wrap modes in the popups.
            var wu = (TextureWrapMode)Mathf.Max(wrapU.intValue, 0);
            var wv = (TextureWrapMode)Mathf.Max(wrapV.intValue, 0);
            var ww = (TextureWrapMode)Mathf.Max(wrapW.intValue, 0);

            // automatically go into per-axis mode if values are already different
            if (wu != wv)
                showPerAxisWrapModes = true;
            if (isVolumeTexture)
            {
                if (wu != ww || wv != ww)
                    showPerAxisWrapModes = true;
            }

            // It's not possible to determine whether any single texture in the whole selection is using per-axis wrap modes
            // just from SerializedProperty values. They can only tell if "some values in whole selection are different" (e.g.
            // wrap value on U axis is not the same among all textures), and can return value of "some" object in the selection
            // (typically based on object loading order). So in order for more intuitive behavior with multi-selection,
            // we go over the actual objects when there's >1 object selected and some wrap modes are different.
            if (!showPerAxisWrapModes)
            {
                if (wrapU.hasMultipleDifferentValues || wrapV.hasMultipleDifferentValues || (isVolumeTexture && wrapW.hasMultipleDifferentValues))
                {
                    if (IsAnyTextureObjectUsingPerAxisWrapMode(wrapU.serializedObject.targetObjects, isVolumeTexture))
                    {
                        showPerAxisWrapModes = true;
                    }
                }
            }

            int value = showPerAxisWrapModes ? -1 : (int)wu;

            // main wrap mode popup
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = !showPerAxisWrapModes && (wrapU.hasMultipleDifferentValues || wrapV.hasMultipleDifferentValues || (isVolumeTexture && wrapW.hasMultipleDifferentValues));
            value = EditorGUILayout.IntPopup(s_Styles.wrapModeLabel, value, s_Styles.wrapModeContents, s_Styles.wrapModeValues);
            if (EditorGUI.EndChangeCheck() && value != -1)
            {
                // assign the same wrap mode to all axes, and hide per-axis popups
                wrapU.intValue = value;
                wrapV.intValue = value;
                wrapW.intValue = value;
                showPerAxisWrapModes = false;
            }

            // show per-axis popups if needed
            if (value == -1)
            {
                showPerAxisWrapModes = true;
                EditorGUI.indentLevel++;
                WrapModeAxisPopup(s_Styles.wrapU, wrapU);
                WrapModeAxisPopup(s_Styles.wrapV, wrapV);
                if (isVolumeTexture)
                {
                    WrapModeAxisPopup(s_Styles.wrapW, wrapW);
                }
                EditorGUI.indentLevel--;
            }
            EditorGUI.showMixedValue = false;
        }

        static void WrapModeAxisPopup(GUIContent label, SerializedProperty wrapProperty)
        {
            // In texture importer settings, serialized properties for wrap modes can contain -1, which means "use default".
            var wrap = (TextureWrapMode)Mathf.Max(wrapProperty.intValue, 0);
            Rect rect = EditorGUILayout.GetControlRect();
            EditorGUI.BeginChangeCheck();
            EditorGUI.BeginProperty(rect, label, wrapProperty);
            wrap = (TextureWrapMode)EditorGUI.EnumPopup(rect, label, wrap);
            EditorGUI.EndProperty();
            if (EditorGUI.EndChangeCheck())
            {
                wrapProperty.intValue = (int)wrap;
            }
        }

        void DoWrapModePopup()
        {
            WrapModePopup(m_WrapU, m_WrapV, m_WrapW, IsVolume(), ref m_ShowPerAxisWrapModes);
        }

        bool IsVolume()
        {
            var t = target as Texture;
            return t != null && t.dimension == UnityEngine.Rendering.TextureDimension.Tex3D;
        }

        void DoSpriteInspector()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.IntPopup(m_SpriteMode, s_Styles.spriteModeOptions, new[] { 1, 2, 3 }, s_Styles.spriteMode);

            // Ensure that PropertyField focus will be cleared when we change spriteMode.
            if (EditorGUI.EndChangeCheck())
            {
                GUIUtility.keyboardControl = 0;
            }

            EditorGUI.indentLevel++;

            // Show generic attributes
            if (m_SpriteMode.intValue != 0)
            {
                EditorGUILayout.PropertyField(m_SpritePixelsToUnits, s_Styles.spritePixelsPerUnit);

                if (m_SpriteMode.intValue != (int)SpriteImportMode.Polygon && !m_SpriteMode.hasMultipleDifferentValues)
                {
                    EditorGUILayout.IntPopup(m_SpriteMeshType, s_Styles.spriteMeshTypeOptions, new[] { 0, 1 }, s_Styles.spriteMeshType);
                }

                EditorGUILayout.IntSlider(m_SpriteExtrude, 0, 32, s_Styles.spriteExtrude);

                if (m_SpriteMode.intValue == 1)
                {
                    EditorGUILayout.IntPopup(m_Alignment, s_Styles.spriteAlignmentOptions, new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, s_Styles.spriteAlignment);

                    if (m_Alignment.intValue == (int)SpriteAlignment.Custom)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(m_SpritePivot, new GUIContent());
                        GUILayout.EndHorizontal();
                    }
                }
            }

            EditorGUILayout.PropertyField(m_ImportHiddenLayers, s_Styles.importHiddenLayer);
            if (m_SpriteMode.intValue == (int)SpriteImportMode.Multiple && !m_SpriteMode.hasMultipleDifferentValues)
            {
                EditorGUILayout.PropertyField(m_MosaicLayers, s_Styles.mosaicLayers);
                using (new EditorGUI.DisabledScope(!m_MosaicLayers.boolValue))
                {
                    EditorGUILayout.PropertyField(m_CharacterMode, s_Styles.characterMode);
                    using (new EditorGUI.DisabledScope(!m_CharacterMode.boolValue))
                    {
                        EditorGUILayout.PropertyField(m_GenerateGOHierarchy, s_Styles.generateGOHierarchy);
                        EditorGUILayout.IntPopup(m_DocumentAlignment, s_Styles.spriteAlignmentOptions, new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, s_Styles.characterAlignment);
                        if (m_DocumentAlignment.intValue == (int)SpriteAlignment.Custom)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(EditorGUIUtility.labelWidth);
                            EditorGUILayout.PropertyField(m_DocumentPivot, new GUIContent());
                            GUILayout.EndHorizontal();
                        }
                        //EditorGUILayout.PropertyField(m_PaperDollMode, s_Styles.paperDollMode);
                    }


                    EditorGUILayout.PropertyField(m_ResliceFromLayer, s_Styles.resliceFromLayer);
                    if (m_ResliceFromLayer.boolValue)
                    {
                        EditorGUILayout.HelpBox(s_Styles.resliceFromLayerWarning.text, MessageType.Info, true);
                    }
                }
                m_ShowExperimental = EditorGUILayout.Foldout(m_ShowExperimental, s_Styles.experimental, true);
                if (m_ShowExperimental)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(m_KeepDupilcateSpriteName, s_Styles.keepDuplicateSpriteName);
                    EditorGUI.indentLevel--;
                }
            }

            using (new EditorGUI.DisabledScope(targets.Length != 1))
            {
                GUILayout.BeginHorizontal();

                GUILayout.FlexibleSpace();
                if (GUILayout.Button(s_Styles.spriteEditorButtonLabel))
                {
                    if (HasModified())
                    {
                        // To ensure Sprite Editor Window to have the latest texture import setting,
                        // We must applied those modified values first.
                        string dialogText = string.Format(s_Styles.unappliedSettingsDialogContent.text, ((AssetImporter)target).assetPath);
                        if (EditorUtility.DisplayDialog(s_Styles.unappliedSettingsDialogTitle.text,
                            dialogText, s_Styles.applyButtonLabel.text, s_Styles.revertButtonLabel.text))
                        {
                            ApplyAndImport();
                            InternalEditorBridge.ShowSpriteEditorWindow(this.assetTarget);

                            // We reimported the asset which destroyed the editor, so we can't keep running the UI here.
                            GUIUtility.ExitGUI();
                        }
                    }
                    else
                    {
                        InternalEditorBridge.ShowSpriteEditorWindow(this.assetTarget);
                    }
                }
                GUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
        }

        void DoTextureDefaultInspector()
        {
            ColorSpaceGUI();
            AlphaHandlingGUI();
        }

        void ColorSpaceGUI()
        {
            ToggleFromInt(m_sRGBTexture, s_Styles.sRGBTexture);
        }

        void POTScaleGUI()
        {
            using (new EditorGUI.DisabledScope(m_IsPOT || m_TextureType.intValue == (int)TextureImporterType.Sprite))
            {
                EnumPopup(m_NPOTScale, typeof(TextureImporterNPOTScale), s_Styles.npot);
            }
        }

        void ReadableGUI()
        {
            ToggleFromInt(m_IsReadable, s_Styles.readWrite);
        }

        void AlphaHandlingGUI()
        {
            EditorGUI.showMixedValue = m_AlphaSource.hasMultipleDifferentValues;
            EditorGUI.BeginChangeCheck();
            int newAlphaUsage = EditorGUILayout.IntPopup(s_Styles.alphaSource, m_AlphaSource.intValue, s_Styles.alphaSourceOptions, s_Styles.alphaSourceValues);

            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                m_AlphaSource.intValue = newAlphaUsage;
            }

            bool showAlphaIsTransparency = (TextureImporterAlphaSource)m_AlphaSource.intValue != TextureImporterAlphaSource.None;
            using (new EditorGUI.DisabledScope(!showAlphaIsTransparency))
            {
                ToggleFromInt(m_AlphaIsTransparency, s_Styles.alphaIsTransparency);
            }
        }

        void MipMapGUI()
        {
            ToggleFromInt(m_EnableMipMap, s_Styles.generateMipMaps);

            if (m_EnableMipMap.boolValue && !m_EnableMipMap.hasMultipleDifferentValues)
            {
                EditorGUI.indentLevel++;
                ToggleFromInt(m_BorderMipMap, s_Styles.borderMipMaps);
                EditorGUILayout.Popup(s_Styles.mipMapFilter, m_MipMapMode.intValue, s_Styles.mipMapFilterOptions);

                ToggleFromInt(m_MipMapsPreserveCoverage, s_Styles.mipMapsPreserveCoverage);
                if (m_MipMapsPreserveCoverage.intValue != 0 && !m_MipMapsPreserveCoverage.hasMultipleDifferentValues)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(m_AlphaTestReferenceValue, s_Styles.alphaTestReferenceValue);
                    EditorGUI.indentLevel--;
                }

                // Mipmap fadeout
                ToggleFromInt(m_FadeOut, s_Styles.mipmapFadeOutToggle);
                if (m_FadeOut.intValue > 0)
                {
                    EditorGUI.indentLevel++;
                    EditorGUI.BeginChangeCheck();
                    float min = m_MipMapFadeDistanceStart.intValue;
                    float max = m_MipMapFadeDistanceEnd.intValue;
                    EditorGUILayout.MinMaxSlider(s_Styles.mipmapFadeOut, ref min, ref max, 0, 10);
                    if (EditorGUI.EndChangeCheck())
                    {
                        m_MipMapFadeDistanceStart.intValue = Mathf.RoundToInt(min);
                        m_MipMapFadeDistanceEnd.intValue = Mathf.RoundToInt(max);
                    }
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }
        }

        void ToggleFromInt(SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            int value = EditorGUILayout.Toggle(label, property.intValue > 0) ? 1 : 0;
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                property.intValue = value;
        }

        void EnumPopup(SerializedProperty property, System.Type type, GUIContent label)
        {
            EditorGUILayout.IntPopup(label.text, property.intValue,
                System.Enum.GetNames(type),
                System.Enum.GetValues(type) as int[]);
        }

        void ExportMosaicTexture()
        {
            var assetPath = ((AssetImporter)target).assetPath;
            var texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            if (texture2D == null)
                return;
            if (!texture2D.isReadable)
                texture2D = InternalEditorBridge.CreateTemporaryDuplicate(texture2D, texture2D.width, texture2D.height);
            var pixelData = texture2D.GetPixels();
            texture2D = new Texture2D(texture2D.width, texture2D.height);
            texture2D.SetPixels(pixelData);
            texture2D.Apply();
            byte[] bytes = texture2D.EncodeToPNG();
            var fileName = Path.GetFileNameWithoutExtension(assetPath);
            var filePath = Path.GetDirectoryName(assetPath);
            var savePath = Path.Combine(filePath, fileName + ".png");
            File.WriteAllBytes(savePath, bytes);
            AssetDatabase.Refresh();
        }

        protected override void ResetValues()
        {
            base.ResetValues();
            LoadPlatformSettings();
            m_TexturePlatformSettingsHelper = new TexturePlatformSettingsHelper(this);
        }

        public int GetTargetCount()
        {
            return targets.Length;
        }

        public TextureImporterPlatformSettings GetPlatformTextureSettings(int i, string name)
        {
            if(m_PlatfromSettings.ContainsKey(name))
                if(m_PlatfromSettings[name].Count > i)
                    return m_PlatfromSettings[name][i];
            return new TextureImporterPlatformSettings()
            {
                name = name,
                overridden = false
            };
        }

        public bool ShowPresetSettings()
        {
            return assetTarget == null;
        }

        public bool DoesSourceTextureHaveAlpha(int v)
        {
            return true;
        }

        public bool IsSourceTextureHDR(int v)
        {
            return false;
        }

        public void SetPlatformTextureSettings(int i, TextureImporterPlatformSettings platformSettings)
        {
            var psdImporter = ((PSDImporter)targets[i]);
            psdImporter.SetPlatformTextureSettings(platformSettings);
            psdImporter.Apply();
        }

        public void GetImporterSettings(int i, TextureImporterSettings settings)
        {
            ((PSDImporter)targets[i]).ReadTextureSettings(settings);
            // Get settings that have been changed in the inspector
            GetSerializedPropertySettings(settings);
        }

        internal TextureImporterSettings GetSerializedPropertySettings(TextureImporterSettings settings)
        {
            if (!m_AlphaSource.hasMultipleDifferentValues)
                settings.alphaSource = (TextureImporterAlphaSource)m_AlphaSource.intValue;

            if (!m_ConvertToNormalMap.hasMultipleDifferentValues)
                settings.convertToNormalMap = m_ConvertToNormalMap.intValue > 0;

            if (!m_BorderMipMap.hasMultipleDifferentValues)
                settings.borderMipmap = m_BorderMipMap.intValue > 0;

            if (!m_MipMapsPreserveCoverage.hasMultipleDifferentValues)
                settings.mipMapsPreserveCoverage = m_MipMapsPreserveCoverage.intValue > 0;

            if (!m_AlphaTestReferenceValue.hasMultipleDifferentValues)
                settings.alphaTestReferenceValue = m_AlphaTestReferenceValue.floatValue;

            if (!m_NPOTScale.hasMultipleDifferentValues)
                settings.npotScale = (TextureImporterNPOTScale)m_NPOTScale.intValue;

            if (!m_IsReadable.hasMultipleDifferentValues)
                settings.readable = m_IsReadable.intValue > 0;

            if (!m_sRGBTexture.hasMultipleDifferentValues)
                settings.sRGBTexture = m_sRGBTexture.intValue > 0;

            if (!m_EnableMipMap.hasMultipleDifferentValues)
                settings.mipmapEnabled = m_EnableMipMap.intValue > 0;

            if (!m_MipMapMode.hasMultipleDifferentValues)
                settings.mipmapFilter = (TextureImporterMipFilter)m_MipMapMode.intValue;

            if (!m_FadeOut.hasMultipleDifferentValues)
                settings.fadeOut = m_FadeOut.intValue > 0;

            if (!m_MipMapFadeDistanceStart.hasMultipleDifferentValues)
                settings.mipmapFadeDistanceStart = m_MipMapFadeDistanceStart.intValue;

            if (!m_MipMapFadeDistanceEnd.hasMultipleDifferentValues)
                settings.mipmapFadeDistanceEnd = m_MipMapFadeDistanceEnd.intValue;

            if (!m_SpriteMode.hasMultipleDifferentValues)
                settings.spriteMode = m_SpriteMode.intValue;

            if (!m_SpritePixelsToUnits.hasMultipleDifferentValues)
                settings.spritePixelsPerUnit = m_SpritePixelsToUnits.floatValue;

            if (!m_SpriteExtrude.hasMultipleDifferentValues)
                settings.spriteExtrude = (uint)m_SpriteExtrude.intValue;

            if (!m_SpriteMeshType.hasMultipleDifferentValues)
                settings.spriteMeshType = (SpriteMeshType)m_SpriteMeshType.intValue;

            if (!m_Alignment.hasMultipleDifferentValues)
                settings.spriteAlignment = m_Alignment.intValue;

            if (!m_SpritePivot.hasMultipleDifferentValues)
                settings.spritePivot = m_SpritePivot.vector2Value;

            if (!m_WrapU.hasMultipleDifferentValues)
                settings.wrapModeU = (TextureWrapMode)m_WrapU.intValue;
            if (!m_WrapV.hasMultipleDifferentValues)
                settings.wrapModeU = (TextureWrapMode)m_WrapV.intValue;
            if (!m_WrapW.hasMultipleDifferentValues)
                settings.wrapModeU = (TextureWrapMode)m_WrapW.intValue;

            if (!m_FilterMode.hasMultipleDifferentValues)
                settings.filterMode = (FilterMode)m_FilterMode.intValue;

            if (!m_Aniso.hasMultipleDifferentValues)
                settings.aniso = m_Aniso.intValue;


            if (!m_AlphaIsTransparency.hasMultipleDifferentValues)
                settings.alphaIsTransparency = m_AlphaIsTransparency.intValue > 0;

            if (!m_TextureType.hasMultipleDifferentValues)
                settings.textureType = (TextureImporterType)m_TextureType.intValue;

            if (!m_TextureShape.hasMultipleDifferentValues)
                settings.textureShape = (TextureImporterShape)m_TextureShape.intValue;

            return settings;
        }
        /// <summary>
        /// Override of AssetImporterEditor.showImportedObject
        /// The property always returns false so that imported objects does not show up in the Inspector.
        /// </summary>
        /// <returns>false</returns>
        public override bool showImportedObject
        {
            get { return false; }
        }
        
        public bool textureTypeHasMultipleDifferentValues
        {
           get { return m_TextureType.hasMultipleDifferentValues; }
        }

        public TextureImporterType textureType
        {
           get { return (TextureImporterType)m_TextureType.intValue; }
        }

        public SpriteImportMode spriteImportMode
        {
            get { return (SpriteImportMode)m_SpriteMode.intValue; }
        }

        public override bool HasModified()
        {
            if (base.HasModified())
                return true;

            return m_TexturePlatformSettingsHelper.HasModified();
        }

        internal class Styles
        {
            public readonly GUIContent textureTypeTitle = new GUIContent("Texture Type", "What will this texture be used for?");
            public readonly GUIContent[] textureTypeOptions =
            {
                new GUIContent("Default", "Texture is a normal image such as a diffuse texture or other."),
                new GUIContent("Sprite (2D and UI)", "Texture is used for a sprite."),
            };
            public readonly int[] textureTypeValues =
            {
                (int)TextureImporterType.Default,
                (int)TextureImporterType.Sprite,
            };

            public readonly GUIContent textureShape = new GUIContent("Texture Shape", "What shape is this texture?");
            private readonly GUIContent textureShape2D = new GUIContent("2D, Texture is 2D.");
            private readonly  GUIContent textureShapeCube = new GUIContent("Cube", "Texture is a Cubemap.");
            public readonly Dictionary<TextureImporterShape, GUIContent[]> textureShapeOptionsDictionnary = new Dictionary<TextureImporterShape, GUIContent[]>();
            public readonly Dictionary<TextureImporterShape, int[]> textureShapeValuesDictionnary = new Dictionary<TextureImporterShape, int[]>();


            public readonly GUIContent filterMode = new GUIContent("Filter Mode");
            public readonly GUIContent[] filterModeOptions =
            {
                new GUIContent("Point (no filter)"),
                new GUIContent("Bilinear"),
                new GUIContent("Trilinear")
            };

            public readonly GUIContent textureFormat = new GUIContent("Format");

            public readonly GUIContent defaultPlatform = new GUIContent("Default");
            public readonly GUIContent mipmapFadeOutToggle = new GUIContent("Fadeout Mip Maps");
            public readonly GUIContent mipmapFadeOut = new GUIContent("Fade Range");
            public readonly GUIContent readWrite = new GUIContent("Read/Write Enabled", "Enable to be able to access the raw pixel data from code.");

            public readonly GUIContent alphaSource = new GUIContent("Alpha Source", "How is the alpha generated for the imported texture.");
            public readonly GUIContent[] alphaSourceOptions =
            {
                new GUIContent("None", "No Alpha will be used."),
                new GUIContent("Input Texture Alpha", "Use Alpha from the input texture if one is provided."),
                new GUIContent("From Gray Scale", "Generate Alpha from image gray scale."),
            };
            public readonly int[] alphaSourceValues =
            {
                (int)TextureImporterAlphaSource.None,
                (int)TextureImporterAlphaSource.FromInput,
                (int)TextureImporterAlphaSource.FromGrayScale,
            };

            public readonly GUIContent generateMipMaps = new GUIContent("Generate Mip Maps");
            public readonly GUIContent sRGBTexture = new GUIContent("sRGB (Color Texture)", "Texture content is stored in gamma space. Non-HDR color textures should enable this flag (except if used for IMGUI).");
            public readonly GUIContent borderMipMaps = new GUIContent("Border Mip Maps");
            public readonly GUIContent mipMapsPreserveCoverage = new GUIContent("Mip Maps Preserve Coverage", "The alpha channel of generated Mip Maps will preserve coverage during the alpha test.");
            public readonly GUIContent alphaTestReferenceValue = new GUIContent("Alpha Cutoff Value", "The reference value used during the alpha test. Controls Mip Map coverage.");
            public readonly GUIContent mipMapFilter = new GUIContent("Mip Map Filtering");
            public readonly GUIContent[] mipMapFilterOptions =
            {
                new GUIContent("Box"),
                new GUIContent("Kaiser"),
            };
            public readonly GUIContent npot = new GUIContent("Non Power of 2", "How non-power-of-two textures are scaled on import.");

            public readonly GUIContent compressionQuality = new GUIContent("Compressor Quality");
            public readonly GUIContent compressionQualitySlider = new GUIContent("Compressor Quality", "Use the slider to adjust compression quality from 0 (Fastest) to 100 (Best)");
            public readonly GUIContent[] mobileCompressionQualityOptions =
            {
                new GUIContent("Fast"),
                new GUIContent("Normal"),
                new GUIContent("Best")
            };

            public readonly GUIContent spriteMode = new GUIContent("Sprite Mode");
            public readonly GUIContent[] spriteModeOptions =
            {
                new GUIContent("Single"),
                new GUIContent("Multiple"),
                new GUIContent("Polygon"),
            };
            public readonly GUIContent[] spriteMeshTypeOptions =
            {
                new GUIContent("Full Rect"),
                new GUIContent("Tight"),
            };

            public readonly GUIContent spritePackingTag = new GUIContent("Packing Tag", "Tag for the Sprite Packing system.");
            public readonly GUIContent spritePixelsPerUnit = new GUIContent("Pixels Per Unit", "How many pixels in the sprite correspond to one unit in the world.");
            public readonly GUIContent spriteExtrude = new GUIContent("Extrude Edges", "How much empty area to leave around the sprite in the generated mesh.");
            public readonly GUIContent spriteMeshType = new GUIContent("Mesh Type", "Type of sprite mesh to generate.");
            public readonly GUIContent spriteAlignment = new GUIContent("Pivot", "Sprite pivot point in its local space. May be used for syncing animation frames of different sizes.");
            public readonly GUIContent characterAlignment = new GUIContent("Pivot", "Character pivot point in its local space using normalized value i.e. 0 - 1");

            public readonly GUIContent[] spriteAlignmentOptions =
            {
                new GUIContent("Center"),
                new GUIContent("Top Left"),
                new GUIContent("Top"),
                new GUIContent("Top Right"),
                new GUIContent("Left"),
                new GUIContent("Right"),
                new GUIContent("Bottom Left"),
                new GUIContent("Bottom"),
                new GUIContent("Bottom Right"),
                new GUIContent("Custom"),
            };

            public readonly GUIContent warpNotSupportWarning = new GUIContent("Graphics device doesn't support Repeat wrap mode on NPOT textures. Falling back to Clamp.");
            public readonly GUIContent anisoLevelLabel = new GUIContent("Aniso Level");
            public readonly GUIContent anisotropicDisableInfo = new GUIContent("Anisotropic filtering is disabled for all textures in Quality Settings.");
            public readonly GUIContent anisotropicForceEnableInfo = new GUIContent("Anisotropic filtering is enabled for all textures in Quality Settings.");
            public readonly GUIContent unappliedSettingsDialogTitle = new GUIContent("Unapplied import settings");
            public readonly GUIContent unappliedSettingsDialogContent = new GUIContent("Unapplied import settings for \'{0}\'.\nApply and continue to sprite editor or cancel.");
            public readonly GUIContent applyButtonLabel = new GUIContent("Apply");
            public readonly GUIContent revertButtonLabel = new GUIContent("Revert");
            public readonly GUIContent spriteEditorButtonLabel = new GUIContent("Sprite Editor");
            public readonly GUIContent resliceFromLayerWarning = new GUIContent("This will reinitialize and recreate all Sprites based on the file’s layer data. Existing Sprite metadata from previously generated Sprites are copied over.");
            public readonly GUIContent alphaIsTransparency = new GUIContent("Alpha Is Transparency", "If the provided alpha channel is transparency, enable this to pre-filter the color to avoid texture filtering artifacts. This is not supported for HDR textures.");
            public readonly GUIContent etc1Compression = new GUIContent("Compress using ETC1 (split alpha channel)|Alpha for this texture will be preserved by splitting the alpha channel to another texture, and both resulting textures will be compressed using ETC1.");
            public readonly GUIContent crunchedCompression = new GUIContent("Use Crunch Compression", "Texture is crunch-compressed to save space on disk when applicable.");

            public readonly GUIContent showAdvanced = new GUIContent("Advanced", "Show advanced settings.");

            public readonly GUIContent platformSettingsLabel  = new GUIContent("Platform Setttings");

            public readonly GUIContent[] platformSettingsSelection;

            public readonly GUIContent wrapModeLabel = new GUIContent("Wrap Mode");
            public readonly GUIContent wrapU = new GUIContent("U axis");
            public readonly GUIContent wrapV = new GUIContent("V axis");
            public readonly GUIContent wrapW = new GUIContent("W axis");


            public readonly GUIContent[] wrapModeContents =
            {
                new GUIContent("Repeat"),
                new GUIContent("Clamp"),
                new GUIContent("Mirror"),
                new GUIContent("Mirror Once"),
                new GUIContent("Per-axis")
            };
            public readonly int[] wrapModeValues =
            {
                (int)TextureWrapMode.Repeat,
                (int)TextureWrapMode.Clamp,
                (int)TextureWrapMode.Mirror,
                (int)TextureWrapMode.MirrorOnce,
                -1
            };

            public readonly GUIContent importHiddenLayer = new GUIContent(L10n.Tr("Import Hidden"), L10n.Tr("Import hidden layers"));
            public readonly GUIContent mosaicLayers = new GUIContent(L10n.Tr("Mosaic"), L10n.Tr("Layers will be imported as individual Sprites"));
            public readonly GUIContent characterMode = new GUIContent(L10n.Tr("Character Rig"), L10n.Tr("Enable to support 2D Animation character rigging"));
            public readonly GUIContent generateGOHierarchy = new GUIContent(L10n.Tr("Use Layer Grouping"), L10n.Tr("GameObjects are grouped according to source file layer grouping"));
            public readonly GUIContent resliceFromLayer = new GUIContent(L10n.Tr("Reslice"), L10n.Tr("Recreate Sprite rects from file"));
            public readonly GUIContent paperDollMode = new GUIContent(L10n.Tr("Paper Doll Mode"), L10n.Tr("Special mode to generate a Prefab for Paper Doll use case"));
            public readonly GUIContent experimental =  new GUIContent(L10n.Tr("Experimental"));
            public readonly GUIContent keepDuplicateSpriteName = new GUIContent(L10n.Tr("Keep Duplicate Name"), L10n.Tr("Keep Sprite name same as Layer Name even if there are duplicated Layer Name"));

            public Styles()
 