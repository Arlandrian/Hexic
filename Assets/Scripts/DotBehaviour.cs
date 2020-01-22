using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotBehaviour : MonoBehaviour
{
    public bool selected = false;

    public int dotIndexX, dotIndexY;

    public Dot dotObject;

    // Shows the dot if headed left or right
    public bool isLeft;

    public bool rotating = false;

    private BoardManager boardManager;

    private void Start() {
        boardManager = BoardManager.Instance;

    }

    public void Select() {
        selected = true;
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach(var sr in sprites) {
            sr.enabled = true;
        }
    }

    public void Deselect() {
        selected = false;
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (var sr in sprites) {
            sr.enabled = false;
        }
    }

    //Rotates three slaves around dot
    public void Rotate(bool clockWise) {
        // Take the slaves and self
        int slaveSideX = dotObject.slavesBoardIndex[0].x;
        int slaveSideY = dotObject.slavesBoardIndex[0].y;
        int slaveTopX = dotObject.slavesBoardIndex[1].x;
        int slaveTopY = dotObject.slavesBoardIndex[1].y;
        int slaveBotX = dotObject.slavesBoardIndex[2].x;
        int slaveBotY = dotObject.slavesBoardIndex[2].y;

        Hex hexSide = boardManager.board[slaveSideX, slaveSideY];
        Hex hexTop = boardManager.board[slaveTopX, slaveTopY];
        Hex hexBot = boardManager.board[slaveBotX, slaveBotY];

        HexBehaviour hexBSide = hexSide.go.GetComponent<HexBehaviour>();
        HexBehaviour hexBTop = hexTop.go.GetComponent<HexBehaviour>();
        HexBehaviour hexBBot = hexBot.go.GetComponent<HexBehaviour>();

        if (isLeft) {
            if (clockWise) {
                // Update Board
                Hex temp1 = boardManager.board[slaveTopX, slaveTopY].Clone();

                boardManager.board[slaveSideX, slaveSideY].x = slaveTopX;
                boardManager.board[slaveSideX, slaveSideY].y = slaveTopY;
                boardManager.board[slaveTopX, slaveTopY] = boardManager.board[slaveSideX, slaveSideY];
                boardManager.board[slaveBotX, slaveBotY].x = slaveSideX;
                boardManager.board[slaveBotX, slaveBotY].y = slaveSideY;
                boardManager.board[slaveSideX, slaveSideY] = boardManager.board[slaveBotX, slaveBotY];
                temp1.x = slaveBotX;
                temp1.y = slaveBotY;
                boardManager.board[slaveBotX, slaveBotY] = temp1;

                Vector3 hexSideTargetPosition = boardManager.hexSockets[slaveTopX, slaveTopY].transform.position;
                Vector3 hexTopTargetPosition = boardManager.hexSockets[slaveBotX, slaveBotY].transform.position;
                Vector3 hexBotTargetPosition = boardManager.hexSockets[slaveSideX, slaveSideY].transform.position;

                hexBSide.SetPosition(hexSideTargetPosition,this);
                hexBTop.SetPosition(hexTopTargetPosition, this);
                hexBBot.SetPosition(hexBotTargetPosition, this);

                // hexBSide.SetRotation(Vector3.forward*60f);
                // hexBTop.SetRotation(Vector3.forward *60f);
                // hexBBot.SetRotation(Vector3.forward *60f);

                transform.RotateAround(transform.position, transform.forward, -120.0f);
            } else {
                // Update Board
                Hex temp1 = boardManager.board[slaveBotX, slaveBotY].Clone();

                boardManager.board[slaveSideX, slaveSideY].x = slaveBotX;
                boardManager.board[slaveSideX, slaveSideY].y = slaveBotY;
                boardManager.board[slaveBotX, slaveBotY] = boardManager.board[slaveSideX, slaveSideY];
                boardManager.board[slaveTopX, slaveTopY].x = slaveSideX;
                boardManager.board[slaveTopX, slaveTopY].y = slaveSideY;
                boardManager.board[slaveSideX, slaveSideY] = boardManager.board[slaveTopX, slaveTopY];
                temp1.x = slaveTopX;
                temp1.y = slaveTopY;
                boardManager.board[slaveTopX, slaveTopY] = temp1;

                Vector3 hexSideTargetPosition = boardManager.hexSockets[slaveBotX, slaveBotY].transform.position;
                Vector3 hexTopTargetPosition =  boardManager.hexSockets[slaveSideX, slaveSideY].transform.position;
                Vector3 hexBotTargetPosition =  boardManager.hexSockets[slaveTopX, slaveTopY].transform.position;

                hexBSide.SetPosition(hexSideTargetPosition, this);
                hexBTop.SetPosition(hexTopTargetPosition, this);
                hexBBot.SetPosition(hexBotTargetPosition, this);

                // hexBSide.SetRotation(Vector3.forward * -60f);
                // hexBTop.SetRotation(Vector3.forward * -60f);
                // hexBBot.SetRotation(Vector3.forward * -60f);

                transform.RotateAround(transform.position, transform.forward, 120.0f);
            }
        } else {
            if (clockWise) {
                // Update Board
                Hex temp1 = boardManager.board[slaveBotX, slaveBotY].Clone();

                Hex side = boardManager.board[slaveSideX, slaveSideY];
                side.x = slaveBotX;
                side.y = slaveBotY;
                boardManager.board[slaveBotX, slaveBotY] = side;

                Hex top = boardManager.board[slaveTopX, slaveTopY];
                top.x = slaveSideX;
                top.y = slaveSideY;
                boardManager.board[slaveSideX, slaveSideY] = boardManager.board[slaveTopX, slaveTopY];
                temp1.x = slaveTopX;
                temp1.y = slaveTopY;
                boardManager.board[slaveTopX, slaveTopY] = temp1;

                Vector3 hexSideTargetPosition = boardManager.hexSockets[slaveBotX, slaveBotY].transform.position;
                Vector3 hexTopTargetPosition = boardManager.hexSockets[slaveSideX, slaveSideY].transform.position;
                Vector3 hexBotTargetPosition = boardManager.hexSockets[slaveTopX, slaveTopY].transform.position;

                hexBSide.SetPosition(hexSideTargetPosition, this);
                hexBTop.SetPosition(hexTopTargetPosition, this);
                hexBBot.SetPosition(hexBotTargetPosition, this);

                // hexBSide.SetRotation(Vector3.forward * -60f);
                // hexBTop.SetRotation(Vector3.forward * -60f);
                // hexBBot.SetRotation(Vector3.forward * -60f);


                transform.RotateAround(transform.position, transform.forward, 120.0f);
            } else {
                // Update Board
                Hex temp1 = boardManager.board[slaveTopX, slaveTopY].Clone();

                Hex side = boardManager.board[slaveSideX, slaveSideY];
                side.x = slaveTopX;
                side.y = slaveTopY;
                boardManager.board[slaveTopX, slaveTopY] = side;

                Hex bot = boardManager.board[slaveBotX, slaveBotY];
                bot.x = slaveSideX;
                bot.y = slaveSideY;
                boardManager.board[slaveSideX, slaveSideY] = bot;

                temp1.x = slaveBotX;
                temp1.y = slaveBotY;
                boardManager.board[slaveBotX, slaveBotY] = temp1;

                Vector3 hexSideTargetPosition = boardManager.hexSockets[slaveTopX, slaveTopY].transform.position;
                Vector3 hexTopTargetPosition = boardManager.hexSockets[slaveBotX, slaveBotY].transform.position;
                Vector3 hexBotTargetPosition = boardManager.hexSockets[slaveSideX, slaveSideY].transform.position;

                hexBSide.SetPosition(hexSideTargetPosition, this);
                hexBTop.SetPosition(hexTopTargetPosition, this);
                hexBBot.SetPosition(hexBotTargetPosition, this);

                // hexBSide.SetRotation(Vector3.forward * 60f);
                // hexBTop.SetRotation(Vector3.forward * 60f);
                // hexBBot.SetRotation(Vector3.forward * 60f);

                transform.RotateAround(transform.position, transform.forward, -120.0f);
            }
        }
        
    }


    public HexBehaviour [] GetSlavesBehaviors() {
        int slaveSideX = dotObject.slavesBoardIndex[0].x;
        int slaveSideY = dotObject.slavesBoardIndex[0].y;
        int slaveTopX = dotObject.slavesBoardIndex[1].x;
        int slaveTopY = dotObject.slavesBoardIndex[1].y;
        int slaveBotX = dotObject.slavesBoardIndex[2].x;
        int slaveBotY = dotObject.slavesBoardIndex[2].y;

        Hex hexSide = boardManager.board[slaveSideX, slaveSideY];
        Hex hexTop = boardManager.board[slaveTopX, slaveTopY];
        Hex hexBot = boardManager.board[slaveBotX, slaveBotY];

        HexBehaviour hexBSide = hexSide.go.GetComponent<HexBehaviour>();
        HexBehaviour hexBTop = hexTop.go.GetComponent<HexBehaviour>();
        HexBehaviour hexBBot = hexBot.go.GetComponent<HexBehaviour>();
        return new HexBehaviour[3] { hexBSide, hexBTop, hexBBot };
    }

    private void OnValidate() {
        if (selected) {
            Select();
        } else {
            Deselect();
        }
    }

}
