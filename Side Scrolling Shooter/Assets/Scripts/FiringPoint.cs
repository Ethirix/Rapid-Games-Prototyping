using System.Collections;
using UnityEngine;

public class FiringPoint
{
    public FiringPoint(Transform transform, float cooldown)
    {
        Transform = transform;
        Cooldown = cooldown;
    }
    
    public Transform Transform { get; }
    public float Cooldown { get; }
    public bool OnCooldown { get; private set; }

    public IEnumerator RunCooldown()
    {
        float timer = 0;
        OnCooldown = true;
        while (true)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= Cooldown)
            {
                break;
            } 
            
            yield return new WaitForFixedUpdate();
        }

        OnCooldown = false;
    }
}