using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestScoreIcon : MonoBehaviour{
    public Sprite crownIcon;
    public Sprite arrowIcon;
    public Image mainIcon;
    public Image mainIconDim;

    private Vector3 startPosition;
    private Vector3 startScale;

    private RectTransform crownRect;
    private RectTransform crownDimRect;


    private void Start() {
        crownRect = mainIcon.GetComponent<RectTransform>();
        crownDimRect = mainIconDim.GetComponent<RectTransform>();
        startPosition = crownRect.anchoredPosition;
        startScale = crownRect.localScale;
    }

    public void EnableArrow() {
        //mainIcon.transform.position = new Vector3(startPosition.x+10f, startPosition.y + 35f, startPosition.z);
        //mainIconDim.transform.position = new Vector3(startPosition.x+10f, startPosition.y + 35f, startPosition.z);
        //mainIcon.transform.localScale = new Vector3(startScale.x+.1f, startScale.y+.1f, startScale.z+.2f);
        //mainIconDim.transform.localScale = new Vector3(startScale.x+.1f, startScale.y+.1f, startScale.z+.2f);
        crownRect.anchoredPosition = new Vector2(startPosition.x+6.1f, startPosition.y+18.3f);
        crownRect.localScale = new Vector2(startScale.x+ .076399f, startScale.y+.076399f);
        crownDimRect.anchoredPosition = new Vector2(startPosition.x+6.1f, startPosition.y+18.3f);
        crownDimRect.localScale = new Vector2(startScale.x + .076399f, startScale.y + .076399f);



        mainIcon.sprite = arrowIcon;
        mainIconDim.sprite = arrowIcon;
    }

    public void EnableCrown() {
        //mainIcon.transform.position = startPosition;
        //mainIcon.transform.localScale = startScale;
        //mainIconDim.transform.position = startPosition;
        //mainIconDim.transform.localScale = startScale;
        crownRect.anchoredPosition = startPosition;
        crownRect.localScale = startScale;
        crownDimRect.anchoredPosition = startPosition;
        crownDimRect.localScale = startScale;

        mainIcon.sprite = crownIcon;
        mainIconDim.sprite = crownIcon;
    }



}
