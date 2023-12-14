using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public TetrominoData[] tetrominoes;
    public Piece activePiece { get; private set; }
    public NewPiece nextPiece { get; private set; }
    public SavedPiece savedPiece { get; private set; }
    public Vector3Int spawnPosition;
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

    private bool isPaused;


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
        //SetNextPiece();
        
    }

    private void Update()
    {

        if (isPaused) { return; }


        if (Input.GetButtonDown("Save") && canSave)
        {
            Swap();
        }
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
            if (ColorUtility.TryParseHtmlString(hexColor, out textColor))
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
            if (ColorUtility.TryParseHtmlString(hexColor, out textColor))
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
}

