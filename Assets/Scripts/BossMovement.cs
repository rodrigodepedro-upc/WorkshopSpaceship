using UnityEngine;

/// <summary>
/// Mou el boss per la pantalla amb un de tres patrons. El patró actiu el decideix
/// la VIDA del boss: com més tocat està, més agressiu es mou.
///   - Lateral: llisca en horitzontal i rebota a les vores.   (vida > 2/3)
///   - Diagonal: lateral + rebot vertical, traçant un ziga-zaga. (1/3 < vida ≤ 2/3)
///   - Follow: persegueix el jugador i torna a poc a poc a la seva fila. (vida ≤ 1/3)
///
/// EXERCICI: els patrons Diagonal i Follow estan SENSE implementar. Completa els TODO de MoveDiagonal() i MoveFollow() prenent MoveLateral() com a exemple.
/// </summary>
public class BossMovement : MonoBehaviour
{
    public enum MovementMode { Lateral, Diagonal, Follow }

    [SerializeField] SpriteRenderer bossSprite = null;
    [SerializeField] Transform player = null;
    [SerializeField] float speed = 1f;
    [Tooltip("Vida del boss. Si es deixa buit, s'agafa el Health d'aquest mateix objecte.")]
    [SerializeField] Health bossHealth = null;
    [Tooltip("Patró actiu actual. El decideix la vida; només es mostra per veure'l canviar.")]
    [SerializeField, ReadOnly] MovementMode currentMode = MovementMode.Lateral;

    float startY = 0f;
    int horizontalDirection = 1;    // +1 cap a la dreta, -1 cap a l'esquerra
    int verticalDirection = 1;      // +1 cap amunt, -1 cap avall (mode diagonal)

    private void Start()
    {
        startY = transform.position.y;

        // Si no s'ha assignat a l'Inspector, agafa el Health d'aquest objecte (el BossIntegrity del boss).
        if (bossHealth == null)
            bossHealth = GetComponent<Health>();
    }

    void Update()
    {
        UpdateMode();

        switch (currentMode)
        {
            case MovementMode.Lateral: MoveLateral(); break;
            case MovementMode.Diagonal: MoveDiagonal(); break;
            case MovementMode.Follow: MoveFollow(); break;
        }
    }

    // Tria el patró segons la fracció de vida (1 = ple, 0 = mort).
    void UpdateMode()
    {
        if (bossHealth == null)
            return;

        float health = bossHealth.HealthFraction;

        if (health > 2f / 3f)
            currentMode = MovementMode.Lateral;
        else if (health > 1f / 3f)
            currentMode = MovementMode.Diagonal;
        else
            currentMode = MovementMode.Follow;
    }

    void MoveLateral()
    {
        Vector3 position = transform.position;
        Vector2 limit = ScreenLimit();

        position.x += horizontalDirection * speed * Time.deltaTime;

        if (position.x >= limit.x) { position.x = limit.x; horizontalDirection = -1; }
        else if (position.x <= -limit.x) { position.x = -limit.x; horizontalDirection = 1; }

        transform.position = position;
    }

    void MoveDiagonal()
    {
        Vector3 position = transform.position;
        Vector2 limit = ScreenLimit();

        // TODO: Com MoveLateral, però movent-se TAMBÉ en vertical (ziga-zaga).
        //   - Mou 'position.x' amb horizontalDirection i 'position.y' amb verticalDirection (tots dos amb speed * Time.deltaTime).

        //   - Quan 'position.x' arribi a +limit.x o -limit.x, fixa'l al límit i inverteix horizontalDirection (igual que a MoveLateral).

        //   - Fes el mateix per a la vertical: quan 'position.y' arribi a +limit.y o -limit.y, fixa'l al límit i inverteix verticalDirection.

        transform.position = position;
    }

    void MoveFollow()
    {
        if (player == null)
            return;

        Vector3 position = transform.position;
        Vector2 limit = ScreenLimit();

        // TODO: Persegueix el jugador en horitzontal i torna a poc a poc a la fila inicial.
        //   - Calcula la x objectiu: la posició x del jugador, limitada amb Mathf.Clamp(player.position.x, -limit.x, limit.x).

        //   - Acosta 'position.x' a aquesta x objectiu amb Mathf.MoveTowards(position.x, objectiu, speed * Time.deltaTime).

        //   - Acosta 'position.y' a 'startY' de la mateixa manera amb Mathf.MoveTowards.

        transform.position = position;
    }

    // Distància màxima que el centre del boss pot recórrer en cada eix, amb un marge segons la mida del seu sprite.
    Vector2 ScreenLimit()
    {
        Camera cam = Camera.main;
        float halfHeight = cam.orthographicSize;            // unitats de món des del centre fins a dalt
        float halfWidth = halfHeight * cam.aspect;          // ... fins a la vora dreta

        Vector2 padding = bossSprite != null ? (Vector2)bossSprite.bounds.extents : Vector2.zero;

        return new Vector2(halfWidth - padding.x, halfHeight - padding.y);
    }
}
