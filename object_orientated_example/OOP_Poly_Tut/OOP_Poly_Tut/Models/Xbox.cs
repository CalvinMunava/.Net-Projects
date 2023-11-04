using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OOP_Poly_Tut.Models
{
    public class Xbox: GamingConsole
    {

        //Data members

        public string _XboxLiveID;
        public string _BatteryType;


        //Default Constructors 


        public Xbox() : base()
        {

        }
        public Xbox(int ID, string Name, string Description, string Model, double Price, int Quantity, string Image, DateTime RealeaseDate,string consoleType, string XboxLiveID, string BatteryType) : base(ID, Name, Description, Model, Price, Quantity, Image, RealeaseDate, consoleType)
        {
            _XboxLiveID = XboxLiveID;
            _BatteryType = BatteryType;
        }

        //Methods

        public override string ItemAbout()
        {
            return "The Following Xbox Is a " + this.Model + " series. \n " +
                   "Make Sure to Use " + this._BatteryType + " with this model.";
        }

        public override double ItemPriceWithVat()
        {
            return this._Price + Math.Round(this._Price * parentVat, 2);
        }

        public override string ItemDelay()
        {

            return "Shipping in 5 days ";
        }


        //Properties

        public string XboxLiveID { get { return _XboxLiveID; } set { _XboxLiveID = value; } }

        public string BatteryType { get { return _BatteryType; } set { _BatteryType = value; } }
    }
}