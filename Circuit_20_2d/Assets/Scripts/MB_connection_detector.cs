using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class MB_connection_detector : ScriptableObject
{

    //public enum Type { vSource, aSource, bResistor, mResistor, lResistor, vMeter, aMeter, wire };

    //public int[,] connections;
    //public List<int[]>[,] connection_stacks;
    int xbound;
    int ybound;
    public GameObject[][] grid;
    //public Dictionary<string, int> placement = new Dictionary<string, int>();
    //public int nonwire_elements;
    List<MB_iterator_struct> iterators = new List<MB_iterator_struct>();

    public void Connection_Detector(GameObject[][] gameboard)//, string[] obj_names)
    { //Need unique names given to each object
        this.grid = gameboard;
        //this.nonwire_elements = obj_names.Length + 1;
        //this.connections = new int[this.nonwire_elements + 1, this.nonwire_elements + 1];
        //this.connection_stacks = new List<int[]>[this.nonwire_elements + 1, this.nonwire_elements + 1];
        //int i = 0;
        //for (i = 0; i < this.nonwire_elements - 1; i++)
        //{ //get dictionary for connection array
        //    this.placement.Add(obj_names[i], i);
        //}
        this.ybound = this.grid.Length - 1;
        this.xbound = this.grid[0].Length - 1;
    }

    public int[] Find_Source()
    {
        int[] ret = new int[2] { 0, 0 };
        //Debug.Log("xbound");
        //D//ebug.Log(this.xbound);
        //Debug.Log("ybound");
        //Debug.Log(this.ybound);
        for (int i = 0; i < this.ybound; i++)
        {
            for (int j = 0; j < this.xbound; j++)
            {
                //Debug.Log("grid i:");
                //Debug.Log(i);
                //Debug.Log("grid j:");
                //Debug.Log(j);
                if (this.grid[i][j] != null)
                {
                    //Debug.Log(this.grid[i][j].gameObject.GetComponent<Attributes>().type);
                    if (this.grid[i][j].gameObject.GetComponent<Attributes>().type == Attributes.Type.vSource)
                    {
                        ret[0] = i;
                        ret[1] = j;
                        return ret;
                    }
                }
            }
        }
        //print("Source Not Found.");
        return ret;
    }

    /*public void Connect(string from, string to)
    {
        connections[placement[from], placement[to]] = 1;
    }*/

    public int Find_Connections()
    {
        int[] start = this.Find_Source(); //int[2]
        MB_iterator_struct first_iterator = ScriptableObject.CreateInstance<MB_iterator_struct>();
        first_iterator.Wire_Traveller(start[0], start[1], 1);
        //Debug.Log("start:");
        //Debug.Log(start[0]);
        //Debug.Log(start[1]);
        if (start[0] == 0)
        {
            return -1;
        }
        int[] first_move = new int[2] { start[0] - 1, start[1] };

        first_iterator.Next(first_move);
        //Debug.Log(first_iterator.x);
        //Debug.Log(first_iterator.y);
        if (this.grid[first_iterator.x][first_iterator.y] == null)
        {
            return -1;
        }
        if (!(this.grid[first_iterator.x][first_iterator.y].gameObject.GetComponent<Attributes>().type == Attributes.Type.wire))
        {
            return -1;
        }
        this.grid[first_iterator.x][first_iterator.y].GetComponent<Attributes>().strength = 1;
        this.iterators.Add(first_iterator);
        Debug.Log("Start iter");
        while (this.iterators.Count != 0)
        { //Main Loop End, Stops when all iterators are blocked or all done
            //Debug.Log("next iter");
            bool any_progress = false;
            List<MB_iterator_struct> new_iterators = new List<MB_iterator_struct>();
            foreach (MB_iterator_struct iterator in this.iterators)
            { //loop through all iterators, move them or spawn more.
                GameObject current_position = this.grid[iterator.x][iterator.y];
                if (get_type(current_position) == Attributes.Type.vSource)
                { // if it ends, don't add and make past strong
                    foreach (int[] past in iterator.past_elements)
                    {
                        this.grid[past[0]][past[1]].gameObject.GetComponent<Attributes>().strength = 1;
                    }
                    any_progress = true;
                    continue;
                }
                //Attributes.Type[] resistortypes = new Attributes.Type[3] { Attributes.Type.bResistor, Attributes.Type.lResistor, Attributes.Type.mResistor };
                if ((get_type(current_position) == Attributes.Type.bResistor) ||
                    (get_type(current_position) == Attributes.Type.lResistor) ||
                    (get_type(current_position) == Attributes.Type.mResistor)
                    )
                //(grid[iterator.x, iterator.y].gameObject.GetComponent<Attributes>().type == Attributes.Type.bResistor, Attributes.Type.lResistor, Attributes.Type.mResistor})
                { //if resist, connect and continue
                    //string to = grid[iterator.x][iterator.y].gameObject.GetComponent<Attributes>().name;
                    //this.Connect(iterator.from, to);
                    //iterator.from = to;
                    if (get_directVal(current_position) == iterator.direction)
                    {
                        iterator.strong = 1;
                        foreach (int[] past in iterator.past_elements)
                        {
                            this.grid[past[0]][past[1]].gameObject.GetComponent<Attributes>().strength = 1;
                        }
                        new_iterators.Add(iterator);
                    }
                    else
                    {
                        if (iterator.strong == 1 && get_directVal(current_position) == (iterator.direction + 2) % 4)
                        {
                            //circuit loopback
                            return -1;
                        }
                    }
                    continue;
                }
                int[][] surroundings = iterator.Surroundings(xbound, ybound); //int[3,2]
                int[][] nexts = new int[3][];                                  //int[3,2]
                int many = 0;
                foreach (int[] around in surroundings)
                {
                    if (this.grid[around[0]][around[1]] == null)
                    {
                        continue;
                    }
                    if (!(get_type(this.grid[around[0]][around[1]]) == Attributes.Type.wire))
                    { //if there isn't anything next to it
                        continue;
                    }
                    else
                    {
                        nexts[many] = around;
                        many += 1;
                    }
                }
                Debug.Log("Many: ");
                Debug.Log(many);
                if (many == 0)
                {
                    return -1; //DEAD END CIRCUIT
                }
                if (many == 1)
                { //if only one way to go
                    //Debug.Log("next guy's strength");
                    //Debug.Log(grid[nexts[0][0]][nexts[0][1]].gameObject.GetComponent<Attributes>().strength);
                    GameObject pipe = this.grid[nexts[0][0]][nexts[0][1]];
                    if (pipe.gameObject.GetComponent<Attributes>().strength == 1)
                    { // it ran in to a strong one
                        if ((get_directVal(pipe) == (iterator.direction + 2) % 4) && (iterator.strong == 1))
                        {//strong
                            return -1; //CIRCUIT LOOPBACK
                        }
                        else
                        {  //either the directions agree or the iterator was weak, wither way don't add the iterator it's overridden
                            //this.iterators.Remove(iterator);
                            continue;
                        }
                    } //end ran into strong
                    if (iterator.strong == 1)
                    { //weak or no direction, still strong
                        current_position.gameObject.GetComponent<Attributes>().directVal = iterator.direction;
                        iterator.Next(nexts[0]); //move it on
                        pipe.gameObject.GetComponent<Attributes>().strength = 1; //make it strong
                        new_iterators.Add(iterator);
                        continue;
                    }
                    if (iterator.strong == 0)
                    { // iterator is weak
                        if (get_strength(pipe) == 0)
                        { // ran into another weak
                            if (get_directVal(pipe) != iterator.direction)
                            { //inconclusive direction
                                new_iterators.Add(iterator);
                                continue; // dont do anything with them
                            }
                            else
                            { //must've cought up with same direction
                                //this.iterators.Remove(iterator);
                                continue;
                            }
                        }
                        else
                        { // it must be weak and run into nothing
                            current_position.gameObject.GetComponent<Attributes>().directVal = iterator.direction;
                            iterator.Next(nexts[0]); //move it
                            pipe.gameObject.GetComponent<Attributes>().strength = 0; //make it weak
                            new_iterators.Add(iterator);
                            continue;
                        }
                    }

                }// end of the one connections case
                if (many == 2)
                { //it's a T connection (split weak unless only one iterator, then split strong.
                    GameObject pipe1 = this.grid[nexts[0][0]][nexts[0][1]];
                    GameObject pipe2 = this.grid[nexts[1][0]][nexts[1][1]];
                    if ((get_strength(pipe1) == -1) && (get_strength(pipe2) == -1)) //splits
                    {
                        MB_iterator_struct split1 = ScriptableObject.CreateInstance<MB_iterator_struct>();
                        MB_iterator_struct split2 = ScriptableObject.CreateInstance<MB_iterator_struct>();
                        if (this.iterators.Count == 1)
                        { // its the only one
                            split1.Wire_Traveller(iterator.x, iterator.y, 1);
                            split2.Wire_Traveller(iterator.x, iterator.y, 1);
                        }
                        else
                        {// there are multiple, split weak
                            split1.Wire_Traveller(iterator.x, iterator.y, 0);
                            split2.Wire_Traveller(iterator.x, iterator.y, 0);
                        }
                        split1.past_elements = new List<int[]>(iterator.past_elements);
                        split2.past_elements = new List<int[]>(iterator.past_elements);
                        split1.Next(nexts[0]);
                        split2.Next(nexts[1]);
                        new_iterators.Add(split1);
                        new_iterators.Add(split2);
                        continue;
                    }
                    int[] in_way = iterator.Momentum(); //int[2]
                    int stem = 0;
                    if (this.grid[iterator.y + in_way[0]][iterator.x + in_way[1]] == null)
                    {
                        stem = 1;
                    }
                    if (stem == 1)
                    {
                        pipe1 = this.grid[nexts[0][0]][nexts[0][1]];
                        pipe2 = this.grid[nexts[1][0]][nexts[1][1]];

                        if ((get_strength(pipe1) == 1) && (get_strength(pipe2) == 1))
                        { // it ran in to two strong ones
                            if (get_directVal(pipe1) == ((get_directVal(pipe2) + 2) % 4) && (iterator.strong == 1))
                            {//If the others  oppose and iterator is strong
                                return -1;   //CIRCUIT LOOPBACK
                            }
                            else
                            {  //either the iterator is strong and others agrees or is weak and is overridden
                                if (get_directVal(pipe1) == ((get_directVal(pipe2) + 2) % 4)) //the iterator is weak other two strong oppose
                                { //turn it around and lets go
                                    MB_iterator_struct new_iterator = ScriptableObject.CreateInstance<MB_iterator_struct>();
                                    new_iterator.Wire_Traveller(iterator.x, iterator.y, 1);
                                    new_iterator.Next(iterator.prev);
                                    //this.iterators.Remove(iterator); //remove the iterator
                                }
                                else //the other two must not oppose
                                {
                                    if (iterator.strong == 1) //if the iterator is strong and they don't oppose
                                    {
                                        continue;
                                        //this.iterators.Remove(iterator);
                                    }
                                    else
                                    {//they dont oppose AND the iterator is weak
                                        foreach (int[] past in iterator.past_elements)
                                        {
                                            this.grid[past[0]][past[1]].gameObject.GetComponent<Attributes>().strength = 1;
                                        }
                                        continue; //has to be pulled along
                                    }
                                }
                                continue;
                            }
                        }
                        else //the other two must not be strong.
                        {
                            if ((get_strength(pipe1) == 0) && (get_strength(pipe2) == 0)) //both are weak
                            {
                                if (iterator.strong == 1 && this.iterators.Count == 1)
                                {//if it's the only one its gotta split
                                    MB_iterator_struct split1 = ScriptableObject.CreateInstance<MB_iterator_struct>();
                                    MB_iterator_struct split2 = ScriptableObject.CreateInstance<MB_iterator_struct>();
                                    split1.Wire_Traveller(iterator.x, iterator.y, 1);
                                    split2.Wire_Traveller(iterator.x, iterator.y, 1);
                                    split1.past_elements = new List<int[]>(iterator.past_elements);
                                    split2.past_elements = new List<int[]>(iterator.past_elements);
                                    split1.Next(nexts[0]);
                                    split2.Next(nexts[1]);
                                    new_iterators.Add(split1);
                                    new_iterators.Add(split2);
                                }
                                continue; //iterator doesn't matter if the iterator is strong or weak, have to wait
                            }
                            if ((get_strength(pipe1) == 1) || (get_strength(pipe2) == 1)) //only one of them is weak
                            {
                                if (iterator.strong == 0) // there are two weaks
                                {
                                    continue;
                                }
                                else //the iterator and one of the pipes is strong
                                {
                                    if (get_strength(pipe1) == 1) // it's the first pipe
                                    {
                                        if ((nexts[0][0] + iterator.Next_From_Direction(get_directVal(pipe1))[0] == iterator.x) &&
                                            (nexts[0][1] + iterator.Next_From_Direction(get_directVal(pipe1))[1] == iterator.y)) //
                                        {
                                            iterator.Next(nexts[1]);
                                        }
                                        else
                                        {
                                            continue; //we can't say, honesly shouldn't happen
                                        }
                                    }
                                    else //it's the second pipe
                                    {
                                        if ((nexts[1][0] + iterator.Next_From_Direction(get_directVal(pipe2))[0] == iterator.x) &&
                                            (nexts[1][1] + iterator.Next_From_Direction(get_directVal(pipe2))[1] == iterator.y))
                                        {
                                            iterator.Next(nexts[0]);
                                        }
                                        else
                                        {
                                            continue; //we can't say, honestly shouldn't happen
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else //iterator is a side of it.
                    {
                        //GameObject pipe1;
                        //GameObject pipe2;
                        int[] top;
                        int[] side;
                        if (iterator.y + in_way[0] == nexts[1][0] && iterator.x + in_way[1] == nexts[1][1])
                        {
                            pipe1 = this.grid[nexts[0][0]][nexts[0][1]];
                            top = nexts[0];
                            pipe2 = this.grid[nexts[1][0]][nexts[1][1]];
                            side = nexts[1];
                        }
                        else
                        {
                            pipe1 = this.grid[nexts[1][0]][nexts[1][1]];
                            top = nexts[1];
                            pipe2 = this.grid[nexts[0][0]][nexts[0][1]];
                            side = nexts[0];
                        }

                        if ((get_strength(pipe1) == 1) && (get_strength(pipe2) == 1))
                        { // it ran in to two strong ones
                            if (get_directVal(pipe1) == ((get_directVal(pipe2) + 2) % 4) && (iterator.strong == 1))
                            {//If the others  oppose and iterator is strong
                                return -1;   //CIRCUIT LOOPBACK
                            }
                            else
                            {  //either the iterator is strong and others agrees or is weak and is overridden
                                int[] toppoint = new int[2] { top[0] + iterator.Next_From_Direction(get_directVal(pipe1))[0], top[1] + iterator.Next_From_Direction(get_directVal(pipe1))[1] };
                                //int[] sidpoint = new int[2] {side[0] + iterator.Next_From_Direction(pipe2.gameObject.GetComponent<Attributes>().directVal)[0], side[1] + iterator.Next_From_Direction(pipe2.gameObject.GetComponent<Attributes>().directVal)[1] };
                                if (iterator.direction == ((get_directVal(pipe2) + 2) % 4) &&
                                    ((toppoint[0] == iterator.x) && toppoint[0] == iterator.y)) //the iterator is weak other two strong oppose
                                { //turn it around and lets go
                                    MB_iterator_struct new_iterator = ScriptableObject.CreateInstance<MB_iterator_struct>();
                                    new_iterator.Wire_Traveller(iterator.x, iterator.y, 1);
                                    new_iterator.Next(iterator.prev);
                                    //this.iterators.Remove(iterator); //remove the iterator
                                }
                                else //the other two must not oppose
                                {
                                    if (iterator.strong == 1) //if the iterator is strong and they don't oppose
                                    {
                                        continue;
                                        //this.iterators.Remove(iterator);
                                    }
                                    else
                                    {//they dont oppose AND the iterator is weak
                                        foreach (int[] past in iterator.past_elements)
                                        {
                                            this.grid[past[0]][past[1]].gameObject.GetComponent<Attributes>().strength = 1;
                                        }
                                        continue; //has to be pulled along
                                    }
                                }
                                continue;
                            }
                        }
                        else //the other two must not be strong.
                        {
                            if ((get_strength(pipe1) == 0) && (get_strength(pipe2) == 0)) //both are weak
                            {
                                if (iterator.strong == 1 && this.iterators.Count == 1)
                                {//if it's the only one its gotta split
                                    MB_iterator_struct split1 = ScriptableObject.CreateInstance<MB_iterator_struct>();
                                    MB_iterator_struct split2 = ScriptableObject.CreateInstance<MB_iterator_struct>();
                                    split1.Wire_Traveller(iterator.x, iterator.y, 1);
                                    split2.Wire_Traveller(iterator.x, iterator.y, 1);
                                    split1.past_elements = new List<int[]>(iterator.past_elements);
                                    split2.past_elements = new List<int[]>(iterator.past_elements);
                                    split1.Next(nexts[0]);
                                    split2.Next(nexts[1]);
                                    new_iterators.Add(split1);
                                    new_iterators.Add(split2);
                                }
                                continue; //iterator doesn't matter if the iterator is strong or weak, have to wait
                            }
                            if ((get_strength(pipe1) == 1) || (get_strength(pipe2) == 1)) //only one of them is weak
                            {
                                if (iterator.strong == 0) // there are two weaks
                                {
                                    continue;
                                }
                                else //the iterator and one of the pipes is strong
                                {
                                    if (pipe1.gameObject.GetComponent<Attributes>().strength == 1) // it's the top pipe
                                    {
                                        if ((nexts[0][0] + iterator.Next_From_Direction(get_directVal(pipe1))[0] == iterator.x) &&
                                            (nexts[0][1] + iterator.Next_From_Direction(get_directVal(pipe1))[1] == iterator.y)) //
                                        {
                                            iterator.Next(nexts[1]);
                                        }
                                        else
                                        {
                                            continue; //we can't say, honesly shouldn't happen
                                        }
                                    }
                                    else //it's the second pipe
                                    {
                                        if ((nexts[1][0] + iterator.Next_From_Direction(get_directVal(pipe2))[0] == iterator.x) &&
                                            (nexts[1][1] + iterator.Next_From_Direction(get_directVal(pipe2))[1] == iterator.y))
                                        {
                                            iterator.Next(nexts[0]);
                                        }
                                        else
                                        {
                                            continue; //we can't say, honestly shouldn't happen
                                        }
                                    }
                                }
                            }
                        }
                    }
                }// end of the two connections case

                if (many == 3)
                { //it's an X connection (probably not great), (split weak unless only one iterator, then split strong.)
                    GameObject pipe1 = this.grid[nexts[0][0]][nexts[0][1]];
                    GameObject pipe2 = this.grid[nexts[1][0]][nexts[1][1]];
                    GameObject pipe3 = this.grid[nexts[1][0]][nexts[1][1]];
                    GameObject[] pipes = new GameObject[] { pipe1, pipe2, pipe3 };
                    foreach (GameObject pipe in pipes)
                    {
                        if ((get_strength(pipe) == -1) || (this.iterators.Count == 1)) //splits
                        {
                            MB_iterator_struct split = ScriptableObject.CreateInstance<MB_iterator_struct>();
                            if (this.iterators.Count == 1)
                            { // its the only one
                                split.Wire_Traveller(iterator.x, iterator.y, 1);
                            }
                            else
                            {// there are multiple, split weak
                                split.Wire_Traveller(iterator.x, iterator.y, 0);
                            }
                            split.past_elements = new List<int[]>(iterator.past_elements);
                            split.Next(nexts[1]);
                            new_iterators.Add(split);
                        }
                    }
                }// end of 3- connection case



            } //All iterators have been spawned, all connections logged.		
              //if (not any_progress){ // no progress was made
              //    break;
              //}	
            this.iterators = new_iterators;
        } //Main Loop End, Stops when all iterators are blocked or all done
        if (this.iterators.Count == 0)
        {
            return 0;
        }
        return -1;
    }


    public static int get_directVal(GameObject circuit_component)
    {
        return circuit_component.gameObject.GetComponent<Attributes>().directVal;
    }

    public static int get_strength(GameObject circuit_component)
    {
        return circuit_component.gameObject.GetComponent<Attributes>().strength;
    }

    public static Attributes.Type get_type(GameObject circuit_component)
    {
        return circuit_component.gameObject.GetComponent<Attributes>().type;
    }
}
