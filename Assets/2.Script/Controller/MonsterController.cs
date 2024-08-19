using System.Collections;
using UnityEngine;

public class MonsterController : CreatureController
{
    #region Variables

    [SerializeField] private int searchRange = 5;
    [SerializeField] private int attackRange = 1;

    #endregion Variables

    #region Properties

    public Vector3Int DestCellPos { set; get; } = Vector3Int.zero;

    public PlayerController Target { set; get; } = null;

    public int AttackRange => attackRange;

    #endregion Properties

    #region Unity Events

    protected override void Start()
    {
        base.Start();

        StartCoroutine(SearchCo());
    }

    #endregion Unity Events

    #region Methods

    private IEnumerator SearchCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            // If the target player object is null
            if (ReferenceEquals(Target, null) == true)
            {
                // Find a player object within range
                Target = Managers.Obj.Find((go) =>
                {
                    if (go.TryGetComponent(out PlayerController controller) == false) return false;
                    if ((controller.CellPos - CellPos).sqrMagnitude > searchRange * searchRange) return false;

                    return true;
                })?.GetComponent<PlayerController>();

                // Nevertheless, if the object does not exist, proceed to the next loop
                if (ReferenceEquals(Target, null) == true) continue;
            }
            else
            {
                if ((Target.CellPos - CellPos).sqrMagnitude < searchRange * searchRange) continue;

                Target = null;
            }
        }
    }

    #region Events

    public override void OnDamaged()
    {
        GameObject effect = Managers.Resource.Instantiate("Effect/DieEffect");
        effect.transform.position = transform.position + new Vector3(0f, 0.2f, 0f);
        GameObject.Destroy(effect, 0.5f);

        Debug.Log(gameObject.name);
        Managers.Obj.Remove(ID);
        Managers.Resource.Destory(gameObject);
    }

    #endregion Events

    #endregion Methods
}
