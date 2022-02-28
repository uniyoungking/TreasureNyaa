using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformHoney : PlatformBase
{
    public override void HitPlatform(PlayerController player)
    {
        player.GetPlayerMovement().Jump(2.3f);
        player.SpeedControl(-2f);
    }
}
