using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mapCtr : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(int lv)
    {
        switch (lv)
        {
            case 1:
                SceneManager.LoadScene("lv1");
                break;    
            case 2:
                SceneManager.LoadScene("Lv2");
                break;
            default:
                SceneManager.LoadScene("Menu");
                break;
        }
    }
}
