using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Valve.VR.InteractionSystem;

public class HeightSlider : Slider {

    GameObject handle;
    GameObject currentHand;
    Hand leftHand;
    Hand rightHand;
    GetHandUIElement getHand;
    public bool anchoring;
    Vector3 newPos;
    Vector3 offset;
    float least;
    float most;

    protected override void Start()
    {
        base.Start();
        currentHand = null;
        leftHand = GameObject.Find("LeftHand").GetComponent<Hand>();
        rightHand = GameObject.Find("RightHand").GetComponent<Hand>();
        least = 0;
        most = GetComponent<RectTransform>().sizeDelta.x - handle.GetComponent<RectTransform>().sizeDelta.x;
        getHand = GetComponent<GetHandUIElement>();
        anchoring = false;
    }

    private void Update()
    {
        if (!anchoring) { return; }

        handle.transform.position = currentHand.transform.position + offset;
        // newPos.x = Mathf.Clamp(newPos.x, least, most);
        // handle.transform.position = newPos;
        // value = newPos.x / most;
        print(value);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        if (rightHand.grabPinchAction.GetStateDown(rightHand.handType)) { currentHand = rightHand.gameObject; }
        else if (leftHand.grabPinchAction.GetStateDown(leftHand.handType)) { currentHand = leftHand.gameObject; }
        else { return; }
       
        anchoring = true;
        offset = currentHand.transform.position - handle.transform.position;
        print("got");
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        currentHand = null;
        anchoring = false;
        print("deselect");
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        print("enter");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        print("exit");
    }
}
