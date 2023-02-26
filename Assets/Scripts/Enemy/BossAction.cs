using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAction : MonoBehaviour
{
    private BossController controller;

    public void SendController(BossController controller)
    {
        this.controller = controller;
    }

    private void FixedUpdate()
    {
        
    }
}
