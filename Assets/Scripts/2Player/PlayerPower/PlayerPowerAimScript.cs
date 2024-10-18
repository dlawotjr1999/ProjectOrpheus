using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerPowerAimScript : MonoBehaviour
{
    private DecalProjector m_projector;

    public void ShowDrawer(float _radius)
    {
        if (!m_projector.enabled) { m_projector.enabled = true; }

        Vector3 size = m_projector.size;
        m_projector.size = new(_radius, _radius, size.z);
    }

    public void TraceAim(Vector3 _pos)
    {
        transform.position = _pos;
    }

    public void HideDrawer()
    {
        if (!m_projector.enabled) { return; }
        m_projector.enabled = false;
    }


    public void SetSize()
    {

    }




    private void SetComps()
    {
        m_projector = GetComponent<DecalProjector>();
    }

    private void Awake()
    {
        SetComps();
    }
}
