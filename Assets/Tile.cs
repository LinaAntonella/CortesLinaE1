using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Candy3{ 
public enum Directions {Up,Right,Down,Left}
[System.Serializable]
public struct CandyData
{
    public int candyType;
    public Vector2Int candyPosition;
    public Directions swipeDirection;

}
public class Tile : MonoBehaviour
{
    public CandyData candyData;
    public Sprite[] sprites;
    private BoardManager boardManager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
    }
    public virtual void OnMouseUp()
    {
        boardManager.SearchCombo(candyData);
    }
    public virtual void Initialize(BoardManager boardManagerReference, Vector2Int NewCandyPosition)
    {
        boardManager = boardManagerReference;
        candyData.candyPosition = NewCandyPosition;


        RandomCandyInitialization();
    }

    public virtual void DisableCandySpot(BoardManager boardManagerReference, Vector2Int[] candyPositions)
    {
        boardManager = boardManagerReference;
        for (int i = 0; i < candyPositions.Length; i++)
        {
            if(candyData.candyPosition == candyPositions[i])
            {
                candyData.candyType = -1;
                GetComponent<SpriteRenderer>().sprite = null;
            }
        }
    }
    public virtual void RandomCandyInitialization()
    {
        candyData.candyType = Random.Range(0,5);

        GetComponent<SpriteRenderer>().sprite = sprites[candyData.candyType];
    }

    public virtual void PushTheTileDown(Vector2Int candyPosition,float boardoffset)
    {
        if(candyData.candyPosition.y > 0 && candyData.candyType != -1)
        {
            candyData.candyPosition.y -= 1;
            transform.position = new Vector2(candyPosition.x * boardoffset, candyPosition.y * boardoffset);
        }
        
    }

    public virtual void DestroyTile()
    {
        Destroy(gameObject);
    }
}


}
