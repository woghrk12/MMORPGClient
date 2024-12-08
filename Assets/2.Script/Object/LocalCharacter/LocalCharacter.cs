using Google.Protobuf.Protocol;
using UnityEngine;

public enum EPlayerInput
{
    NONE = 0,
    UP = 1 << 0,
    DOWN = 1 << 1,
    LEFT = 1 << 2,
    RIGHT = 1 << 3,
    ATTACK = 1 << 4,
    SKILL = 1 << 5,
}

public interface IInputHandler
{
    public void HandleInput(EPlayerInput input);
}

public class LocalCharacter : Character
{
    #region Variables

    private Camera mainCamera = null;

    #endregion Variables

    #region Properties

    public EPlayerInput PlayerInput { protected set; get; } = EPlayerInput.NONE;

    #endregion Properties

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        mainCamera = Camera.main;
    }

    protected override void Update()
    {
        PlayerInput = GetInput();

        mainCamera.transform.position = new Vector3(CachedTransform.position.x, CachedTransform.position.y, -10f);

        base.Update();
    }

    #endregion Unity Events

    #region Methods

    public override void Init(ObjectInfo info)
    {
        base.Init(info);

        AddState(ECreatureState.Idle, new LocalCharacterIdleState(this));
        AddState(ECreatureState.Move, new LocalCharacterMoveState(this));

        CreatureDead += () => Managers.Resource.Instantiate("UI/DeadUI");
    }

    private EPlayerInput GetInput()
    {
        EPlayerInput input = EPlayerInput.NONE;

        if (Input.GetKey(KeyCode.W))
        {
            input |= EPlayerInput.UP;
        }
        if (Input.GetKey(KeyCode.S))
        {
            input |= EPlayerInput.DOWN;
        }
        if (Input.GetKey(KeyCode.A))
        {
            input |= EPlayerInput.LEFT;
        }
        if (Input.GetKey(KeyCode.D))
        {
            input |= EPlayerInput.RIGHT;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            input |= EPlayerInput.ATTACK;
        }
        if (Input.GetKey(KeyCode.R))
        {
            input |= EPlayerInput.SKILL;
        }

        return input;
    }

    #endregion Methods
}
