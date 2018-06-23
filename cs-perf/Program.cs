using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CS_Perf.Models;

namespace CS_Perf
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateCsvFile(5000000);  // 50 lac
            TestArraySize(10000000);

            List<Employee> employees = ReadCsvFile();

            Console.WriteLine(String.Format("Read {0} employees.", employees.Count));

            var sizeInBytes = MemSize<List<Employee>>.SizeOf(employees);

            Console.WriteLine("Size List<Employee>[100000] :{0} in bytes", sizeInBytes);
            Console.WriteLine("Size List<Employee>[100000] :{0} in MB", ConvertBytesToMegabytes(sizeInBytes));



            Console.ReadLine();
        }

        static void TestArraySize(int size)
        {
            //int size = 10000000;
            object[] array = new object[size];

            long before = GC.GetTotalMemory(true);
            for (int i = 0; i < size; i++)
            {
                array[i] = new object();
            }
            long after = GC.GetTotalMemory(true);

            double diff = after - before;

            Console.WriteLine("Per object: " + diff / size);

            // Stop the GC from messing up our measurements
            GC.KeepAlive(array);
        }

        static List<Employee> ReadCsvFile()
        {
            string filePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\data\employees.csv";
            List<Employee> employees = new List<Employee>();
            char[] separators = { ',' };
            string[] input;

            StreamReader sr = new StreamReader(filePath);

            string data = string.Empty;

            while ((data = sr.ReadLine()) != null)
            {
                input = data.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                var id = Convert.ToInt32(input[0]);
                var firstName = input[1];
                Employee e = new Employee(id, firstName);

                employees.Add(e);
                //Console.WriteLine(input[1]);
            }

            return employees;
        }

        static void CreateCsvFile(int noOfRecords)
        {
            string filePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +  @"\data\employees.csv";

            Console.WriteLine(filePath);

            using (var w = new StreamWriter(filePath))
            {
                for ( var i = 1; i <= noOfRecords; i++)
                {
                    var id = i.ToString();
                    var name = "name " + i.ToString();
                    var line = string.Format("{0},{1}", id, name);
                    w.WriteLine(line);
                    w.Flush();
                }
            }
        }

        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        static double ConvertKilobytesToMegabytes(long kilobytes)
        {
            return kilobytes / 1024f;
        }
    }
}
