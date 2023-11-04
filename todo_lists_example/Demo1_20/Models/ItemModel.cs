using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Demo1_20.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo1_20.Models
{
    public class ItemModel : CategoryModel
    {

        //Datamembers
        public int mitemID;
        public string mitemName;
        public int mCatID;
        public bool mCompleted;
        //End Data Members

        //Default Contructors

        public ItemModel() : base()
        {

        }

        public ItemModel(int itemID, string itemName, int catID, bool Completed) : base(catID)
        {
            mitemID = itemID;
            mitemName = itemName;
            mCatID = catID;
            mCompleted = Completed;

        }

        //End Default Contructors

        //Methods

        //End Metheds

        //Properties

        public int itemID
        {
            get { return mitemID; }

            set { mitemID = value; }
        }

        public string itemName
        {
            get { return mitemName; }

            set { mitemName = value; }

        }

        public bool Completed
        {
            get { return mCompleted; }

            set { mCompleted = value; }

        }
        //End Properties
    }
}