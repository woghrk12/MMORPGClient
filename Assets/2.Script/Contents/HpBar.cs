using UnityEngine;

public class HpBar : MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform fillTransform = null;

    private Transform hpBarTransform = null;
    private Transform targetTransform = null;

    private float yOffset = 0f;

    private int maxHp = 1;
    private int curHp = 0;

    #endregion Variables

    #region Unity Events

    private void Update()
    {
        if (ReferenceEquals(targetTransform, null) == true) return;

        hpBarTransform.position = targetTransform.position + new Vector3(0f, yOffset, 0f);
    }

    #endregion Unity Events

    #region Methods

    public void InitHpBar(Transform objTransform, float yOffset)
    {
        if (ReferenceEquals(objTransform, null) == true) return;

        hpBarTransform = GetComponent<Transform>();

        targetTransform = objTransform;
        this.yOffset = yOffset;

        hpBarTransform.position = targetTransform.position + new Vector3(0f, this.yOffset, 0f);
    }

    public void SetMaxHp(int maxHp)
    {
        this.maxHp = maxHp;

        SetHpBar();
    }

    public void SetCurHp(int curHp)
    {
        this.curHp = curHp;

        SetHpBar();
    }

    private void SetHpBar()
    {
        float ratio = Mathf.Clamp01((float)curHp / maxHp);

        if (ratio <= 0f)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (gameObject.activeSelf == false)
            {
                gameObject.SetActive(true);
            }

            fillTransform.localScale = new Vector3(ratio, 1f, 1f);
        }
    }

    #endregion Methods
}
