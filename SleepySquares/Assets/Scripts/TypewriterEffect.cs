using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypewriterEffect : MonoBehaviour{

    private string instructions;
    private TextMeshProUGUI txt;
    public float delayTime = 0f;
    public float textSpeed = 0.01f;
    private Coroutine coroutine;
    public string[] all_instructions;
    public int index = 0;
    [SerializeField] TutorialGoNext goNext = default;
    public bool writing = false;
    public bool go = false;
    public bool swap = false;
    public GameObject nextArrow = default;
    private bool lastArrow = false;
    public bool repairText = false;
    public bool swapTutorialFirst = false;
    [SerializeField] GameObject switchButton = default;
    [SerializeField] GameObject switchArrow = default;

    private void OnEnable()
    {

        txt = gameObject.GetComponent<TextMeshProUGUI>();
        TurnOnHitbox();
        coroutine = StartCoroutine(TypewriterEffectOnText());
    }

    private void GetInstructions()
    {
        txt.maxVisibleCharacters = 0;
        instructions = all_instructions[index].Trim();
        instructions = LocalisationSystem.GetLocalisedValue(instructions);
        txt.text = instructions;
        index++;
    }

    public void FinishTypingOrNextInstructionsOrNextTutorialOnClick()
    {
        if (writing)
        {
            FinishTyping();
        }
        else
        {
            if (index >= all_instructions.Length)
            {
                TurnOffHitbox();
                go = true;
            }
            else
            {
                coroutine = StartCoroutine(TypewriterEffectOnText());
            }

        }
    }

    IEnumerator TypewriterEffectOnText() {
        writing = true;
        nextArrow.SetActive(false);
        //Debug.Log("arrow off");

        GetInstructions();

        yield return new WaitForSeconds(delayTime);
        //int totalVisibleCharacters = instructions.Length+1;
        int totalVisibleCharacters = txt.textInfo.characterCount + 1;

        int counter = 0;
        while (counter < totalVisibleCharacters) {
            
            txt.maxVisibleCharacters = counter;
            counter++;
            yield return new WaitForSeconds(textSpeed);
        }
        writing = false;
        if (index != all_instructions.Length)
        {
            //Debug.Log("writing done");
            TurnOnArrow();
        }
    }

    public void FinishTyping()
    {
        StopCoroutine(coroutine);
        txt.maxVisibleCharacters = instructions.Length + 1;
        writing = false;
        if (index != all_instructions.Length)
        {
            //Debug.Log("finish");
            TurnOnArrow();
        }
    }

    private void TurnOnArrow()
    {
        nextArrow.SetActive(true);
        //Debug.Log("arrow on");
    }

    private void TurnOnHitbox()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    private void TurnOffHitbox()
    {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }


    private void Update()
    {
        RaycastForHitBox();

        if (go)
        {
            if (goNext.animationDone)
            {
                goNext.GoToNextOnClick();
            }
        }

        if (writing == false && goNext.animationDone && index == all_instructions.Length)
        {
            lastArrow = true;
        }


        if (writing == false && repairText == true && index == all_instructions.Length)
        {
            lastArrow = true;
        }
        

        if (lastArrow)
        {
            //Debug.LogWarning("last");
            TurnOnArrow();
        }

        if (swapTutorialFirst == true)
        {
            if(writing == false && index == all_instructions.Length)
            {
                switchButton.GetComponent<Image>().raycastTarget = true;
                switchButton.GetComponent<Button>().interactable = true;
                switchArrow.SetActive(true);
            }
        }
    }

    private void RaycastForHitBox()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask_hitBox = LayerMask.NameToLayer(layerName: "instructionsHitBox");
            RaycastHit2D hit_onHitBox = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << layerMask_hitBox);

            if (hit_onHitBox.collider != null)
            {
                FinishTypingOrNextInstructionsOrNextTutorialOnClick();
            }
        }
    }

}
