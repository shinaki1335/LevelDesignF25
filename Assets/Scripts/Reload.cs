using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour
{
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(Random.Range(1,GameMaster.Levels));
        }   
    }
}
