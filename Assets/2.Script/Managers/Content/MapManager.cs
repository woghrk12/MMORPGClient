using System.IO;
using UnityEngine;

public class MapManager
{
    #region Variables

    private GameObject mapObject = null;

    private bool[,] isblocked = null;
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

        isblocked = new bool[height, width];
        for (int y = 0; y < height; y++)
        {
            string line = reader.ReadLine();
            for (int x = 0; x < width; x++)
            {
                isblocked[y, x] = line[x] == '1' ? true : false;
            }
        }
    }

    public void DestroyMap()
    {
        if (ReferenceEquals(mapObject, null) == true) return;

        GameObject.Destroy(mapObject);
    }

    public bool CheckCanMove(Vector3Int targetCellPos)
    {
        return isblocked[targetCellPos.y, targetCellPos.x] == false;
    }

    #endregion Methods
}
