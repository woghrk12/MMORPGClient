using Google.Protobuf.Protocol;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    #region Variables

    private EMoveDirection moveDirection = EMoveDirection.None;

    #endregion Variables

    #region Properties

    public int ID { set; get; } = -1;

    public string Name { set; get; } = string.Empty;

    public Vector3Int Position { set; get; } = Vector3Int.zero;

    public EMoveDirection MoveDirection
    { 
        set
        {
            if (moveDirection == value) return;

            moveDirection = value;

            if (moveDirection == EMoveDirection.Left)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            if (moveDirection == EMoveDirection.Right)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }

            if (moveDirection != EMoveDirection.None)
            {
                FacingDirection = moveDirection;
            }
        }
        get => moveDirection;
    }

    public EMoveDirection FacingDirection { private set; get; } = EMoveDirection.Right;

    public int MoveSpeed { set; get; } = 0;

    #endregion Properties
}
