using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform player;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static Vector3 GetPlayerPosition()
    {
        if (GameManager.Instance == null)
        {
            return Vector3.zero;
        }

        return (Instance.player != null) ? GameManager.Instance.player.position : Vector3.zero;
    }
}
