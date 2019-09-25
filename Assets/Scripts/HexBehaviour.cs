using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexBehaviour : MonoBehaviour
{
    // Enum for keeping the state of hex
    public enum HexState { Idle, Falling, Rotating };

    public HexState state = HexState.Idle;

    public Hex hexReference;

    [SerializeField]
    private Vector3 targetPosition;
    
    public float moveTime = 1.0f;
    private float moveT = 0.0f;

    [Space]
    private Quaternion targetRotation;
    [SerializeField]
    private Vector3 targetRot;
    public float rotationTime = 1.0f;
    private float rotationT=0.0f;

    // Private References
    private BoardManager boardManager;
    private GameManager gameManager;

    public ParticleSystem particles;
    void Awake()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
        boardManager = GameObject.FindGameObjectWithTag("BoardManager").GetComponent<BoardManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        particles = GetComponent<ParticleSystem>();
    }

    private void Update() {
        /*
        switch (state) {
            case HexState.Idle:

                break;
            case HexState.Falling:

                break;
            case HexState.Rotating:

                break;

        }*/
    }
    // The are two case
    // One: Falling
    // Two: Rotating
    // For Falling Just Move and Check the board for matches
    // For Rotating ++
    // After Moving to the target, Check the board
    // If there is match stop and increment the MovementCount in GM
    // If there is no match Rotate the dot again and check the board 2 more times
    // If there is no match after 3 rotating then stop, DONT increment the MovementCouint in GM

    // Used 2 Function overriding for two different case
    // In Dot.Rotate used B function
    // In Falling used A function

    #region Function A
    public void SetPosition(Vector3 pos) {
       // if (moving)
           // return;
        targetPosition = pos;
        StartCoroutine(Move());
    }

    private IEnumerator Move() {
        state = HexState.Falling;
        moveT = 0.0f;

        while (transform.position != targetPosition) {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveT);
            moveT += Time.deltaTime / moveTime;
            yield return new WaitForEndOfFrame();
        }
        MoveEnded();
    }

    private void MoveEnded() {
        state = HexState.Idle;
        bool isThereAnyMatch = boardManager.RefreshMatchingHexes();
    }
    #endregion

    #region Function B
    public void SetPosition(Vector3 pos, DotBehaviour dotB) {
        // if (moving)
        // return;
        targetPosition = pos;
        StartCoroutine(MoveB());
    }

    private IEnumerator MoveB() {
        state = HexState.Rotating;

        moveT = 0.0f;
        while (transform.position != targetPosition) {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveT);
            moveT += Time.deltaTime / moveTime;
            yield return new WaitForEndOfFrame();
        }
        MoveEndedB();
    }

    private void MoveEndedB() {
        state = HexState.Idle;
        
        bool isThereAnyMatch = boardManager.RefreshMatchingHexes();
        if (!isThereAnyMatch ) {
            //DONT Increment gameManager.movementCount
            if (gameManager.rotationCount < 3) {
                if (gameManager.lastRotationDirection) {
                    gameManager.OnClockWiseSwipe();
                } else {
                    gameManager.OnCounterClockWiseSwipe();
                }
            }
        } else {
            gameManager.rotationCount = 3;
            gameManager.gameState.MovementCount++;
            foreach(BombBehaviour bb in boardManager.bombsReferences) {
                bb.timeOut--;
            }
        }
    }
    #endregion
    // Maybe Activated in future
    public void SetRotation(Vector3 rotation) {
        /*
        if (rotating)
            return;
        targetRot = transform.rotation.eulerAngles + rotation;
        StartCoroutine(Rotate());*/
    }

    private void RotationEnded() {
    }

    private bool CheckDownIsEmpty() {
        if (hexReference.y == 0)
            return false;
        if(boardManager.board[hexReference.x,hexReference.y-1] == null) {
            return true;
        }
        return false;
    }

    public Vector3 GetTargetPosition() {
        return targetPosition;
    }

}

