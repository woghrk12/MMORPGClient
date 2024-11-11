using Google.Protobuf.Protocol;
using System.Collections;
using UnityEngine;

namespace LocalCharacterState
{
    public class IdleState : CreatureState.BaseState<LocalCharacter>
    {
        #region Variables

        private Coroutine coCooldownInput = null;

        #endregion Variables

        #region Properties

        public sealed override ECreatureState StateID => ECreatureState.Idle;

        #endregion Properties

        #region Methods

        public override void OnUpdate()
        {
            if (ReferenceEquals(coCooldownInput, null) == false) return;

            EPlayerInput input = controller.PlayerInput;
            EPlayerInput attackInput = input & (EPlayerInput.ATTACK | EPlayerInput.SKILL);

            if (attackInput != EPlayerInput.NONE)
            {
                PerformAttackRequest packet = new()
                {
                    AttackID = attackInput == EPlayerInput.ATTACK ? 1 : 2
                };

                Managers.Network.Send(packet);

                coCooldownInput = StartCoroutine(CooldownInputCo(0.2f));
                return;
            }

            EPlayerInput directionInput = input & (EPlayerInput.UP | EPlayerInput.DOWN | EPlayerInput.LEFT | EPlayerInput.RIGHT);
            
            if (directionInput != EPlayerInput.NONE)
            {
                controller.CurState = ECreatureState.Move;
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