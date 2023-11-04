using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OOP_Poly_Tut.Models
{
    public class Playstation : GamingConsole
    {

        //Data members
        public string _PlaystationNetworkID;
        public int _DualShockRating;


        //Default Constructors 

        public Playstation() : base()
        {

        }

        public Playstation(int ID, string Name, string Description, string Model, double Price, int Quantity, string Image, DateTime RealeaseDate,string consoleType ,string PlaystationNetworkID,int DualShockRating) : base (ID,Name, Description, Model, Price, Quantity, Image, RealeaseDate, consoleType)
        {
            _PlaystationNetworkID = PlaystationNetworkID;
            _DualShockRating = DualShockRating;
        }


        //Methods

        public override string ItemAbout()
        {
            return "The Following Playstation Is " + this.Model + ". \n " +
                   "Uses NetworkID " + this._PlaystationNetworkID + " .";
        }

        public override double ItemPriceWithVat()
        {
            return this._Price + Math.Round(this._Price * parentVat, 2);
        }

        public override string ItemDelay()
        {

            return "Shipping in  " + Convert.ToString(calculateShipping()) + " days.";
        }


        private int calculateShipping()
        {
            Random days = new Random();
            int day = days.Next(0, 14);
            return day;
        }




        //Properties

        public string PlaystationNetworkID { get { return _PlaystationNetworkID; } set { _PlaystationNetworkID = value; } }

        public int DualShockRating { get { return _DualShockRating; } set { _DualShockRating = value; } }

    }
}