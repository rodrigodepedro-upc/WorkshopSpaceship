using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    [SerializeField] float lifeTime = 3f;
    [SerializeField] float damage = 10f;
    [SerializeField] LayerMask targetLayer;

    float timeAlive = 0f;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        timeAlive += Time.deltaTime;

        if (timeAlive >= lifeTime)
            Disable();
    }

    public void Enable(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        timeAlive = 0f;

        gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only react to objects on the target layer 
        if ((targetLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        Health healthOther = other.GetComponent<Health>();

        if (healthOther != null)
            healthOther.DealDamage(damage);

        Health healthSelf = GetComponent<Health>();

        if(healthSelf != null) 
            healthSelf.DealDamage(healthSelf.MaxHealth);
        else
            Disable();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
}
