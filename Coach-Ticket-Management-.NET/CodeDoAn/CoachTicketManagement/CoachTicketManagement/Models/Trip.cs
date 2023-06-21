using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachTicketManagement.Models
{
    public class Trip
    {
        public virtual int IDTRIP { get; set; }
        public virtual int IDTIME { get; set; }
        public virtual int IDBUSLINE { get; set; }
        public virtual int IDEMPLOYEE { get; set; }
        public virtual int IDCOACH { get; set; }
        public virtual int IDDRIVER { get; set; }
        public virtual DateTime DEPARTUREDAY { get; set; }
        public virtual int AMOUNTEMPTYSEAT { get; set; }

    }
}
