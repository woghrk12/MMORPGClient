using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager
{
    #region Variables

    private GameObject mapObject = null;

    private Dictionary<int, MMORPG.Object>[,] collision = null;
    private int minX = 0;
    private int maxX = 0;
    private int minY = 0;
    private int maxY = 0;
    private int width = 0;
    private int height = 0;

    #endregion Variables

    #region Methods

    public void LoadMap(int mapId)
    {
        DestroyMap();

        string mapName = "Map_" + mapId.ToString("000");
        mapObject = Managers.Resource.Instantiate($"Map/{mapName}");
         
        if (Utility.FindChild(mapObject, out GameObject collisionObj, "Tilemap_Collision", true) == true)
        {
            collisionObj.SetActive(false);
        }

        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/Map/{mapName}");
        StringReader reader = new(textAsset.text);

        minX = int.Parse(reader.ReadLine());
        maxX = int.Parse(reader.ReadLine());
        minY = int.Parse(reader.ReadLine());
        maxY = int.Parse(reader.ReadLine());

        width = maxX - minX + 1;
        height = maxY - minY + 1;

        collision = new Dictionary<int, MMORPG.Object>[height, width];
        for (int y = height - 1; y >= 0; y--)
        {
            string line = reader.ReadLine();
            for (int x = 0; x < width; x++)
            {
                collision[y, x] = new Dictionary<int, MMORPG.Object>();

                if (line[x] == '1')
                {
                    collision[y, x].Add(-1, null);
                }
            }
        }
    }

    public void DestroyMap()
    {
        if (ReferenceEquals(mapObject, null) == true) return;

        GameObject.Destroy(mapObject);
    }

    public void AddObject(MMORPG.Object obj)
    {
        if (ReferenceEquals(obj, null) == true) return;

        Vector2Int cellPos = ConvertPosToCell(obj.Position);
        collision[cellPos.y, cellPos.x].Add(obj.ID, obj);
    }

    public void RemoveObject(MMORPG.Object obj)
    {
        if (ReferenceEquals(obj, null) == true) return;

        Vector2Int cellPos = ConvertPosToCell(obj.Position);
        collision[cellPos.y, cellPos.x].Remove(obj.ID);
    }

    public void MoveObject(MMORPG.Object obj, Vector2Int targetPos)
    {
        if (ReferenceEquals(obj, null) == true) return;
        if (obj.Position == targetPos) return;

        Vector2Int curCellPos = ConvertPosToCell(obj.Position);
        Vector2Int targetCellPos = ConvertPosToCell(targetPos);

        collision[curCellPos.y, curCellPos.x].Remove(obj.ID);
        collision[targetCellPos.y, targetCellPos.x].Add(obj.ID, obj);

        obj.Position = targetPos;
    }

    public bool CheckCanMove(Vector2Int position, bool isIgnoreWall = false, bool isIgnoreObject = false)
    {
        if (position.x < minX || position.x > maxX || position.y < minY || position.y > maxY) return false;

        Vector2Int cellPos = ConvertPosToCell(position);

        foreach (KeyValuePair<int, MMORPG.Object> pair in collision[cellPos.y, cellPos.x])
        {
            if (pair.Key == -1 && isIgnoreWall == false) return false;
            if (pair.Value.IsCollidable == true && isIgnoreObject == false) return false;
        }
        return true;
    }

    private Vector2Int ConvertPosToCell(Vector2Int Pos) => new Vector2Int(Pos.x - minX, Pos.y - minY);

    #endregion Methods
}
