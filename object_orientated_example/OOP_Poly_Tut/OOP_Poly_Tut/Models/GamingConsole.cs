using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OOP_Poly_Tut.Models
{
    public abstract class GamingConsole
    {
        //Data members

        public List<Playstation> Pslist = new List<Playstation>();
        public List<Xbox> Xblist = new List<Xbox>();

        public int _Id;
        public string _Name;
        public string _Description;
        public string _Model;
        public double _Price;
        public int _Quantity;
        public string _Image;
        public DateTime _ReleaseDate;
        public string _ShippingTime;
        public double parentVat = 0.18;
        public string _consoleType;

        //Default Constructors 

        public GamingConsole() //Parameterless constructor
        {

        }

        public GamingConsole(int ID, string Name, string Description, string Model, double Price, int Quantity, string Image, DateTime RealeaseDate, string consoleType)
        {
            _Id = ID;
            _Name = Name;
            _Description = Description;
            _Model = Model;
            _Price = Price;
            _Quantity = Quantity;
            _Image = Image;
            _ReleaseDate = RealeaseDate;
            _consoleType = consoleType;
        }


        //Methods

        public abstract string ItemAbout();

        public abstract string ItemDelay();

        public abstract double ItemPriceWithVat();



        //Properties

        public int ID { get { return _Id; } set { _Id = value; } }

        public string Name { get { return _Name; } set { _Name = value; } }

        public string Description { get { return _Description; } set { _Description = value; } }

        public string Model { get { return _Model; } set { _Model = value; } }

        public double Price { get { return _Price; } set { _Price = value; } }

        public int Quantity { get { return _Quantity; } set { _Quantity = value; } }

        public string Image { get { return _Image; } set { _Image = value; } }

        public DateTime ReleaseDate { get { return _ReleaseDate; } set { _ReleaseDate = value; } }

    }
}