using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Store
{
    public class Item : INotifyPropertyChanged
    {   public string Name { get; set; }
        public decimal Price { get; set; }
        private decimal totalPrice;
        public decimal TotalPrice
        {
            get
            {
                return totalPrice;
            }
            set
            {
                totalPrice = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalPrice)));
            }
        }
        private decimal quantity;
        public decimal Quantity {
            get
            {
                return quantity;
            }
            set
            {
                quantity = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Quantity)));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
