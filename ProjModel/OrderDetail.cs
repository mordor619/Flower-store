using System;
using System.Collections.Generic;

#nullable disable

namespace FlowerStore.ProjModel
{
    public partial class OrderDetail
    {
        public int OrderId { get; set; }
        public int? FlowerId { get; set; }
        public int? CustomerId { get; set; }
        public int? CartId { get; set; }
        public double? Totalprice { get; set; }
        public string Remark { get; set; }
        public string PaymentStatus { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Flower Flower { get; set; }
    }
}
