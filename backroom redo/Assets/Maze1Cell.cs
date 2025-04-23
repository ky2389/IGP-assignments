using UnityEngine;

public class Maze1Cell : MonoBehaviour
{
    [HideInInspector]
    public bool isVisited = false; // Flag to check if the cell has been visited
    public float mazeSize=5;
    //0=Left, 1=Right, 2=Up, 3=Down
    public GameObject[] walls;
    [HideInInspector]
    public int locX;
    [HideInInspector]
    public int locY;
    public void Init(int x, int y)
    {
        locX = x;
        locY = y;
    }
}
