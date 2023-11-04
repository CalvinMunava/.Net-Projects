using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Demo1_20.Models;

namespace Demo1_20.Models
{
 

        public class CategoryModel
        {
        //Datamembers
            public int mcatID;
            public string mcatName;//Chocolates
            public List<ItemModel> mItems = new List<ItemModel>(); //[barone,topdeck,kitkat]


            //Default Contructors

            public CategoryModel()
            {
            }

            public CategoryModel(int catID)
            {
                this.catID = catID;
            }

            public CategoryModel(int catID, string catName, List<ItemModel> Items)
            {
                mcatID = catID;
                mcatName = catName; 
                mItems = Items;

            }
        //End Default Contructors

        //Methods

        public void addItem(ItemModel item)
        {
            mItems.Add(item);
        }


        public void removeItem(ItemModel item)
        {
            mItems.Remove(item);
        }

        //End Methods


        //Properties

        public int catID
            {
                get { return mcatID; }

                set { mcatID = value; }
            }

        public string catName
            {
                get { return mcatName; }

                set { mcatName = value; }

            }

        public List<ItemModel> Items
        {
            get { return mItems; }
        }

        //End Properties



    }
}
