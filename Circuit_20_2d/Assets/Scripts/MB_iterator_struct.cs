using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MB_iterator_struct : ScriptableObject
{

    public int x, y;
    public string from;
    public int[] prev = new int[2] { 0, 0 };
    public int strong = 1;
    public int direction = 0;
    public List<int[]> past_elements = new List<int[]>();
    public List<int[]> connected_elements = new List<int[]>();



    public void Wire_Traveller(int x_cord, int y_cord, int strength)
    {
        //Constructor
        this.x = x_cord;    //x coordinate in array
        this.y = y_cord;    //y coordinate in array
        this.strong = strength; //strength of flow (remove 
    }

    public void Make_Strong()
    {
        this.strong = 1;
    }

    public void Next(int[] next_pt)
    {
        //moves the iterator on, saves where it came from
        this.prev = new int[2] { this.x, this.y };
        this.past_elements.Add(this.prev);
        if (this.prev[0] - next_pt[0] == 1)
        {//moving up
            this.direction = 0;
        }
        if (this.prev[0] - next_pt[0] == -1)
        {//moving down
            this.direction = 2;
        }
        if (this.prev[1] - next_pt[1] == 1)
        {//moving left
            this.direction = 3;
        }
        if (this.prev[1] - next_pt[1] == -1)
        {//moving right
            this.direction = 1;
        }
        this.x = next_pt[0];
        this.y = next_pt[1];
    }

    public int[] Momentum()
    {
        if (this.direction == 0)
        {
            return new int[2] { -1, 0 };
        }
        if (this.direction == 1)
        {
            return new int[2] { 0, 1 };
        }
        if (this.direction == 2)
        {
            return new int[2] { 1, 0 };
        }
        else
        {
            return new int[2] { 0, -1 };
        }
    }

    public int[] Next_From_Direction(int i)
    {
        if (i == 0)
        {
            return new int[2] { -1, 0 };
        }
        if (i == 1)
        {
            return new int[2] { 0, 1 };
        }
        if (i == 2)
        {
            return new int[2] { 1, 0 };
        }
        else
        {
            return new int[2] { 0, -1 };
        }
    }

    public int[][] Surroundings(int xbound, int ybound)
    {
        int[][] greedy_surroundings = Surroundings_helper();
        int[][] trimmed_surroundings = new int[3][];
        int i = 0;
        foreach (int[] coord in greedy_surroundings)
        {
            if ((coord[0] < 0 || coord[0] > xbound) || (coord[1] < 0 || coord[1] > ybound))
            {
                continue;
            }
            else
            {
                trimmed_surroundings[i] = coord;
                i += 1;
            }
        }
        return trimmed_surroundings;
    }

    public int[][] Surroundings_helper()
    {
        //Returns the surroundings (except where it came from)
        if (this.prev[0] == this.x - 1)
        {
            return new int[3][] {new int[2] {this.x,this.y+1 },
                new int[2] { this.x+1,this.y },
                new int[2] { this.x,this.y-1 }};
        }
        if (this.prev[0] == this.x + 1)
        {
            return new int[3][] {new int[2] {this.x-1,this.y },
                new int[2] { this.x,this.y+1 },
                new int[2] { this.x,this.y-1 } };
        }
        if (this.prev[1] == this.y - 1)
        {
            return new int[3][] {new int[2]{this.x-1,this.y },
                new int[2] { this.x,this.y+1 },
                new int[2] { this.x+1,this.y }};
        }
        return new int[3][] {new int[2] {this.x-1,this.y },
            new int[2] { this.x+1,this.y },
            new int[2] { this.x,this.y-1 } };
    }


}
