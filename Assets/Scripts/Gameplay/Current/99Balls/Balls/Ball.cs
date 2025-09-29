using Gameplay.Current._99Balls.Interactables;
using UnityEngine;

namespace Gameplay.Current._99Balls.Balls
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private CircleCollider2D col;
        [Space]
        [SerializeField] private LayerMask collisionLayer;

        public void Push(Vector2 direction, float power = 1)
        {
            rb.isKinematic = false;
            col.enabled = true;

            rb.AddForce(direction * power, ForceMode2D.Impulse);
        }

        public void StopMoving()
        {
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            
            col.enabled = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            TryInteract(collision.gameObject);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            TryInteract(collision.gameObject);
        }

        private void TryInteract(GameObject go)
        {
            if (((1 << go.layer) & collisionLayer.value) != 0)
            {
                if (go.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact(this);
                }
            }
        }
    }
}