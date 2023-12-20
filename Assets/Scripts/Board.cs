using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public TetrominoData[] tetrominoes;
    public Piece activePiece { get; private set; }
    public NewPiece nextPiece { get; private set; }
    public SavedPiece savedPiece { get; private set; }
    public Vector3Int spawnPosition;
    public NewPiece[] tempPiece;
    public Vector3Int[] tempPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector3Int previewPosition;
    public Vector3Int holdPosition;
    public bool canSave;

    [SerializeField]
    private int upScore;

    [SerializeField]
    private AudioClip cleanLineSFX;

    [SerializeField]
    private AudioClip cleanTetrisSFX;

    [SerializeField]
    private AudioClip dropPieceSFX;

    [SerializeField]
    private AudioClip gameOverSFX;

    [SerializeField]
    private Signal gameOverSignal;

    [SerializeField]
    private Signal pauseSignal;

    [SerializeField]
    private Tile filledTile;

    private bool isPaused;

    private bool choosingRandomPiece;


    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2); //o in�cio � 0
            return new RectInt(position, boardSize);
        }
    }

    private void Awake()
    {
        canSave = true;

        nextPiece = gameObject.AddComponent<NewPiece>();

        tempPiece = new NewPiece[4];

        for (int i = 0; i < tempPosition.Length; i++)
        {
            tempPiece[i] = gameObject.AddComponent<NewPiece>();
        }

        savedPiece = gameObject.AddComponent<SavedPiece>();

        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponent<Piece>();

        for (int i = 0; i < tetrominoes.Length; i++)
        { 
            tetrominoes[i].Initialize();
        }
    }

    public void ChangePauseState()
    {
        isPaused = !isPaused;
    }

    private void Start()
    {
        SetNextPiece();
        SpawnPiece(nextPiece.data);
        
    }

    private void Update()
    {

        if (isPaused) { return; }

        if (choosingRandomPiece)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                SpawnPiece(tempPiece[0].data);
                OnNewRandomPieceSelected();   
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SpawnPiece(tempPiece[1].data);
                OnNewRandomPieceSelected();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SpawnPiece(tempPiece[2].data);
                OnNewRandomPieceSelected();

            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SpawnPiece(tempPiece[3].data);
                OnNewRandomPieceSelected();
            }

           
        }

        //For debug purposes:
        if (Input.GetKeyDown(KeyCode.I))
        {
            UpLines();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateRandomPieces();
        }



        /*
        if (Input.GetButtonDown("Save") && canSave)
        {
            Swap();
        }*/
    }

    public void OnNewRandomPieceSelected()
    {
        CleanRandomPieces();
        choosingRandomPiece = false;
        GetComponent<Piece>().EnableMovement();
    }

    public void ChangeToRandomChoose()
    {
        choosingRandomPiece = true;
        GenerateRandomPieces();
    }

    private void Swap()
    {  
        Clear(activePiece);
        SetSavedPiece();
    }

    private void SetSavedPiece()
    {
        TetrominoData preData = activePiece.data;

        if (savedPiece.cells != null)
        {
            for (int i = 0; i < savedPiece.cells.Length; i++)
            {
                Vector3Int tilePosition = savedPiece.cells[i] + savedPiece.position;
                tilemap.SetTile(tilePosition, null);
            }
        }

        if (savedPiece.saved)
        {
            SpawnPiece(savedPiece.data);
        }
        else
        {
            SpawnPiece(nextPiece.data);
        }
            
        savedPiece.Initialize(this, holdPosition, preData);

        //set da next piece:
        for (int i = 0; i < savedPiece.cells.Length; i++)
        {
            Vector3Int tilePosition = savedPiece.cells[i] + savedPiece.position;
            tilemap.SetTile(tilePosition, savedPiece.data.tile);
        }

        canSave = false;
    }

    public void GenerateRandomPieces()
    {
        //clean the old pieces
        CleanRandomPieces();

        //set the new pieces
        for (int j = 0; j<tempPosition.Length; j++)
        {
            int random = Random.Range(0, tetrominoes.Length);
            TetrominoData data = tetrominoes[random];
            tempPiece[j].Initialize(this, tempPosition[j], data);

            for (int i = 0; i < tempPiece[j].cells.Length; i++)
            {
                Vector3Int tilePosition = tempPiece[j].cells[i] + tempPiece[j].position;
                tilemap.SetTile(tilePosition, tempPiece[j].data.tile);
            }
        }
        
    }

    public void CleanRandomPieces()
    {
        for (int j = 0; j < tempPosition.Length; j++)
        {
            for (int i = 0; i < tempPiece[j].cells.Length; i++)
            {
                Vector3Int tilePosition = tempPiece[j].cells[i] + tempPiece[j].position;
                tilemap.SetTile(tilePosition, null);
            }
        }
    }

    public void SpawnPiece(TetrominoData data)
    {
        //int random = Random.Range(0, this.tetrominoes.Length);

        //TetrominoData data = this.tetrominoes[random];

        activePiece.Initialize(this, spawnPosition, data);

        if(IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
        }
        else
        {
            GameOver();
        }

        canSave = true;
        
    }

    public void SpawnNextPiece( )
    {
        //int random = Random.Range(0, this.tetrominoes.Length);

        //TetrominoData data = this.tetrominoes[random];

        activePiece.Initialize(this, spawnPosition, nextPiece.data);

        if(IsValidPosition(activePiece, spawnPosition)) //first we are going to change to a backup position and then move to the original position
        {
            Set(activePiece);
        }
        else
        {
            GameOver();
        }

        canSave = true;
        
    }

    public void SetNextPiece()
    {
        SoundManager.instance.PlaySound(dropPieceSFX);

        //clear da next piece:
        if (nextPiece.cells != null)
        {
            for (int i = 0; i < nextPiece.cells.Length; i++)
            {
                Vector3Int tilePosition = nextPiece.cells[i] + nextPiece.position;
                tilemap.SetTile(tilePosition, null);
            }
        }

        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];
        nextPiece.Initialize(this, previewPosition, data);

        //set da next piece:
        for (int i = 0; i < nextPiece.cells.Length; i++)
        {
            Vector3Int tilePosition = nextPiece.cells[i] + nextPiece.position;
            tilemap.SetTile(tilePosition, nextPiece.data.tile);
        }

    }

    private void GameOver()
    {
        this.tilemap.ClearAllTiles();

        

        SoundManager.instance.PlaySound(gameOverSFX);

        SoundManager.instance.PlaySadBgSong();

        gameOverSignal.Raise();

        pauseSignal.Raise();

        UIManager uiMan = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIManager>();

        if (ScoreManager.instance.GetScore() > JsonReadWriteSystem.INSTANCE.playerData.MaxScore)
        {
            JsonReadWriteSystem.INSTANCE.playerData.MaxScore = ScoreManager.instance.GetScore();
            
            uiMan.resultsTxt.text = "New high score: " + ScoreManager.instance.GetScore().ToString() + ", congratulations!";
            string hexColor = "#5D9300";
            Color textColor;
            if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out textColor))
            {
                // Assign the color to the text component
                uiMan.resultsTxt.color = textColor;
            }
            else
            {
                Debug.LogError("Invalid hexadecimal color code: " + hexColor);
            }
        }
        else
        {
            uiMan.resultsTxt.text = "You didn't get a new high score :(. Your best was: " + JsonReadWriteSystem.INSTANCE.playerData.MaxScore.ToString();
            string hexColor = "#EC483C";
            Color textColor;
            if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out textColor))
            {
                // Assign the color to the text component
                uiMan.resultsTxt.color = textColor;
            }
            else
            {
                Debug.LogError("Invalid hexadecimal color code: " + hexColor);
            }
        }

        JsonReadWriteSystem.INSTANCE.Save();
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            //piece.cells = posi��o padr�o
            //piece.cells[i] + piece.positon = posi��o global
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            //piece.cells = posi��o padr�o
            //piece.cells[i] + piece.positon = posi��o global
            tilemap.SetTile(tilePosition, null);
        }
    }
    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            if (this.tilemap.HasTile(tilePosition))
            {
                return false;
            }

        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row;
        int rowsCleaned = 0;

        row = bounds.yMin;

        while (row < bounds.yMax)
        {
            if (isLineFull(row))
            {
                rowsCleaned++;
                
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
        if(rowsCleaned > 0)
        {
            if (rowsCleaned < 4)
            {

                ScoreManager.instance.UpdateScore(upScore * rowsCleaned);
                SoundManager.instance.PlaySound(cleanLineSFX);
            }
            else
            {
                ScoreManager.instance.UpdateScore(upScore * 6);
                SoundManager.instance.PlaySound(cleanTetrisSFX);
            }
        }
        
    }

    private bool isLineFull(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(position))
            {
                return false;
            }

            //there is no need to check if it is a ghost or not because we are going randomize the empty column
        }

        return true;
    }

    private void LineClear(int row)
    {

        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            this.tilemap.SetTile(position, null); //limpa cada coordenada
        }

        while(row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);

                TileBase above = this.tilemap.GetTile(position); //rouba a c�lula de cima

                position = new Vector3Int(col, row, 0);

                this.tilemap.SetTile(position, above); //coloca a de cima na atual
            }

            row++;
        }
    }

    public void UpLines()
    {
        RectInt bounds = this.Bounds;

        int row = bounds.yMax; //last line

        int random = Random.Range(0, bounds.xMax); //choose a random column

        //move all the lines one line above
        while (row > bounds.yMin - 1)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row - 1, 0);

                TileBase below = this.tilemap.GetTile(position); //get below cell

                position = new Vector3Int(col, row, 0);

                this.tilemap.SetTile(position, below); //switches
            }

            row--;
        }

        //turn the first row into occupied

        row = bounds.yMin; //last line

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {

            if(col != random)
            {
                Vector3Int position = new Vector3Int(col, row, 0);

                this.tilemap.SetTile(position, filledTile);
            }
            
        }

    }
}

