using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] private float magnetForce = 0.25f;
    [SerializeField, Range(0f, 10f)] private float maxDistance = 3f;
    [SerializeField, Range(0f, 10f)] private float maxDistanceOP = 1f;
    [SerializeField, Range(0f, 10f)] private float magnetForceOP = 1f;
    [SerializeField] private LayerMask magnetLayer;

    private Collider2D[] colliders;
    private Collider2D[] collidersOP;

    private void Awake()
    {
        colliders = new Collider2D[10];
        collidersOP = new Collider2D[10];
    }

    private void FixedUpdate()
    {
        if (Inventory.instance.isEmpty() < 0)
        {
            MagnetOP();
        }
        else
        {
            MagnetDefault();
        }
    }

    private void MagnetDefault()
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, maxDistance, colliders, magnetLayer);

        for (int i = 0; i < count; i++)
        {
            Collider2D collider = colliders[i];
            if (collider != null)
            {
                Vector2 direction = gameObject.transform.position - collider.transform.position;
                float distance = Vector2.Distance(gameObject.transform.position, collider.transform.position);

                float magnetStrength = Mathf.Clamp01(1 - (distance / maxDistance)) * magnetForce;
                collider.transform.Translate(magnetStrength * Time.fixedDeltaTime * direction.normalized);
            }
        }
    }

    private void MagnetOP()
    {
        int countOP = Physics2D.OverlapCircleNonAlloc(transform.position, maxDistanceOP, collidersOP, magnetLayer);

        for (int i = 0; i < countOP; i++)
        {
            Collider2D colliderOP = collidersOP[i];
            if (colliderOP != null)
            {
                Vector2 directionOP = (gameObject.transform.position - colliderOP.transform.position) * -1;
                float distanceOP = Vector2.Distance(gameObject.transform.position, colliderOP.transform.position);

                float magnetStrengthOP = Mathf.Clamp01(1 - (distanceOP / maxDistanceOP)) * magnetForceOP;
                colliderOP.transform.Translate(magnetStrengthOP * Time.fixedDeltaTime * directionOP.normalized);
            }
        }
    }
}