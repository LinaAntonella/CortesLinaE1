using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Candy3
{ 
public class BoardManager : MonoBehaviour
{
    public int boardSizeX;
    public int boardSizeY;
    public float tileOffSet = 1f;
    public GameObject tilePrefab;
    [SerializeField]
    private Vector2Int boardSize ;
    public Vector2Int[] candyDisable;
    [SerializeField]
    public Tile[,] tiles;

    public int algo;

    void Start()
    {
        boardSize = new Vector2Int(boardSizeX, boardSizeY);
        InitializeBoard();
    }

    void InitializeBoard()
    {
        
        Vector2 tilePosition = Vector2.zero;
        GameObject tile;
        Vector2Int boardPosition = Vector2Int.zero;

        tiles = new Tile[boardSize.x, boardSize.y];

        for (int x = 0; x < boardSize.x; x++)
        {
            for (int y = 0; y < boardSize.y; y++)
            {
                tilePosition.x = x * tileOffSet;
                tilePosition.y = y * tileOffSet;

                boardPosition.x = x;
                boardPosition.y = y;

                tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);


                tile.GetComponent<Tile>().Initialize(this, boardPosition);
                tile.GetComponent<Tile>().DisableCandySpot(this,candyDisable);

                tiles[x, y] = tile.GetComponent<Tile>();

            }
        }

    }

    public void SearchCombo(CandyData candyData)
    {
        int temppossibleCombo = 0;
        algo = temppossibleCombo;
        Tile[] tilestoDestroy = new Tile[boardSizeX * boardSizeY];
        for (int x = candyData.candyPosition.x; x < boardSize.x; x++)
        {

            if (tiles[x, candyData.candyPosition.y] == null)
                break;


            if (tiles[x, candyData.candyPosition.y].candyData.candyType != candyData.candyType || tiles[x, candyData.candyPosition.y].candyData.candyType == -1)
                break;


            else if (tiles[x, candyData.candyPosition.y].candyData.candyType == candyData.candyType)
            {

                tilestoDestroy[temppossibleCombo] = tiles[x, candyData.candyPosition.y];
                temppossibleCombo++;
                algo = temppossibleCombo;

            }
            

        }
        

        for (int x = candyData.candyPosition.x; x >= 0; x--)
        {
            if (tiles[x, candyData.candyPosition.y] == null)
                break;


            else if (tiles[x, candyData.candyPosition.y].candyData.candyType != candyData.candyType || tiles[x, candyData.candyPosition.y].candyData.candyType == -1)
                break;


            else if (tiles[x, candyData.candyPosition.y].candyData.candyType == candyData.candyType)
            {
                tilestoDestroy[temppossibleCombo] = tiles[x, candyData.candyPosition.y];
                temppossibleCombo++;
                algo = temppossibleCombo;
            }


        }
        

        for (int y = candyData.candyPosition.y; y < boardSize.y; y++)
        {

            if (tiles[candyData.candyPosition.x, y] == null)
                break;
            else
            if (tiles[candyData.candyPosition.x, y].candyData.candyType != candyData.candyType || tiles[candyData.candyPosition.x, y].candyData.candyType == -1)
                break;
            else
            if (tiles[candyData.candyPosition.x, y].candyData.candyType == candyData.candyType)
            {
            
                tilestoDestroy[temppossibleCombo] = tiles[candyData.candyPosition.x, y];
                temppossibleCombo++;
                algo = temppossibleCombo;

            }
            

        }
        
        

        for (int y = candyData.candyPosition.y; y >= 0; y--)
        {

            if (tiles[candyData.candyPosition.x, y] == null)
                break;
            else
            if (tiles[candyData.candyPosition.x, y].candyData.candyType != candyData.candyType || tiles[candyData.candyPosition.x, y].candyData.candyType == -1)
                break;
            else
            if (tiles[candyData.candyPosition.x, y].candyData.candyType == candyData.candyType)
            {
                tilestoDestroy[temppossibleCombo] = tiles[candyData.candyPosition.x, y];
                temppossibleCombo++;
                algo = temppossibleCombo;
            }              

        }
        
        if (tilestoDestroy != null)
        {
            tilestoDestroy[temppossibleCombo] = tiles[candyData.candyPosition.x, candyData.candyPosition.y];
            StartCoroutine(DestroyAllTileinCombo(tilestoDestroy));
            //StartCoroutine(PushTiles());
        }

        

    }

    IEnumerator DestroyAllTileinCombo(Tile[] tiles)
    {
        GameObject temp;
        for (int i = 0; i < tiles.Length; i++)
        {
            if(tiles[i] != null)
            {
                temp = tiles[i].gameObject;
                Vector2Int temppos;
                temppos = temp.GetComponent<Tile>().candyData.candyPosition;
                Destroy(temp);
            }
           
        }

        yield return new WaitForEndOfFrame();

        StartCoroutine(PushTiles());

        yield return new WaitForEndOfFrame();
        RefillBoard();
      
    }

    IEnumerator PushTiles()
    {
        GameObject tile;
        Vector2 tilePosition = Vector2.zero;
        Vector2Int boardPosition = Vector2Int.zero;
        for (int x = 0; x < boardSizeX; x++)
        {
            int tempy = 0;
            
            for (int y = 0; y < boardSizeY; y++)
            {               
                if(tiles[x,y] != null && y<boardSizeY && y >= 0)
                {
                    
                    Debug.Log(tiles[x, y].candyData.candyPosition + "pushtiles()");
                    tile = tiles[x, y].gameObject;
                    if(y > 0)
                    {    
                        tempy = y - 1;
                    }
                    
                    while(y > 0 && tiles[x,tempy]== null && y < boardSizeY)
                    {
                        if(y > 0)
                        {
                                tile.GetComponent<Tile>().PushTheTileDown(tile.GetComponent<Tile>().candyData.candyPosition, tileOffSet);
                                tiles[x, tempy] = tile.GetComponent<Tile>();
                                tiles[x, tempy + 1] = null;
                        }
                        
                        

                    }       
                }
            }
            tempy--;
            }
        yield return null;
    }

    public void CheckAndRefill(int pushxtimes)
    {
        
       
        PushTiles();
        
        
        
        
                    
    }
    public void RefillBoard()
    {
        GameObject tile;
        Vector2 tilePosition = Vector2.zero;
        Vector2Int boardPosition = Vector2Int.zero;
        for (int x = 0; x < boardSizeX; x++)
        {
            for (int y = 0; y < boardSizeY; y++)
            {
                tilePosition.x = x * tileOffSet;
                tilePosition.y = y * tileOffSet;

                boardPosition.x = x;
                boardPosition.y = y;
                if (tiles[x,y] == null)
                {
                    tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);


                    tile.GetComponent<Tile>().Initialize(this, boardPosition);
                    tiles[x, y] = tile.GetComponent<Tile>();
                }
            }
        }
    }

}
}

