using System.Collections; 
using UnityEngine;

public class ProjectileControl : MonoBehaviour
{
    [SerializeField] int HitPower = 25;
    private string targetTag;
    [SerializeField] private float bulletLifeTime = 10f;

    private void Start()
    { 
        //Запускаем уничтожение снаряда с задержкой
        StartCoroutine(DestroyTheProjectile(bulletLifeTime));
        //Указываем что снаряды не должны взаимодействовать с определенными объектами в сцене
        Physics.IgnoreLayerCollision(8, 8); 
    } 

    private void OnTriggerEnter(Collider other)
    {
        //Проверяем на столкновение с указанной целью или с стенами
        if (other.transform.tag.Contains(targetTag) || other.transform.CompareTag("WALL"))
        {
            Destroy(gameObject);
        }
    }

    //Процедура отложенного уничтожения снаряда
    private IEnumerator DestroyTheProjectile(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject); 
    }

    public int GetHitPower()
    {
        var hitPower = HitPower;
        return hitPower;
    }

    public void SetHitPower(int hitPower)
    {
        HitPower = hitPower;
    }
    
    public void SetTargetTag(string targetTagValue)
    {
        targetTag = targetTagValue;
    } 

    public string GetTargetTag( )
    {
        var getTargetTag = targetTag;
        return getTargetTag;
    } 
}
