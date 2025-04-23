using UnityEngine;
using System.Collections.Generic;
public class Maze1Generator : MonoBehaviour
{
    [SerializeField]
    PrefabDatabase prefabDatabase; // Reference to the PrefabDatabase ScriptableObject
    [SerializeField]
    int mazeX=20;
    [SerializeField]
    int mazeY=20;
    [SerializeField]
    Transform mazeGroup;
    Maze1Cell[,] mazeCellMap; // 2D array to store the maze cells
    Stack<Maze1Cell> pathFindingCells= new Stack<Maze1Cell>(); // Stack to keep track of the pathfinding cells
    void Start()
    {
        GenerateMaze(); // Start generating the maze
    }
    void GenerateMaze(){
        mazeCellMap=new Maze1Cell[mazeX, mazeY]; // Initialize the maze cell map
        for(int i=0;i<mazeX;i++){
            for(int j=0;j<mazeY;j++){
                Maze1Cell cell=Instantiate(prefabDatabase.prefabList[0],mazeGroup).GetComponent<Maze1Cell>();
                cell.transform.position=new Vector3(i*cell.mazeSize,0,j*cell.mazeSize); // Set the position of the cell
                cell.Init(i,j);
                mazeCellMap[i,j]=cell; // Store the cell in the maze cell map
            }
        }
        RecursiveBackTracking(mazeCellMap[Random.Range(0,mazeX),Random.Range(0,mazeY)]); // Start the recursive backtracking algorithm from the first cell
    }
    void RecursiveBackTracking(Maze1Cell selectedCell){
        selectedCell.isVisited=true; // Mark the cell as visited
        List<Maze1Cell> unvisitedCells=new List<Maze1Cell>(); // List to store unvisited neighboring cells
        if(selectedCell.locX>0 && !mazeCellMap[selectedCell.locX-1,selectedCell.locY].isVisited){
            unvisitedCells.Add(mazeCellMap[selectedCell.locX-1,selectedCell.locY]); // Add left cell to unvisited cells
        }
        if(selectedCell.locX<mazeX-1 && !mazeCellMap[selectedCell.locX+1,selectedCell.locY].isVisited){
            unvisitedCells.Add(mazeCellMap[selectedCell.locX+1,selectedCell.locY]); // Add right cell to unvisited cells
        }
        if(selectedCell.locY>0 && !mazeCellMap[selectedCell.locX,selectedCell.locY-1].isVisited){
            unvisitedCells.Add(mazeCellMap[selectedCell.locX,selectedCell.locY-1]); // Add down cell to unvisited cells
        }
        if(selectedCell.locY<mazeY-1 && !mazeCellMap[selectedCell.locX,selectedCell.locY+1].isVisited){
            unvisitedCells.Add(mazeCellMap[selectedCell.locX,selectedCell.locY+1]); // Add up cell to unvisited cells
        }
        if(unvisitedCells.Count>0){ // If there are unvisited cells
            pathFindingCells.Push(selectedCell); // Push the current cell to the stack
            Maze1Cell nextCell=unvisitedCells[Random.Range(0,unvisitedCells.Count)]; // Randomly select a neighboring cell
            RemoveWall(selectedCell,nextCell); // Remove the wall between the current cell and the next cell
            RecursiveBackTracking(nextCell); // Recursively call the function for the next cell
        }else if(pathFindingCells.Count>0){ // If there are no unvisited cells and the stack is not empty
            Maze1Cell backCell=pathFindingCells.Pop(); // Pop the last cell from the stack
            RecursiveBackTracking(backCell); // Recursively call the function for the back cell
        }

    }
    void RemoveWall(Maze1Cell thisCell,Maze1Cell nextCell){
        if(thisCell.locX==nextCell.locX){ // If the cells are in the same column
            if(thisCell.locY<nextCell.locY){ // If the current cell is above the next cell
                thisCell.walls[3].SetActive(false); // Remove the down wall of the current cell
                nextCell.walls[2].SetActive(false); // Remove the up wall of the next cell
            }else{ // If the current cell is below the next cell
                thisCell.walls[2].SetActive(false); // Remove the up wall of the current cell
                nextCell.walls[3].SetActive(false); // Remove the down wall of the next cell
            }
        }else{ // If the cells are in different columns
            if(thisCell.locX>nextCell.locX){ // If the current cell is to the left of the next cell
                thisCell.walls[1].SetActive(false); // Remove the left wall of the current cell
                nextCell.walls[0].SetActive(false); // Remove the right wall of the next cell
            }else{ // If the current cell is to the right of the next cell
                thisCell.walls[0].SetActive(false); // Remove the right wall of the current cell
                nextCell.walls[1].SetActive(false); // Remove the left wall of the next cell
            }
        }
    }
}
