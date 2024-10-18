using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float m_magnitude = 1.0f; // 임펄스 강도를 확인하기 위해 기본 값을 설정합니다.
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
            m_impulseSource.m_ImpulseDefinition = new CinemachineImpulseDefinition(); // ImpulseDefinition 초기화
            Debug.Log("ImpulseDefinition set: " + (m_impulseSource.m_ImpulseDefinition != null));
        }

        m_impulseSource.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Explosion;
        m_impulseSource.m_ImpulseDefinition.m_AmplitudeGain = 1.0f; // 추가로 임펄스 강도를 설정합니다.
        m_impulseSource.m_ImpulseDefinition.m_FrequencyGain = 1.0f; // 추가로 주파수 강도를 설정합니다.

        m_impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = 0.2f;    // 공격 시간
        m_impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = 0.3f;   // 유지 시간
        m_impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = 0.5f;      // 감쇠 시간

        m_impulseSource.m_DefaultVelocity = new Vector3(1.0f, 1.0f, 1.0f); // 임펄스의 방향과 크기를 충분히 키웁니다.
    }

    private void Start()
    {
        m_impulseSource = gameObject.GetComponent<CinemachineImpulseSource>();
        SetImpulseInfo();
    }
}
