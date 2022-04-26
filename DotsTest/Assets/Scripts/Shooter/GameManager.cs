using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform player;
    public GameObject bullet;


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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static void DebugTest()
    {
        Debug.Log("hi there bullet spawner ");
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
