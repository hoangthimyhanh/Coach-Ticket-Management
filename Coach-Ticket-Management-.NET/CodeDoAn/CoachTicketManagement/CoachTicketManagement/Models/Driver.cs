using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachTicketManagement.Models
{
    public class Driver : Person
    {
        public virtual string DEGREE { get; set; }
        public virtual string IDTRIP { get; set; }
    }
}
