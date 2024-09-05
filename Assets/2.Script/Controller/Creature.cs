using Google.Protobuf.Protocol;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    #region Variables

    private EMoveDirection facingDirection = EMoveDirection.None;

    #endregion Variables

    #region Properties

    public int ID { set; get; } = -1;

    public string Name { set; get; } = string.Empty;

    public Vector3Int CellPos { set; get; } = Vector3Int.zero;

    public EMoveDirection FacingDirection
    {
        set
        {
            if (facingDirection == value) return;

            facingDirection = value;

            if (facingDirection == EMoveDirection.Left)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            if (facingDirection == EMoveDirection.Right)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
        get => facingDirection;
    }

    public int MoveSpeed { set; get; } = 0;

    #endregion Properties
}
