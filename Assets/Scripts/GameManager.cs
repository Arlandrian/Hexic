using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("Settings")]
    public int pointMultiplier = 5;
    [SerializeField] private float circleCastRadius = 1.0f;
    [SerializeField] private float touchSwipeDistanceThreshold = 0.5f;


    [Header("For Debug")]
    public float timeScale=1.0f;
    [HideInInspector]
    public bool lastRotationDirection;//true clockWise, false cClockWise
    [HideInInspector]
    public int rotationCount;

    public GameState gameState;

    [Space]

    public bool bombSpawn;
    public bool isGameOver = false;

    //Private variables
    private Touch touch;
    private RaycastHit2D[] touchHits;

    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;

    GameObject lastSelectedDot = null;

    private void Awake() {
        gameState.Init();
    }

    void Update() {
#if UNITY_EDITOR
        Time.timeScale = timeScale;
        if (lastSelectedDot) {
            Debug.DrawLine(lastSelectedDot.transform.position, firstTouchPosition,Color.red);
            Debug.DrawLine(lastSelectedDot.transform.position, finalTouchPosition,Color.green);

        }
#endif
        if (!BoardManager.Instance.IsBoardChanging())
            HandleInput();
    }

    void HandleInput() {
#if UNITY_EDITOR || UNITY_STANDALONE

        if (Input.GetMouseButtonDown(0)) {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        }
        if (Input.GetMouseButtonUp(0)) {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float dst = Vector2.Distance(firstTouchPosition, finalTouchPosition);

            if (dst < touchSwipeDistanceThreshold) {
                OnTouch();
            } else {
                OnSwipe();
            }
        }
#elif UNITY_ANDROID || UNITY_IOS
        
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase) {

                case TouchPhase.Began:
                    // Record initial touch position.
                    firstTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    break;

                case TouchPhase.Ended:

                    finalTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                    float dst = Vector2.Distance(firstTouchPosition, finalTouchPosition);

                    if(dst < touchSwipeDistanceThreshold) {
                        OnTouch();
                    } else {
                        OnSwipe();
                    }
                    break;
            }
        }

#endif
    }
    // Makes a circle cast and takes the nearest to the touch
    void OnTouch() {
        Vector3 touchWorldPosition = finalTouchPosition;
        touchHits = Physics2D.CircleCastAll(touchWorldPosition, circleCastRadius, Vector2.zero);
        float minDist = 4.0f;
        GameObject selectedDot = null;
        foreach(RaycastHit2D hit in touchHits) {
            Vector2 hitPos = hit.collider.gameObject.transform.position;
            float dst = SqrDistance(touchWorldPosition, hitPos);
            if (dst < minDist) {
                minDist = dst;
                selectedDot = hit.collider.gameObject;
            }
        }
        if (selectedDot == null)
            return;

        if (lastSelectedDot == null)
            lastSelectedDot = selectedDot;

        if (selectedDot != lastSelectedDot) {
            //Unselect
            lastSelectedDot.GetComponent<DotBehaviour>().Deselect();
        }

        //Select
        lastSelectedDot = selectedDot;
        lastSelectedDot.GetComponent<DotBehaviour>().Select();
    }

    void OnSwipe() {
        if (lastSelectedDot == null)
            return;

        Vector2 origin = lastSelectedDot.transform.position;
        Vector2 OStart = firstTouchPosition - origin;
        Vector2 OEnd   = finalTouchPosition - origin;
        rotationCount = 0;
        if (Vector2.SignedAngle(OStart, OEnd) < 0.0f) {//ClockWise
            lastRotationDirection = true;
            OnClockWiseSwipe();
        } else {//C_ClockWise
            lastRotationDirection = false;
            OnCounterClockWiseSwipe();
        }
    }

    public void OnClockWiseSwipe() {
        var dotBehaviour = lastSelectedDot.GetComponent<DotBehaviour>();
        dotBehaviour.Rotate(true);
        rotationCount++;
        
    }

    public void OnCounterClockWiseSwipe() {
        var dotBehaviour = lastSelectedDot.GetComponent<DotBehaviour>();
        dotBehaviour.Rotate(false);
        rotationCount++;
    }

    public void AddScore(int score)
    {
        gameState.Score += score;
        UIManager.Instance.ScoreChanged();
    }

    public void IncrementMovementCount()
    {
        gameState.MovementCount++;
        UIManager.Instance.MovementCountChanged();
    }


    public void GameOver() {
        if (!isGameOver) {
            StartCoroutine(ExplodeAllHexagons());
        }

    }

    private IEnumerator ExplodeAllHexagons() {
        lastSelectedDot.SetActive(false);
        isGameOver = true;
        Hex[,] board = BoardManager.Instance.board;
        foreach (Hex hex in board) {
            BoardManager.Instance.DestroyHex(hex);
            yield return new WaitForSeconds(0.1f);
        }
        //Game Over UI
        ShowGameOverUI();
    }

    private void ShowGameOverUI() {

        UIManager.Instance.ShowGameOver();
        // Animation will start after gameobject activated
    }

    public void RestartGame() {
        SceneManager.LoadScene(0);
    }

    static float SqrDistance(Vector2 a, Vector2 b) {
        return Mathf.Pow(b.x - a.x, 2) + Mathf.Pow(b.y - a.y, 2);
    }

    private void OnDrawGizmos() {
        if (Input.touchCount > 0) {
            Gizmos.DrawWireSphere(firstTouchPosition, circleCastRadius);
        }
    }
}
[System.Serializable]
public struct GameState
{
    public int Score;
    public int MovementCount;
    //Board

    public void Init() {
        Score = 0;
        MovementCount = 0;
    }
}