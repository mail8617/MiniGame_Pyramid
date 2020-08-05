using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pyramid_Board : GameObjectBase
{
    readonly Vector2 boardSize = new Vector2(0.7f, 0.7f);
    
    [SerializeField]
    GameObject selectImage;

    [SerializeField]
    private Pyramid_Block tempBlock;

    bool canSelect = false;

    public Pyramid_Block Block { private set; get; }
    public Vector2 Index { private set; get; }

    public void Init(Vector2 idx)
    {
        this.Index = idx;
        SetPosition();
    }

    void SetPosition()
    {
        float xOffSet = Index.y * (boardSize.x * 0.5f);
        cachedTransform.localPosition = new Vector3(Index.x * boardSize.x + xOffSet, Index.y * boardSize.y);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (canSelect)
                    {
                        //Debug.Log(Index + ". Board Clicked");
                        OnSelected();
                    }
                }
            }
        }
    }

    void OnSelected()
    {
        canSelect = false;
        SetBlockByPlayer();
    }

    void SetBlockByPlayer()
    {
        SetBlock(Pyramid_UIManager.instance.CurrentSelectedBlockType);
        onSelected?.Invoke();
    }

    public void SetBlockByComputer(Pyramid_BlockType blockType)
    {
        SetBlock(blockType);
        Pyramid_Main.instance.NextTurn();
    }

    void SetBlock(Pyramid_BlockType blockType)
    {
        //Debug.Log(Index +". SetBlock : " + blockType);
        Block = tempBlock.Spawn(cachedTransform, Vector3.zero);
        Block.Init(blockType);
        Block.cachedTransform.localScale = Vector3.one;
        Block.ShowSituateAnimation();
    }

    public void RemoveBlock()
    {
        Block?.Recycle();
        Block = null;
    }

    Action onSelected;
    public void ShowCanSelect(Action onSelected)
    {
        if (Block == null)
        {
            canSelect = true;
            selectImage.SetActive(true);
            this.onSelected = onSelected;
        }
    }

    public void HideCanSelect()
    {
        canSelect = false;
        selectImage.SetActive(false);
    }

    
}
