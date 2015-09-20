using UnityEngine;
using System.Collections;

public class GameHandler : MonoBehaviour {
	public Canvas Menu;
	public void StartGame(){
		Menu.enabled = false;

	}

	public void ExitGame(){
		Application.Quit ();
	}
}
