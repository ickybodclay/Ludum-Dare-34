using UnityEngine;

public class Peasants : MonoBehaviour {
    private ParticleSystem part;
    private ParticleCollisionEvent[] collisionEvents;

    private void Start () {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new ParticleCollisionEvent[16];
    }
    
    private void OnParticleCollision(GameObject other) {
        HydraHead head = other.GetComponentInParent<HydraHead>();

        if (head != null) {
            int safeLength = part.GetSafeCollisionEventSize();
            if (collisionEvents.Length < safeLength)
                collisionEvents = new ParticleCollisionEvent[safeLength];

            int peasantsHitCount = part.GetCollisionEvents(other, collisionEvents);
            head.Eat(peasantsHitCount);
        }
    }
}
