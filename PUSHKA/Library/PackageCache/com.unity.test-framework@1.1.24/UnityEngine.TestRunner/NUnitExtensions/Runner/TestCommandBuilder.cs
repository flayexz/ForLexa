using System;
using JetBrains.Annotations;
// using UnityEditor.SettingsManagement;
using UnityEngine;

namespace Unity.Cloud.Collaborate.Settings
{
    [UsedImplicitly]
    internal class CollabSettings
    {
        public enum DisplayMode
        {
            Simple,
            Advanced
        }

        public enum OpenLocation
        {
            Docked,
            Window
        }

        // List of setting keys
        public const string settingRelativeTimestamp = "general.relativeTimestamps";
        // public const string settingAutoFetch = "general.autoFetch";
        // public const string settingDisplayMode = "general.displayMode";
        public const string settingDefaultOpenLocation = "general.defaultOpenLocation";

        // [UserSetting] attribute registers this setting with the UserSettingsProvider so that it can be automatically
        // shown in the UI.
        // [UserSetting("General Settings", "Default Open Location")]
        // [UsedImplicitly]
        // static CollabSetting<OpenLocation> s_DefaultOpenLocation = new CollabSetting<OpenLocation>(settingDefaultOpenLocation, OpenLocation.Docked);
        //
        // [UserSetting("General Settings", "Relative Timestamps")]
        // [UsedImplicitly]
        // static CollabSetting<bool> s_RelativeTimestamps = new CollabSetting<bool>(settingRelativeTimestamp, true);
        //
        // [UserSetting("General Settings", "Automatic Fetch")]
        // [UsedImplicitly]
        // static CollabSetting<bool> s_AutoFetch = new CollabSetting<bool>(settingAutoFetch, true);
        //
        // [UserSetting("General Settings", "Display Mode")]
        // [UsedImplicitly]
        // static CollabSetting<DisplayMode> s_DisplayMode = new CollabSetting<DisplayMode>(settingDisplayMode, DisplayMode.Simple);
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        using UnityEditor;

namespace Unity.Cloud.Collaborate.Settings
{
    /// <summary>
    /// This class will act as a manager for the <see cref="Settings"/> singleton.
    /// </summary>
    internal static class CollabSettingsManager
    {
        // Project settings will be stored in a JSON file in a directory matching this name.
        // const string k_PackageName = "com.unity.collab-proxy";

        // static UnityEditor.SettingsManagement.Settings s_Instance;
        //
        // internal static UnityEditor.SettingsManagement.Settings instance =>
        //     s_Instance ?? (s_Instance = new UnityEditor.SettingsManagement.Settings(k_PackageName));

        // The rest of this file is just forwarding the various setting methods to the instance.

        // public static void Save()
        // {
        //     instance.Save();
        // }

        public static T Get<T>(string key, SettingsScope scope = SettingsScope.Project, T fallback = default)
        {
            return fallback;
            //return instance.Get(key, scope, fallback);
        }

        // public static void Set<T>(string key, T value, SettingsScope s