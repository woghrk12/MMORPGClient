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

        Vector3Int cellPos = ConvertPosToCell(obj.Position);
        collision[cellPos.y, cellPos.x].Add(obj.ID, obj);
    }

    public void Removeobject(MMORPG.Object obj)
    {
        if (ReferenceEquals(obj, null) == true) return;

        Vector3Int cellPos = ConvertPosToCell(obj.Position);
        collision[cellPos.y, cellPos.x].Remove(obj.ID);
    }

    public void MoveObject(int objectID, Vector3Int curPos, Vector3Int targetPos)
    {
        Vector3Int curCellPos = ConvertPosToCell(curPos);
        Vector3Int targetCellPos = ConvertPosToCell(targetPos);

        if (collision[curCellPos.y, curCellPos.x].TryGetValue(objectID, out MMORPG.Object obj) == false) return;

        collision[curCellPos.y, curCellPos.x].Remove(objectID);
        collision[targetCellPos.y, targetCellPos.x].Add(objectID, obj);
    }

    public bool CheckCanMove(Vector3Int position, bool isIgnoreObject = false)
    {
        if (position.x < minX || position.x > maxX) return false;
        if (position.y < minY || position.y > maxY) return false;

        int cellPosX = position.x - minX;
        int cellPosY = position.y - minY;

        if (collision[cellPosY, cellPosX].ContainsKey(-1) == true) return false;
        if (isIgnoreObject == false && collision[cellPosY, cellPosX].Count > 0) return false;

        return true;
    }

    private Vector3Int ConvertPosToCell(Vector3Int Pos) => new Vector3Int(Pos.x - minX, Pos.y - minY, 0);

    #endregion Methods
}
