using System;
using System.Collections.Generic;

#nullable disable

namespace FlowerStore.ProjModel
{
    public partial class Cart
    {
        public Cart()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int CartId { get; set; }
        public int? CustomerId { get; set; }
        public int? FlowerId { get; set; }
        public int? Quantity { get; set; }
        public double? ItemPrice { get; set; }
        public string Status { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Flower Flower { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
