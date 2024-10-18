using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MiniuumScript : MonsterScript
{
    public override bool CanPurify => true;


    [SerializeField]
    private VisualEffect m_headFire;


    public override void SetDestination(Vector3 _destination)
    {
        base.SetDestination(_destination);
        m_anim.SetBool("IS_MOVING", true);
    }
    public override void StartApproach()
    {
        base.StartApproach();
        m_anim.SetBool("IS_MOVING", true);
    }
    public override void StopMove()
    {
        base.StopMove();
        m_anim.SetBool("IS_MOVING", false);
    }





    private readonly float DashSpeed = 5;

    private readonly float MaxUpperVel = 0.5f;

    private bool IsDashing { get; set; }
    private Vector2 DashDir { get; set; }

    public override void AttackTriggerOn()
    {
        base.AttackTriggerOn();
        DashDir = (CurTarget.Position2 - Position2).normalized;
        StartCoroutine(DashCoroutine());
    }
    private IEnumerator DashCoroutine()
    {
        IsDashing = true;
        while (!IsDead && Vector2.Distance(Position2, CurTarget.Position2) > 0.5f && IsDashing)
        {
            Vector3 vel = m_rigid.velocity;
            if(vel.y > MaxUpperVel) { vel.y =  MaxUpperVel; }
            m_rigid.velocity = DashSpeed * new Vector3(DashDir.x, vel.y, DashDir.y);
            yield return null;
        }
        m_rigid.velocity = Vector3.zero;
    }

    public override void AttackTriggerOff()
    {
        base.AttackTriggerOff();
        IsDashing = false;
    }

    public override void StartDissolve()
    {
        base.StartDissolve();
        m_headFire.Stop();
    }
}
