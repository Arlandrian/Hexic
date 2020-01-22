using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombBehaviour : MonoBehaviour
{
    [SerializeField] private Text countDownText =null;
    [SerializeField] private int countDown = 9;

    public void Init(int startingCountDown)
    {
        countDown = startingCountDown;
    }

    public void DecrementCountDown() {
        countDown--;
        countDownText.text = countDown.ToString();
        if (countDown <= 0) {
            countDown = 0;
            GameManager.Instance.GameOver();
        }

    }

    private void OnDestroy() {
        GameManager.Instance.AddScore(15);
        BoardManager.Instance.bombsReferences.Remove(this);
    }
}
