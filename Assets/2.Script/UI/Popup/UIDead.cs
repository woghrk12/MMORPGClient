using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.UI;

public class UIDead : UIPopup
{
    #region Variables

    [SerializeField] private Button reviveButton = null;

    #endregion Variables

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        reviveButton.onClick.AddListener(() =>
        {
            CharacterReviveRequest packet = new()
            {
                CharacterID = Managers.Obj.LocalCharacter.ID
            };

            Managers.Network.Send(packet);
        });
    }

    #endregion Unity Events
}
