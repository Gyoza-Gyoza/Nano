using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    public static Menus instance;

    [SerializeField]
    private GameObject pauseMenu;

    public GameObject PauseMenu
    { get { return pauseMenu; } }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
            Time.timeScale = pauseMenu.activeInHierarchy ? 0f : 1f;
        }
    }
}
