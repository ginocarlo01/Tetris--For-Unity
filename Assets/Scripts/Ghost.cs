
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    public Tile tile;
    public Board board;
    public Piece trackingPiece;
    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[4];
    }

    private void LateUpdate() //after all other updates
    {
        Clear();
        Copy();
        Drop();
        Set();
    }

    private void Clear()
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3Int tilePosition = this.cells[i] + this.position;
            //piece.cells = posi��o padr�o
            //piece.cells[i] + piece.positon = posi��o global
            tilemap.SetTile(tilePosition, null);
        }
    }
    
    private void Copy()
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            this.cells[i] = this.trackingPiece.cells[i];
        }
    }
     
    private void Drop()
    {
        Vector3Int position = this.trackingPiece.position;

        int current = position.y;

        int bottom = -this.board.boardSize.y / 2 - 1; //uma linha para cima

        this.board.Clear(this.trackingPiece); //limpa para n�o dar conflito

        for(int row = current; row >= bottom; row--)
        {
            position.y = row;

            if(this.board.IsValidPosition(trackingPiece, position))
            {
                this.position = position;
            }
            else
            {
                break;
            }
        }

        this.board.Set(this.trackingPiece); //traz de volta
    }

    private void Set()
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3Int tilePosition = this.cells[i] + this.position;
            //piece.cells = posi��o padr�o
            //piece.cells[i] + piece.positon = posi��o global
            tilemap.SetTile(tilePosition, this.tile);
        }
    }

}