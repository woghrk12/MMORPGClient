using UnityEngine;

public class GameScene : BaseScene
{
    #region Methods

    protected override void Init()
    {
        base.Init();

        SceneType = EScene.GAME;

        /*
        for (int i = 0; i < 1; i++)
        {
            GameObject monster = Managers.Resource.Instantiate("Object/Monster");
            monster.name = $"Monster_{i}";

            Vector3Int pos = new Vector3Int() { x = 0, y = 5 };
            MonsterController controller = monster.GetComponent<MonsterController>();
            controller.CellPos = pos;

            controller.ID = 10;
            Managers.Obj.Add(10, monster);
        }
        */

        Managers.Map.LoadMap(1);
    }

    public override void Clear()
    {

    }

    #endregion Methods
}
