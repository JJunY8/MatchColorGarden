using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gem : MonoBehaviour
{
    [HideInInspector]
    public Sprite[] sprites;

    public Vector2Int posIndex;

    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private bool mousePressed;
    private float swipeAngle = 0;
   
    private Gem otherGem;
    private Gem addedGem;

    public enum GemType { empty, avocado, banana, coconut, dragonfruit, egg, kiwi, lychee, mango,
                          papaya, passionfruit, strawberry, watermelon }
    public GemType type;

    private Board board;

    private int index = 0;


    void Awake()
    {
        sprites = Resources.LoadAll<Sprite>("Images/fruit");
    }

    void Update()
    {
      
        if (Vector2.Distance(transform.position, posIndex) > .01f)
        {
            transform.position = Vector2.Lerp(transform.position, posIndex, board.moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(posIndex.x, posIndex.y, 0f);
            board.allGems[posIndex.x, posIndex.y] = this;
        }

        if (mousePressed && Input.GetMouseButtonUp(0))
        {
            mousePressed = false;

            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    public void SetupGem(Vector2Int pos, Board theBoard)
    {
        posIndex = pos;
        board = theBoard;
      
    }

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePressed = true;
    }

    private void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x);
        swipeAngle = swipeAngle * 180 / Mathf.PI;

        if (Vector3.Distance(firstTouchPosition, finalTouchPosition) > .4f)
        {
            MoveGem();
        }
    }

    private void MoveGem()
    {
        int count = System.Enum.GetValues(typeof(GemType)).Length - 3;

        Gem currentGem = board.allGems[posIndex.x, posIndex.y];
        SpriteRenderer spriteR = currentGem.GetComponent<SpriteRenderer>();
        

        if (swipeAngle < 45 && swipeAngle > -45 && posIndex.x < board.width - 1)
        {
            otherGem = board.allGems[posIndex.x + 1, posIndex.y];
            SpriteRenderer spriteother = otherGem.GetComponent<SpriteRenderer>();
            if (currentGem.type != 0)
            {
                if(currentGem.type == otherGem.type)
                {
                    otherGem.posIndex.x--;
                    otherGem.type = 0;
                    spriteother.sprite = sprites[(int)otherGem.type];
                    posIndex.x++;
                    if((int)currentGem.type < count)
                    {
                        currentGem.type += 3;
                        spriteR.sprite = sprites[(int)currentGem.type];
                    }
                }
                else
                {
                    otherGem.posIndex.x--;
                    posIndex.x++;
                }

            }
            
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && posIndex.y < board.height - 1)
        {
            otherGem = board.allGems[posIndex.x, posIndex.y + 1];
            SpriteRenderer spriteother = otherGem.GetComponent<SpriteRenderer>();
            if (currentGem.type != 0)
            {
                if (currentGem.type == otherGem.type)
                {
                    otherGem.posIndex.y--;
                    otherGem.type = 0;
                    spriteother.sprite = sprites[(int)otherGem.type];
                    posIndex.y++;
                    if ((int)currentGem.type < count)
                    {
                        currentGem.type += 3;
                        spriteR.sprite = sprites[(int)currentGem.type];
                    }
                }
                else
                {
                    otherGem.posIndex.y--;
                    posIndex.y++;
                    //持失
                }
                
            }
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && posIndex.y > 0)
        {
            otherGem = board.allGems[posIndex.x, posIndex.y - 1];
            SpriteRenderer spriteother = otherGem.GetComponent<SpriteRenderer>();
            if (currentGem.type != 0)
            {

                if (currentGem.type == otherGem.type)
                {
                    otherGem.posIndex.y++;
                    otherGem.type = 0;
                    spriteother.sprite = sprites[(int)otherGem.type];
                    posIndex.y--;
                    if ((int)currentGem.type < count)
                    {
                        currentGem.type += 3;
                        spriteR.sprite = sprites[(int)currentGem.type];
                    }
                }
                else
                {
                    otherGem.posIndex.y++;
                    posIndex.y--;
                    //持失
                }
                
            }
        }
        else if (swipeAngle > 135 || swipeAngle < -135 && posIndex.x > 0)
        {
            otherGem = board.allGems[posIndex.x - 1, posIndex.y];
            SpriteRenderer spriteother = otherGem.GetComponent<SpriteRenderer>();
            if (currentGem.type != 0)
            {
                if (currentGem.type == otherGem.type)
                {
                    otherGem.posIndex.x++;
                    otherGem.type = 0;
                    spriteother.sprite = sprites[(int)otherGem.type];
                    posIndex.x--;
                    if ((int)currentGem.type < count)
                    {
                        currentGem.type += 3;
                        spriteR.sprite = sprites[(int)currentGem.type];
                    }
                }
                else
                {
                    otherGem.posIndex.x++;
                    posIndex.x--;
                    //持失
                }
                
            }
        }
        
        
        board.allGems[posIndex.x, posIndex.y] = this;
        board.allGems[otherGem.posIndex.x, otherGem.posIndex.y] = otherGem;
        AddGem();

    }

    private void AddGem()
    {
        int[] usingSprite = board.stage.stats[board.stagenumber].UsingGems;
        int choseGem;
        int choseSprite;
        List<Vector2Int> emptyBlocks = new List<Vector2Int>();
        
        try
        {
            for (int a = 0; a < board.stage.stats[board.stagenumber].width; a++)
            {
                for (int k = 0; k < board.stage.stats[board.stagenumber].height; k++)
                {
                    if (board.allGems[a, k].type == 0)
                    {
                        emptyBlocks.Add(board.allGems[a, k].posIndex);
                    }
                }
            }

            choseGem = Random.Range(0, emptyBlocks.Count);
            choseSprite = Random.Range(1, usingSprite.Length);
            Vector2Int choseGemPosition = emptyBlocks[choseGem];

            SpriteRenderer spriteR = board.allGems[choseGemPosition.x, choseGemPosition.y].GetComponent<SpriteRenderer>();
            spriteR.sprite = sprites[choseSprite];
            board.allGems[choseGemPosition.x, choseGemPosition.y].type = (GemType)choseSprite;
        }
        catch
        {
            Debug.Log("GameOver");
            SceneManager.LoadScene("Gameover");
        }

        index++;

    }

}
