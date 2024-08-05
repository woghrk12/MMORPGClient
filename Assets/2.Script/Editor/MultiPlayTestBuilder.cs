using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MultiPlayTestBuilder
{
    [MenuItem("Tools/Run MultiPlayer/2 Players")]
    private static void PerformWin64Build2()
    {
        PerformWin64Build(2);
    }

    [MenuItem("Tools/Run MultiPlayer/3 Players")]
    private static void PerformWin64Build3()
    {
        PerformWin64Build(3);
    }

    [MenuItem("Tools/Run MultiPlayer/4 Players")]
    private static void PerformWin64Build4()
    {
        PerformWin64Build(4);
    }

    private static void PerformWin64Build(int playerCount)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
        Application.runInBackground = true;

        string[] scenePaths = GetScenePaths();
        string buildPath = "Builds/Win64/";
        string projectName = GetProjectName();

        for (int index = 0; index < playerCount; index++)
        {
            string folderName = projectName + index.ToString();

            BuildPipeline.BuildPlayer(
                scenePaths, 
                buildPath + folderName + "/" + folderName + ".exe",
                BuildTarget.StandaloneWindows64,
                BuildOptions.AutoRunPlayer
                );
        }
    }

    private static string GetProjectName()
    {
        string[] s = Application.dataPath.Split('/');

        return s[s.Length - 2];
    }

    private static string[] GetScenePaths()
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        string[] paths = new string[scenes.Length];

        for (int index = 0; index < scenes.Length; index++)
        {
            paths[index] = scenes[index].path;
        }

        return paths;
    }
}
