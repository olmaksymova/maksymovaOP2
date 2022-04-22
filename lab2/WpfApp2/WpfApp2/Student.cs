namespace WpfApp2
{
    struct Student
    {
        private long ID;
        private string FirstName;
        private string SecondName;
        private string Group;

        public Student(long ID, string FirstName, string SecondName, string Group)
        {
            this.ID = ID;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.Group = Group;
        }

        public long getID() => ID;

        public string PrintStudent()
        {
            return $"{ID} {SecondName} {FirstName} {Group}";
        }

    }
}
