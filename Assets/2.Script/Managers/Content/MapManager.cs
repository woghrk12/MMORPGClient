using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct Pos
{
    public Pos(int x, int y) 
    {
        X = x;
        Y = y;
    }

    public int X;
    public int Y;
}

public struct PQNode : IComparable<PQNode>
{
    public int F;
    public int G;
    public int X;
    public int Y;

    public int CompareTo(PQNode other)
    {
        if (F == other.F) return 0;

        return F < other.F ? 1 : -1;
    }
}

public class MapManager
{
    #region Variables

    private bool[,] isBlocked = null;

    #endregion Variables

    #region Properties

    public Grid CurrentGrid { private set; get; } = null;

    public int MinX { private set; get; } = 0;
    public int MaxX { private set; get; } = 0;
    public int MinY { private set; get; } = 0;
    public int MaxY { private set; get; } = 0;

    public int Width => MaxX - MinX + 1;
    public int Height => MaxY - MinY + 1;

    #endregion Properties

    #region Methods

    public void LoadMap(int mapId)
    {
        DestroyMap();

        string mapName = "Map_" + mapId.ToString("000");
        GameObject go = Managers.Resource.Instantiate($"Map/{mapName}");

        if (Utility.FindChild(go, out GameObject collisionObj, "Tilemap_Collision", true) == true)
        {
            collisionObj.SetActive(false);
        }

        CurrentGrid = go.GetComponent<Grid>();

        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/Map/{mapName}");
        StringReader reader = new(textAsset.text);

        MinX = int.Parse(reader.ReadLine());
        MaxX = int.Parse(reader.ReadLine());
        MinY = int.Parse(reader.ReadLine());
        MaxY = int.Parse(reader.ReadLine());

        isBlocked = new bool[Height, Width];
        for (int y = 0; y < Height; y++)
        {
            string line = reader.ReadLine();
            for (int x = 0; x < Width; x++)
            {
                isBlocked[y, x] = (line[x] == '1' ? true : false);
            }
        }
    }

    public void DestroyMap()
    {
        GameObject map = GameObject.Find("Map");

        if (ReferenceEquals(map, null) == true) return;

        GameObject.Destroy(map);
        CurrentGrid = null;
    }

    public bool CheckCanMove(Vector3Int cellPos)
    {
        if (cellPos.x < MinX || cellPos.x > MaxX) return false;
        if (cellPos.y < MinY || cellPos.y > MaxY) return false;

        return isBlocked[MaxY - cellPos.y, cellPos.x - MinX] == false;
    }

    #endregion Methods

    #region A* PathFinding

    private int[] dx = new int[] { 1, -1, 0, 0 };
    private int[] dy = new int[] { 0, 0, -1, 1 };
    private int[] cost = new int[] { 10, 10, 10, 10 };

    public bool FindPath(Vector3Int startCellPos, Vector3Int destCellPos, out List<Vector3Int> path)
    {
        bool[,] closedArray = new bool[Height, Width];
        int[,] openArray = new int[Height, Width];
        Pos[,] parentArray = new Pos[Height, Width];
        PriorityQueue<PQNode> pq = new();

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                closedArray[y, x] = false;
                openArray[y, x] = Int32.MaxValue;
                parentArray[y, x] = new Pos(-1, -1);
            }
        }

        Pos start = ConvertCellToPos(startCellPos);
        Pos dest = ConvertCellToPos(destCellPos);

        openArray[start.Y, start.X] = Utility.CalculateDistance(start, dest);

        pq.Push(new PQNode() { F = openArray[start.Y, start.X], G = 0, X = start.X, Y = start.Y });

        while (pq.Count > 0)
        {
            PQNode node = pq.Pop();

            if (node.X == dest.X && node.Y == dest.Y) break;
            if (closedArray[node.Y, node.X] == true) continue;

            closedArray[node.Y, node.X] = true;

            for (int i = 0; i < dx.Length; i++)
            {
                Pos next = new Pos(node.X + dx[i], node.Y + dy[i]);

                if (next.X == dest.X && next.Y == dest.Y)
                {
                    pq.Push(new PQNode() { F = 0, G = 0, X = next.X, Y = next.Y });
                    parentArray[next.Y, next.X] = new Pos(node.X, node.Y);
                    break;
                }

                if (CheckCanMove(ConvertPosToCell(next)) == false) continue;
                if (closedArray[next.Y, next.X] == true) continue;

                int g = node.G + cost[i];
                int h = Utility.CalculateDistance(next, dest);

                if (openArray[next.Y, next.X] < g + h) continue;

                openArray[dest.Y, dest.X] = g + h;
                pq.Push(new PQNode() { F = g + h, G = g, X = next.X, Y = next.Y });
                parentArray[next.Y, next.X] = new Pos(node.X, node.Y);
            }
        }

        path = new List<Vector3Int>();
        int tx = dest.X;
        int ty = dest.Y;

        while (parentArray[ty, tx].X >= 0 && parentArray[ty, tx].Y >= 0)
        {
            path.Add(ConvertPosToCell(new Pos(tx, ty)));

            Pos parent = parentArray[ty, tx];
            tx = parent.X;
            ty = parent.Y;
        }

        if (tx != start.X || ty != start.Y) return false;

        path.Add(ConvertPosToCell(new Pos(tx, ty)));
        path.Reverse();

        return true;
    }

    private Pos ConvertCellToPos(Vector3Int cell)
    {
        return new Pos(cell.x - MinX, MaxY - cell.y);
    }

    private Vector3Int ConvertPosToCell(Pos pos)
    {
        return new Vector3Int(pos.X + MinX, MaxY - pos.Y, 0);
    }

    #endregion A* PathFinding
}
