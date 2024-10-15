using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.UI;

public class ReviveButton : MonoBehaviour
{
    #region Variables

    private Button button = null;

    #endregion Variables

    #region Unity Events

    private void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            ObjectReviveRequest packet = new()
            {
                ObjectID = Managers.Obj.LocalPlayer.ID
            };

            Managers.Network.Send(packet);

            Destroy(transform.parent.gameObject);
        });
    }

    #endregion Unity Events
}