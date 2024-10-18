using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class MonsterScript
{
    // 길찾기
    protected AIPath m_aiPath;
    public bool Arrived { get { return !m_aiPath.pathPending && m_aiPath.hasPath && m_aiPath.reachedEndOfPath; } }


    // 활동 범위
    [SerializeField]
    private MonsterSpawner m_spawnPoint;         // 활동 기준점
    public void SetSpawnPoint(MonsterSpawner _point) { m_spawnPoint = _point; }         // 기준점 설정
    public bool HasPoint { get { return m_spawnPoint != null; } }                       // 기준점 존재 여부
    public SpawnerData SpawnerData { get { return m_spawnPoint.SpawnerData; } }
    public Vector3 SpawnPosition { get { if (!HasPoint) { return new(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity); } return m_spawnPoint.SpawnPosition; } }
    public float SpawnFenceRange { get { return m_combatInfo.FenceRange * m_spawnPoint.RangeMultiplier; } }
    protected bool OutOfRange(Vector3 _position) { if(!HasPoint) { return false; } return Vector3.Distance(SpawnPosition, _position) > SpawnFenceRange; }


    // 기본 움직임
    protected bool IsTracing { get; set; }

    public override float CurSpeed { get { return m_aiPath.maxSpeed; } protected set { m_aiPath.maxSpeed = value; } }

    [SerializeField]
    private FRange m_roamingDistance = new(2.5f, 7.5f);

    public virtual Vector3 SetRandomRoaming()
    {
        Vector3 destination = Position;
        for(int i=0;i<32;i++)
        {
            float distance = m_roamingDistance.Num;
            Vector2 dir = FunctionDefine.DegToVec(Random.Range(0, 360f));
            Vector3 dir3 = new(dir.x, 0, dir.y);
            destination = transform.position + distance * dir3;
            if (!OutOfRange(destination)) { break; }
        }
        return destination;
    }
    public virtual void SetDestination(Vector3 _destination)
    {
        m_aiPath.isStopped = false;
        m_aiPath.destination = _destination;
        m_aiPath.SearchPath();
        CurSpeed = MoveSpeed;
        StartCoroutine(CheckDespawn());
    }
    private IEnumerator CheckDespawn()
    {
        while (m_aiPath.pathPending) { yield return null; }
        float time = DespawnTime;
        while (IsRoaming && time > 0 && m_aiPath.endReachedDistance > 1 && !m_aiPath.reachedDestination)
        {
            time -= Time.deltaTime;
            if(time <= 0) { DespawnMonster(); }
            yield return null;
        }
    }

    public override void StopMove()                  // 움직임 초기화
    {
        base.StopMove();
        if (!HasPath) { return; }

        m_aiPath.destination = ValueDefine.NULL_DESTINATION;
        m_aiPath.SetPath(null);
        HasPath = false;
    }

    public virtual void LookTarget()
    {
        if(CurTarget == null) { return; }

        Vector2 dir = (CurTarget.Position2 - Position2);
        RotateToDir(dir, ERotateSpeed.SLOW);
    }
    public override void StartTracing()
    {
        IsTracing = true;
    }

    public void DetectCliff()
    {
        Ray ray = new Ray(Position + Vector3.up * ObjectHeight, transform.forward + Vector3.down);
        if (!Physics.Raycast(ray, ObjectHeight * 10, ValueDefine.GROUND_LAYER))
        {
            RefindPath();
        }
    }
    private void RefindPath()
    {
        SetDestination(Position - transform.forward);
    }
}
