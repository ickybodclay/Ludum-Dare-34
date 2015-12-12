using UnityEngine;

public class Peasants : MonoBehaviour {
    private ParticleSystem m_ParticleSystem;
    private AudioSource m_AudioSource;

    private ParticleCollisionEvent[] m_CollisionEvents;

    private void Start () {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_AudioSource = GetComponent<AudioSource>();

        m_CollisionEvents = new ParticleCollisionEvent[16];
    }
    
    private void OnParticleCollision(GameObject other) {
        HydraHead head = other.GetComponentInParent<HydraHead>();

        if (head != null) {
            int safeLength = m_ParticleSystem.GetSafeCollisionEventSize();
            if (m_CollisionEvents.Length < safeLength)
                m_CollisionEvents = new ParticleCollisionEvent[safeLength];

            int peasantsHitCount = m_ParticleSystem.GetCollisionEvents(other, m_CollisionEvents);
            head.Eat(peasantsHitCount);

            if (!m_AudioSource.isPlaying) {
                m_AudioSource.pitch = Random.Range(0.9f, 1.1f);
                m_AudioSource.Play();
            }
        }
    }
}
