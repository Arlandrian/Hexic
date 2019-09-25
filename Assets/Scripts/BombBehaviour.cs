using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombBehaviour : MonoBehaviour
{
    public Text timeOutText;
    public int timeOut = 9;
    private GameManager gm;
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (timeOut <= 0) {
            timeOut = 0;
            gm.GameOver();
        }
        timeOutText.text = timeOut.ToString();
    }

    private void OnDestroy() {
        gm.gameState.Score += 15;
        gm.boardManager.bombsReferences.Remove(this);
    }
}
