using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

	//Loads scene based on scene name
	public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
