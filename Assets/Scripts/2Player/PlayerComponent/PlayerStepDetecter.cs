using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlayerStepDetecter : MonoBehaviour
{
    private PlayerController m_player;

    private bool ShouldCheck { get { return m_player.IsIdle || m_player.IsMoving; } }

    [SerializeField]
    private float m_groundCheckRange = 0.04f;
    [SerializeField]
    private float m_soundDelay = 0.2f;
    private bool CanPlaySound { get; set; } = true;

    private bool IsTouchingGround { get; set; } = true;
    private bool PrevTouchingGround { get; set; } = true;

    private readonly Collider[] GroundChecker = new Collider[1];

    private void CheckTouchingGround()
    {
        IsTouchingGround = Physics.OverlapSphereNonAlloc(transform.position, m_groundCheckRange, GroundChecker, ValueDefine.GROUND_LAYER) > 0;
        if(!PrevTouchingGround && IsTouchingGround && CanPlaySound) { PlayStepSound(); }
        PrevTouchingGround = IsTouchingGround;
    }
    private void PlayStepSound()
    {
        int idx = Random.Range((int)EPlayerSE.STEP1, (int)EPlayerSE.STEP2 + 1);
        GameManager.PlaySE((EPlayerSE)idx, transform.position);
        CanPlaySound = false;
        StartCoroutine(StepSoundDelay());
    }
    private IEnumerator StepSoundDelay()
    {
        yield return new WaitForSeconds(m_soundDelay);
        CanPlaySound = true;
    }



    private void Awake()
    {
        m_player = GetComponentInParent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (m_player == null || !ShouldCheck) { return; }
        CheckTouchingGround();
    }
}
