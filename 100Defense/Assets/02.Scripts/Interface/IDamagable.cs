/// <summary>
/// 데미지를 입는 대상에게 있을 Interface
/// </summary>
public interface IDamagable
{
   int Health { get; set; }
    void Damage(int _damage);
}
