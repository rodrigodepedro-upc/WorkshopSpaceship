using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ship8
{
    public class ExplosionController : MonoBehaviour {

        [Tooltip("Child game objects that should be destroyed during explosion. For that 'DestroyPart(x)' will be called from animation clip.")]
        public GameObject[] removeParts;
        [Tooltip("Array of children that have animation for explosion and should explode by calling from parent animation clip.")]
        public ExplosionController[] childrenExplosion;

        [Tooltip("Optional object destroyed once this explosion's animation finishes playing. Wire to a ship/boss root to remove it after its death explosion. Leave empty for pooled/reused explosions (e.g. bombs).")]
        public GameObject destroyOnComplete;

        /*
        [Tooltip("Main parent that should be destroyed after all explosins complete. Will call in 'DestroyMainParent' function from AnimationClip")]
        public GameObject mainaParent;
        */

        Animator animator;
        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void DestroyPart(int index)
        {
            if (removeParts != null && index >= 0 && index < removeParts.Length)
                Destroy(removeParts[index]);
            else
                Debug.LogWarning("Index is out of range in DestroPart. index: " + index);
        }

        public void StartExplosion()
        {
            if (animator == null)
                animator = GetComponent<Animator>();
            animator.SetBool("expl", true);

            if (destroyOnComplete != null)
                StartCoroutine(DestroyWhenComplete());
        }

        IEnumerator DestroyWhenComplete()
        {
            // Wait one frame for the (instant) transition out of Idle into the Explosion state so the state info below reports the explosion clip.
            yield return null;

            // The Explosion state has no exit transition, so the explosion has finished once its clip has played to the end.
            while (animator != null && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                yield return null;

            if (destroyOnComplete != null)
                Destroy(destroyOnComplete);
        }

        /// <summary>
        /// Call this function from animation clip in the last frame to remove GameObject.
        /// </summary>
        public void DestroyObject()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Call this function from animation clip in the last frame to remove parent GameObject.
        /// </summary>
        public void DestroyParentObject()
        {
            Destroy(transform.parent.gameObject);
        }

        public void ChildExplosion(int index)
        {
            if (childrenExplosion != null && index >= 0 && index < childrenExplosion.Length)
                childrenExplosion[index].StartExplosion();
        }

        public void DestroyChildren()
        {
            if (removeParts != null && removeParts.Length > 0)
                foreach (GameObject child in removeParts)
                    Destroy(child);
        }

        /*
        public void DestroyMainParent()
        {
            if (mainaParent != null)
                Destroy(mainaParent);
            else
                Debug.Log("mainaParent not set for object name: " + name);
        }
        */
    }

}
