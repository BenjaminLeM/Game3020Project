using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShotGunPickUpBehaviour : BasePickUpBehaviour
{
    PickUp m_pickup;
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
        FindAnyObjectByType<PlayerCharacter>().gameObject.AddComponent<ShotgunJump>();
        FindAnyObjectByType<PlayerController>().setGunComp(FindAnyObjectByType<PlayerCharacter>().GetComponent<ShotgunJump>());
        GameObject gun = Instantiate(Resources.Load<GameObject>("Models/ShortDoubleBarrel"));
        gun.transform.parent = FindAnyObjectByType<PlayerCharacter>().transform.GetChild(0).GetChild(0);
        gun.transform.position = gun.transform.parent.position + gun.transform.parent.forward - (gun.transform.parent.up * 0.25f);
        gun.transform.forward = -gun.transform.parent.forward;

        StartCoroutine(despawnObject(0));
    }

    IEnumerator despawnObject(float delay)
    {
        GetComponent<Collider>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
