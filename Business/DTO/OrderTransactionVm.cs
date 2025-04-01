using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class OrderTransactionVm<T>
    {
        public int OrderId { get; set; }
        public ActionType ActionType { get; set; }
        public T Data { get; set; }
    }
}
