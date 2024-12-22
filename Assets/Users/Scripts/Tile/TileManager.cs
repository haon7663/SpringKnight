using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class TileManager : MonoBehaviour
{
    public static TileManager Inst { get; private set; }
    void Awake() => Inst = this;
    [Serializable]
    public struct TileWithObstacle { 
        public GameObject tileMap;
        public int obstacleCount;
    }
    [Serializable]
    public struct TileMaps { public TileWithObstacle[] tileMaps;}
    public TileMaps[] stageTileMaps;

    [Serializable]
    public struct Tile
    { 
        public bool onTile;
        public Vector2 position;
    }
    public Tile[] tiles;

    public int tileSize;
    [SerializeField] Transform tileGrid;
    [SerializeField] GameObject obstacle_SlimeBlock;

    void Start()
    {
        var tileWithobstacle = stageTileMaps[GameManager.Inst.curStage].tileMaps[GameManager.Inst.curPhase];
        GameObject tile = Instantiate(tileWithobstacle.tileMap);
        tile.transform.SetParent(tileGrid);

        var tileIndex = int.Parse(tile.name.Substring(0, tile.name.IndexOf('-')));
        tileSize = tileIndex;
        float scale = tileIndex / 8f;
        Movement.Inst.tileMultiSpeed = Mathf.Lerp(scale, 1, 0.5f);

        var sizeX = tileSize - 2;
        var sizeY = sizeX * 2;
        tiles = new Tile[sizeX * sizeY];

        for (int i = 0; i < tiles.Length; i++)
        {
            int quotient = Mathf.FloorToInt((float)i / sizeX);
            int remain = i % sizeX;
            float posX = -sizeX / 2 + remain + 0.5f;
            float posY = sizeY / 2 - quotient - 0.5f;

            tiles[i].position = new Vector2(posX, posY);
        }

        for (int i = 0; i < tileWithobstacle.obstacleCount; i++)
        {
            var mOp = Random.Range(0, 2) == 0 ? -1 : 1;
            var spawnPos = new Vector2(mOp * ((float)tileSize) / 2, Random.Range(-(tileSize - 2), tileSize - 1));
            GameObject obstacle = Instantiate(obstacle_SlimeBlock, spawnPos, Quaternion.identity);
            obstacle.transform.localScale = new Vector2(mOp, 1);
        }
    }

    public void TakeTile(int value, bool onTile)
    {
        tiles[value].onTile = onTile;
    }
}
