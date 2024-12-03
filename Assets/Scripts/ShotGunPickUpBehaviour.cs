using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShotGunPickUpBehaviour : BasePickUpBehaviour
{
    PickUp m_pickup;
    [SerializeField]
    GameObject gun;
    private void Awake()
    {
        if (transform.childCount > 0)
            pickupVisual = transform.GetChild(0);
        TryGetComponent<PickUp>(out m_pickup);
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
        GameObject player = FindAnyObjectByType<PlayerCharacter>().gameObject;
        player.GetComponent<PlayerController>().setGunComp(gun.GetComponent<ShotgunJump>());
        gun.GetComponent<ShotgunJump>().attached_rb = player.GetComponent<Rigidbody>();
        gun.transform.parent = FindAnyObjectByType<PlayerCharacter>().transform.GetChild(0).GetChild(0).GetChild(0);
        gun.transform.position = gun.transform.parent.position + gun.transform.parent.forward - (gun.transform.parent.up * 0.25f);
        gun.transform.forward = -gun.transform.parent.forward;

        Destroy(gameObject);
    }
}
