using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Transform unityChan;
    Vector3 move;
    public float moveSpeed;


    public RectTransform pad;
    bool walking;

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        transform.localPosition = Vector2.ClampMagnitude(eventData.position - (Vector2)pad.position, pad.rect.width * 0.5f);

        move = new Vector3(transform.localPosition.x, 0, transform.localPosition.y).normalized;

        if (!walking)
        {
            walking = true;
            unityChan.GetComponent<Animator>().SetBool("Walk", true);
        }

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        move = Vector3.zero;
        StopCoroutine("PlayerMove");

        walking = false;
        unityChan.GetComponent<Animator>().SetBool("Walk", false);

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine("PlayerMove");
    }

    IEnumerator PlayerMove()
    {
        
        while(true)
        {
            unityChan.Translate(move * moveSpeed * Time.deltaTime, Space.World);

            if (move != Vector3.zero)
                unityChan.rotation = Quaternion.Slerp(unityChan.rotation,Quaternion.LookRotation(move),5*Time.deltaTime); 
            yield return null;
        }
    }

   
}
