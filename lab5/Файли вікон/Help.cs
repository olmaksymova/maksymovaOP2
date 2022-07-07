using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;

namespace CourseWork
{
    class Help
    {
        public static Dictionary<string, string> ColumnNames = new Dictionary<string, string>
        {
            { "date_contract","Дата підпису" },
            { "date_sell","Дата продажу" },
            { "price","Ціна авто" },
            { "comission","Комісія" },
            { "note","Примітка" },
            { "name","Ім'я (назва)" },
            { "brand","Марка авто" },
            { "year","Рік випуску" },
            { "mileage","Пробіг авто" },
            { "surname","Прізвище" },
            { "patronymic","По батькові" },
            { "city","Місто" },
            { "address","Адреса" },
            { "phone","Телефон" }
        };
        public static void DeleteContract(string id)
        {
            string fileName = $@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\CourseWork\contracts\{id}.txt";

            if (File.Exists(fileName))
            {
                try
                {
                    File.Delete(fileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine("The deletion failed: {0}", e.Message);
                }
            }
            else
            {
                Console.WriteLine("Specified file doesn't exist");
            }
        }
        public static string CreateContract(int id, (string, string) CustomerNames, string OwnerNames, string CAR,
            int YearContract, string Dealers, double PRICE)
        {
            NumberFormatInfo numFormInfo = new NumberFormatInfo()
            {
                NumberDecimalSeparator = ".",
            };
            string sqlConnection = @"Data Source = HELGAMAX\MSSQLSERVERS;" + "Initial Catalog = CarsDB;" + "Integrated Security = true";
            List<string> strokes = new List<string>();
            StreamReader ContractSample = new StreamReader(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\CourseWork\contract.txt");

            while (!ContractSample.EndOfStream)
            {
                string a = ContractSample.ReadLine();
                strokes.Add(a);
            }
            ContractSample.Close();


            var conn = new SqlConnection(sqlConnection);
            conn.Open();
            List<int> Helper = new List<int>();
            string path = $@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\CourseWork\contracts\{id}.txt";
            using (FileStream fs = File.Create(path))
            {
                int j = 0;
                string[] Line = strokes[j].Split(' ');
                while (Line[0] != "далі" && j < strokes.Count - 1)
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(strokes[j] + "\n");
                    fs.Write(info, 0, info.Length);
                    j++;
                    Line = strokes[j].Split(' ');
                }
                byte[] info1 = new UTF8Encoding(true).GetBytes(OwnerNames + ", ");
                fs.Write(info1, 0, info1.Length);

                while (Line[0] != "," && j < strokes.Count - 1)
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(strokes[j] + "\n");
                    fs.Write(info, 0, info.Length);
                    j++;
                    Line = strokes[j].Split(' ');
                }
                info1 = new UTF8Encoding(true).GetBytes(CustomerNames.Item1 + ", що проживає за адресою " + CustomerNames.Item2);
                fs.Write(info1, 0, info1.Length);

                while (Line[Line.Length - 1] != "(модель):" && j < strokes.Count - 1)
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(strokes[j] + "\n");
                    fs.Write(info, 0, info.Length);
                    j++;
                    Line = strokes[j].Split(' ');
                }
                j++;
                info1 = new UTF8Encoding(true).GetBytes("Марка(модель):" + CAR + " ");
                fs.Write(info1, 0, info1.Length);

                while (Line[0] != "рік" && j < strokes.Count - 1)
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(strokes[j] + "\n");
                    fs.Write(info, 0, info.Length);
                    j++;
                    Line = strokes[j].Split(' ');
                }
                j++;
                info1 = new UTF8Encoding(true).GetBytes(YearContract.ToString() + " рік, виданого експертом " + Dealers + ", становить " + PRICE + " доларів.\n");
                fs.Write(info1, 0, info1.Length);
                while (Line[0] != "доларів," && j < strokes.Count - 1)
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(strokes[j] + "\n");
                    fs.Write(info, 0, info.Length);
                    j++;
                    Line = strokes[j].Split(' ');
                }
                info1 = new UTF8Encoding(true).GetBytes(PRICE.ToString() + " ");
                fs.Write(info1, 0, info1.Length);
                while (j < strokes.Count)
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(strokes[j] + "\n");
                    fs.Write(info, 0, info.Length);
                    j++;
                }
            }
            var GetFiles = new SqlCommand($"update contracts set contract = '{path}' where id_contract = {id}", conn);
            GetFiles.ExecuteNonQuery();

            conn.Close();
            return path;
        }
    }
}
