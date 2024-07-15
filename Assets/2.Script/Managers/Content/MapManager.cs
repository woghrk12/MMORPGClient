using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    #region Variables

    public Grid CurrentGrid { private set; get; } = null;

    #endregion Variables

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
    }

    public void DestroyMap()
    {
        GameObject map = GameObject.Find("Map");

        if (ReferenceEquals(map, null) == true) return;

        GameObject.Destroy(map);
        CurrentGrid = null;
    }

    #endregion Methods
}
