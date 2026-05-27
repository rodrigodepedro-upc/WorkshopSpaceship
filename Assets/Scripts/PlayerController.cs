using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] List<ProjectileSpawner> playerProjectileSpawners = null;
    [SerializeField] SpriteRenderer shipSprite = null;

    // TODO 1: Afegeix aquí un camp serialitzat 'speed' (un float, p. ex. amb valor 1) per ajustar la velocitat de la nau des de l'Inspector.
    //         L'utilitzaràs a Update(), on es calcula 'movement' més avall.

    BoxCollider2D shipCollider = null;

    private void Start()
    {
        shipCollider = GetComponent<BoxCollider2D>();
    }

    // Update es crida un cop per fotograma
    void Update()
    {
        // Moviment

        Vector3 inputDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.A)) inputDirection.x -= 1f;
        if (Input.GetKey(KeyCode.D)) inputDirection.x += 1f;

        if (Input.GetKey(KeyCode.S)) inputDirection.y -= 1f;
        if (Input.GetKey(KeyCode.W)) inputDirection.y += 1f;

        // '.normalized' fa que el moviment en diagonal tingui la mateixa velocitat que el moviment recte.
        Vector3 movement = inputDirection.normalized;

        // TODO 1: Multiplica 'movement' pel teu nou camp 'speed' perquè la nau es mogui a la velocitat indicada a l'Inspector.

        // TODO 2: Multiplica també per Time.deltaTime perquè 'speed' sigui "unitats per segon" i no depengui dels fotogrames per segon.

        Vector3 newPosition = transform.position + movement;

        // Limitació de la posició

        Camera cam = Camera.main;
        float halfHeight = cam.orthographicSize;            // unitats de món des del centre fins a dalt
        float halfWidth = halfHeight * cam.aspect;          // ... fins a la vora dreta

        Vector2 padding = shipCollider.size / 2f;

        newPosition.x = Mathf.Clamp(newPosition.x, -halfWidth + padding.x, halfWidth - padding.x);
        newPosition.y = Mathf.Clamp(newPosition.y, -halfHeight + padding.y, 0f);

        transform.position = newPosition;

        // Dispar

        // TODO 3: Quan el jugador mantingui premuda la tecla Espai, fes que tots els spawners disparin.
        //   - Input.GetKey(KeyCode.Space) és cert mentre es manté premuda l'Espai.

        //   - Recorre 'playerProjectileSpawners' amb un foreach i crida Shoot() a cada spawner.
    }
}
