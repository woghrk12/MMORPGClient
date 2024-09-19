using Google.Protobuf.Protocol;
using System.Collections;
using UnityEngine;

namespace LocalPlayerState
{
    public class IdleState : State
    {
        #region Variables

        private Coroutine coCooldownInput = null;

        #endregion Variables

        #region Properties

        public sealed override ECreatureState StateID => ECreatureState.Idle;

        #endregion Properties

        #region Methods

        public override void OnUpdate(EPlayerInput input)
        {
            EPlayerInput attackInput = input & (EPlayerInput.ATTACK | EPlayerInput.SKILL);

            if (ReferenceEquals(coCooldownInput, null) == true && attackInput != EPlayerInput.NONE)
            {
                PerformAttackRequest packet = new()
                {
                    AttackInfo = new AttackInfo() { AttackID = 1 }
                };

                Managers.Network.Send(packet);

                coCooldownInput = StartCoroutine(CooldownInputCo(0.2f));
            }

            EPlayerInput directionInput = input & (EPlayerInput.UP | EPlayerInput.DOWN | EPlayerInput.LEFT | EPlayerInput.RIGHT);

            if (directionInput != EPlayerInput.NONE)
            {
                controller.SetState(ECreatureState.Move, directionInput);
                return;
            }
        }

        private IEnumerator CooldownInputCo(float time)
        {
            yield return new WaitForSeconds(time);

            coCooldownInput = null;
        }

        #endregion Methods
    }
}