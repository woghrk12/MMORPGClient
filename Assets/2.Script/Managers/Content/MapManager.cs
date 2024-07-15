using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

        int width = MaxX - MinX + 1;
        int height = MaxY - MinY + 1;

        isBlocked = new bool[height, width];
        for (int y = 0; y < height; y++)
        {
            string line = reader.ReadLine();
            for (int x = 0; x < width; x++)
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
}
