using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace Lekser
{
    public class Token
    {
        public string Name;
        public string Value;

        public Token(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }


    class Program
    {
        //Przygotowanie regexów 
        public static Regex Oper = new Regex(@"(\+|\-|\/|\*)");
        public static Regex ID = new Regex(@"[a-zA-Z_$][a-zA-Z0-9_$]*");
        public static Regex @Float = new Regex(@"([0-9]+\.[0-9]+)");
        public static Regex Digit = new Regex(@"[0-9]+");
        public static Regex Bracket = new Regex(@"\(|\)");
        public static Regex Error = new Regex(@"[^\s]");
        public static Regex Compare_regex = new Regex(@"(\+|\-|\/|\*)|" + @"[^\w.+\-*\/\(\)]|" + @"[0-9]+\.[0-9]+|" + @"[0-9]+|" + @"\(|\)|" + @"[a-zA-Z_$][a-zA-Z0-9_$]*|" + @"[^\s]");

        static void Main(string[] args)
        {
            List<string> List = new List<string>(); //stworzenie listy stringów do przechowania wyrażenia

            string line;
            try
            {
                using (StreamReader sr = new StreamReader("dane.txt")) //odczyt danych z pliku.txt
                {
                    if ((line = sr.ReadLine()) != null)
                    {
                        List.Add(line);
                        Console.WriteLine(line.ToString());
                        Console.WriteLine("\n");
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Brak pliku:");
                Console.WriteLine(e.Message);
            }
            List<Token> TokenList = new List<Token>(); //stworzenie listy w celu przechowania nazwy i wartości tokenu



            for (int i = 0; i < List.Count; i++)
            {

                foreach (Match match in Regex.Matches(List[i], Compare_regex.ToString())) //sprawdzenie czy w wyrażeniu przechowanym w Liście znajduje się dany regex
                {

                    //jeśli w wyrażeniu znajduje się któryś z regexów dodaj nowy Token
                    if (Oper.IsMatch(match.ToString()))
                    {
                        TokenList.Add(new Token("Oper", match.Value));

                    }
                    else if (ID.IsMatch(match.ToString()))
                    {
                        TokenList.Add(new Token("ID", match.Value));

                    }

                    else if (@Float.IsMatch(match.ToString()))
                    {
                        TokenList.Add(new Token("Float", match.Value));

                    }

                    else if (Digit.IsMatch(match.ToString()))
                    {
                        TokenList.Add(new Token("Digit", match.Value));


                    }


                    else if (Bracket.IsMatch(match.ToString()))
                    {
                        TokenList.Add(new Token("Bracket", match.Value));

                    }


                    else if (Error.IsMatch(match.ToString()))
                    {
                        TokenList.Add(new Token("Error", match.Value));

                    }
                }

            }

            //PARSER----------------------------------------------PARSER----------------------------------------PARSER-------

            int SumLeft = 0;  //licznik lewych nawiasow
            int SumRight = 0; //licznik prawych nawiasow
            int Count = TokenList.Count;


            for (int i = 0; i < Count; i++)
            {
                string space = " ";

                if (TokenList[i].Name == "Error")
                {
                    Console.WriteLine("ERROR: podales bledne wyrazenie!");
                    break;
                }
                else
                {
                    if (TokenList[0].Name == "Oper" && TokenList[i + 1].Name == "Bracket"
                        || TokenList[i].Value == "(" && TokenList[i + 1].Name == "Oper"
                        )
                    {
                        Console.WriteLine("ERROR: operator nie moze sąsiadować w taki sposób z nawiasem!\n");
                        break;
                    }
                    else
                    {
                        if (TokenList[i].Name == "Oper" && TokenList[i + 1].Name == "Oper"
                            || TokenList[i].Name == "Oper" && TokenList[i - 1].Name == "Oper"
                            || TokenList[i].Value == "(" && TokenList[i + 1].Value == ")")
                        {
                            Console.WriteLine("ERROR: operatory ani nawiasy nie moga znajdowac sie obok siebie!\n");
                            break;
                        }
                        else
                        {


                            if (
                              i != Count - 1 && TokenList[i].Value == ")" && TokenList[i + 1].Name == "Digit"
                           || i != Count - 1 && TokenList[i].Value == ")" && TokenList[i + 1].Name == "Float"
                           || i != Count - 1 && TokenList[i].Value == ")" && TokenList[i + 1].Name == "ID"

                               )


                            {
                                Console.WriteLine("ERROR: brak operatora po nawiasie!\n");
                                break;
                            }
                            else
                            {


                                if (

                                  i <0 && i!= Count - 1 && TokenList[i].Value == "(" && TokenList[i - 1].Name == "Digit"
                               || i < 0 && i != Count - 1 && TokenList[i].Value == "(" && TokenList[i - 1].Name == "Float"
                               || i < 0 && i != Count - 1 && TokenList[i].Value == "(" && TokenList[i - 1].Name == "ID"

                                   )

                                {
                                    Console.WriteLine("ERROR: brak operatora przed nawiasem!\n");
                                    break;
                                }

                                else

                                {


                                    if (i != Count - 1 && TokenList[i].Name == space)
                                    {
                                        Console.WriteLine("ERROR: Spacja w wyrazeniu!");
                                        break;
                                    }


                                    else
                                    {
                                        if (i != Count - 1 && TokenList[i].Value==")" && TokenList[i+1].Value=="(")
                                        {
                                            Console.WriteLine("ERROR: brak operatora miedzy nawiasami!");
                                        }
                                        else

                                        {

                                            if (i != Count - 1 && TokenList[i].Name == TokenList[i + 1].Name && TokenList[i + 1].Name == "Bracket")
                                            {
                                                Console.WriteLine("ERROR: błąd z nawiasami!");
                                                break;
                                            }




                                            else






                                            {
                                                if (TokenList[0].Name == "Oper")
                                                {
                                                    Console.WriteLine("ERROR: operator nie moze byc na poczatku wyrazenia!\n");
                                                    break;
                                                }
                                                else
                                                {
                                                    if (TokenList[Count - 1].Name == "Oper" || TokenList[Count - 1].Value == "(")
                                                    {
                                                        Console.WriteLine("ERROR: na koncu wyrazenia nie moze znajdowac sie otwarcie nawiasu ani operator!\n");
                                                        break;
                                                    }
                                                    else
                                                     if (TokenList[i].Name == "Bracket")
                                                    {
                                                        if (TokenList[i].Value == "(")
                                                        {
                                                            SumLeft++;
                                                        }
                                                        else
                                                        {
                                                            SumRight++;
                                                        }
                                                    }
                                                    if (i == Count - 1 && SumLeft != SumRight)
                                                    {
                                                        Console.WriteLine("ERROR: blad z nawiasami!\n");
                                                        break;

                                                    }

                                                    if (i == Count - 1 && SumLeft == SumRight)
                                                    {
                                                        Console.WriteLine("Poprawne wyrazenie!");
                                                        break;

                                                    }




                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }



            }
        }
    }
}