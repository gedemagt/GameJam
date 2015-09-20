using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnvironmentController : MonoBehaviour {
    public GameObject background;
    public float rotateionCounter = 0;
    public GameObject crowd;
    public Image[] crowdColor;
    public AudioClip cheering;
    public Goal goalLeft;
    public Goal goalRight;
    public Button startRestart;
    public Sprite retardCat;
    public Image spriteCat;
    public Image tearImageRight;

    public PlayerController player1;
    public PlayerController player2;

    public ParticleSpawner spawner;

    public Canvas Menu;
    GameObject[] catBalls;

    public void StartGame() {
        StartCoroutine(StartingGame());
        spriteCat.sprite = retardCat;
    }

    IEnumerator StartingGame() {

        yield return new WaitForSeconds(0.1f);
        GameObject[] catBalls = GameObject.FindGameObjectsWithTag("CatBall");
        foreach (var item in catBalls) {
            item.GetComponent<BallController>().Reset();
        }
        player1.Reset();
        player2.Reset();

        //player1.count = 0;
        //player2.count = 0;
        Menu.enabled = false;

    }

    public void ExitGame() {
        tearImageRight.enabled = true;
    }

    IEnumerator ExitingGame() {
        yield return new WaitForSeconds(1);
        Application.Quit();
    }

    // Use this for initialization
    void Start () {
        goalLeft.onCount += CallFromGoal;
        goalRight.onCount += CallFromGoal;
    }

    void CallFromGoal() {
        if (player1.count >= 10 || player2.count >= 10 && Menu.enabled == false) {
            string playerWon = player2.count >= 10 ? "player2" : "player1";
            UpDown(true, playerWon);
            Menu.enabled = true;
        }


    }
	
	// Update is called once per frame
	void Update () {
        background.transform.rotation = Quaternion.Euler(new Vector3(background.transform.rotation.x, background.transform.rotation.y, background.transform.rotation.z + rotateionCounter));
        rotateionCounter += 0.1f;


    }

    void UpDown(bool up, string winner) {
        spawner.DoStop();
        if (up) {
            iTween.MoveBy(crowd, new Vector3(0f, 1f, 0), 1);
            gameObject.GetComponent<AudioSource>().clip = cheering;
            for (int i = 0; i < crowdColor.Length; i++) {
                crowdColor[i].color = winner == "player2" ? Color.red : Color.green;
            }
            gameObject.GetComponent<AudioSource>().Play();
        }
        if (!up) {
            iTween.MoveBy(crowd, new Vector3(0f, -3f, 0), 1);
            gameObject.GetComponent<AudioSource>().Stop();
        }


    }
}
