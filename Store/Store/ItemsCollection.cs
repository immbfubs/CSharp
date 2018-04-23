using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;

namespace Store
{
    public class ItemsCollection : ObservableCollection<Item>
    {
        public decimal Total { get; set; }
        public void PutInBasket (Item itm)
        {
            bool newItem = true;
            foreach (Item i in this)
            {
                if (i.Name == itm.Name && i.Price == itm.Price)
                {
                    i.Quantity += itm.Quantity;
                    i.TotalPrice = Decimal.Round(i.Quantity * i.Price,2);
                    newItem = false;
                }
            }
            if (newItem)
            {
                this.Add(itm);
            }
        }
    }
}