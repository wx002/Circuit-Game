using System.Collections.Generic;

public class Wire_Traveller{
	public int x,y;
	public string from;
	int[] prev = new int[2] { 0, 0 };
	int strong = 1;
    int direction = 0;
	List<int[]> past_elements = new List<int[]>();
	List<int[]> connected_elements = new List<int[]>(); 
	

	public Wire_Traveller(int x_cord, int y_cord, int strength){
		//Constructor
		this.x = x_cord;	//x coordinate in array
		this.y = y_cord;	//y coordinate in array
		this.strong = strength;	//strength of flow (remove 
	}
	
	public void Make_Strong(){
		this.strong = 1;
	}

	public void Next(int[] next_pt){
		//moves the iterator on, saves where it came from
		this.prev = new int[2] { this.x, this.y };
		this.past_elements.Add(this.prev);
		if (this.prev[0] - next_pt[0] == 1){//moving up
			this.direction = 0;
		}
		if (this.prev[0] - next_pt[0] == -1){//moving down
			this.direction = 2;
		}
		if (this.prev[1] - next_pt[1] == 1){//moving left
			this.direction = 3;
		}
		if (this.prev[1] - next_pt[1] == -1){//moving right
			this.direction = 1;
		}
		this.x = next_pt[0];
		this.y = next_pt[1];
	}

	public int[] Momentum(){
		if (this.direction == 0){
			return new int[2] { -1, 0 };
		}
		if (this.direction == 1){
			return new int[2] { 0, 1 };
		}
		if (this.direction == 2){
			return new int[2] { 1, 0 };
		}
		else{
			return new int[2] { 0, -1 };
		}
	}

	public static int[] Next_From_Direction(int i){
                if (i == 0){
			return new int[2] { -1, 0 };
		}
		if (i == 1){
			return new int[2] { 0, 1 };
		}
		if (i == 2){
			return new int[2] { 1, 0 };
		}
		else{
			return new int[2] { 0, -1 };
		}
	}


	public int[,] Surroundings(){
		//Returns the surroundings (except where it came from)
		if (this.prev[0] == this.x-1){
			return new int[3, 2] {{this.x,this.y+1 },
                { this.x+1,this.y },
                { this.x,this.y-1 }};
		}
		if (this.prev[0] == this.x+1){
			return new int[3,2] {{this.x-1,this.y },
                { this.x,this.y+1 },
                { this.x,this.y-1 } };
		}
		if (this.prev[1] == this.y-1){
			return new int[3, 2] {{this.x-1,this.y },
                { this.x,this.y+1 },
                { this.x+1,this.y }};
		}
		return new int [3,2] {{this.x-1,this.y },
            { this.x+1,this.y },
            { this.x,this.y-1 } };
	}

	
}