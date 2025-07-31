using UnityEngine;
using Zenject;
using System;

public class TileData : IResetable
{
    public bool IsMine { get; private set; }
    public bool IsRevealed { get; private set; }
    public bool IsFlagged { get; private set; }
    public int NeighbourMinesCount { get; private set; }

    public readonly int I;
    public readonly int J;

    public event Action<TileData> OnTileStateChanged;

    public TileData(int coordinateI, int coordinateJ)
    {
        I = coordinateI;
        J = coordinateJ;
    }

    public void SetAsMine()
    {
        IsMine = true;
        NotifyStateChange();
    }

    public void SetNeighborMinesCount(int count)
    {
        NeighbourMinesCount = count;
        NotifyStateChange();
    }

    public void Reveal()
    {
        IsRevealed = true;
        NotifyStateChange();
    }

    public void ToggleFlag()
    {
        IsFlagged = !IsFlagged;
        NotifyStateChange();
    }

    private void NotifyStateChange()
    {
        OnTileStateChanged?.Invoke(this);
    }

    public void Reset()
    {
        IsMine = false;
        IsRevealed = false;
        IsFlagged = false;

        NeighbourMinesCount = 0;
        NotifyStateChange();
    }

    public class Factory : PlaceholderFactory<int, int, TileData> { }
}
