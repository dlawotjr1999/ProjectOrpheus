using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AnimateAttackScript : ObjectAttackScript
{
    private BoxCollider m_collider;

    private const int MAX_TRAIL_FRAME = 2;
    [SerializeField]
    private float m_modelMultiplier = 30.90435f;


    public override void AttackOff()
    {
        base.AttackOff();
        m_trailList.Clear();
        m_trailFillerList.Clear();
    }


    protected readonly LinkedList<BufferObject> m_trailList = new();
    protected LinkedList<BufferObject> m_trailFillerList = new();

    private Collider[] ColliderCache = new Collider[128];
    private void CheckTrail()
    {
        Vector3 localScale = transform.localScale;
        Vector3 size = m_collider.size * m_modelMultiplier;
        size.x *= localScale.x; size.y *= localScale.y; size.z *= localScale.z;


        Vector3 center = m_modelMultiplier*m_collider.center;
        center.x *= localScale.x; center.y *= localScale.y; center.z *= localScale.z;
        Vector3 offset = m_collider.transform.TransformDirection(center);
        BufferObject col = new()
        {
            Size = size,
            Rotation = m_collider.transform.rotation,
            Position = m_collider.transform.position + offset
        };
        m_trailList.AddFirst(col);
        if (m_trailList.Count > MAX_TRAIL_FRAME) { m_trailList.RemoveLast(); }
        if (m_trailList.Count > 1) { m_trailFillerList = FillTrail(m_trailList.First.Value, m_trailList.Last.Value); }

        Collider[] hits = ScanColliders(col).ToArray();
        bool hit = HitObject(hits);
        foreach (BufferObject co in m_trailFillerList)
        {
            hits = ScanColliders(co).ToArray();
            hit |= HitObject(hits);
        }
        if (Attacker.IsPlayer && hit) { ((PlayerController)Attacker).HitTarget(); }
    }
    private LinkedList<BufferObject> FillTrail(BufferObject _from, BufferObject _to)
    {
        LinkedList<BufferObject> fillerList = new();
        float distance = Mathf.Abs((_from.Position - _to.Position).magnitude);
        Vector3 localScale = transform.localScale;
        Vector3 size = m_collider.size * m_modelMultiplier;
        size.x *= localScale.x; size.y *= localScale.y; size.z *= localScale.z;
        float gap = size.z;
        if (distance > gap)
        {
            float steps = Mathf.Ceil(distance / gap);
            float stepAmount = 1 / (steps + 1);
            float stepValue = 0;
            for (int i = 0; i<(int)steps; i++)
            {
                stepValue += stepAmount;
                BufferObject obj = new()
                {
                    Size = size,
                    Position = Vector3.Lerp(_from.Position, _to.Position, stepValue),
                    Rotation = Quaternion.Lerp(_from.Rotation, _to.Rotation, stepValue)
                };
                fillerList.AddFirst(obj);
            }
        }
        return fillerList;
    }

    public virtual List<Collider> ScanColliders(BufferObject _col)
    {
        ColliderCache = new Collider[128];
        List<Collider> list = new();
        if (Physics.OverlapBoxNonAlloc(_col.Position, _col.Size / 2, ColliderCache, _col.Rotation, ValueDefine.HITTABLE_LAYER, QueryTriggerInteraction.Collide) > 0)
        {
            foreach(Collider col in ColliderCache) { list.Add(col); }
            ColliderCache = new Collider[128];
        }
        if (Attacker.IsMonster && Physics.OverlapBoxNonAlloc(_col.Position, _col.Size / 2, ColliderCache, _col.Rotation, ValueDefine.HITTABLE_PLAYER_LAYER, QueryTriggerInteraction.Collide) > 0)
        {
            foreach (Collider col in ColliderCache) { list.Add(col); }
        }
        return list;
    }
    private bool HitObject(Collider[] _hits)
    {
        if (!IsAttacking) { return false; }

        bool hit = false;
        foreach (Collider collider in _hits)
        {
            if(collider== null || !collider.isTrigger) { continue; }
            IHittable hittable = collider.GetComponentInParent<IHittable>();
            if (hittable == null) { Debug.LogError("히터블 스크립트 없음"); continue; }
            if (!CheckTarget(collider)) { continue; }
            Vector3 pos = CheckNHit(hittable);
            CreateHitEffect(hittable, pos);
            hit |= hittable.IsMonster;
            if(m_hitSound != null) { GameManager.PlaySE(m_hitSound, transform.position); }
        }
        return hit;
    }
    public virtual bool CheckTarget(Collider _collider)
    {
        ObjectScript obj = _collider.GetComponentInParent<ObjectScript>();
        if (obj == null || obj == m_attacker || obj.IsDead || CheckHit(obj)) { return false; }
        return true;
    }
    public virtual void CreateHitEffect(IHittable _hittable, Vector3 _pos) { }
    public virtual Vector3 CheckNHit(IHittable _hittable)
    {
        if (CheckHit(_hittable)) { return Vector3.zero; }
        Vector3 pos = GetComponent<Collider>().ClosestPoint(transform.position);
        HitData hit = new(m_attacker, Damage, pos, CCList);
        if (_hittable.GetHit(hit))
        {
            AddHitObject(_hittable);
        }
        return pos;
    }

    private void OnDrawGizmos()
    {
        if (!IsAttacking) { return; }
        foreach (BufferObject col in m_trailList)
        {
            Gizmos.color = Color.blue;
            Gizmos.matrix = Matrix4x4.TRS(col.Position, col.Rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, col.Size);
        }
        foreach (BufferObject bet in m_trailFillerList)
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = Matrix4x4.TRS(bet.Position, bet.Rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, bet.Size);
        }
    }


    public virtual void SetComps()
    {
        m_collider = GetComponent<BoxCollider>();
    }

    private void Awake()
    {
        SetComps();
    }
    public override void Start()
    {
        m_attacker = GetComponentInParent<ObjectScript>();
    }
    private void FixedUpdate()
    {
        if (IsAttacking)
        {
            CheckTrail();
        }
    }
}
