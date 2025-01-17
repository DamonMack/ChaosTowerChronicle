using UnityEngine;
using UnityEngine.Events;

/// Your standard mortal being. It has limited Health Points, and can take damage and receive healing.
[DisallowMultipleComponent]
public class Mortal : Killable, Damageable
{
    [SerializeField]
    protected Element element;

    [SerializeField]
    protected AudioClip hurtSFX;

    [field: SerializeField]
    public int maxHp { get; protected set; }
    public int hp { get; protected set; } = 100;

    [field: SerializeField]
    public Team team { get; protected set; }

    protected void Start()
    {
        this.hp = this.maxHp;
    }

    public virtual void Damage(int dmg, Element damageType)
    {
        Debug.Assert(dmg >= 0);
        int realDamage = dmg;
        if (this.element != null)
        {
            realDamage = this.element.GetRealDamage(dmg, damageType);
        }
        this.hp = Mathf.Max(0, hp - realDamage);
        if(hurtSFX != null)
        {
            AudioSource.PlayClipAtPoint(hurtSFX, Camera.main.transform.position);
        }

        Damageable.SendDamageEvent(gameObject, dmg, damageType);

        if (this.hp == 0)
        {
            Kill();
        }
    }

    public virtual void Heal(int health)
    {
        Debug.Assert(health >= 0);

        this.hp = Mathf.Min(this.maxHp, this.hp + health);

        Damageable.SendHealEvent(gameObject, health);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lava"))
        {
            Kill();
        }
    }
}
