using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Pyramid_BlockType
{
    None = 0,
    Red,
    White,
    Green,
    Blue,
    Purple,
    Joker,
    Max,
}
public class Pyramid_Block : GameObjectBase
{
    [SerializeField]
    SpriteRenderer image;

    [SerializeField]
    Animator animator;

    public Pyramid_BlockType BlockType { private set; get; }

    public void Init(Pyramid_BlockType blockType)
    {
        BlockType = blockType;
        image.sprite = Pyramid_Main.instance.GetBlockImage(blockType);
    }

    public void ShowSituateAnimation()
    {
        animator.Play("Appear");
    }

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

    //        if (hit.collider != null)
    //        {
    //            if(hit.collider.gameObject == gameObject)
    //                Debug.Log(blockType + ". Clicked");
    //        }

    //    }
    //}
}
