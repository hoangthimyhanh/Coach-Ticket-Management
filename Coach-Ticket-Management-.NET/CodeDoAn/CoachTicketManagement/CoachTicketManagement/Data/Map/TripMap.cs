using CoachTicketManagement.Models;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachTicketManagement.Data.Map
{
    public class TripMap : ClassMap<Trip>
    {
        public TripMap()
        {
            Id(x => x.IDTRIP);
            Map(x => x.IDTIME);
            Map(x => x.IDBUSLINE);
            Map(x => x.IDEMPLOYEE);
            Map(x => x.IDCOACH);
            Map(x => x.IDDRIVER);
            Map(x => x.DEPARTUREDAY);
            Map(x => x.AMOUNTEMPTYSEAT);
            Table("TBL_TRIP");

        }
    }
}
