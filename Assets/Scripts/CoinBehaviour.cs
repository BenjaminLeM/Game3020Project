using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : BasePickUpBehaviour
{
    PickUp m_pickup;

    GameManager gameManager;

    AudioSource audioSource;

    AudioClip coinPickUp;
    private void Awake()
    {
        if (transform.childCount > 0)
            pickupVisual = transform.GetChild(0);
        TryGetComponent<PickUp>(out m_pickup);
        gameManager = FindAnyObjectByType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        coinPickUp = Resources.Load<AudioClip>("SFX/CoinPickUpNoise");
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
        audioSource.PlayOneShot(coinPickUp, 0.3f);
        StartCoroutine(despawnCoin(coinPickUp.length));
    }

    IEnumerator despawnCoin(float delay) 
    {
        GetComponent<Collider>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
