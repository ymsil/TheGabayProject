using System;
using System.Collections.Generic;
using System.Linq;
using BE;
//using DS;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using System.ComponentModel;

namespace DAL
{
    class Dal_Xml_imp : IDal
    {
        private XElement _prayersRoot;
        private static readonly string _cad = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        readonly string _prayersPath = Path.Combine(_cad + @"...\members.xml");

        
        public Dal_Xml_imp()
        {
            if (!File.Exists(_prayersPath)) CreateFile();
            else LoadData();
        }

        private readonly Random _r = new Random();

        private void CreateFile()
        {
           _prayersRoot = new XElement("Prayers");
            _prayersRoot.Save(_prayersPath);
        }
        private void LoadData()
        {
            try
            {
                _prayersRoot = XElement.Load(_prayersPath);
            }
            catch
            {
                throw new Exception("בעיה בטעינת קבצים");
            }
        }

        private XElement ConvertPrayer(Prayer prayer)
        {
            XElement prayerElement = new XElement("prayer");
            foreach (PropertyInfo item in typeof(BE.Prayer).GetProperties())
                prayerElement.Add
                    (
                    new XElement(item.Name, item.GetValue(prayer, null))
                    );
            return prayerElement;
        }
        Prayer ConvertPrayer(XElement element)
        {
            Prayer prayer = new Prayer();
            foreach (PropertyInfo item in typeof(BE.Prayer).GetProperties())
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(item.PropertyType);
                object convertValue = typeConverter.ConvertFromString(element.Element(item.Name).Value);

                item.SetValue(prayer, convertValue);
            }
            return prayer;
        }

        public void AddPrayer(Prayer prayer)
        {
            bool flag = false;
            Prayer p = new Prayer();

            if (prayer.Id == 0)
            {
                while (flag == false)
                {
                    var t = _r.Next(1, 1000);
                    prayer.Id = t;
                    p = GetPrayer(prayer.Id);
                    if (p == null)
                        flag = true;
                }

                _prayersRoot.Add(ConvertPrayer(prayer));
                _prayersRoot.Save(_prayersPath);
            }
            else
            {
                if (prayer.Id < 0)
                    throw new Exception("המספר שהוכנס אינו חוקי");

                p = GetPrayer(prayer.Id);
                if (p != null)
                    throw new Exception("קיים מתפלל עם אותו מספר מזהה");

                _prayersRoot.Add(ConvertPrayer(prayer));
                _prayersRoot.Save(_prayersPath);
            }
        }
        public bool RemovePrayer(int prayerId)
        {
            XElement toDelete = (from item in _prayersRoot.Elements()
                let xElement = item.Element("Id")
                where xElement != null && int.Parse(xElement.Value) == prayerId
                                 select item).FirstOrDefault();

            if (toDelete == null)
                throw new Exception("לא קיים מתפלל כזה במערכת");

            toDelete.Remove();
            _prayersRoot.Save(_prayersPath);
            return true;
        }
        public void UpdatePrayer(Prayer prayer)
        {
            XElement toUpdate = _prayersRoot.Elements()
                .Select(item => new {item, xElement = item.Element("Id")})
                .Where(t => t.xElement != null && int.Parse(t.xElement.Value) == prayer.Id)
                .Select(t => t.item).FirstOrDefault();

            if (toUpdate == null)
                throw new Exception("מתפלל זה לא קיים");

            foreach (PropertyInfo item in typeof(BE.Prayer).GetProperties())
            {
                XElement xElement = toUpdate.Element(item.Name);
                if (xElement != null) xElement.SetValue(item.GetValue(prayer));
            }

            _prayersRoot.Save(_prayersPath);
        }
        
        public Prayer GetPrayer(int prayerId)
        {
            XElement prayer;
            try
            {
                prayer = (from item in _prayersRoot.Elements()
                    let xElement = item.Element("Id")
                    where xElement != null && int.Parse(xElement.Value) == prayerId
                          select item).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            if (prayer == null)
                return null;

            return ConvertPrayer(prayer);
        }

        public IEnumerable<Prayer> GetAllPrayers(Func<Prayer, bool> p = null)
        {
            if (p == null)
                return from prayer in _prayersRoot.Elements()
                       select ConvertPrayer(prayer);

            return from item in _prayersRoot.Elements()
                   let s = ConvertPrayer(item)
                   where p(s)
                   select s;
        }

        public bool Empty(Prayer prayer)
        {
            if (_prayersRoot.IsEmpty)
                return true;

            return false;
        }
    }
}
