using UnityEngine;

public enum Maze2TunnelDirectionIndicator { 
    None, 
    Up, 
    Down, 
    Left, 
    Right 
}
public class Maze2Cell : MonoBehaviour
{
    public Maze2TunnelDirectionIndicator tunnelDirection=Maze2TunnelDirectionIndicator.None; // Direction of the tunnel
    [HideInInspector]
    public bool isVisited = false; // Flag to check if the cell has been visited
    public float mazeSize = 5; // Size of the maze cell
    public GameObject wall; // Array of wall GameObjects
    [HideInInspector]
    public int locX;
    [HideInInspector]
    public int locY; // Location of the cell in the maze
    public void Init(int x, int y)
    {
        locX = x; // Set the X coordinate of the cell
        locY = y; // Set the Y coordinate of the cell
    }
}
