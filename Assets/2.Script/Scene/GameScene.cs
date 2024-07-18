using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    #region Methods

    protected override void Init()
    {
        base.Init();

        SceneType = EScene.GAME;

        GameObject player = Managers.Resource.Instantiate("Creature/Player");
        player.name = "Player";
        Managers.Obj.Add(player);

        for (int i = 0; i < 5; i++)
        {
            GameObject monster = Managers.Resource.Instantiate("Creature/Monster");
            monster.name = $"Monster_{i}";

            Vector3Int pos = new Vector3Int() { x = Random.Range(-10, 10), y = Random.Range(-10, 10) };
            MonsterController controller = monster.GetComponent<MonsterController>();
            controller.CellPos = pos;

            Managers.Obj.Add(monster);
        }

        Managers.Map.LoadMap(1);
    }

    public override void Clear()
    {

    }

    #endregion Methods
}
