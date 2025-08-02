using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class LightIndicator : MonoBehaviour
{
    [SerializeField] private Material offMaterial;
    [SerializeField] private Material onMaterial;
    [SerializeField] private GameObject lightObject;
    private MeshRenderer meshRenderer;
    public bool defaultState = false;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        SetState(defaultState);
    }

    public void SetState(bool state)
    {
        if (state)
        {
            meshRenderer.material = onMaterial;
            if (lightObject != null)
            {
                lightObject.SetActive(true);
            }
        }
        else
        {
            meshRenderer.material = offMaterial;
            if (lightObject != null)
            {
                lightObject.SetActive(false);
            }
        }
    }
}
