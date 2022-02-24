using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CGPA_Calculator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            PrintHead();
            
            Console.Write("Enter Student ID: ");
            var stdId = Console.ReadLine();
            var student = FetchStudent(stdId);
            PrintStudentInfo(student);
            Menu(student.Id);

        }

        static void Menu(string stdId)
        {
            while (true)
            {
                Console.WriteLine("-----------------------------------------------------------------------------");
                Console.WriteLine("Select Option: ");
                Console.WriteLine("\t1. Calculate CGPA");
                Console.WriteLine("\t2. Add Semester");
                Console.WriteLine("\t3. Exit");
                Console.Write("Function: ");
                var key = Convert.ToInt16(Console.ReadLine());
                switch (key)
                {
                    case 1:
                        CGPACalc(stdId);
                        break;
                    case 2:
                        AppendSemester(stdId);
                        break;
                    case 3:
                        break;
                    default:
                        break;
                }

                if (key == 3 || key == 0)
                {
                    PrintExitDialog();
                    break;
                }
            }
        }

        static void CGPACalc(string stdId)
        {
            
            string[] temp;
            int n;
            string sem, code, title, grade;
            double credit, semCgpa, semCredit, semGpaCr, tempGpaCr;

            double cgpa = 0, gpaCr = 0, totCredit = 0;
            
            using (StreamReader sr = File.OpenText(stdId+".txt"))
            {
                sr.ReadLine(); // Skip first 2 lines
                sr.ReadLine(); 
                
                while (!sr.EndOfStream)
                {
                    Console.WriteLine("-----------------------------------------------------------------------------");
                    temp = sr.ReadLine().Split('\t');
                    sem = temp[0];
                    n = Convert.ToInt16(temp[1]);

                    Console.WriteLine("Semester: \t"+sem+"\n");
                    semCgpa = 0;
                    semCredit = 0;
                    semGpaCr = 0;
                    Console.WriteLine("Course\tTitle of The Course\tCredits\tGrade\tGPACredits");
                    for (int i = 0; i < n; i++)
                    {
                        temp = sr.ReadLine().Split('\t');
                        code = temp[0];
                        title = temp[3];
                        credit = Convert.ToDouble(temp[1]);
                        grade = temp[2];
                        tempGpaCr = GetGpaCr(credit, grade);
                        Console.WriteLine(code+"\t"+PadTitle(title)+credit+"\t"+grade+"\t"+tempGpaCr);
                        semCredit += credit;
                        semGpaCr += tempGpaCr;
                    }
                    semCgpa = semGpaCr / semCredit;
                    totCredit += semCredit;
                    gpaCr += semGpaCr;
                    cgpa = gpaCr / totCredit;
                    
                    Console.WriteLine("-----------------------------------------------------------------------------");
                    Console.WriteLine("\t\tCGPA: "+Math.Round(cgpa, 2)+"\t\tTerm GPA: "+Math.Round(semCgpa,2));
                }
            }
        }

        static string PadTitle(string title)
        {
            var size = title.Length;
            if (size < 8)
                return title + "\t\t\t";
            else if (size <16)
                return title + "\t\t";
            else if (size <24)
                return title + "\t";
            else
                return title;
        }

        static double GetGpaCr(double credit, string grade)
        {
            return credit * ConvertGrade(grade);
        }


        static void AppendSemester(string stdId)
        {
            Console.WriteLine("-----------------------------------------------------------------------------");
            Console.Write("\tInput Semester Name (eg. Summer-2018): ");
            var sem = Console.ReadLine();
            Console.Write("\tInput Number of Courses in Semester: ");
            var n = Convert.ToInt16(Console.ReadLine());
            string code, title, credit, grade;
            using (StreamWriter sr = File.AppendText(stdId+".txt"))
            {
                sr.WriteLine(sem+"\t"+n);
                for (int i = 0; i < n; i++)
                {   
                    Console.WriteLine();
                    Console.Write("\tCourse code for Course "+(i+1)+": ");
                    code = Console.ReadLine();
                    Console.Write("\tCourse title for Course "+(i+1)+": ");
                    title = Console.ReadLine();
                    Console.Write("\tCourse credit for Course "+(i+1)+": ");
                    credit = Console.ReadLine();
                    Console.Write("\tCourse grade for Course "+(i+1)+": ");
                    grade = Console.ReadLine();
                    sr.Write(code+"\t"+credit+"\t"+grade.ToUpper()+"\t"+title);
                    sr.WriteLine();
                }
            }
        }

        static void PrintExitDialog()
        {
            Console.WriteLine("\nThe system will now exit! Thanks for using CGPA Calculator!");
            Console.WriteLine("-----------------------------------------------------------------------------");
        }

        static Student FetchStudent(string stdId)
        {
            Student student;
            string name, dept;
            var fileId = stdId + ".txt";

            Console.WriteLine("-----------------------------------------------------------------------------");

            if (File.Exists(fileId) && File.ReadAllText(fileId).Length > 0)
            {
                var lines = File.ReadAllLines(stdId + ".txt");
                name = lines[0];
                dept = lines[1];
            }
            else
            {
                Console.WriteLine("Student file does not exist! Input student information to create a new file.");
                Console.Write("Enter Student Name: ");
                name = Console.ReadLine();
                Console.Write("Enter Student Department: ");
                dept = Console.ReadLine();
                string[] lines = {name, dept};
                /*
                File.Create(fileId);
                */
                using (StreamWriter sr = File.CreateText(fileId))
                {
                    sr.WriteLine(name);
                    sr.WriteLine(dept);
                }
            }
            student = new Student(name, stdId, dept);
            return student;
        }

        static void PrintHead()
        {
            Console.WriteLine("-----------------------------------------------------------------------------");
            Console.WriteLine("\t\t\t    EAST WEST UNIVERSITY!");
            Console.WriteLine("\t\t\t       CGPA Calculator");
            Console.WriteLine("-----------------------------------------------------------------------------");
        }

        static void PrintStudentInfo(Student student)
        {
            Console.Write("\t\tStudent Name:\t\t");
            Console.WriteLine(student.Name);
            Console.Write("\t\tStudent ID:\t\t");
            Console.WriteLine(student.Id);
            Console.Write("\t\tStudent Department:\t");
            Console.WriteLine(student.Department);
            Console.WriteLine("-----------------------------------------------------------------------------");
        }


        static double ConvertGrade(string grade)
        {
            double cg = 0;
            switch (grade)
            {
                case "A+":
                    cg = 4.0;
                    break;
                case "A":
                    cg = 4.0;
                    break;
                case "A-":
                    cg = 3.7;
                    break;
                case "B+":
                    cg = 3.3;
                    break;
                case "B":
                    cg = 3.0;
                    break;
                case "B-":
                    cg = 2.7;
                    break;
                case "C+":
                    cg = 2.3;
                    break;
                case "C":
                    cg = 2.0;
                    break;
                case "C-":
                    cg = 1.7;
                    break;
                case "D+":
                    cg = 1.3;
                    break;
                case "D":
                    cg = 1.0;
                    break;
                case "F":
                    cg = 0.0;
                    break;
            }

            return cg;
        }
    }
}