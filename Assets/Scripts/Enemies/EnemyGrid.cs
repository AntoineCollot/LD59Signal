using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrid : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    [SerializeField] Vector2 arenaSize;
    float CellSizeX => arenaSize.x / gridSize.x;
    float CellSizeY => arenaSize.y / gridSize.y;
    Vector2 CellSize => new Vector3(CellSizeX, CellSizeY);

    public HashSet<EnemyMovement> enemies { get; private set; }
    public List<EnemyMovement>[,] grid;

    public static EnemyGrid Instance;

    private void Awake()
    {
        Instance = this;
        InitGrid();
        enemies = new();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGrid();
    }

    public void Register(EnemyMovement enemy)
    {
        enemies.Add(enemy);
    }

    public void Deregister(EnemyMovement enemy)
    {
        enemies.Remove(enemy);
    }

    void UpdateGrid()
    {
        ClearGrid();
        FillGrid();
    }

    void InitGrid()
    {
        grid = new List<EnemyMovement>[gridSize.x, gridSize.y];
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = new();
            }
        }
    }

    void ClearGrid()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y].Clear();
            }
        }
    }

    void FillGrid()
    {
        int x, y;
        foreach (EnemyMovement enemy in enemies)
        {
            if (TryGetCellCoords(enemy.Position2D, out x, out y))
                grid[x, y].Add(enemy);
        }
    }

    Vector3 GetCellCenterPos(int x, int y)
    {
        return new Vector3((x - gridSize.x * 0.5f + 0.5f) * CellSizeX, 0, (y - gridSize.y * 0.5f + 0.5f) * CellSizeY);
    }

    bool TryGetCellCoords(Vector2 pos, out int x, out int y)
    {
        pos.x += arenaSize.x * 0.5f;
        pos.y += arenaSize.y * 0.5f;

        x = Mathf.FloorToInt(pos.x / arenaSize.x * gridSize.x);
        y = Mathf.FloorToInt(pos.y / arenaSize.y * gridSize.y);

        return IsInGrid(x, y);
    }

    bool IsInGrid(int x, int y)
    {
        return x >= 0 && x < gridSize.x && y >= 0 && y < gridSize.y;
    }

    public List<EnemyMovement> GetEnemiesAround(Vector2 pos)
    {
        List<EnemyMovement> enemies = new List<EnemyMovement>();

        if (!TryGetCellCoords(pos, out int cellX, out int cellY))
            return enemies;

        for (int x = cellX - 1; x <= cellX + 1; x++)
        {
            for (int y = cellY - 1; y <= cellY + 1; y++)
            {
                if (!IsInGrid(x, y))
                    continue;

                enemies.AddRange(grid[x, y]);
            }
        }

        return enemies;
    }

#if UNITY_EDITOR
    [SerializeField] Transform testTargetDebug;
    private void OnDrawGizmosSelected()
    {
        Vector3 cellSize = new Vector3(CellSizeX, 0, CellSizeY);
        if (testTargetDebug != null)
        {
            bool isInGrid = TryGetCellCoords(testTargetDebug.position.ToVector2(), out int targetX, out int targetY);
            if (isInGrid)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.yellow;
            Gizmos.DrawCube(GetCellCenterPos(targetX, targetY), cellSize);
        }

        Gizmos.color = Color.cyan;
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Gizmos.DrawWireCube(GetCellCenterPos(x, y), cellSize);
            }
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(arenaSize.x, 0, arenaSize.y));
    }
#endif
}
