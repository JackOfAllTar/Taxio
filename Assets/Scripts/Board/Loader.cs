using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameController;
    public GameObject uiController;

    private void Awake()
    {
        Instantiate(gameController);
        Instantiate(uiController);
    }
}
