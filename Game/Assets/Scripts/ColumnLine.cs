using UnityEngine;

public class ColumnLine : MonoBehaviour
{
    [SerializeField] private Material win_material;
    [SerializeField] private Material fail_material;
    [SerializeField] private Material default_material;

    private Renderer _renderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Win() { _renderer.material = win_material; }
    private void Fail() { _renderer.material = fail_material; }
    private void Default() { _renderer.material = default_material; }
}
