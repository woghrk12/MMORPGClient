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

        Managers.Map.LoadMap(1);
    }

    public override void Clear()
    {

    }

    #endregion Methods
}
