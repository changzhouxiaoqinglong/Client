using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    /// <summary>
    /// 跟随目标
    /// </summary>
    private Transform target;

    public float height = 400f;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, target.position.y + height , target.position.z);
        }
    }

    /// <summary>
    /// 设置目标
    /// </summary>
    /// <param name="target">车的transform</param>
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    /// <summary>
    /// 获取当前target的角度
    /// </summary>
    /// <returns>返回vector3</returns>
    public Vector3 GetAngle()
    {
        return target == null ? Vector3.zero : target.eulerAngles;
    }
    /// <summary>
    /// 获取当前target的坐标
    /// </summary>
    /// <returns>返回vector3</returns>
    public Vector3 GetPoint()
    {
        return target == null ? Vector3.zero : target.position;
    }

    /// <summary>
    /// 设置摄像机与地面的距离
    /// </summary>
    /// <param name="size">距离</param>
    public void SetOrthSize(float size)
    {
        transform.GetComponent<Camera>().orthographicSize = size;
    }

}
