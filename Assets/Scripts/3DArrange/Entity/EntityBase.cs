using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    public bool isTransformGizmo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTransformGizmo)
            transform.position = new TerrainChangeMgr().GetTerrainPosByPos(transform.position);
    }
}
