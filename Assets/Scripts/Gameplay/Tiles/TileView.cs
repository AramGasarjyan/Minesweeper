using TMPro;
using UnityEngine;
using Zenject;

public class TileView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshPro _minesCountText;

    private TileData _tileData;
    private TileViewSettings _viewSettings;

    [Inject]

    public void Construct(TileData tileData, TileViewSettings viewSettings)
    {
        _tileData = tileData;
        _viewSettings = viewSettings;

        _tileData.OnTileStateChanged += HandleTileStateChanged;
        UpdateView();
    }

    private void OnDestroy()
    {
        _tileData.OnTileStateChanged -= HandleTileStateChanged;
    }

    private void HandleTileStateChanged(TileData data)
    {
        UpdateView();
    }

    private void UpdateView()
    {
        if(_tileData.IsRevealed)
        {
            _spriteRenderer.sprite = _tileData.IsMine ? _viewSettings.MinedSprite : _viewSettings.RevealedSprite;

            if (!_tileData.IsMine && _tileData.NeighbourMinesCount > 0)
            {
                _minesCountText.text = _tileData.NeighbourMinesCount.ToString();
                _minesCountText.gameObject.SetActive(true);
            }
            else _minesCountText.text = string.Empty;

            _minesCountText.gameObject.SetActive(true);
        }
        else
        {
            _minesCountText.gameObject.SetActive(false);
            _spriteRenderer.sprite = _tileData.IsFlagged ? _viewSettings.FlaggedSprite : _viewSettings.DefaultSprite;
        }
    }

    public class Factory : PlaceholderFactory<TileData, TileView> { }
}
