using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    [SerializeField] private List<GameObject> squares = default;
    private int index = 0;


    public void AnimationOnClick()
    {
        if (index == squares.Count)
        {
            index = 0;
        }

        squares[index].GetComponent<SquareMechanics_Gameboard>().MoveAlongRoute(1f);
        index++;
    }


}
