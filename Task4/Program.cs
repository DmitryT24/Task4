using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4
{
    internal class StudentInfo
    {
        public List<Student> listStud;
        public DirectoryInfo path;
        public bool SetListFile()
        {
            if (!ReadStudentsFromBinFile(Directory.GetCurrentDirectory(), out listStud))
                return false;

            if (!CreateDesctop())
                return false;

            if (!SetStudFile())
                return false;

            return true;
        }

        static bool ReadStudentsFromBinFile(string pathDirectory, out List<Student> result)
        {
            result = new List<Student>();
            var directoryFile = Path.Combine(pathDirectory, "students.dat");
            if (!File.Exists(directoryFile))
            {
                Console.WriteLine($"Файл students.dat отсутсвует в дирректории {pathDirectory}");
                Console.ReadKey();
                return false;
            }
            FileStream fs = new FileStream(directoryFile, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            // Console.WriteLine(sr.ReadToEnd());

            fs.Position = 0;

            BinaryReader br = new BinaryReader(fs);

            while (fs.Position < fs.Length)
            {
                Student student = new Student();

                student.Name = br.ReadString();
                student.Group = br.ReadString();
                long dt = br.ReadInt64();
                student.DateOfBirth = DateTime.FromBinary(dt);
                student.AverageScore = br.ReadDecimal();

                result.Add(student);
            }

            fs.Close();
            return true;
        }

        public void ReadInFile()
        {
            foreach (Student st in listStud)
            {
                Console.WriteLine($"-> {st.Name}   {st.Group} {st.AverageScore} {st.DateOfBirth.ToString("dd.MM.yyyy")} ");
            }
        }
        public bool CreateDesctop()
        {
            path = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Students"));// @"e:\C#\SKILLFACTORY\Task4\Students\");
            if (!path.Exists)
            {
                Console.WriteLine("Не удалось создать каталог на рабочем столе!!!");
                Console.ReadKey();
                return false;
            }
            return true;
        }
        public bool SetStudFile()
        {
            bool isFileCreat = true;
            foreach (Student st in listStud)
            {
                string textFile = Path.Combine(path.FullName, (string)st.Group + ".txt");
                StreamWriter textWriter = new StreamWriter(textFile, false, Encoding.UTF8);
                textWriter.Close();
            }

            foreach (Student st in listStud)
            {
                string textFile = Path.Combine(path.FullName, (string)st.Group + ".txt");
                StreamWriter textWriter = new StreamWriter(textFile, true, Encoding.UTF8);
                if (File.Exists(textFile))
                {
                    textWriter.WriteLine($"Имя - {st.Name,-5}\t, дата рождения: {st.DateOfBirth.ToString("dd.MM.yyyy")}\t, средний балл:{st.AverageScore}\t ");
                }
                else
                {
                    Console.WriteLine($"Упс! Не удалось создать файл! {(string)st.Group + ".txt"} ");
                    Console.ReadKey();
                    isFileCreat = false;
                }
                textWriter.Close();
            }
            return isFileCreat;
        }

    }



    internal class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal AverageScore { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                StudentInfo studentInfo = new StudentInfo();
                if (studentInfo.SetListFile())
                {
                    Console.WriteLine("Файлы успешно созданы!");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Внимание! Приложение выполнилось с ошибкой!");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Упс! Ошибка: {ex.Message}");
                Console.ReadKey();
            }
        }

    }
}
