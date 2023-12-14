using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public Sprite snakeHeadSprite;

    public Sprite foodSprite;

    public static GameAssets Instance;

    private void Awake()
    {
        Instance = this;
    }
}
