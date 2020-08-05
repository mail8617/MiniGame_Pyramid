using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pyramid_Computer : Pyramid_Player
{
    public override void StartTurn()
    {
        SituateRandomBlock();

        //Debug.Log("StartTurn blocks.Count " + playerType + " / " + blocks.Count);
        Pyramid_UIManager.instance.UpdateLeftBlockCount(playerType, blocks.Count);
    }

}
