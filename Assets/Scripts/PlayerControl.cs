using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //速度
    public float speed = 3f;
    public float jumpForce = 5f;
    //灵敏度
    public float xScensitivity = 10;
    public float yScensitivity = 10;
    public RecoilControl recoil;

    [HideInInspector]
    public bool highSpeed = false;
    [HideInInspector]
    public bool isAiming = false;

    [Header("音效设置")]
    public AudioClip stepSound; // 存放脚步音效文件
    private AudioSource audioSource; // 玩家身上的喇叭

    private float xRotation = 0;
    private Rigidbody rb;
    private Animator ani;
    private Vector3 velocity;
    private bool jump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;

        // 获取玩家身上的 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Mouse();
        HighSpeed();
        Move();
        Jump();
        Aim();
    }

    //鼠标旋转
    void Mouse()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        // 1. 正常的鼠标上下旋转
        xRotation -= y * yScensitivity;
        xRotation = Mathf.Clamp(xRotation, -80, 80);

        // 2. 获取当前的后坐力偏移量（如果没有挂载后坐力脚本，则偏移量为 0）
        Vector3 recoilOffset = recoil != null ? recoil.currentRotation : Vector3.zero;

        // 3. 核心融合：鼠标旋转(xRotation) + 后坐力偏移(recoilOffset)！
        ani.transform.localRotation = Quaternion.Euler(xRotation + recoilOffset.x, recoilOffset.y, 0);

        // 4. 玩家主体的左右转身
        transform.Rotate(Vector3.up * x * xScensitivity);
    }

    void HighSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift) && IsGround())
        {
            highSpeed = true;
            speed = 5;
            ani.SetBool("Holstered", true);
        }
        else
        {
            highSpeed = false;
            speed = 3;
            ani.SetBool("Holstered", false);
        }
    }

    void Move()
    {
        //获取水平轴输入 -1 0 1
        float horizontal = Input.GetAxis("Horizontal");
        //获取垂直轴输入
        float vertical = Input.GetAxis("Vertical");
        //创建向量
        Vector3 dir = (transform.forward * vertical + transform.right * horizontal).normalized;
        //速度
        velocity = dir * speed;
        velocity.y = rb.velocity.y;
        //移动动画
        ani.SetFloat("Movement", dir.magnitude);

        // ———— 新增：脚步声控制逻辑 ————
        // 判断玩家是否正在输入移动指令
        bool isMoving = horizontal != 0 || vertical != 0;

        // 只有在移动且在地面上时，才播放脚步声
        if (isMoving && IsGround())
        {
            if (!audioSource.isPlaying && stepSound != null)
            {
                audioSource.clip = stepSound;
                audioSource.Play();
            }
        }
        else
        {
            // 停止移动或在空中跳跃时，停止播放脚步声
            audioSource.Stop();
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGround())
        {
            jump = true;
        }
    }

    void Aim()
    {
        if (Input.GetMouseButton(1))
        {
            isAiming = true;
            ani.SetBool("Aim", true);
            float aim = ani.GetFloat("Aiming");
            ani.SetFloat("Aiming", Mathf.Lerp(aim, 1, 0.1f));
        }
        else
        {
            isAiming = false;
            ani.SetBool("Aim", false);
            float aim = ani.GetFloat("Aiming");
            ani.SetFloat("Aiming", Mathf.Lerp(aim, 0, 0.1f));
        }
    }

    public bool IsGround()
    {
        RaycastHit hit;
        bool res = Physics.Raycast(transform.position + Vector3.up * 0.2f, -Vector3.up, out hit,
            0.4f, LayerMask.GetMask("Ground"));
        return res;
    }

    private void FixedUpdate()
    {
        if (jump)
        {
            jump = false;
            velocity.y = jumpForce;
        }
        rb.velocity = velocity;
    }
}