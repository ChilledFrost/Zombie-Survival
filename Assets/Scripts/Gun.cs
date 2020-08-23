using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Gun : MonoBehaviourPunCallbacks
{
    // Public Variables
    public Transform shootPosition;
    public LayerMask enemyMask;
    public SpriteRenderer muzzleFlash;

    public bool readyToShoot;
    public int damage = 20;

    public int maxBullets;
    public int bulletsRemaining;

    public float timeBetweenShots;
    public float reloadTime;

    // Timers
    private float timeBetweenShotsTimer;

    public IEnumerator Reload(Action callBack)
    {
        readyToShoot = false;
        yield return new WaitForSeconds(reloadTime);
        bulletsRemaining = maxBullets;
        UIManager.instance.ManageAmmoUI(bulletsRemaining);
        readyToShoot = true;
        callBack();
    }

    private IEnumerator ResetTimeBetweenShotsTimer()
    {
        readyToShoot = false;
        timeBetweenShotsTimer = timeBetweenShots;
        while(timeBetweenShotsTimer > 0)
        {
            print("Decreasing");
            print(timeBetweenShotsTimer);
            timeBetweenShotsTimer -= Time.deltaTime;
            yield return null;
        }
        readyToShoot = true;
    }
    [PunRPC]
    private void EnableMuzzleFlash()
    {
        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        muzzleFlash.enabled = true;
        yield return new WaitForSeconds(.1f);
        muzzleFlash.enabled = false;
    }

    public void Shoot(Vector2 direction)
    {
        if (bulletsRemaining <= 0 || !readyToShoot) return;
        bulletsRemaining--;
        UIManager.instance.ManageAmmoUI(bulletsRemaining);

        StartCoroutine(ResetTimeBetweenShotsTimer());

        RaycastHit2D raycastHit2D = Physics2D.Raycast(shootPosition.position, direction.normalized, 5f, enemyMask);
        Debug.DrawRay(shootPosition.position, direction.normalized * 5, Color.white, .1f);

        photonView.RPC("EnableMuzzleFlash", RpcTarget.All);

        if (raycastHit2D.transform != null)
        {
            EnemyController enemy = raycastHit2D.transform.gameObject.GetComponent<EnemyController>();
            enemy.Damage(damage);
        }

    }
}
