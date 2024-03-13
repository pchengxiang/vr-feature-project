using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardRoom
{
    public bool[] status = new bool[4];
    public int room_number;
    public int board_number;
}

public class DungeonPatternGenerator : MonoBehaviour
{     
    public List<GameObject> rooms;
    public int size_x;
    public int size_y;
    BoardRoom[] board;
    public void CreateSimpleDungeonPattern()
    {
        board = new BoardRoom[size_x * size_y];
        for (int i = 0; i < size_y; i++)
        {
            for (int j = 0; j < size_x; j++)
            {
                int index = i * size_x + j;
                board[index] = new BoardRoom();
                if (i != 0)
                    board[index].status[(int)CellDirection.NORTH] = true;
                if (j != 0)
                    board[index].status[(int)CellDirection.WEST] = true;
                if (j != size_x - 1)
                    board[index].status[(int)CellDirection.EAST] = true;
                if (i != size_y - 1)
                    board[index].status[(int)CellDirection.SOUTH] = true;
            }
        }
    }

    public void ApplyDungeonPattern()
    {
        int room_number;
        for (int i = 0; i < size_y; i++)
            for (int j = 0; j < size_x; j++)
            {
                int index = i * size_x + j;
                if (i != 0)
                {
                    if (j != 0)
                    {
                        room_number = RandomIntExclusiveSeveralNumber(0, rooms.Count, new List<int>
                            {board[index-1].room_number , board[index - size_x].room_number});
                    }
                    else
                        room_number = RandomIntExclusiveOneNumber(0, rooms.Count, board[index - size_x].room_number);
                }
                else
                {
                    if (j != 0)
                        room_number = RandomIntExclusiveOneNumber(0, rooms.Count, board[index - 1].room_number);
                    else
                        room_number = Random.Range(0, rooms.Count);
                }
                RoomBehaviour behaviour = rooms[room_number].GetComponent<RoomBehaviour>();
                behaviour.room_number = room_number;
                behaviour.board_number = index;
                board[index].board_number = index;
                board[index].room_number = room_number;
            }
    }

    public void PrintDungeonPattern()
    {
        string output = "";
        for (int i = 0; i < size_y; i++)
        {
            output += i;
            for (int j = 0; j < size_x; j++)
            {
                int index = i * size_x + j;
                output += " " + j + " ";
                for (int k = 0; k < 4; k++)
                {
                    output += board[index].status[k] + " ";
                }
                output += " ";
            }
            output += "\n";
        }
        Debug.Log(output);
        for(int i = 0; i < size_y; i++)
        {
            string s = "";
            for(int j = 0; j < size_x; j++)
            {
                int index = i * size_x + j;
                s = s + board[index].room_number + " ";
            }
            Debug.Log(s);
        }
    }
    public int FindNeighbor(int position, CellDirection dir)
    {
        switch (dir)
        {
            case CellDirection.EAST:
                return position + 1;
            case CellDirection.WEST:
                return position - 1;
            case CellDirection.SOUTH:
                return position + size_x;
            case CellDirection.NORTH:
                return position - size_x;
        }
        return -1;
    }

    public CellDirection Opposite(CellDirection dir)
    {
        switch (dir)
        {
            case CellDirection.EAST:
                return CellDirection.WEST;
            case CellDirection.WEST:
                return CellDirection.EAST;
            case CellDirection.SOUTH:
                return CellDirection.NORTH;
            case CellDirection.NORTH:
                return CellDirection.SOUTH;
        }
        return new CellDirection();
    }

    public int RandomIntExclusiveOneNumber(int min,int max, int exclusive)
    {
        var randomNum = Random.Range(min,max-1);
        if (randomNum >= exclusive)
            randomNum++;
        return randomNum;
    }

    /// <summary>
    /// Sample: 
    /// min = 3, max = 7, exclusives = [4,6] , randomNum = 5
    /// newMax = max - exclsives.Count + 1 => 7 - 2 + 1 = 6
    /// randomNum = Range(min, newMax) => Range(3,6) => 3,4,5 
    /// randomNum = randomNum + 1 when randomNum > exclusive[i], i = 1,2
    /// Result:
    /// randomNum = 7
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max">final result include max</param>
    /// <param name="exclusives">integers of ordered list by increment</param>
    /// <returns></returns>
    public int RandomIntExclusiveSeveralNumber(int min, int max, List<int> exclusives)
    {
        var randomNum = Random.Range(min, max - exclusives.Count);
        exclusives.Sort();
        foreach (var exclusive in exclusives)
            if (randomNum >= exclusive)
                randomNum++;
        return randomNum;
    }

    public BoardRoom[] GetBoard()
    {
        return board;
    }
}
public enum CellDirection
{
    EAST,
    WEST,
    SOUTH,
    NORTH
}
