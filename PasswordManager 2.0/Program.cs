using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.IO;

namespace PasswordManager_2._0
{
    class Program
    {
       public static string caminho = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\365.lol";

        static void Main(string[] args)
        {
            Console.Title = "Password Manager V2.0";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Clear();
            while (true)
            {
                Console.Clear();
                Console.Write("Comando: ");
                string comando = Console.ReadLine();
                switch (comando)
                {
                    case "exit":
                        System.Environment.Exit(0);
                        break;

                    case "add account":
                        Console.Clear();
                        Account conta = new Account();
                        Console.Write("Descricao: ");
                        conta.descricao = Console.ReadLine();
                        Console.Clear();
                        Console.Write("Username: ");
                        conta.username = Console.ReadLine();
                        Console.Clear();
                        Console.Write("Password: ");
                        conta.password = ReadPassword();
                        Console.Clear();
                        save(conta);
                        Console.WriteLine("Conta de " + conta.descricao + " guardada com sucesso!");
                        Console.ReadKey();
                        break;

                    /*case "update":
                        foreach(Account conta4 in load(caminho).contas)
                        {
                            Account temp = new Account { };
                           string nome = encriptaB64(conta4.username);
                           string desc = encriptaB64(conta4.descricao);
                           string pass = encriptaB64(conta4.password);
                            temp.username = nome;
                            temp.descricao = desc;
                            temp.password = pass;
                            save(temp);
                        }
                        break;*/

                    case "list accounts":
                        Console.Clear();
                        if (File.ReadAllText(caminho) != "")
                        {
                            Console.WriteLine("[Contas]");
                            ListaContas lista = load(caminho);
                            foreach (Account conta3 in lista.contas)
                            {
                                Console.WriteLine(decriptaB64(conta3.descricao) + " - " + decriptaB64(conta3.username));
                            }
                            Console.ReadKey();
                        }
                        else { Console.WriteLine("Nao existem contas guardadas!"); Console.Read(); }
                        break;

                    case "get password":
                        Console.Clear();
                        Console.Write("Nome da conta: ");
                        string nome2 = Console.ReadLine();
                        bool encontrou = false;
                        foreach(Account conta5 in load(caminho).contas)
                        {
                            if(encriptaB64(nome2) == conta5.username)
                            {
                                encontrou = true;
                                Console.Clear();
                                print(conta5);
                                Console.ReadKey();
                            }

                        }
                        if (encontrou == true) { break; }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Conta nao encontrada, verifique se escreveu bem!");
                            Console.ReadKey();
                            break;
                        }

                    case "delete account":
                        Console.Clear();
                        ListaContas tempo = new ListaContas { };
                        Console.Write("Nome da conta: ");
                        string nome3 = Console.ReadLine();
                        bool encontrou2 = false;
                        foreach (Account conta5 in load(caminho).contas)
                        {
                            if (encriptaB64(nome3) == conta5.username)
                            {
                                encontrou2 = true;
                                Console.Clear();
                                Console.WriteLine("Conta " + nome3 + " apagada com sucesso!");
                                Console.ReadKey();
                            }
                            else
                            {
                                tempo.contas.Add(conta5);

                            }

                        }

                        if (encontrou2 == true) { File.WriteAllText(caminho, Newtonsoft.Json.JsonConvert.SerializeObject(tempo)); break; }
                        else
                        {

                            Console.Clear();
                            Console.WriteLine("Conta nao encontrada, verifique se escreveu bem!");
                            Console.ReadKey();
                            break;
                        }

                    default:
                        Console.Clear();
                        Console.WriteLine("Comando \"" + comando + "\" desconhecido!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public static ListaContas load(string caminho1)
        {
            ListaContas lista = Newtonsoft.Json.JsonConvert.DeserializeObject<ListaContas>(File.ReadAllText(caminho));
            return lista;
        }

        public static void save(Account conta)
        {
            ListaContas lista = new ListaContas { };
            if (File.ReadAllText(caminho) != "")
            {
                foreach (Account continha in load(caminho).contas)
                {
                    lista.contas.Add(continha);
                }
            }
            lista.contas.Add(conta);
            File.WriteAllText(caminho, Newtonsoft.Json.JsonConvert.SerializeObject(lista));
        }

        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password;
        }

        private static void print(Account conta)
        {
            Console.WriteLine("Descricao: " + decriptaB64(conta.descricao));
            Console.WriteLine("Username: " + decriptaB64(conta.username));
            Console.WriteLine("Password: " + decriptaB64(conta.password));
            Console.ReadKey();
        }

        private static string encriptaB64(string texto)
        {
            var bytes = Encoding.UTF8.GetBytes(texto);
            var base64 = Convert.ToBase64String(bytes);
            return base64.ToString();
        }

        private static string decriptaB64(string texto)
        {
            var base64 = texto;
            var data = Convert.FromBase64String(base64);
            var decoded = Encoding.UTF8.GetString(data);
            return decoded.ToString();
        }
    }
}
