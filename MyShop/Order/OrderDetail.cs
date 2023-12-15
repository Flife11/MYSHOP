using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int Order  { get; set; }
        public int Book { get; set; }
        public int Quantity {  get; set; }
    }
}
