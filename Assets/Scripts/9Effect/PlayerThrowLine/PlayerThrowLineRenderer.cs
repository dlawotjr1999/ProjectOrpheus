using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowLineRenderer : MonoBehaviour
{
    private LineRenderer m_renderer;

    private const int PointCount = 20;                      // 포물선 지점 개수

    private readonly List<Vector3> m_points = new();        // 지점 리스트

    public void DrawThrowLine(Vector3 _force, float _mass, Vector3 _start)      // 포물선 그리기
    {
        if (!m_renderer.enabled) { m_renderer.enabled = true; }

        Vector3 velocity = (_force / _mass) * Time.fixedDeltaTime;

        float duration = (4 * velocity.y) / -Physics.gravity.y;

        float stepTime = duration / PointCount;

        m_points.Clear();

        for (int i = 0; i<PointCount; i++)
        {
            float timePassed = stepTime * i;

            Vector3 moveVector = new(velocity.x * timePassed,
                velocity.y * timePassed - 0.5f * -Physics.gravity.y * timePassed * timePassed,
                velocity.z * timePassed);

            if (Physics.Raycast(_start, moveVector, out _, moveVector.magnitude))
            {
                break;
            }
            
            m_points.Add(_start + moveVector);
        }

        m_renderer.positionCount = m_points.Count;
        m_renderer.SetPositions(m_points.ToArray());
    }

    public void HideThrowLine()                                                 // 포물선 숨기기
    {
        m_renderer.enabled = false;
    }


    private void Awake()
    {
        m_renderer = GetComponent<LineRenderer>();
    }
}
