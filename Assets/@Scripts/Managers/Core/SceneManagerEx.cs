using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class SceneManagerEx 
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(EScene type)
    {
        // TODO
        //Managers.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(EScene type)
    {
        var name = System.Enum.GetName(typeof(EScene), type);
        return name;
    }

    public void Clear()
    {
        //CurrentScene.Clear();
    }
}
