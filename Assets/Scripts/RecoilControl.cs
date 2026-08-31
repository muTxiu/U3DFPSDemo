using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilControl : MonoBehaviour
{
    [Header("后坐力幅度")]
    public float recoilX = -3f;        // 上下后坐力
    public float randomRecoilY = 1f;   // 左右随机后坐力

    [Header("后坐力速度")]
    public float speed = 10;
    public float returnSpeed = 5;

    private Vector3 targetRotation;

    // 改为 public，让外部可以随时读取当前的后坐力偏移量
    public Vector3 currentRotation;

    void Update()
    {
        // 只做数学计算，不再修改 transform
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, speed * Time.deltaTime);
    }

    public void Fire()
    {
        float randY = Random.Range(-randomRecoilY, randomRecoilY);
        targetRotation += new Vector3(recoilX, randY, 0);
    }
}