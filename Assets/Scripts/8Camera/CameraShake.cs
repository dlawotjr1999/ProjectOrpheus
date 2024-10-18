using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float m_magnitude = 1.0f; // ���޽� ������ Ȯ���ϱ� ���� �⺻ ���� �����մϴ�.
    private CinemachineImpulseSource m_impulseSource;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: " + other.name);
        CameraShaking(m_magnitude);
    }

    public void CameraShaking(float magnitude)
    {
        Debug.Log("CameraShaking called with magnitude: " + magnitude);
        if (m_impulseSource != null)
        {
            Debug.Log("Generating impulse...");
            m_impulseSource.GenerateImpulse(magnitude);
            Debug.Log("Impulse generated");
        }
        else
        {
            Debug.LogError("m_impulseSource is null!");
        }
    }

    public void SetImpulseInfo()
    {
        if (m_impulseSource.m_ImpulseDefinition == null)
        {
            m_impulseSource.m_ImpulseDefinition = new CinemachineImpulseDefinition(); // ImpulseDefinition �ʱ�ȭ
            Debug.Log("ImpulseDefinition set: " + (m_impulseSource.m_ImpulseDefinition != null));
        }

        m_impulseSource.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Explosion;
        m_impulseSource.m_ImpulseDefinition.m_AmplitudeGain = 1.0f; // �߰��� ���޽� ������ �����մϴ�.
        m_impulseSource.m_ImpulseDefinition.m_FrequencyGain = 1.0f; // �߰��� ���ļ� ������ �����մϴ�.

        m_impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = 0.2f;    // ���� �ð�
        m_impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = 0.3f;   // ���� �ð�
        m_impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = 0.5f;      // ���� �ð�

        m_impulseSource.m_DefaultVelocity = new Vector3(1.0f, 1.0f, 1.0f); // ���޽��� ����� ũ�⸦ ����� Ű��ϴ�.
    }

    private void Start()
    {
        m_impulseSource = gameObject.GetComponent<CinemachineImpulseSource>();
        SetImpulseInfo();
    }
}
