using UnityEngine;

// Aquest script desplaça dues còpies de la mateixa textura cap avall per crear un fons que es mou sense fi (scroll infinit).
// Quan una còpia surt per baix, la tornem a col·locar a dalt de l'altra perquè el bucle no tingui cap tall.
//
// EXERCICI: programa aquest comportament de zero seguint els TODO en ordre.
public class BackgroundAnimation : MonoBehaviour
{
    // TODO 1: Declara aquí tres camps serialitzats ([SerializeField]) perquè es puguin assignar des de l'Inspector:
    //           - SpriteRenderer textureA
    //           - SpriteRenderer textureB
    //           - float speed   (posa-li un valor per defecte, p. ex. 1)
    //         Després, a l'Inspector, arrossega les dues textures del fons (els seus SpriteRenderer) als camps textureA i textureB.

    // Update es crida un cop per fotograma.
    void Update()
    {
        // TODO 2: Calcula quant s'han de moure les textures aquest fotograma.
        //         Volem que baixin: multiplica Vector3.down per (speed * Time.deltaTime) i desa-ho en una variable Vector3 anomenada 'step'.
        //         (Recorda: multiplicar per Time.deltaTime fa que 'speed' sigui "unitats per segon" i no depengui dels fotogrames per segon.)

        // TODO 3: Mou les dues textures cap avall sumant 'step' a la seva posició.
        //         Per a la primera:  textureA.transform.position += step;
        //         Fes el mateix amb textureB.

        // --- Reaprofitament: tornar a dalt la textura que surt per baix ---

        // TODO 4: Hem de saber quina textura va al davant (a DALT) i quina al darrere (a BAIX).
        //         Declara dues variables SpriteRenderer, per exemple 'upper' i 'lower'.
        //         Amb un if/else, compara textureA.transform.position.y amb textureB.transform.position.y.
        //         La que tingui la 'y' MÉS GRAN és la 'upper' (està més amunt); l'altra és la 'lower'.

        // TODO 5: Quan la textura de dalt hagi baixat per sota del centre (upper.transform.position.y < 0), col·loca la de baix just a sobre d'ella perquè quedin enganxades sense buit.
        //         Fes servir un if amb aquesta assignació a dins:
        //           lower.transform.position = upper.transform.position + Vector3.up * upper.size.y;
        //         (upper.size.y és l'alçada de la textura.)
    }
}
