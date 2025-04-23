using System.Collections.Generic;
using UnityEngine;

public class Maze2Generator : MonoBehaviour
{
    [SerializeField]
    private PrefabDatabase prefabDatabase; // Reference to the prefab database
    [SerializeField]
    private int mazeX = 59; // Width of the maze
    [SerializeField]
    private int mazeY = 59; // Height of the maze
    [SerializeField]
    private Transform mazeGroup; // Parent transform for the maze cells
    private Maze2Cell[,] mazeCellMap; // 2D array to store the maze cells
    List<Maze2Cell> unvisitedCells = new List<Maze2Cell>(); // List to keep track of unvisited cells
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateMaze(); // Start generating the maze
    }

    void GenerateMaze()
    {
        mazeCellMap=new Maze2Cell[mazeX, mazeY]; // Initialize the maze cell map
        for(int i=0;i<mazeX;i++){
            for(int j=0;j<mazeY;j++){
                Maze2Cell cell=Instantiate(prefabDatabase.prefabList[1],mazeGroup).GetComponent<Maze2Cell>();
                cell.transform.position=new Vector3(i*cell.mazeSize,0,j*cell.mazeSize); // Set the position of the cell
                cell.Init(i,j);
                mazeCellMap[i,j]=cell; // Store the cell in the maze cell map
            }
        }
        //pick a starting cell
        Maze2Cell startCell = mazeCellMap[1,1];
        unvisitedCells.Add(startCell); // Add the starting cell to the unvisited cells list
        RecursiveRandomPrim(startCell); // Start the recursive random Prim's algorithm
    }
    void RecursiveRandomPrim(Maze2Cell startCell){

    }
    List<Maze2Cell> GetNeighbors(Maze2Cell cell){
        List<Maze2Cell> neighbors = new List<Maze2Cell>();
        // Add logic to get neighboring cells
        return neighbors;
    }
}
