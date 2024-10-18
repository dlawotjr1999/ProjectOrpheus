using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YumScript : RangedAttackMonster
{
    public override Vector3 AttackOffset => new(0, 0.5f, 0.3f);


    public override bool CanPurify => true;
}
