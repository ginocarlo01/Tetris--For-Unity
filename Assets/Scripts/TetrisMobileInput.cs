using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisMobileInput : IMobileInputState
{
    Piece piece;
    public TetrisMobileInput(Piece piece)
    {
        this.piece = piece;
    }
    public void OnBeginState()
    {
#if UNITY_EDITOR
        Debug.Log("Tetris input");
#endif
        MobileButtonsManager.instance.Reset();


        MobileButtonsManager.instance.up.onClick.AddListener(MobileInputUp);
        MobileButtonsManager.instance.down.onClick.AddListener(MobileInputDown);
        MobileButtonsManager.instance.left.onClick.AddListener(MobileInputLeft);
        MobileButtonsManager.instance.right.onClick.AddListener(MobileInputRight);
    }

    void MobileInputUp() => piece.VerticalInt = 1;
    void MobileInputDown() => piece.VerticalInt = -1;
    void MobileInputLeft() => piece.HorizontalInt = -1;
    void MobileInputRight() { piece.HorizontalInt = 1; Debug.Log(piece.HorizontalInt); }
}
