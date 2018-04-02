using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace tree {
    class Program {
        static void Main(string[] args) {
            string message = Properties.Settings.Default.message;
            if(message == "") {
                message = "Scan completed no virus or hacking found you do not need a fixation";
            }

            if(args.Length > 0) {
                if(args[0] == "-m") {
                    if(args.Length > 1) {
                        Properties.Settings.Default.message = args[1];
                        Properties.Settings.Default.Save();
                        Console.WriteLine("Message set to: " + args[1]);
                    }
                    else {
                        Console.WriteLine("Message is currently: " + message);
                    }        
                    return;
                }
            }

            Console.CursorVisible = false;
            Console.TreatControlCAsInput = true; //dont allow the user to cancel by using ctrl+c

            Console.CancelKeyPress += Console_CancelKeyPress;

            Node node = new Node() { Path = Directory.GetCurrentDirectory() };
            //Node node = new Node() { Path = @"C:\" };
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(message);

            Console.CursorVisible = true;
            
        }
        
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e) {
            e.Cancel = true; //dont allow the user to cancel by using ctrl+break
        }
    }

    public class Node {
        private string path;
        public string Path { get { return path; } set { path = value; PopulateChildren(); } }
        public List<Node> SubDirectories = new List<Node>();

        public void List() {
            for(int i = 0; i < SubDirectories.Count; i++) {
                SubDirectories[i].PrintPretty(getIndent(i), i == SubDirectories.Count - 1 ? true : false);
            }
        }

        private string getIndent(int length) {
            /*StringBuilder s = new StringBuilder();
            for(int i = 0; i < length-1; i++) {
                s.Append("   ");
            }*/
            return "    "; // s.ToString();
        }

        public void PrintPretty(string indent, bool last) {
            Console.Write(indent);
            if(last) {
                Console.Write("└───");
                indent += "    ";
            }
            else {
                Console.Write("├───");
                indent += "│   ";
            }
            Console.WriteLine(new DirectoryInfo(Path).Name);

            for(int i = 0; i < SubDirectories.Count; i++)
                SubDirectories[i].PrintPretty(indent, i == SubDirectories.Count - 1);
        }

        private void PopulateChildren() {
            try {
                string[] subs = Directory.GetDirectories(Path);
                foreach(string dir in subs) {
                    //if(dir.Length <= 32) {
                        Node node = new Node();
                        node.Path = dir;
                        SubDirectories.Add(node);
                    //}
                }
            } catch (Exception ex) {
                //ignore exceptions, like unathorized access... just skip over those directories.
            }
            List();
        }
    }
}
