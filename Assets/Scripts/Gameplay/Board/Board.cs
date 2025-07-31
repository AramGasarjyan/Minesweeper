using System.Linq;
using System.Collections.Generic;

public class Board : IResetable
{
    public delegate void GameEndedDelegate(bool hasWon);


    public readonly int Width;
    public readonly int Height;
    public readonly int MinesCount;

    public event GameEndedDelegate OnGameEnded;

    private TileData[,] _tiles;
    private TileData.Factory _tilesFactory;


    private bool _isFirstClick = true;
    private int _revealedTilesCount = 0;

    public Board(int width, int height, int minesCount, TileData.Factory tilesFactory)
    {
        Width = width;
        Height = height;
        MinesCount = minesCount;

        _tilesFactory = tilesFactory; 

        CreateTiles();
    }

    public void RevealTile(int i, int j)
    {
        if (!IsInBounds(i, j)) return;

        TileData tileToReveal = _tiles[i, j];

        if (_isFirstClick)
        {
            InitializeTheBoard(i, j);
            _isFirstClick = false;
        }

        if (tileToReveal.IsMine)
        {
            tileToReveal.Reveal();
            EndGame(false);
            return;
        }

        RevealTile(tileToReveal);
        CheckWinCondition();
    }

    public void PlaceFlag(int i, int j)
    {
        if (!IsInBounds(i, j)) return;

        TileData tileToFlag = _tiles[i, j];
        if (tileToFlag.IsRevealed) return;

        tileToFlag.ToggleFlag();
    }

    public TileData GetTileByCoordinate(int i, int j)
    {
        return IsInBounds(i, j) ? _tiles[i, j] : null;
    }

    //-------------

    private void InitializeTheBoard(int userClickedI, int userClickedJ)
    {
        PlaceMines(userClickedI, userClickedJ);
        SetMineNeigbourTilesCount();
    }

    private void CreateTiles()
    {
        _tiles = new TileData[Width, Height];
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                _tiles[j, i] = _tilesFactory.Create(j, i);
            }
        }
    }

    private void PlaceMines(int exceptI, int exceptJ)
    {
        System.Random random = new System.Random();
        int tilesCount = Width * Height;
        int exceptIndex = exceptI * Width + exceptJ;

        List<int> possibleMineIndexes = Enumerable.Range(0, tilesCount).ToList();
        possibleMineIndexes.Remove(exceptIndex);

        IEnumerable<int> minesPositions = possibleMineIndexes.OrderBy(x => random.Next()).Take(MinesCount);

        foreach (var index in minesPositions)
        {
            int mineI = index / Width;
            int mineJ = index % Width;

            _tiles[mineI, mineJ].SetAsMine();
        }
    }

    private void SetMineNeigbourTilesCount()
    {
        TileData currTile;
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                currTile = _tiles[i, j];

                if (currTile.IsMine) continue;

                int count = CountMinesAround(currTile.I, currTile.J);
                currTile.SetNeighborMinesCount(count);
            }
        }

        int CountMinesAround(int i, int j)
        {
            int minesCount = 0;
            foreach (var item in GetTileNeighbours(_tiles[i, j]))
            {
                if (item.IsMine) minesCount++;
            }

            return minesCount;
        }
    }

    private IEnumerable<TileData> GetTileNeighbours(TileData tile)
    {
        for (int offsetI = -1; offsetI <= 1; offsetI++)
        {
            for (int offsetJ = -1; offsetJ <= 1; offsetJ++)
            {
                int checkI = tile.I + offsetI;
                int checkJ = tile.J + offsetJ;

                if (IsInBounds(checkI, checkJ))
                {
                    yield return _tiles[checkI, checkJ];
                }
            }
        }
    }

    private void RevealTile(TileData tile)
    {
        tile.Reveal();
        _revealedTilesCount++;

        if (tile.NeighbourMinesCount == 0)
        {
            ExpandReveal(tile);
        }
    }

    private void ExpandReveal(TileData startTile)
    {
        Queue<TileData> expandQueue = new Queue<TileData>();
        expandQueue.Enqueue(startTile);

        while (expandQueue.Count > 0)
        {
            TileData currentTile = expandQueue.Dequeue();
            foreach (var neighbour in GetTileNeighbours(currentTile))
            {
                if(!neighbour.IsRevealed && !neighbour.IsFlagged)
                {
                    neighbour.Reveal();
                    _revealedTilesCount++;

                    if (neighbour.NeighbourMinesCount == 0)
                    {
                        expandQueue.Enqueue(neighbour);
                    }
                }
            }
        }
    }

    private bool IsInBounds(int i, int j)
    {
        return i >= 0 && i < Height && j >= 0 && j < Width;
    }

    private void CheckWinCondition()
    {
        int nonMinedTilesCount = Width * Height - MinesCount;
        if (_revealedTilesCount == nonMinedTilesCount)
        {
            EndGame(true);
        }
    }

    private void EndGame(bool hasWon)
    {
        OnGameEnded?.Invoke(hasWon);
    }

    public void Reset()
    {
        _isFirstClick = true;
        _revealedTilesCount = 0;

        foreach (var tile in _tiles)
        {
            tile.Reset();
        }
    }
}
