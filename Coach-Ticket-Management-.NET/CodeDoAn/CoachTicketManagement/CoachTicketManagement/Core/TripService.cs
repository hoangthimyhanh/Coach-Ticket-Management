using CoachTicketManagement.Data;
using CoachTicketManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachTicketManagement.Core
{
    public class TripService
    {
        #region Singleton
        private static TripService instance;
        public static TripService Instance
        {
            get { if (instance == null) instance = new TripService(); return instance; }
            private set { instance = value; }
        }
        private TripService() { }
        #endregion

        public List<Trip> GetTrips()
        {
            List<Trip> trip = new List<Trip>();
            using (var session = NHibernateHelper.OpenSession())
            {
                trip = session.Query<Trip>().ToList();
            }
            return trip;
        }
        public Trip GetTrip(int idTrip)
        {
            return GetTrips().SingleOrDefault(x => x.IDTRIP == idTrip);
        }

    }
}
