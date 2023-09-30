using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class run_parser
{
    /* This is the debugging class for Parser, should only be used in 
     * the debug build Parser (Windows Console Application)
     * 
     * DO NOT PUT THIS INTO UNITY PROJECT, IT WOULD NOT WORK DUE TO
     * DIFFERENCE OF C# SYNTAXS!!!!!!!!
     * 
     * 
     * When consider adding indexs, add the approaciate indexs based on parsed indexing
     * into get_reduced_list within Parser class
     */ 
    static void Main(string[] args)
    {

        string text = Parser.read_file("netlist.log");
        List<List<string>> t = Parser.parse_text(text);
        List<List<string>> parsed = Parser.get_reduced_list(t);

        Console.WriteLine("Examples of dictinoary data");
        Console.WriteLine("Current data");
        Dictionary<string, double> current_dic = Parser.get_component_data(parsed, Parser.DataType.CURRENT);
        print_dictionary(current_dic);
        Console.WriteLine("\n node voltage data \n");
        Dictionary<string, double> voltage_dic = Parser.get_voltage(parsed);
        print_dictionary(voltage_dic);
        Console.WriteLine("\n power data \n");
        Dictionary<string, double> power_dic = Parser.get_component_data(parsed, Parser.DataType.POWER);
        print_dictionary(power_dic);
        Console.WriteLine("\n Example of full log w/ proper indexing");
        print_full_log("example.log");
        Console.WriteLine("\n Example of parsed log w/ proper indexing");
        print_parsed_log("example.log");

        Console.ReadLine();
    }

    /*the following are list of helper functions for debugging parser*/

    //This function prints a dictionary
    public static void print_dictionary<T,K>(Dictionary<T, K> dic)
    {
        foreach (KeyValuePair<T, K> pair in dic)
        {
            Console.WriteLine("key = {0}, value = {1}", pair.Key, pair.Value);
        }
    }

    //This function prints the full log in console with proper indexing
    public static void print_full_log(string logfile)
    {
        string fulllog = Parser.read_file(logfile);
        List<List<string>> fulllog_list = Parser.parse_text(fulllog);
        foreach (List<string> sublist in fulllog_list)
        {
            foreach (string s in sublist)
            {
                Console.WriteLine("{0}\t mainIndex = {1},subIndex = {2}", s, fulllog_list.IndexOf(sublist), sublist.IndexOf(s));
            }
        }
    }

    //This function prints the parsed log in console w/ proper indexing
    public static void print_parsed_log(string logfile)
    {
        string fullog = Parser.read_file(logfile);
        List<List<string>> parsed = Parser.get_reduced_list(Parser.parse_text(fullog));
        foreach (List<string> sublist in parsed)
        {
            foreach (string s in sublist)
            {
                Console.WriteLine("{0}\t mainIndex = {1},subIndex = {2}", s, parsed.IndexOf(sublist), sublist.IndexOf(s));
            }
        }
    }

}

