using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
public class WeaponParabola : WeaponBase
{
    public GameObject BulletPrefab;
    public AnimationCurve tractory;
    private int pointCount = 10;
    private Vector3[] pathPoints;


    public override void Initializer(Transform wt, UnitBase parent)
    {
        base.Initializer(wt, parent);
        SetTarget(GameManager.Instance.GetPlayerControl());
        pathPoints = new Vector3[pointCount];
    }
    //인자값 사용은 나중에 생각하자
    public override void Attack(int shootBullet)
    {
        SetTarget(0);
        if (targetUnits.Count == 0) 
        {
            Debug.LogWarning( parentUnit.name + " HitBox Range No Enemy");
            return;
        }

        Vector3 XZOne = new Vector3(1, 0, 1);
        Vector3 targetPos = Utility.VectorProduct(targetUnits[0].GetSkinnedMeshPostionToPostion(), XZOne);
        Vector3 weaponPos = Utility.VectorProduct(weaponTransfrom.position, XZOne);


        Vector3 dir = targetPos - weaponPos;
        Vector3 ratio = dir / (pointCount - 1);


        //총알생성
        GameObject bullet = Instantiate(BulletPrefab.gameObject, weaponTransfrom.position, weaponTransfrom.rotation);
        //총알에게 발사위치와 공격포인트를 넘겨준다.
        NomalAmmo nomalAmmo = bullet.GetComponent<NomalAmmo>();
        nomalAmmo.Initialize(targetUnits[0], this);

        for (int i = 0; i < pointCount; i++)
        {
            pathPoints[i] = bullet.transform.position + i * ratio;
            pathPoints[i].y += tractory.Evaluate((float)i / pointCount);
        }
        bullet.transform.LookAt(pathPoints[1]);

        Path path = new Path(PathType.CatmullRom, pathPoints, 1);

        Utility.KillTween(nomalAmmo.ammoMoveTween);
        nomalAmmo.ammoMoveTween = nomalAmmo.transform.DOPath(path, 8).SetLookAt(0.01f).
            SetSpeedBased(true).SetEase(Ease.OutQuad).OnComplete(() => OnAmmoReachedDestination(nomalAmmo));

        nomalAmmo.ammoMoveTween.Play();

    }

    void OnAmmoReachedDestination(NomalAmmo ammoClass)
    {
        ammoClass.HandleDestory();
    }

}
