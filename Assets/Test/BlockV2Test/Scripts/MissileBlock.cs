using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBlock : ChainingBlock
{
    [SerializeField]
    private GameObject missile;

    [SerializeField]
    private float shootFrequency,
        missileSpeed;

    private float timer = 0;

    private bool shooting = false;

    private void Update()
    {
        if (shooting) ShootMissiles();
    }
    public override void Deactivate()
    {
        StartCoroutine(Deactivate(new HashSet<ChainingBlock>(), delay));
    }
    protected override void TurnOn()
    {
        spriteRenderer.color = activeColor;
        shooting = true;
        timer = 1f;
    }
    protected override void TurnOff()
    {
        spriteRenderer.color = inactiveColor;
        shooting = false;
    }
    private void ShootMissiles()
    {
        if(timer < 1f)
        {
            timer += Time.deltaTime * shootFrequency;
        }
        else
        {
            timer = 0f; 
            Missile projectile = Instantiate(missile, transform.position, Quaternion.identity).GetComponent<Missile>();

            if (projectile != null)
            {
                Debug.Log("Shooting missile");
                projectile.MissileSpeed = missileSpeed;
            }
        }
    }
}
