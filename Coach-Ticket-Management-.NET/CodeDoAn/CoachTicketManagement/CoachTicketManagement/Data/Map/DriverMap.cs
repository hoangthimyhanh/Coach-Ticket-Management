using CoachTicketManagement.Models;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachTicketManagement.Data.Map
{
    public class DriverMap : ClassMap<Driver>
    {
        public DriverMap()
        {
            Id(x => x.Id, "IDDRIVER");
            Map(x => x.Name, "NAMEDRIVER");
            Map(x => x.IdWard);
            Map(x => x.DateOfBirth, "DATEOFBIRTHDRIVER");
            Map(x => x.Gender, "GENDERDRIVER");
            Map(x => x.IdentityCard, "IDENTITYCARDDRIVER");
            Map(x => x.Phone, "PHONEDRIVER");
            Map(x => x.Email, "EMAILDRIVER");
            Map(x => x.DEGREE);
            Table("TBL_DRIVER");
        }
    }
}
