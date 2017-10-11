using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public void LoadLevel(string name) { 

        SceneManager.LoadScene(1);
        
	}

	public void QuitRequest(){

		Application.Quit ();
	}

}
