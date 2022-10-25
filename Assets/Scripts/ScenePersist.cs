using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    int mySceneBuildIndex;
    private void Awake()
    {
        mySceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

        ScenePersist[] scenePersist = FindObjectsOfType<ScenePersist>();
        int noOfScenePersist = scenePersist.Length;

        foreach (ScenePersist scene in scenePersist)
        {
            if (scene.mySceneBuildIndex != SceneManager.GetActiveScene().buildIndex)
            {
                Destroy(scene.gameObject);
                DontDestroyOnLoad(gameObject); //This will only happen if two SceneHandlers are present. This runs from the 'new' SceneHandler. So we save 'this' one.
                noOfScenePersist--; //since it isn't automatically updated
                break;
            }
        }

        if (noOfScenePersist > 1)
        {
            Destroy(gameObject);
        }

        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}