using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Programmesana_Sanija_Airita.Models;
namespace Programmesana_Sanija_Airita.Controllers.DataAccess
{
    public class ConnectionClass
    {
        public ProgrammesanaEntities Entity { get; set; }
        public ConnectionClass()
        {
            Entity = new ProgrammesanaEntities();
        }
    }
}