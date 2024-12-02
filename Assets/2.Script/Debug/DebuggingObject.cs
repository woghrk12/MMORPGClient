using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuggingObject : MonoBehaviour
{
    private MMORPG.Object target = null;

    [SerializeField] private Image image = null;
    [SerializeField] private Text typeText = null;
    [SerializeField] private Text posText = null;

    private void Update()
    {
        posText.text = $"({target.Position.x}, {target.Position.y})";
        transform.position = new Vector3(target.Position.x, target.Position.y) + new Vector3(0.5f, 0.5f);
    }

    public void Init(MMORPG.Object target)
    {
        this.target = target;

        switch (target.GameObjectType)
        {
            case EGameObjectType.Character:
                image.color = Color.blue;
                typeText.text = "Character";
                break;

            case EGameObjectType.Monster:
                image.color = Color.red;
                typeText.text = "Monster";
                break;

            case EGameObjectType.Projectile:
                image.color = Color.black;
                typeText.text = "Projectile";
                break;
        }
    }
}
