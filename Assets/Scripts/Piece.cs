
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

    [SerializeField]
    private float moveDelay;
    [SerializeField]
    private float moveTimer = 0;
    [SerializeField]
    private bool canMove = true;

    [SerializeField]
    private float horizontal = 0;

    [SerializeField]
    private float vertical = 0;

    [SerializeField]
    private int horizontalInt = 0;

    [SerializeField]
    private int verticalInt = 0;

    private bool isPaused;
    public bool canBeControlled;

    [SerializeField]
    private Signal tetrisLostSignal;

    bool disableThisPiece = false;
    bool alreadyDisabled = false;

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

        canBeControlled = true;
    }

    public void ChangePauseState()
    {
        isPaused = !isPaused;
    }

    private void Start()
    {
        DisableMovement();
    }

    private void Update()
    {
        if (disableThisPiece && !alreadyDisabled)
        {
            this.board.Clear(this);
            alreadyDisabled = true;
        }


        if (isPaused) { return; }

        if(!canBeControlled) { return; }

        this.board.Clear(this);

        this.lockTime += Time.deltaTime;

        moveTimer += Time.deltaTime;

        if(moveTimer >= moveDelay)
        {
            moveTimer = 0;
            if (!canMove)
            {
                canMove = true;
            }
        }

        if (Input.GetButtonDown("Rotate"))
        {
            Rotate(1);
        }
        
        
        horizontalInt = (int)(Input.GetAxis("Horizontal"));
        verticalInt = (int)(Input.GetAxis("Vertical"));

        if (canMove && horizontalInt != 0 || canMove && verticalInt != 0)
        {
            Move(new Vector2Int(horizontalInt, verticalInt));

        }
            
        if (Input.GetButtonDown("Drop"))
        {
            HardDrop();
        }

        if(Time.time >= stepTime) //� chamado a cada x segundos, time.time � o tempo atual 
        {
            Step();
        }

        this.board.Set(this);

        if (disableThisPiece && !alreadyDisabled)
        {
            this.board.Clear(this);
            alreadyDisabled = true;
        }
    }

    private void Step()
    {
        this.stepTime = Time.time + stepDelay;

        Move(Vector2Int.down);

        if(this.lockTime >= this.lockDelay) //se o movimento acima for inv�lido, ele ir� chamar o lock
        {
            Lock() ;
        }
    }

    public void EnableMovement()
    {
        disableThisPiece = false;
        alreadyDisabled = false;
        canBeControlled = true;
    }
    public void DisableMovement()
    {
        disableThisPiece = true;
        canBeControlled = false;
    }

    private void Lock()
    {
        this.board.Set(this); //�ltimo set antes da morte  
        this.board.ClearLines(); //toda vez que uma pe�a for posicionada
        this.board.UpdateLinesForFoodSpawn();
        this.board.SpawnPiece(this.board.nextPiece.data);
        this.board.SetNextPiece();
        tetrisLostSignal.Raise();
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
        canMove = false;
        moveTimer = 0;

        Vector3Int newPosition = this.position;
        
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = board.IsValidPosition(this, newPosition);

        if (valid)
        {
            this.position = newPosition;
            this.lockTime = 0; //aplicar aqui j� aplica no rotation por causa dos testes de wallkick
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
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i]; //pega a pr�pria c�lula

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
