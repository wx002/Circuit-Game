using System;
using System.Collections.Generic;
using System.Linq;

    /* BEFORE MODIFYING THIS, MAKE SURE YOU REALLY UNDERSTAND WHAT IS GOING ON
     * READ THE DEV MAUNAL VERY CLEARLY
     * PAY CLOSE ATTENTION TO ALL COMMENTS
     * ONLY DEBUG PARSER IN DEBUG BUILD (WINDOWS CONSOLE APPLICATION)
     * 
     * When consider adding indexs, add the approaciate indexs based on parsed indexing
     * into get_reduced_list within Parser class
     */

public class Parser
{
    public enum DataType { CURRENT, POWER};
    
    
    //standard read file function, reference C# maunal if needed
    public static string read_file(string f)
    {
        string line;
        //full log file array
        string text = "";
        System.IO.StreamReader file = new System.IO.StreamReader(@f);
        while ((line = file.ReadLine()) != null)
        {
            text += line + "\n";
        }
        return text;
    }

    public static List<List<string>> parse_text(string text)
    {
        //normal line split options
        string[] lines = text.Split(
        new[] { "\r\n", "\n","\r" }, //ensure compatability between mac/linux/windows
        StringSplitOptions.None
        );
        List<List<string>> parsed = new List<List<string>>();
        List<string> section = new List<string>();
        for (int i = 0; i < lines.Length; i++)
        {
            if(lines[i].Length > 0) //only adding non empty lines
            {
                section.Add(lines[i]);
            }
            else
            {
                //this is to preserve the index structure for referencing the full log indexs
                //This way, adding any line from full only need to handle in get_reduced_list
                //We can get away w/ not changing anything else by adding the correct index assume 
                // same parsing structure.
                List<string> t = new List<string>();
                t = section.ToList();
                parsed.Add(t);
                
                section.Clear();
                
            }
        }
        return parsed;

    }

    private static List<List<string>> clean_list(List<List<string>> list)
    {
        //first 3 line and last 3 lines are useless
        list.RemoveAt(0);
        list.RemoveAt(0);
        list.RemoveAt(0);
        list.RemoveAt(list.Count-1);
        list.RemoveAt(list.Count - 1);
        list.RemoveAt(list.Count - 1);
        list.RemoveAt(list.Count - 1);
        return list;
    }
    

    //get the reduce size list
    public static List<List<string>> get_reduced_list(List<List<string>> list)
    {
        /*
         * component name: subindex 1
         * current: subindex 8
         * power: subindex 9
         * node data_voltage: subindex 4+
         * source data: subindex 12,13
         * 
         * component data:
         *  main index starts from 6 and onward
         */
        List<List<string>> reduced_list = new List<List<string>>();
        list = clean_list(list);
        if (list.Count > 6) { 
        
            reduced_list.Add(list[1]);
            reduced_list.Add(list[6]);
        }
        if (list.Count > 6)
        {
            for (int num = 7; num < list.Count; num++)
            {
                if (list[num].Count > 1)
                {
                    reduced_list.Add(list[num]);
                }
            }
        }
        return reduced_list;
    }

    //get voltage for nodes
    public static Dictionary<string, double> get_voltage(List<List<string>> list)
    {
        if (list.Count > 0)
        {
            List<string> voltage_list = list[0];
            Dictionary<string, double> voltage = new Dictionary<string, double>();

            for (int i = 4; i < voltage_list.Count; i++)
            {
                List<string> t = voltage_list[i].Split(new[] {" ","\t" }, StringSplitOptions.None).ToList();
                t.RemoveAll(item => item == string.Empty);
                voltage.Add(t[0], Convert.ToDouble(t[1]));
            }
            return voltage;
        }
        else
        {
            return null;
        }
    }

    //get data for componenets index: 1 to index - 2
    public static Dictionary<string, double> get_component_data(List<List<string>> list, DataType t)
    {
        Dictionary<string, double> data = new Dictionary<string, double>();
        int max_len = list.Count;
        int target_index;
        int source_data_index;
        
        if (t == DataType.CURRENT)
        {
            target_index = 8;
            source_data_index = 12;
        }
        else
        {
            target_index = 9;
            source_data_index = 13;
        }

        //error handling
        if (list.Count < max_len)
            return null;

        //speical case for power source
        for (int i = 1; i < max_len; i++)
        {

            if (list[i].Count < 1)
                return null;

            List<string> name = new List<string>();
            //name list
            if (list[i].Count > 1)
            {
                name = list[i][1].Split(new[] { " " }, StringSplitOptions.None).ToList();
            }
            name.RemoveAll(item => item == string.Empty);

            for (int j = 1; j < name.Count; j++)
            {
                if (list.Count < max_len - 1)
                    return null;

                if (i == max_len - 1)//handles all components minus the voltage source
                {
                    if (list[i].Count < source_data_index)
                        return null;

                    List<string> data_list = list[i][source_data_index].Split(new[] { " " }, StringSplitOptions.None).ToList();
                    data_list.RemoveAll(item => item == string.Empty);
                    data.Add(name[j], Convert.ToDouble(data_list[j]));
                }
                else//handles the special case of the voltage source
                {
                    List<string> data_list = list[i][target_index].Split(new[] { " " }, StringSplitOptions.None).ToList();
                    data_list.RemoveAll(item => item == string.Empty);
                    data.Add(name[j], Convert.ToDouble(data_list[j]));
                }
            }

        }
        return data;
    }

}









