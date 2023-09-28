using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeartSpawner : MonoBehaviour
{
    public void AddHeart(GameObject heartPre)
    {
        Instantiate(heartPre);
    }

    public bool RemoveHear(GameObject heartPre)
    {
        var _hearts = GetComponentsInChildren<GameObject>();
        if (_hearts.Length > 0)
        {
            Destroy(_hearts.FirstOrDefault());
            return true;
        }
        else
        {
            return false;
        }
    }
}