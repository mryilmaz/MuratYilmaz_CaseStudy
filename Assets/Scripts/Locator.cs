using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locator : MonoBehaviour
{
    public float cubeSize = 0.05f;
    private void OnDrawGizmos()
    {
        
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        
        Gizmos.DrawCube(transform.position, new Vector3(cubeSize, cubeSize, cubeSize));
    }

}
