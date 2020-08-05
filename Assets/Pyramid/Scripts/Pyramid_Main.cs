using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System.Linq;

public class GameObjectBase : MonoBehaviour
{
    public Transform cachedTransform { protected set; get; }

    protected virtual void Awake()
    {
        cachedTransform = transform;
    }
}

public class Pyramid_Main : GameObjectBase
{
    public static Pyramid_Main instance { private set; get; }

    readonly int maxHeight = 8;
    readonly int totalBlockCount = 36;
    readonly int playerCount = 3;

    [SerializeField]
    Pyramid_TurnManager turnManager;

    [SerializeField]
    private Pyramid_Board tempBoard;

    [SerializeField]
    private Transform boardParent;

    //[SerializeField]
    //private Transform blockParent;

    [SerializeField]
    private SpriteAtlas blockAtlas;

    [SerializeField]
    private int gameCycleCount = 3;

    List<List<Pyramid_Board>> boards = new List<List<Pyramid_Board>>();
    List<Pyramid_Player> players = new List<Pyramid_Player>();

    List<Pyramid_Board> situatableBoards;

    public int CurrentGameCount { private set; get; }
    public int GameCycleCount { get { return gameCycleCount; } }
    public bool isPlayFirstTime { private set; get; }

    public List<Pyramid_Player> Players { get { return players; } }

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }

    private void Start()
    {
        CurrentGameCount = 1;
        SetBoards();
        SetPlayers();
        SuffleAndGiveBlocks();

        turnManager.Init(players);

        isPlayFirstTime = PlayerPrefs.GetInt("PlayFirstTime", 0) == 0;

        if (isPlayFirstTime)
            Pyramid_UIManager.instance.OpenHowToPlayPopup();
    }

    void SetBoards()
    {
        int currentHeight = maxHeight;

        for (int y = 0; y < maxHeight; y++)
        {
            boards.Add(new List<Pyramid_Board>());
            
            for (int x = 0; x < currentHeight; x++)
            {
                Pyramid_Board board = Instantiate<Pyramid_Board>(tempBoard, boardParent);
                board.Init(new Vector2(x, y));
                boards[y].Add(board);
            }

            currentHeight--;
        }
    }

    void SetPlayers()
    {
        players.Add(new Pyramid_Player());
        players.Add(new Pyramid_Computer());
        players.Add(new Pyramid_Computer());
    }

    void SuffleAndGiveBlocks()
    {
        List<int> startBlockIdxs = new List<int>();
        for(int i = 0; i < totalBlockCount; i++)
        {
            startBlockIdxs.Add(i);
        }

        //조커1
        //5종류 종류별로 7개. 3명한테 분배.

        List<int>[] playerBlockIdxs = new List<int>[playerCount];
        List<Pyramid_BlockType>[] typeList = new List<Pyramid_BlockType>[playerCount];

        int startBlockCountEach = totalBlockCount / playerCount;
        for (int i = 0; i < playerCount; i++)
        {
            playerBlockIdxs[i] = new List<int>();
            typeList[i] = new List<Pyramid_BlockType>();
            for (int j = 0; j < startBlockCountEach; j++)
            {
                int randomIdx = Random.Range(0, startBlockIdxs.Count);
                int blockNum = startBlockIdxs[randomIdx];
                playerBlockIdxs[i].Add(blockNum);
                startBlockIdxs.RemoveAt(randomIdx);

                typeList[i].Add(GetBlockTypeByStartIndex(blockNum));
                //Debug.Log(blockNum + " / type = " + GetBlockTypeByStartIndex(blockNum));
            }
            //Debug.Log("player" + i + ". blocks = " + string.Join(",", playerBlockIdxs[i].ToArray()));
            //Debug.Log("player" + i + ". blockCount = " + playerBlockIdxs[i].Count);

            if (i == 0)
            {
                Pyramid_UIManager.instance.SetPlayerBlocks(typeList[i]);
            }

            players[i].Init((Turn)i, typeList[i], Pyramid_UIManager.instance.playerBlocks);
        }
    }

    Pyramid_BlockType GetBlockTypeByStartIndex(int startIdx)
    {
        return (Pyramid_BlockType)((startIdx / 7) + 1);
    }


    public Sprite GetBlockImage(Pyramid_BlockType blockType)
    {
        return blockAtlas.GetSprite(blockType.ToString());
    }

    public Sprite GetSelectedBlockImage(Pyramid_BlockType blockType)
    {
        return blockAtlas.GetSprite(blockType.ToString() + "_Selected");
    }

    public void Restart()
    {
        CurrentGameCount = 1;
        ResetPlayersScore();

        HideAllSituatableBoards();
        SuffleAndGiveBlocks();
        RemoveAllBlocks();
        turnManager.Restart();
    }

    public void NextGame()
    {
        Debug.Log("NextGame");
        StartCoroutine(NextGameRoutine());
    }

    IEnumerator NextGameRoutine()
    {
        yield return null;

        yield return new WaitForSeconds(1f);

        CurrentGameCount++;
        Pyramid_UIManager.instance.NextGame();
        HideAllSituatableBoards();
        SuffleAndGiveBlocks();
        RemoveAllBlocks();
        turnManager.Restart();
    }

    void ResetPlayersScore()
    {
        for(int i = 0; i < players.Count; i++)
        {
            players[i].ResetScore();
        }
    }

    void RemoveAllBlocks()
    {
        for (int y = 0; y < maxHeight; y++)
        {
            for (int x = 0; x < boards[y].Count; x++)
            {
                boards[y][x].RemoveBlock();
            }
        }
    }

    public void CheckHasSituatableBlocks()
    {
        //GetSituatableTypes();
    }

    public HashSet<Pyramid_BlockType> GetSituatableTypes()
    {
        HashSet<Pyramid_BlockType> situatableTypes = new HashSet<Pyramid_BlockType>();

        if(boards[0].Exists(board => board.Block == null))
        {
            for(int i = 1; i < (int)Pyramid_BlockType.Max; i ++)
            {
                situatableTypes.Add((Pyramid_BlockType)i);
            }
            return situatableTypes;
        }

        situatableTypes.Add(Pyramid_BlockType.Joker);

        for (int y = maxHeight - 1; y >= 1; y--)
        {
            for (int x = 0; x < boards[y].Count; x++)
            {
                if (boards[y][x].Block == null)
                {
                    if (boards[y - 1][x].Block != null && boards[y - 1][x + 1].Block != null)
                    {
                        if (boards[y - 1][x].Block.BlockType != Pyramid_BlockType.Joker && boards[y - 1][x + 1].Block.BlockType != Pyramid_BlockType.Joker)
                        {
                            situatableTypes.Add(boards[y - 1][x].Block.BlockType);
                            situatableTypes.Add(boards[y - 1][x + 1].Block.BlockType);
                        }
                    }
                }
            }
        }

        return situatableTypes;
    }

    public void OnBlockSelected(Pyramid_BlockType blockType)
    {
        HideAllSituatableBoards();
        CheckAndShowSituatableBoards(blockType);
    }
    
    public void HideAllSituatableBoards()
    {
        if (situatableBoards != null)
        {
            for (int j = 0; j < situatableBoards.Count; j++)
            {
                situatableBoards[j].HideCanSelect();
            }
        }
    }
    
    void CheckAndShowSituatableBoards(Pyramid_BlockType blockType)
    {
        situatableBoards = GetSituatableBoards(blockType);

        if(situatableBoards.Count == 0)
        {
            //Game End.
        }

        for (int i = 0; i < situatableBoards.Count; i++)
        {
            situatableBoards[i].ShowCanSelect(() =>
            {
                players[(int)Turn.Player].RemoveSelectedBlock();
                HideAllSituatableBoards();
                Pyramid_UIManager.instance.OnBlockSituated(Pyramid_UIManager.instance.CurrentSelectedBlock);
                NextTurn();
            });
        }
    }

    public List<Pyramid_Board> GetSituatableBoards(Pyramid_BlockType blockType)
    {
        situatableBoards = new List<Pyramid_Board>();

        for (int y = maxHeight -1 ; y  >= 1; y--)
        {
            for(int x = 0; x < boards[y].Count; x++)
            {
                if (boards[y][x].Block == null)
                {
                    if (boards[y - 1][x].Block != null && boards[y - 1][x + 1].Block != null)
                    {
                        if(blockType == Pyramid_BlockType.Joker)
                        {
                            situatableBoards.Add(boards[y][x]);
                        }
                        else if (boards[y - 1][x].Block.BlockType == blockType || boards[y - 1][x + 1].Block.BlockType == blockType)
                        {
                            if (boards[y - 1][x].Block.BlockType != Pyramid_BlockType.Joker && boards[y - 1][x + 1].Block.BlockType != Pyramid_BlockType.Joker)
                            {
                                situatableBoards.Add(boards[y][x]);
                            }
                        }
                    }
                }
            }
        }

        situatableBoards.AddRange(boards[0].Where(board => board.Block == null));

        return situatableBoards;
    }

    public void NextTurn()
    {
        turnManager.NextTurn();
    }
}
