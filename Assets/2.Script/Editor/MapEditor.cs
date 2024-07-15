using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapEditor
{
#if UNITY_EDITOR

    [MenuItem("Tools/GenerateMap %#g")]
    private static void GenerateMap()
    {
        GameObject[] gameObjects = Resources.LoadAll<GameObject>("Prefabs/Map");

        foreach (GameObject go in gameObjects)
        {
            if (Utility.FindChild(go, out Tilemap tilemap, "Tilemap_Collision", true) == false) return;

            using (var writer = File.CreateText($"Assets/Resources/Data/Map/{go.name}.txt"))
            {
                int xMin = tilemap.cellBounds.xMin;
                int xMax = tilemap.cellBounds.xMax;
                int yMin = tilemap.cellBounds.yMin;
                int yMax = tilemap.cellBounds.yMax;

                writer.WriteLine(xMin);
                writer.WriteLine(xMax);
                writer.WriteLine(yMin);
                writer.WriteLine(yMax);

                for (int y = yMax; y >= yMin; y--)
                {
                    for (int x = xMin; x <= xMax; x++)
                    {
                        TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));

                        writer.Write(ReferenceEquals(tile, null) == true ? "0" : "1");
                    }

                    writer.WriteLine();
                }
            }
        }

        Debug.Log("Map data generation complete.");
    }

#endif
}
