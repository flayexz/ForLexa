layerAssemblies, "Player projects", "For each player project generate an additional csproj with the name 'project-player.csproj'");
			RegenerateProjectFiles();
			EditorGUI.indentLevel--;
		}

		void RegenerateProjectFiles()
		{
			var rect = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(new GUILayoutOption[] { }));
			rect.width = 252;
			if (GUI.Button(rect, "Regenerate project files"))
			{
				_generator.Sync();
			}
		}

		void SettingsButton(ProjectGenerationFlag preference, string guiMessage, string toolTip)
		{
			var prevValue = _generator.AssemblyNameProvider.ProjectGenerationFlag.HasFlag(preference);
			var newValue = EditorGUILayout.Toggle(new GUIContent(guiMessage, toolTip), prevValue);
			if (newValue != prevValue)
			{
				_generator.AssemblyNameProvider.ToggleProjectGeneration(preference);
			}
		}

		public void SyncIfNeeded(string[] addedFiles, string[] deletedFiles, string[] movedFiles, string[] movedFromFiles, string[] importedFiles)
		{
			_generator.SyncIfNeeded(addedFiles.Union(deletedFiles).Union(movedFiles).Union(movedFromFiles), importedFiles);

			foreach (var file in importedFiles.Where(a => Path.GetExtension(a) == ".pdb"))
			{
				var pdbFile = FileUtility.GetAssetFullPath(file);

				// skip Unity packages like com.unity.ext.nunit
				if (pdbFile.IndexOf($"{Path.DirectorySeparatorChar}com.unity.", StringComparison.OrdinalIgnoreCase) > 0)
					continue;

				var asmFile = Path.ChangeExtension(pdbFile, ".dll");
				if (!File.Exists(asmFile) || !Image.IsAssembly(asmFile))
					continue;

				if (Symbols.IsPortableSymbolFile(pdbFile))
					continue;

				UnityEngine.Debug.LogWarning($"Unity is only able to load mdb or portable-pdb symbols. {file} is using a legacy pdb format.");
			}
		}

		public void SyncAll()
		{
			AssetDatabase.Refresh();
			_generator.Sync();
		}

		bool IsSupportedPath(string path)
		{
			// Path is empty with "Open C# Project", as we only want to open the solution without specific files
			if (string.IsNullOrEmpty(path))
				return true;

			// cs, uxml, uss, shader, compute, cginc, hlsl, glslinc, template are part of Unity builtin extensions
			// txt, xml, fnt, cd are -often- par of Unity user extensions
			// asdmdef is mandatory included
			if (_generator.IsSupportedFile(path))
				return true;

			return false;
		}

		private static void CheckCurrentEditorInstallation()
		{
			var editorPath = CodeEditor.CurrentEditorInstallation;
			try
			{
				if (Discovery.TryDiscoverInstallation(editorPath, out _))
					return;
			}
			catch (IOException)
			{
			}

			UnityEngine.Debug.LogWarning($"Visual Studio executable {editorPath} is not found. Please change your settings in Edit > Preferences > External Tools.");
		}

		public bool OpenProject(string path, int line, int column)
		{
			CheckCurrentEditorInstallation();

			if (!IsSupportedPath(path))
				return false;

			if (!IsProjectGeneratedFor(path, out var missingFlag))
				UnityEngine.Debug.LogWarning($"You are trying to open {path} outside a generated project. This might cause problems with IntelliSense and debugging. To avoid this, you can change your .csproj preferences in Edit > Preferences > External Tools and enable {GetProjectGenerationFlagDescription(missingFlag)} generation.");

			if (IsOSX)
				return OpenOSXApp(path, line, column);

			if (IsWindows)
				return OpenWindowsApp(path, line);

			return false;
		}

		private static string GetProjectGenerationFlagDescription(ProjectGenerationFlag flag)
		{
			switch (flag)
			{
				case ProjectGenerationFlag.BuiltIn:
					return "Built-in packages";
				case ProjectGenerationFlag.Embedded:
					return "Embedded packages";
				case ProjectGenerationFlag.Git:
					return "Git packages";
				case ProjectGenerationFlag.Local:
					return "Local packages";
				case ProjectGenerationFlag.LocalTarBall:
					return "Local tarball";
				case ProjectGenerationFlag.PlayerAssemblies:
					return "Player projects";
				case ProjectGenerationFlag.Registry:
					return "Registry packages";
				case ProjectGenerationFlag.Unknown:
					return "Packages from unknown sources";
				case ProjectGenerationFlag.None:
				default:
					return string.Empty;
			}
		}

		private bool IsProjectGeneratedFor(string path, 