using System.Collections;
using UnityEngine;
using UnityEngine.VFX;


public class Dissolve_Script : MonoBehaviour
{
    public InputSystem inputSystem;
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect VFXGraph;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    private Material[] skinnedMaterials;

    private Animator Animator;

    // Start is called before the first frame update
    void Start()
    {
        if (skinnedMesh != null)
            skinnedMaterials = skinnedMesh.materials;
        inputSystem = new();
        inputSystem.Player.Enable();

        Animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputSystem.Player.Jump.triggered) 
        {
            Animator.SetTrigger("Dying");
            StartCoroutine(DissolveCo());
        }
    }

    IEnumerator DissolveCo()
    {
        yield return new WaitForSeconds(1f);

        if(VFXGraph != null)
        {
            VFXGraph.Play();
        }
        if(skinnedMaterials.Length >0)
        {
            float counter = 0;

            while (skinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                for(int i=0; i<skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
