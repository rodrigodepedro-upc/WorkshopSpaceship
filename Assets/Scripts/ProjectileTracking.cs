using UnityEngine;

/// <summary>
/// Fa que un projectil mogut per un Rigidbody2D (p. ex. una bomba llançada) persegueixi suaument un objectiu (un Transform).
/// A cada pas de física gira una mica la velocitat actual cap a l'objectiu mantenint la mateixa rapidesa, de manera que descriu una corba lleugera en lloc de girar de cop.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileTracking : MonoBehaviour
{
    [Tooltip("Graus per segon màxims que la trajectòria es doblega cap a l'objectiu. Petit = una corba lleugera.")]
    [SerializeField] float turnSpeed = 20f;
    [Tooltip("Gira el sprite perquè miri cap a la direcció del moviment (la part de dalt del sprite = la velocitat).")]
    [SerializeField] bool faceTravelDirection = true;

    Rigidbody2D rb;
    Transform target;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // El BombLauncher crida aquest mètode just després de llançar la bomba per dir-li quin objectiu ha de perseguir.
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void FixedUpdate()
    {
        if (target == null)
            return;

        Vector2 velocity = rb.linearVelocity;
        float speed = velocity.magnitude;

        if (speed < Mathf.Epsilon)
            return;

        // TODO: Gira la velocitat cap a l'objectiu mantenint la mateixa rapidesa.
        //   - La direcció cap a l'objectiu és: (Vector2)target.position - rb.position

        //   - Gira 'velocity' cap a aquesta direcció amb Vector3.RotateTowards, com a molt 'turnSpeed' graus aquest pas (passa graus a radians amb Mathf.Deg2Rad i multiplica per Time.fixedDeltaTime).

        //   Substitueix la línia de sota pel resultat del gir.
        Vector2 steered = velocity;

        rb.linearVelocity = steered.normalized * speed;

        if (faceTravelDirection)
            transform.up = rb.linearVelocity;
    }
}
