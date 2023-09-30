public class Connection_Detector{
	public int[,] connections;
    public List<int[]>[,] connection_stacks; 
	int xbound;
	int ybound;
	public gameobj[,] grid;
    public Dictionary<string, int> placement = new Dictionary<string, int>();
    public int nonwire_elements;
	List<MB_iterator_struct> iterators = new List<MB_iterator_struct>();

	public Connection_Detector(gameobj[,] gameboard, string[] obj_names){ //Need unique names given to each object
		this.grid = gameboard;
		this.nonwire_elements = obj_names.Length+1;
		this.connections = new int[this.nonwire_elements+1,this.nonwire_elements+1];
        this.connection_stacks = new List<int[]>[this.nonwire_elements + 1, this.nonwire_elements + 1];
        int i = 0;
		for (i = 0; i < this.nonwire_elements-1;i++){ //get dictionary for connection array
			this.placement.Add(obj_names[i],i);
		}
		this.ybound = gameboard.Length;
        this.ybound;
	}

	public int[] Find_Source(){
		int[] ret = new int[2] {0,0};
		for (int i=0; i < this.nonwire_elements; i++){
			for (int j = 0; j < this.nonwire_elements; i++){
				if (this.grid[name] = "Voltage_Source"){
					ret[0] = i;
					ret[1] = j;
					return ret;
				}	
			}	
		}
		Console.WriteLine("Source Not Found.");
        return new int[] { -1, -1 }();
	}

	public void Connect(string from, string to){
		connections[placement[from],placement[to]] = 1;
	}
	
	public void Find_Conections(){
		int[] start = this.Find_Source() ; //int[2]
        MB_iterator_struct first_iterator = new MB_iterator_struct();
        first_iterator.WireTraveller(start[0],start[1],1);
        int[] first_move = new int[2] { start[0] - 1, start[1] };

        first_iterator.Next( first_move);
		if (!(this.gameboard[first_iterator.x][first_iterator.y] is Wire)){
			exit;
		}
		this.gameboard[first_iterator.x,first_iterator.y].strength = 1;
		this.iterators.add(first_iterator);
		while(!(this.iterators.IsEmpty())){ //Main Loop End, Stops when all iterators are blocked or all done
			bool any_progress = False;
			foreach (gameobj iterator in this.iterators){ //loop through all iterators, move them or spawn more.
				if (gameboard[iterator.x,iterator.y] is volt_sink){ // if it ends, delete and make past strong
					foreach (gameobj past in iterator.past_elements){
						this.grid[past[0],past[1]].strength = 1;
					}
					any_progress = True;
					this.iterators.remove(iterator);
					continue;
				}
				if (gameboard[iterator.x,iterator.y] is resistor){ //if resist, connect and continue
                    string to = gameboard[iterator.x, iterator.y].name; 
					this.Connect(from,to);
					iterator.from = to;
				}
				int [,] surroundings = iterator.Surroundings(); //int[3,2]
				int [,] nexts;                                  //int[3,2]
				int many = 0;
				foreach (int[] around in surroundings){
					if (this.grid[around[0],around[1]] == NULL){ //if there isn't anything next to it
						continue;
					}
					else{
						nexts[many] = around;
						many += 1;
					}
				}
				if (many == 0){
					return; //DEAD END CIRCUIT
				}
				if (many == 1){ //if only one way to go
					if (this.grid[nexts[0][0],nexts[0][0]].strength == 1){ // it ran in to a strong one
                        if ((this.grid[nexts[0][0], nexts[0][1]].direction == (iterator.direction + 2) % 4) && (iterator.strong == 1)) {//strong
                            return; //CIRCUIT LOOPBACK
                        }
                        else {  //either the directions agree or the iterator was weak, wither way delete the iterator it's overridden
                            this.iterators.remove(iterator);
                            continue;
                        }
					} //end ran into strong
					if (iterator.strong == 1){ //weak or no direction, still strong
						iterator.Next(nexts[0]); //move it on
						this.grid[nexts[0][0],nexts[0][1]].strength = 1; //make it strong
						continue;
					}
					if (iterator.strong == 0){ // iterator is weak
                        if (this.grid[nexts[0][0], nexts[0][1]].strength == 0)
                        { // ran into another weak
                            if (this.grid[nexts[0][0], nexts[0][1]].direction != iterator.direction)
                            { //inconclusive direction
                                continue; // dont do anything with them
                            }
                            else
                            { //must've cought up with same direction
                                this.iterators.remove(iterator);
                                continue;
                            }
                        }
                        else
                        { // it must be weak and run into nothing
                            iterator.Next(nexts[0]); //move it
                            this.grid[nexts[0][0], nexts[0][1]].strength = 0; //make it weak
                            continue;
                        }
					}

				}// end of the one connections case
				if (many == 2){ //it's a T connection (split weak unless only one iterator, then split strong.
                        int[] in_way = iterator.Momentum(); //int[2]
                        int stem = 0;
                        if (this.grid[iterator.y + in_way[0]][iterator.x + in_way[1]] == null)
                        {
                            stem = 1;
                        }
                        if (stem == 1) {
                            gameobj pipe1 = this.grid[nexts[0][0]][nexts[0][1]];
                            gameobj pipe2 = this.grid[nexts[1][0]][nexts[1][1]];
                            if ((pipe1.strength == 1) && (pipe2.strength == 1))
                            { // it ran in to two strong ones
                                if (pipe1.direction == ((pipe2.direction + 2) % 4) && (iterator.strong == 1))
                                {//If the other  oppose and are strong
                                    return;   //CIRCUIT LOOPBACK
                                }
                                else
                                {  //either the iterator is weak and agrees or is weak and is overridden
                                    if (pipe1.direction == ((pipe2.direction + 2) % 4)) //the iterator is weak other two strong oppose
                                    { //turn it around and lets go
                                        Wire_Traveller new_iterator = Wire_Traveller(iterator.x, iterator.y, 1);
                                        new_iterator.Next(iterator.prev);
                                        this.iterators.remove(iterator); //remove the iterator
                                    }
                                    else //the other two must not oppose
                                    {
                                        if (iterator.strong == 1) //if the iterator is strong and they don't oppose
                                        {
                                            this.iterators.remove(iterator);
                                        }
                                        else
                                        {
                                            continue; //do nothing, can't say
                                        }
                                    }
                                    continue;
                                }
                            }
                            else //the other two must not be strong.
                            {
                                if ((pipe1.strength == 0) && (pipe2.strength == 0)) //both are weak
                                {
                                    continue; //iterator doesn't matter if the iterator is strong or weak, have to wait
                                }
                                if ((pipe1.strength == 1) || (pipe2.strength == 1)) //only one of them is weak
                                {
                                    if (iterator.strong == 0) // there are two weaks
                                    {
                                        continue;
                                    }
                                    else //the iterator and one of the pipes is strong
                                    {
                                        if (pipe1.strength == 1) // it's the first pipe
                                        {
                                            if ((nexts[0][0] + iterator.Next_From_Direction(pipe1.direction)[0] ==iterator.x) && (nexts[0][1] + iterator.Next_From_Direction(pipe1.direction)[1] == iterator.y)){
                                                iterator.Next(nexts[1]);
                                            }
                                            else
                                            {
                                                continue; //we can't say
                                            }
                                        }
                                        else //it's the second pipe
                                        {
                                        if ((nexts[1][0] + iterator.Next_From_Direction(pipe2.direction)[0] == iterator.x) && (nexts[1][1] + iterator.Next_From_Direction(pipe2.direction)[1] == iterator.y))
                                        {
                                            iterator.Next(nexts[0]);
                                        }
                                        else
                                        {
                                            continue; //we can't say
                                        }
                                    }
                                    }
                                }
                            }
                        }
                        else
                        {
                        gameobj pipe1;
                        gameobj pipe2;
                        if (iterator.y + in_way[0] == nexts[0][0] && iterator.x + in_way[1] == nexts[0][1] ){
                            pipe1 = this.grid[nexts[0][0]][nexts[0][1]];
                        }
                        gameobj pipe2 = this.grid[nexts[1][0]][nexts[1][1]];
                        if ((pipe1.strength == 1) && (pipe2.strength == 1))
                        { // it ran in to two strong ones
                            if (pipe1.direction == ((pipe2.direction + 2) % 4) && (iterator.strong == 1))
                            {//If the other  oppose and are strong
                                return;   //CIRCUIT LOOPBACK
                            }
                            else
                            {  //either the iterator is weak and agrees or is weak and is overridden
                                if (pipe1.direction == ((pipe2.direction + 2) % 4)) //the iterator is weak other two strong oppose
                                { //turn it around and lets go
                                    Wire_Traveller new_iterator = Wire_Traveller(iterator.x, iterator.y, 1);
                                    new_iterator.Next(iterator.prev);
                                    this.iterators.remove(iterator); //remove the iterator
                                }
                                else //the other two must not oppose
                                {
                                    if (iterator.strong == 1) //if the iterator is strong and they don't oppose
                                    {
                                        this.iterators.remove(iterator);
                                    }
                                    else
                                    {
                                        continue; //do nothing, can't say
                                    }
                                }
                                continue;
                            }
                        }
                        else //the other two must not be strong.
                        {
                            if ((pipe1.strength == 0) && (pipe2.strength == 0)) //both are weak
                            {
                                continue; //iterator doesn't matter if the iterator is strong or weak, have to wait
                            }
                            if ((pipe1.strength == 1) || (pipe2.strength == 1)) //only one of them is weak
                            {
                                if (iterator.strong == 0) // there are two weaks
                                {
                                    continue;
                                }
                                else //the iterator and one of the pipes is strong
                                {
                                    if (pipe1.strength == 1) // it's the first pipe
                                    {
                                        if ((nexts[0][0] + iterator.Next_From_Direction(pipe1.direction)[0] == iterator.x) && (nexts[0][1] + iterator.Next_From_Direction(pipe1.direction)[1] == iterator.y))
                                        {
                                            iterator.Next(nexts[1]);
                                        }
                                        else
                                        {
                                            continue; //we can't say
                                        }
                                    }
                                    else //it's the second pipe
                                    {
                                        if ((nexts[1][0] + iterator.Next_From_Direction(pipe2.direction)[0] == iterator.x) && (nexts[1][1] + iterator.Next_From_Direction(pipe2.direction)[1] == iterator.y))
                                        {
                                            iterator.Next(nexts[0]);
                                        }
                                        else
                                        {
                                            continue; //we can't say
                                        }
                                    }
                                }
                            }
                        }
                    }
				}// end of the two connections case













				if (many == 3){ //it's an X connection (probably not great), (split weak unless only one iterator, then split strong.)
			


				}// end of 3- connection case

		
		
			} //All iterators have been spawned, all connections logged.		
			//if (not any_progress){ // no progress was made
            //    break;
            //}		
		} //Main Loop End, Stops when all iterators are blocked or all done
	}
}		