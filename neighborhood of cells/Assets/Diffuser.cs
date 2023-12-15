using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Diffuser : MonoBehaviour{
    public Material mat;
    private Texture2D texture;
    private float[,] grid, nextGrid;
    private int width = 256;
    private int height = 256;
    public float aliveLow = 0.55f;
    public float aliveHigh = 0.45f;
    public float change = 0.1f;
    public int R = 4;
    public int r = 3;

    int mod(int x, int m) {
        int n = x % m;
        return n < 0 ? n + m : n;
    }

    float update(int x, int y){
        float sum = 0;
        int t = 0;
        for(int i = -R; i <= R; i++){
            for(int j = -R; j <= R; j++){
                int distance = i*i + j*j;
                if(distance < R * R && distance > r * r){
                    sum += grid[mod(x + i, width), mod(y + j, height)];
                    t++;
                }
            }
        }
        return ((sum > t*aliveLow && sum < t*aliveHigh) ? change : -change);
    }

    void Start(){
        //Intialize grid and texture
        texture = new Texture2D(width, height);
        grid = new float[width, height];
        //fill grid and texture
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                texture.SetPixel(x, y, Color.red);
                grid[x, y] = Random.Range(0f, 1f);
            }
        }
        texture.Apply(); //not sure if this is needed
        mat.SetTexture("_MainTex", texture); //sends texture
    }


    void Update(){
        //Debug.Log(Input.mousePosition);
        for(int i = 0; i < 3; i++){
            nextGrid = new float[width, height];
            for(int x = 0; x < width; x++){
                for(int y = 0; y < height; y++){
                    //Generate next Grid using current grid
                    nextGrid[x, y] = Mathf.Max(Mathf.Min(grid[x, y] + update(x,y), 1), 0);
                }
            }
            grid = nextGrid;
        }
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                //apply next Grid to texture
                texture.SetPixel(x, y, Color.Lerp(Color.green, Color.blue, nextGrid[x, y]));
            }
        }
        texture.Apply();
    }
}
