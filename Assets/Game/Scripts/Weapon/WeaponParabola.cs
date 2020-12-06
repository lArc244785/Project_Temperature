using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEngine;
public class WeaponParabola : WeaponBase
{
    public GameObject BulletPrefab;
    public AnimationCurve tractory;
    private int pointCount = 10;
    private Vector3[] pathPoints;
    private NomalAmmo nomalAmmo;
    private Vector3 ratio;
    public override void Initializer(Transform wt, UnitBase parent)
    {
        base.Initializer(wt, parent);
        SetTarget(GameManager.Instance.GetPlayerControl());
        pathPoints = new Vector3[pointCount];
    }
    //인자값 사용은 나중에 생각하자
    public override void Attack(int shootBullet)
    {
        if (targetUnits.Count < 1) return;
        //총알생성
        GameObject bullet = Instantiate(BulletPrefab.gameObject, weaponTransfrom.position, Quaternion.identity);
        //총알에게 발사위치와 공격포인트를 넘겨준다.
        nomalAmmo = bullet.GetComponent<NomalAmmo>();
        nomalAmmo.Initialize(targetUnits[0], this);

        for (int i = 0; i < pointCount; i++)
        {
            pathPoints[i] = bullet.transform.position + i * ratio;
            pathPoints[i].y += tractory.Evaluate((float)i / pointCount);
        }
        
        //bullet.transform.LookAt(pathPoints[1]);

        Path path = new Path(PathType.CatmullRom, pathPoints, 1);

        Utility.KillTween(nomalAmmo.ammoMoveTween);
        nomalAmmo.ammoMoveTween = nomalAmmo.transform.DOPath(path, pointCount - 2)./*SetLookAt(1.0f).*/
            SetSpeedBased(true).SetEase(Ease.OutQuad).OnComplete(() => nomalAmmo.TrackingModeOn());


        nomalAmmo.ammoMoveTween.Play();

    }

    public void Test()
    {
        Debug.Log("Complet");
    }

    public override void SetShootAttackPath()
    {
        base.SetShootAttackPath();
        targetUnits.Clear();
        targetUnits.Add(GameManager.Instance.GetPlayerControl());

        Vector3 XZOne = new Vector3(1, 0, 1);
        Vector3 targetPos = Utility.VectorProduct(targetUnits[0].GetSkinnedMeshPostionToPostion(), XZOne);
        Vector3 weaponPos = Utility.VectorProduct(weaponTransfrom.position, XZOne);
        targetPos += targetUnits[0].GetRigidbody().velocity.normalized * 2.0f;

        Vector3 dir = targetPos - weaponPos ;
        ratio = dir / (pointCount - 1);

    }


    void OnAmmoReachedDestination(NomalAmmo ammoClass)
    {
        ammoClass.HandleDestory();
    }

}
