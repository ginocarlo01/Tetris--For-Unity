
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int position { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public int rotationIndex { get; private set; }
    
    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    public float stepTime;
    private float lockTime;

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.data = data;
        this.position = position;
        rotationIndex = 0;
        this.stepTime = Time.time + stepDelay;
        this.lockTime = 0;

        if(this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for(int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i]; //converte para vector 3
        }
    }

    private void Update()
    {
        this.board.Clear(this);

        this.lockTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2Int.right);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }

        if(Time.time >= stepTime) //é chamado a cada x segundos, time.time é o tempo atual 
        {
            Step();
        }

        this.board.Set(this);
    }

    private void Step()
    {
        this.stepTime = Time.time + stepDelay;

        Move(Vector2Int.down);

        if(this.lockTime >= this.lockDelay) //se o movimento acima for inválido, ele irá chamar o lock
        {
            Lock() ;
        }
    }

    private void Lock()
    {
        this.board.Set(this); //último set antes da morte  
        this.board.ClearLines(); //toda vez que uma peça for posicionada
        this.board.SpawnPiece(this.board.nextPiece.data);
        this.board.SetNextPiece();
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }

        Lock();
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = board.IsValidPosition(this, newPosition);

        if (valid)
        {
            this.position = newPosition;
            this.lockTime = 0; //aplicar aqui já aplica no rotation por causa dos testes de wallkick
        }

        return valid;
    }

    private void Rotate(int direction)
    {
        int originalIndex = this.rotationIndex;
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction);

        if(!TestWallKicks(this.rotationIndex, direction))
        {
            this.rotationIndex = originalIndex;
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3 cell = this.cells[i];

            int x, y;

            switch (this.data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            this.cells[i] = new Vector3Int(x, y);
        }
    }

    public bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection); //retorna o index da tabela (linha)

        for(int i = 0; i < this.data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i]; //pega a própria célula

            if (Move(translation))
            {
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if(rotationDirection < 0)
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, this.data.wallKicks.Length);
    }

    private int Wrap(int input, int min, int max)
    {
        if(input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }

}
