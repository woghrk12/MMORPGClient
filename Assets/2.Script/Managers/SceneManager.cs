using System;
using UnityEngine;

using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager
{
    #region Properties

    public BaseScene CurrentScene
    {
        get
        {
            return GameObject.FindObjectOfType<BaseScene>();
        }
    }

    #endregion Properties

    #region Methods

    public void LoadScene(EScene type)
    {
        Managers.Clear();

        UnitySceneManager.LoadScene(GetSceneName(type));
    }

    public void Clear()
    {
        CurrentScene?.Clear();
    }

    private string GetSceneName(EScene type)
    {
        string name = Enum.GetName(typeof(EScene), type);

        return name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();
    }

    #endregion Methods
}
