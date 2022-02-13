using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformHoney : PlatformBase
{
    public override void HitPlatform(PlayerController player)
    {
        player.GetPlayerMovement().Jump();
        player.SpeedControl(-2f);
    }
}
