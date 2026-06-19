using MonoGameLibrary.Entities;

namespace KingdomLike.Interfaces;

public interface IDamageable
{
    public void TakeDamage(float damage, Entity fromEntity);
}
