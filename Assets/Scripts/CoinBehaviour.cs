using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : BasePickUpBehaviour
{
    PickUp m_pickup;

    GameManager gameManager;
    private void Awake()
    {
        if (transform.childCount > 0)
            pickupVisual = transform.GetChild(0);
        TryGetComponent<PickUp>(out m_pickup);
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void Start()
    {
        if (m_pickup != null)
        {
            m_pickup.SetPickUpBehaviour(this);
        }
    }

    public override void PickUpAction()
    {
        gameManager.addScore(100);
        Destroy(gameObject);
    } 
}
