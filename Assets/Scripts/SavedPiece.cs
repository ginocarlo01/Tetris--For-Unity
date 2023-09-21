using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class SavedPiece : MonoBehaviour
{

    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int position { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public int rotationIndex { get; private set; }

    public bool saved;

    public Tilemap tilemap { get; private set; }

    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    public float stepTime;
    private float lockTime;

    private void Awake()
    {
        this.tilemap = GetComponent<Tilemap>();
        this.cells = new Vector3Int[4];
        saved = false;
    }

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        saved = true;
        this.board = board;
        this.data = data;
        this.position = position;
        rotationIndex = 0;
        this.stepTime = Time.time + stepDelay;
        this.lockTime = 0;

        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i]; //converte para vector 3
        }

    }


}
