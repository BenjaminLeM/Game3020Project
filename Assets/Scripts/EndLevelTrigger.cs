using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    GameManager m_gameManager;

    private void Awake()
    {
        m_gameManager = FindAnyObjectByType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_gameManager.winGame();
        }
    }
}
