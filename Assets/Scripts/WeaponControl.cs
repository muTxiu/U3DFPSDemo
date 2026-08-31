using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    //发射位置
    public GameObject FirePoint;
    //子弹
    public GameObject BulletPre;
    //火焰效果
    public GameObject FirePre;

    // 【新增】枪声音效相关变量
    [Header("音效设置")]
    public AudioSource weaponAudio; // 挂载在枪口或枪身处的专属喇叭
    public AudioClip fireSound;     // 开火的音频文件

    //时间间隔
    public float bulletInterval = 0.3f;
    private float timer = 0;
    private PlayerControl pc;
    private RecoilControl rc;

    void Start()
    {
        rc = GetComponent<RecoilControl>();
        pc = GetComponent<PlayerControl>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetMouseButton(0) && timer >= bulletInterval && !pc.highSpeed)
        {
            timer = 0;

            //后坐力 (加上空值判断更安全)
            if (rc != null)
            {
                rc.Fire();
            }

            //创建子弹
            Instantiate(BulletPre, FirePoint.transform.position, FirePoint.transform.rotation);
            //显示效果
            Destroy(Instantiate(FirePre, FirePoint.transform.position, FirePoint.transform.rotation), 0.1f);

            // 【新增】播放开火音效
            if (fireSound != null && weaponAudio != null)
            {
                weaponAudio.PlayOneShot(fireSound);
            }
        }
    }
}