using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pyramid_Player
{
    protected int score = 0;
    protected Turn playerType;
    protected List<Pyramid_BlockType> blocks = new List<Pyramid_BlockType>();
    private List<Pyramid_UIBlock> uIBlocks;

    public int Score { get { return score; } }

    public void Init(Turn playerType, List<Pyramid_BlockType> blocks, List<Pyramid_UIBlock> uiBlocks)
    {
        this.playerType = playerType;
        this.blocks = blocks;
        this.uIBlocks = uiBlocks;

        Pyramid_UIManager.instance.UpdateLeftBlockCount(playerType, blocks.Count);
        Pyramid_UIManager.instance.UpdateScore(playerType, score);
    }

    public void ResetScore()
    {
        score = 0;
    }

    public virtual void StartTurn()
    {
        Pyramid_UIManager.instance.OnStartPlayerTurn(Pyramid_Main.instance.GetSituatableTypes());
    }

    public virtual void EndTurn()
    {
        Pyramid_UIManager.instance.OnEndPlayerTurn();
    }

    public void RemoveSelectedBlock()
    {
        blocks.Remove(blocks.First(b => b == Pyramid_UIManager.instance.CurrentSelectedBlockType));
        score += 10;
        Pyramid_UIManager.instance.UpdateScore(playerType, score);
    }

    protected List<Pyramid_BlockType> randomTypes = new List<Pyramid_BlockType>();
    public bool HasSituatableBlock(HashSet<Pyramid_BlockType> situatableTypes)
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            if (situatableTypes.Contains(blocks[i]))
                return true;
        }

        return false;
    }

    public void SetRandomTypes(HashSet<Pyramid_BlockType> situatableTypes)
    {
        randomTypes.Clear();

        for (int i = 0; i < blocks.Count; i++)
        {
            if (situatableTypes.Contains(blocks[i]) && !randomTypes.Contains(blocks[i]))
                randomTypes.Add(blocks[i]);
        }
    }

    protected Pyramid_BlockType situateType;
    public void SituateRandomBlockByTimeOver()
    {
        if (randomTypes.Count > 0)
        {
            Pyramid_Main.instance.HideAllSituatableBoards();
            SituateRandomBlock();
            Pyramid_UIManager.instance.OnBlockSituated(uIBlocks.First(b => b.BlockType == situateType));
        }
    }

    protected void SituateRandomBlock()
    {
        if (randomTypes.Count > 0)
        {
            //Debug.Log("Computer randomTypes: " + string.Join(",", randomTypes.ToArray()));
            GetRandomSituateType();
            //Debug.Log("Computer situateType: " + situateType);

            List<Pyramid_Board> situatableBoards = Pyramid_Main.instance.GetSituatableBoards(situateType);

            int situateBoardIdx = Random.Range(0, situatableBoards.Count);

            //Debug.Log("Computer situateBoardIdx: " + situateBoardIdx + " / situatableBoards.Count: " + situatableBoards.Count);

            situatableBoards[situateBoardIdx].SetBlockByComputer(situateType);
            blocks.Remove(blocks.First(b => b == situateType));
            score += 10;
            Pyramid_UIManager.instance.UpdateScore(playerType, score);
        }
    }

    void GetRandomSituateType()
    {
        int randomIdx = Random.Range(0, randomTypes.Count);
        situateType = randomTypes[randomIdx];
    }
}
