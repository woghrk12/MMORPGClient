using System.Collections;
using UnityEngine;

namespace Monster
{
    public class IdleState : Creature.IdleState<MonsterController>
    {
        #region Variables

        private Coroutine coPatrol = null;

        #endregion Variables

        #region Methods

        private IEnumerator PatrolCo()
        {
            yield return new WaitForSeconds(Random.Range(1, 4));
            
            for (int i = 0; i < 10; i++)
            {
                Vector3Int randPos = controller.CellPos + new Vector3Int(Random.Range(-5, 5), Random.Range(-5, 5), 0);

                if (Managers.Map.CheckCanMove(randPos) == false) continue;
                if (ReferenceEquals(Managers.Obj.Find(randPos), null) == false) continue;

                controller.DestCellPos = randPos;
                controller.SetState(ECreatureState.MOVE);

                yield break;
            }
        }

        #region Events

        public override void OnStart()
        {
            coPatrol = null;
        }

        public override void OnUpdate()
        {
            if (ReferenceEquals(coPatrol, null) == false) return;

            coPatrol = StartCoroutine(PatrolCo());
        }

        public override void OnExit()
        {
            if (ReferenceEquals(coPatrol, null) == true) return;
            
            StopCoroutine(coPatrol);
            coPatrol = null;
        }

        #endregion Events

        #endregion Methods
    }
}
