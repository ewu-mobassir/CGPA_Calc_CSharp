namespace CGPA_Calculator
{
    class Student
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Department { get; set; }

        public Student(string name, string id, string department)
        {
            Name = name;
            Id = id;
            Department = department;
        }
        
        public Student(){}
    }

}