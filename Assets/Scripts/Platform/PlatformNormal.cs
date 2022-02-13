using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformNormal : PlatformBase
{
    public override void HitPlatform(PlayerController player)
    {
        player.GetPlayerMovement().Jump();
    }
}
