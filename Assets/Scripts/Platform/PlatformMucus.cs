using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMucus : PlatformBase
{
    public override void HitPlatform(PlayerController player)
    {
        player.GetPlayerMovement().Jump();
        player.GetPlayerMovement().AddForceRandom_X(1f);
        player.SpeedControl(2f);
    }
}
