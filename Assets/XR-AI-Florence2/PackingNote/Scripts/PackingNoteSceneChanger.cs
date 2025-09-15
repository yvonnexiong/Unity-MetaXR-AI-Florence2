using UnityEngine;
using UnityEngine.SceneManagement;



public class PackingNoteSceneChanger : MonoBehaviour
{
    
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
