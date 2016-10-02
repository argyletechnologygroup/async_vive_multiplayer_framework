using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.Collections;

public class BuildScript
{
    [MenuItem("MyTools/Make Separate Builds")]
    public static void BuildGame ()
    {
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Games", "", "");
        string[] levelsVR = new string[] {"Assets/game/scenes/connect-menu_vr.unity"};
        string[] levelsNonVR = new string[] { "Assets/game/scenes/connect-menu_nonvr.unity" };
        string[] levelsServer = new string[] { "Assets/game/scenes/connect-menu_server.unity" };

        //Build VR
        BuildPipeline.BuildPlayer(levelsVR, path + "/Game_VR.exe", BuildTarget.StandaloneWindows, BuildOptions.None);

        //Build non VR
        BuildPipeline.BuildPlayer(levelsNonVR, path + "/Game_NonVR.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
        
        //Build Server
        BuildPipeline.BuildPlayer(levelsServer, path + "/Game_Server.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
    }
}