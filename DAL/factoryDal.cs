using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class FactoryDal
    {
        public static IDal GetDal()
        {
            return new Dal_Xml_imp();
            //return new Dal_imp();
        }
    }
}