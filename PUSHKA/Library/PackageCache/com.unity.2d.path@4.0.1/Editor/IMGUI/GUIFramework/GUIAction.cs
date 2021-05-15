/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Unity Technologies.
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Unity.CodeEditor;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Unity.VisualStudio.EditorTests")]
[assembly: InternalsVisibleTo("Unity.VisualStudio.Standalone.EditorTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Microsoft.Unity.VisualStudio.Editor
{
	[InitializeOnLoad]
	public class VisualStudioEditor : IExternalCodeEditor
	{
		private static readonly IVisualStudioInstallation[] _installations;

		internal static bool IsOSX => Application.platform == RuntimePlatform.OSXEditor;
		internal static bool IsWindows => !IsOSX && Path.DirectorySeparatorChar == FileUtility.WinSeparator && Environment.NewLine == "\r\n";

		CodeEditor.Installation[] IExternalCodeEditor.Installations => _installations
			.Select(i => i.ToCodeEditorInstallation())
			.ToArray();

		private readonly IGenerator _generator = new ProjectGeneration();

		static VisualStudioEditor()
		{
			try
			{
				_installations = Discovery
					.GetVisualStudioInstallations()
					.ToArray();
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError($"Error detecting Visual Studio installations: {ex}");
				_installations = Array.Empty<VisualStudioInstallation>();
			}

			CodeEditor.Register(new VisualStudioEditor());
		}

		internal static bool IsEnabled
		{
			get
			{
				return CodeEditor.CurrentEditor is VisualStudioEditor;
			}
		}

		public void CreateIfDoesntExist()
		{
			if (!_generator.HasSolutionBeenGenerated())
				_generator.Sync();
		}

		public void Initialize(string editorInstallationPath)
		{
		}

		internal virtual bool TryGetVisualStudioInstallationForPath(string editorPath, out IVisualStudioInstallation installation)
		{
			// lookup for well known installations
			foreach (var candidate in _installations)
			{
				if (!string.Equals(Path.GetFullPath(editorPath), Path.GetFullPath(candidate.Path), StringComparison.OrdinalIgnoreCase))
					continue;

				installation = candidate;
				return true;
			}

			return Discovery.TryDiscoverInstallation(editorPath, out installation);
		}

		public virtual bool TryGetInstallationForPath(string editorPath, out CodeEditor.Installation installation)
		{
			var result = TryGetVisualStudioInstallationForPath(editorPath, out var vsi);
			installation = vsi == null ? default : vsi.ToCodeEditorInstallation();
			return result;
		}

		public void OnGUI()
		{
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			var package = UnityEditor.PackageManager.PackageInfo.FindForAssembly(GetType().Assembly);

			var style = new GUIStyle
			{
				richText = true,
				marg