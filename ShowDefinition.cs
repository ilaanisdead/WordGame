using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDefinition : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // playing animation for the gameobject that shows defns, antonyms and synonyms
    void OnEnable(){
        animator.SetTrigger("show");
    }
}
