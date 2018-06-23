using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Reflection;
using System.Text;
using CS_Perf.Models;

namespace CS_Perf
{
    class Program
    {
        const int size = 10000000;
        const string FILE_NAME = "1-lac-3-fields.csv";

        static void Main(string[] args)
        {
            //CreateCsvFile(size,50); 
            //TestArraySize(size);

            TestListOfEmployes();
            TestDictOfEmployes();
            TestListOfEmployeesStruct();
          

            /*
            var sizeInBytes = MemSize<List<Employee>>.SizeOf(employees);

            Console.WriteLine("Size List<Employee>[100000] :{0} in bytes", sizeInBytes);
            Console.WriteLine("Size List<Employee>[100000] :{0} in MB", ConvertBytesToMegabytes(sizeInBytes));
            */

            Console.ReadLine();
        }

        static void TestListOfEmployeesStruct ()
        {
            long before = GC.GetTotalMemory(true);
            List<EmployeeStruct> employees = ReadCsvFileAsStruct();
            long after = GC.GetTotalMemory(true);

            Console.WriteLine(String.Format("Read {0} employees.", employees.Count));


            var diff = after - before;

            Console.WriteLine("Total Memory (Bytes): List<EmployeesStruct> {0}", diff);
            Console.WriteLine("Total Memory (MB): List<EmployeesStruct> {0}", ConvertBytesToMegabytes(diff));
            Console.WriteLine("Total Memory (GB): List<EmployeesStruct> {0}", ConvertBytesToMegabytes(diff) / 1024);

            Console.WriteLine("Per Employee: " + diff / size);

            // Stop the GC from messing up our measurements
            GC.KeepAlive(employees);

        }

        static void TestListOfEmployes()
        {

            long before = GC.GetTotalMemory(true);
            List<Employee> employees = ReadCsvFileAsList();
            long after = GC.GetTotalMemory(true);

            Console.WriteLine(String.Format("Read {0} employees.", employees.Count));


            var diff = after - before;

            Console.WriteLine("Total Memory (Bytes): List<Employees> {0}", diff);
            Console.WriteLine("Total Memory (MB): List<Employees> {0}", ConvertBytesToMegabytes(diff));
            Console.WriteLine("Total Memory (GB): List<Employees> {0}", ConvertBytesToMegabytes(diff)/1024);

            Console.WriteLine("Per Employee: " + diff / size);

            // Stop the GC from messing up our measurements
            GC.KeepAlive(employees);

        }


        static void TestDictOfEmployes()
        {

            long before = GC.GetTotalMemory(true);
            List<dynamic> employees = ReadCsvFileAsDict();
            long after = GC.GetTotalMemory(true);

            Console.WriteLine(String.Format("DICT: Read {0} employees.", employees.Count));


            var diff = after - before;

            Console.WriteLine("Total Memory (Bytes): DICT<> {0}", diff);
            Console.WriteLine("Total Memory (MB): DICT<> {0}", ConvertBytesToMegabytes(diff));
            Console.WriteLine("Total Memory (GB): DICT<> {0}", ConvertBytesToMegabytes(diff) / 1024);

            Console.WriteLine("Per Employee: " + diff / size);

            // Stop the GC from messing up our measurements
            GC.KeepAlive(employees);

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

            Console.WriteLine("Array Per object: " + diff / size);

            // Stop the GC from messing up our measurements
            GC.KeepAlive(array);
        }

        static string GetCSVFileName()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\data\" + FILE_NAME;
        }
        static List<Employee> ReadCsvFileAsList()
        {
            string filePath = GetCSVFileName();
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
                Employee e = new Employee(id, firstName, input);

                employees.Add(e);
                //Console.WriteLine(input[1]);
            }

            return employees;
        }

        static List<EmployeeStruct> ReadCsvFileAsStruct()
        {
            string filePath = GetCSVFileName();
            List<EmployeeStruct> employees = new List<EmployeeStruct>();
            char[] separators = { ',' };
            string[] input;

            StreamReader sr = new StreamReader(filePath);

            string data = string.Empty;

            while ((data = sr.ReadLine()) != null)
            {
                input = data.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                var id = Convert.ToInt32(input[0]);
                var firstName = input[1];
                EmployeeStruct e = new EmployeeStruct(id, firstName, input);

                employees.Add(e);
                //Console.WriteLine(input[1]);
            }

            return employees;
        }


        static List<dynamic> ReadCsvFileAsDict()
         {
            string filePath = GetCSVFileName();

            List<dynamic> employees = new List<dynamic>();

            char[] separators = { ',' };
            string[] input;

            StreamReader sr = new StreamReader(filePath);

            string data = string.Empty;

            
            while ((data = sr.ReadLine()) != null)
            {
                int i = 0;

                IDictionary<string, object> fields = new ExpandoObject();

                input = data.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                var id = Convert.ToInt32(input[0]);
                var firstName = input[1];


                foreach(var f in input)
                {
                    fields["field" + i.ToString()] = f;
                    i++;
                }

                employees.Add(fields);
                //Console.WriteLine(input[1]);
            }

            return employees;
        }

        static void CreateCsvFile(int noOfRecords, int noOfFields)
        {
            string filePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +  @"\data\employees.csv";

            Console.WriteLine(filePath);

            List<string> fields = new List<string>();
            string field = "id";
            using (var w = new StreamWriter(filePath))
            {
                for ( var i = 1; i <= noOfRecords; i++)
                {
                    var id = i.ToString();
                    fields.Add(id);

                    for(var col = 0; col < noOfFields; col++)
                    {
                        field = string.Format("field-{0}-{1}", i, col); // "field-" + i.ToString() + col.ToString();
                        fields.Add(field);
                    }
                    var line = string.Join(",", fields);

                    w.WriteLine(line);

                    fields = new List<string>();

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
