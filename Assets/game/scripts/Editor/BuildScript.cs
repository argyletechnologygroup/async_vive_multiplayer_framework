using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;

public class BuildScript
{
    internal class BuildPaths
    {
        readonly string executableName;
        readonly string[] scenes;

        public BuildPaths(string executableName, string[] scenes)
        {
            this.executableName = executableName;
            this.scenes = makeScenePathList(scenes);
        }

        public string Executable
        {
            get { return executableName; }
        }

        public string[] Scenes
        {
            get { return scenes; }
        }

        private static string[] makeScenePathList(string[] scenes) {
            return (
                from scene in scenes
                let scenePath = string.Format("Assets/game/scenes/{0}.unity", scene)
                select scenePath
            ).ToArray();
        }
    }

    private static BuildPaths vrClientPaths = new BuildPaths("viventure_VR.exe", new string[] { "connect-menu_vrclient", "lobby" });
    private static BuildPaths clientPaths = new BuildPaths("viventure.exe", new string[] { "connect-menu_client", "lobby" });
    private static BuildPaths serverPaths = new BuildPaths("viventure_dedicated.exe", new string[] { "connect-menu_server", "lobby" });

    //private static BuildPaths vrClientPaths = new BuildPaths("Client_VR.exe",   new string[] { "lobby" });
    //private static BuildPaths clientPaths = new BuildPaths("Client.exe",        new string[] { "lobby" });
    //private static BuildPaths serverPaths = new BuildPaths("Server.exe",        new string[] { "lobby" });

    [MenuItem("Bulid/Build All")]
    public static void BuildGame()
    {
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Games", "", "");
        //BuildServer(path);
        BuildClient(path);
        BuildVRClient(path);
    }

    [MenuItem("Bulid/Build Server")]
    public static void BuildServer()
    {
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Server", "", "");
        BuildServer(path);
    }

    [MenuItem("Bulid/Build Client")]
    public static void BuildClient()
    {
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Client", "", "");
        BuildClient(path);
    }

    [MenuItem("Bulid/Build VR Client")]
    public static void BuildVRClient()
    {
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built VR Client", "", "");
        BuildVRClient(path);
    }

    private static void BuildServer(string path)
    {
        //Build VR
        PlayerSettings.virtualRealitySupported = false;
        PlayerSettings.productName = "Viventure Server";
        BuildPipeline.BuildPlayer( serverPaths.Scenes, path + "/" + serverPaths.Executable, BuildTarget.StandaloneWindows, BuildOptions.Development);
    }

    private static void BuildClient(string path)
    {
        //Build VR
        PlayerSettings.virtualRealitySupported = false;
        PlayerSettings.productName = "Viventure Client";
        BuildPipeline.BuildPlayer( clientPaths.Scenes, path + "/" + clientPaths.Executable, BuildTarget.StandaloneWindows, BuildOptions.Development);
    }

    private static void BuildVRClient(string path)
    {
        //Build VR
        PlayerSettings.virtualRealitySupported = true;
        PlayerSettings.productName = "Viventure VR Client";
        BuildPipeline.BuildPlayer( vrClientPaths.Scenes, path + "/" + vrClientPaths.Executable, BuildTarget.StandaloneWindows, BuildOptions.Development);
    }
}