using UnityEngine;

public class MapManager
{
    #region Variables

    private GameObject mapObject = null;

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
    }

    public void DestroyMap()
    {
        if (ReferenceEquals(mapObject, null) == true) return;

        GameObject.Destroy(mapObject);
    }

    #endregion Methods
}
