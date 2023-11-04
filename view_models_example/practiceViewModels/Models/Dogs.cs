using System;


namespace practiceViewModels.Models
{

    public class Dogs
    {

        //Data Members
        public int _ID;
        private string _NameOfDog;
        private string _Bread;
        private int _Age;

        //Default Constructors
        public Dogs()
        {
            _ID = 0;
            _NameOfDog = "";
            _Bread = "";
            _Age = 0;

        }

        //Constructor
        public Dogs(int ID,string NameOfDog, string Bread, int Age)
        {
            _ID = ID;
            _NameOfDog = NameOfDog;
            _Bread = Bread;
            _Age = Age;
        }


        //Methods
        public bool checkAge(int Age)
        {
            if(Age > 18)
            {
                return true;
            }
            return false;
        }

        public string checkBreed(string Bread)
        {
            if (Bread == "rotweiler")
            {
                return "Has Black fur";
            }
            else if(Bread == "German")
            {
                return "works for the cops";
            }
            else
            {
                return "idk";
            }
        }


        //properties
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }

        }
        public string NameOfDog
        {
         get{ return _NameOfDog; }
         set { _NameOfDog = value; }
            
        }

        public string Bread
        {
            get { return _Bread; }
            set { _Bread = value; }

        }

        public int Age
        {
            get { return _Age; }
            set { _Age = value; }

        }
    }
}
