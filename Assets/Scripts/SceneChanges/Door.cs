using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string sceneToLoad;
    public Vector3 playerPosition;
    public VectorValue playerSpawnPosition;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameManager.manager.SaveOnSceneChange();
            playerSpawnPosition.initialValue = playerPosition;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
