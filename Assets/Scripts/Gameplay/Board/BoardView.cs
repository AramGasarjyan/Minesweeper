using UnityEngine;
using Zenject;

public class BoardView : MonoBehaviour, IResetable
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Transform _tilesParentTransform;

    [Header("Visual Settings")]
    [SerializeField] private float _tileSpacing = 0.5f;
    [SerializeField] private float _paddingFromSides = 1f;

    private Board _board;
    private TileView.Factory _tileViewFactory;

    [Inject]
    public void Construct(Board board, TileView.Factory tileViewFactory)
    {
        _board = board;
        _tileViewFactory = tileViewFactory;
    }

    public void CreateBoardView()
    {
        SpawnTileViews();
        FitBoardInScreen();
    }

    public bool TryGetTileCoordinates(Vector2 worldPosition, out Vector2Int coordinates)
    {
        coordinates = default;

        float offsetI = (_board.Height - 1) / 2f;
        float offsetJ = (_board.Width - 1) / 2f;

        int i = Mathf.RoundToInt((worldPosition.y / _tileSpacing) + offsetI);
        int j = Mathf.RoundToInt((worldPosition.x / _tileSpacing) + offsetJ);

        if (i >= 0 && i < _board.Height && j >= 0 && j < _board.Width)
        {
            coordinates.y = i;
            coordinates.x = j;

            return true;
        }

        return false;
    }

    public void Reset()
    {
        foreach (Transform tileChild in _tilesParentTransform)
        {
            Destroy(tileChild.gameObject);
        }
    }

    // ------------

    private void SpawnTileViews()
    {
        for (int i = 0; i < _board.Height; i++)
        {
            for (int j = 0; j < _board.Width; j++)
            {
                TileData tileData = _board.GetTileByCoordinate(i, j);
                TileView spawnedTileView = _tileViewFactory.Create(tileData);

                spawnedTileView.transform.SetParent(_tilesParentTransform);
                spawnedTileView.transform.localPosition = CalculateTilePosition(i, j);
            }
        }
    }

    private Vector2 CalculateTilePosition(int i, int j)
    {
        float positionX = (j - (_board.Width - 1) / 2f) * _tileSpacing;
        float positionY = (i - (_board.Height - 1) / 2f) * _tileSpacing;

        return new Vector2(positionX, positionY);
    }

    private void FitBoardInScreen()
    {
        float boardWidthInWorldSpace = _board.Width * _tileSpacing;
        float boardHeightInWorldSpace = _board.Height * _tileSpacing;

        float targetWidth = (boardWidthInWorldSpace / _mainCamera.aspect) / 2f;
        float targetHeight = boardHeightInWorldSpace / 2f;

        _mainCamera.orthographicSize = Mathf.Max(targetWidth, targetHeight) + _paddingFromSides;
    }
}
