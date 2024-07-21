using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : CreatureController
{
    #region Methods

    #region Events

    public override void OnDamaged()
    {
        GameObject effect = Managers.Resource.Instantiate("Effect/DieEffect");
        effect.transform.position = transform.position + new Vector3(0f, 0.2f, 0f);
        GameObject.Destroy(effect, 0.5f);

        Debug.Log(gameObject.name);
        Managers.Obj.Remove(gameObject);
        Managers.Resource.Destory(gameObject);
    }

    #endregion Events

    #endregion Methods
}
