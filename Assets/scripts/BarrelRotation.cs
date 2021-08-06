using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelRotation : MonoBehaviour
{
    public Transform pivot;
    public Transform barrel;

    public Soldier soldier;

    private void Update()
    {
        if (soldier != null && soldier.target != null)
        {
            Vector2 relative = soldier.target.transform.position - pivot.position;
            float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;

            Vector3 newRotation = new Vector3(0, 0, angle);
            pivot.localRotation = Quaternion.Euler(newRotation);
        }
    }
}
