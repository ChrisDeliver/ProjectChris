using UnityEngine;

public class Sorter : MonoBehaviour
{
    [SerializeField] private bool isStatic = true;
    [SerializeField] private float offset = 0f;
    
    private int order = 0;
    private Vector3 p;
    private Renderer r;

    private void Awake()
    {
        r = GetComponent<Renderer>();
    }

    private void LateUpdate()
    {
        r.sortingOrder = (int)(order - transform.position.y + offset);

        if (isStatic)
            Destroy(this);
    }

    private void OnDrawGizmos()
    {
        p = gameObject.transform.position;
        Gizmos.DrawCube(new Vector3(p.x, p.y + offset), new Vector3(0.1f, 0.1f));
    }
}
