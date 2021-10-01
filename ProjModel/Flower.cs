using System;
using System.Collections.Generic;

#nullable disable

namespace FlowerStore.ProjModel
{
    public partial class Flower
    {
        public Flower()
        {
            Carts = new HashSet<Cart>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Occassion { get; set; }
        public int? AvailableQuantity { get; set; }
        public double? UnitPrice { get; set; }
        public byte[] FlImage { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
