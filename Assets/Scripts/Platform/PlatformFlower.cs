using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFlower : PlatformBase
{
    public override void HitPlatform(PlayerController player)
    {
        player.GetPlayerMovement().Jump(3.5f);
    }
}
