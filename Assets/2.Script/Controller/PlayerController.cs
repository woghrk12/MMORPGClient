using UnityEngine;

public class PlayerController : CreatureController
{
    #region Methods

    /*
    protected IEnumerator StartSkillAttack()
    {
        State = ECreatureState.SKILL;

        GameObject go = Managers.Resource.Instantiate("Creature/BigSwordHero3_Skill1");
        ProjectileController controller = go.GetComponent<ProjectileController>();
        controller.SetProjectile(LastMoveDirection);
        controller.CellPos = CellPos;

        yield return new WaitForSeconds(1f);

        State = ECreatureState.IDLE;

        coSkill = null;
    }*/

    #region Events

    public override void OnDamaged()
    {
        Debug.Log("OnDamaged");
    }

    #endregion Events

    #endregion Methods
}
