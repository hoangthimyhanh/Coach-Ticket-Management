using CoachTicketManagement.Data;
using CoachTicketManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachTicketManagement.Core
{
    public class DriverService
    {
        #region Singleton
        private static DriverService instance;
        public static DriverService Instance
        {
            get { if (instance == null) instance = new DriverService(); return instance; }
            private set { instance = value; }
        }
        private DriverService() { }
        #endregion

        public List<Driver> GetDrivers()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session.Query<Driver>().ToList();
            }
        }
        public Driver GetDriver(int idDriver)
        {
            return GetDrivers().SingleOrDefault(x => x.Id == idDriver);
        }
    }
}
