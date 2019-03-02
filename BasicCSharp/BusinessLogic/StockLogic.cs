﻿using BasicCSharp.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BasicCSharp.BusinessLogic
{
    public class StockLogic
    {
        private readonly string _conString;

        public StockLogic(string connectionString)
        {
            _conString = connectionString;
        }

        public DataTable GetAllCategory()
        {
            DACategory dACategory = new DACategory(_conString);
            return dACategory.GetAllCategory();
        }

        public DataTable GetAllItem()
        {
            DAItem dAItem = new DAItem(_conString);
            return dAItem.GetAllItem();
        }

        public void AddCategory(string categoryName)
        {
            DACategory dACategory = new DACategory(_conString);
            if (!string.IsNullOrEmpty(categoryName))
            {
                if (dACategory.CategoryIsNotExist(categoryName.Trim()))
                {
                    dACategory.AddCategory(categoryName);
                }
            }
        }

        public void AddItem(string itemName, string itemPrice, int categoryId)
        {
            DAItem dAItem = new DAItem(_conString);
            if (!string.IsNullOrEmpty(itemName) && categoryId != 0)
            {
                if (dAItem.ItemIsNotExist(itemName.Trim()))
                {
                    dAItem.AddItem(itemName, itemPrice, categoryId);
                }
            }
        }


        public void UpdateItem(string CONTSTANT_CatID ,string itemId, string itemName, string itemPrice, string categoryId)
        {
            DAItem dAItem = new DAItem(_conString);
            if (!string.IsNullOrEmpty(itemName) && categoryId != null)
            {
                string itemOldName = GetItemName(itemId);
                string categoryNewName = GetCategoryName(categoryId);
                string categoryOldName = GetCategoryName(CONTSTANT_CatID); //Set CONTSTANT_CatID from Method GvItem_Selected 
                UpdatePriceOrderItem(itemName, itemPrice);
                UpdateItemNameOrderItem(itemOldName, itemName);
                UpdateCategoryOrderItem(categoryOldName, categoryNewName);


                dAItem.UpdateItem(itemId, itemName, itemPrice, categoryId);

            }
        }

        public void UpdateCategory(string categoryName, string categoryId)
        {
            DACategory dACategory = new DACategory(_conString);
            DAOrderItem dAOrderItem = new DAOrderItem(_conString);
            if (!string.IsNullOrEmpty(categoryName))
            {
                if (dACategory.CategoryIsNotExist(categoryName.Trim()))
                {
                    string categoryOldName = dACategory.GetCategoryName(categoryId);
                    dAOrderItem.UpdateCategoryOrderItem(categoryOldName, categoryName);
                    dACategory.UpdateCategory(categoryName, categoryId);
                }
            }
        }



    }
}