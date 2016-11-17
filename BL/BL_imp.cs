using System;
using System.Collections.Generic;
using System.Linq;
using BE;

namespace BL
{
    public class BL_imp : IBL
    {
        private readonly DAL.IDal dal;

        public BL_imp()
        {
            dal = DAL.FactoryDal.GetDal();
            // init();
        }

        public void AddPrayer(Prayer prayer)
        {
            if (Empty(prayer) != true)
            {
                var b = from p in GetAllPrayers()
                    where p.LastName == prayer.LastName && p.FirstName == prayer.FirstName
                    select p;
                if (b.Count() != 0)
                    throw new Exception("There is a prayer with the exact same name");
            }
            dal.AddPrayer(prayer);
        }

        public bool RemovePrayer(int prayerId)
        {
            return dal.RemovePrayer(prayerId);
        }

        public void UpdatePrayer(Prayer prayer)
        {
            //var b = GetAllPrayers(t => t.LastName == prayer.LastName
            //                                && t.FirstName == prayer.FirstName
            //                                && t.BMparasha == prayer.BMparasha
            //                                && t.Tribe == prayer.Tribe
            //                                && t.LastAliyaParasha == prayer.LastAliyaParasha
            //                                && t.LastAliyaInThisYear == prayer.LastAliyaInThisYear
            //                                );
            var b = from t in GetAllPrayers()
                where t.LastName == prayer.LastName
                      && t.FirstName == prayer.FirstName
                      && t.BMparasha == prayer.BMparasha
                      && t.Tribe == prayer.Tribe
                      && t.LastAliyaParasha == prayer.LastAliyaParasha
                      && t.LastAliyaInThisYear == prayer.LastAliyaInThisYear
                select t;

            if (b.Count() > 1)
                throw new Exception("There is a prayer with the same name, parasha, heritage & the same last aliya");
            dal.UpdatePrayer(prayer);
        }

/*
                                                                        public Prayer GetPrayer(int prayerId)
                                                                        {
                                                                            return dal.GetPrayer(prayerId);
                                                                        } 
                                                                */

        public IEnumerable<Prayer> GetAllPrayers(Func<Prayer, bool> p = null)
        {
            return dal.GetAllPrayers(p).OrderBy(prayer => prayer.LastName);
        }

        public IEnumerable<Prayer> GetAllCohanim()
        {
            return GetAllPrayers().Where(t => t.Tribe == Lineage.כהן);
        }

        public IEnumerable<Prayer> GetAllLevits()
        {
            return GetAllPrayers().Where(t => t.Tribe == Lineage.לוי);
        }

        public IEnumerable<Prayer> GetAllIsraels()
        {
            return GetAllPrayers().Where(t => t.Tribe == Lineage.ישראל);
        }

        public bool Empty(Prayer prayer)
        {
            return dal.Empty(prayer);
        }

        public IEnumerable<Prayer> NumberOfAliyot(IEnumerable<Prayer> prayers, int num)
        {
            if (num > prayers.Count())
                throw new Exception("not enough people for Aliyot");
            List<Prayer> temPrayers = prayers.ToList();
            List<Prayer> Aliyot = new List<Prayer>();
            for (int i = 0; i < num; i++)
            {
                var p = LeastAliyot(temPrayers);
                Aliyot.Add(p);
                temPrayers.Remove(p);
            }
            return Aliyot;
        }

        public Prayer LeastAliyot(IEnumerable<Prayer> prayers)
        {
            Prayer prayer = prayers.First();
            foreach (Prayer p in prayers)
            {
                if (p.LastAliyaInThisYear == prayer.LastAliyaInThisYear)
                {
                    if (p.LastAliyaParasha <= prayer.LastAliyaParasha) prayer = p;
                }
                else if (!p.LastAliyaInThisYear && prayer.LastAliyaInThisYear) prayer = p;
            }
            return prayer;
        }

        public void UpdateAllPrayers(Parashot lastShabat, IEnumerable<Prayer> aliyot)
        {
            Prayer temPrayer = new Prayer();
            if (lastShabat != Parashot.לא_ידוע ||
                lastShabat != Parashot.ראש_השנה ||
                lastShabat != Parashot.יום_כיפור ||
                lastShabat != Parashot.סוכות ||
                lastShabat != Parashot.פסח)
            {
                foreach (Prayer prayer in GetAllPrayers())
                {
                    if (prayer.LastAliyaParasha != lastShabat) continue;
                    prayer.LastAliyaInThisYear = false;
                    UpdatePrayer(prayer);
                }
            }
            foreach (Prayer p in aliyot)
            {
                temPrayer = p;
                temPrayer.LastAliyaInThisYear = true;
                temPrayer.LastAliyaParasha = lastShabat;
                UpdatePrayer(temPrayer);
            }
        }

        public IEnumerable<Prayer> BmPrayers(string bmParasha)
        {
            switch (bmParasha)
            {
                case "ויקהל":
                    return
                        GetAllPrayers(
                            prayer =>
                                prayer.BMparasha.ToString() == bmParasha ||
                                prayer.BMparasha.ToString() == "ויקהל_פקודי");
                case "ויקהל_פקודי":
                    return GetAllPrayers(prayer => prayer.BMparasha.ToString() == bmParasha
                                                   || prayer.BMparasha.ToString() == "ויקהל"
                                                   || prayer.BMparasha.ToString() == "פקודי");
                case "פקודי":
                    return
                        GetAllPrayers(
                            prayer =>
                                prayer.BMparasha.ToString() == bmParasha ||
                                prayer.BMparasha.ToString() == "ויקהל_פקודי");
                case "תזריע":
                    return
                        GetAllPrayers(
                            prayer =>
                                prayer.BMparasha.ToString() == bmParasha ||
                                prayer.BMparasha.ToString() == "תזריע_מצורע");
                case "תזריע_מצורע":
                    return
                        GetAllPrayers(
                            prayer =>
                                prayer.BMparasha.ToString() == bmParasha ||
                                prayer.BMparasha.ToString() == "תזריע" ||
                                prayer.BMparasha.ToString() == "מצורע");
                case "מצורע":
                    return
                        GetAllPrayers(
                            prayer =>
                                prayer.BMparasha.ToString() == bmParasha ||
                                prayer.BMparasha.ToString() == "תזריע_מצורע");
                case "אחרי_מות":
                    return
                        GetAllPrayers(
                            prayer =>
                                prayer.BMparasha.ToString() == bmParasha ||
                                prayer.BMparasha.ToString() == "אחרי_מות_קדושים");
                case "אחרי_מות_קדושים":
                    return
                        GetAllPrayers(
                            prayer =>
                                prayer.BMparasha.ToString() == bmParasha ||
                                prayer.BMparasha.ToString() == "אחרי_מות" ||
                                prayer.BMparasha.ToString() == "קדושים");
                case "קדושים":
                    return
                        GetAllPrayers(
                            prayer =>
                                prayer.BMparasha.ToString() == bmParasha ||
                                prayer.BMparasha.ToString() == "אחרי_מות_קדושים");
                case "בהר":
                    return
                        GetAllPrayers(
                            prayer =>
                                prayer.BMparasha.ToString() == bmParasha ||
                                prayer.BMparasha.ToString() == "בהר_בחוקותי");
                case "בהר_בחוקותי":
                    return
                        GetAllPrayers(
                            prayer =>
                                prayer.BMparasha.ToString() == bmParasha ||
                                prayer.BMparasha.ToString() == "בהר" ||
                                prayer.BMparasha.ToString() == "בחוקתי");
                case "בחוקתי":
                    return
                        GetAllPrayers(
                            prayer =>
                                prayer.BMparasha.ToString() == bmParasha ||
                                prayer.BMparasha.ToString() == "בהר_בחוקותי");
                case "מטות":
                    return
                        GetAllPrayers(
                            prayer =>
                                prayer.BMparasha.ToString() == bmParasha ||
                                prayer.BMparasha.ToString() == "מטות_מסעי");
                case "מטות_מסעי":
                    return
                        GetAllPrayers(
                            prayer =>
                                prayer.BMparasha.ToString() == "מטות" ||
                                prayer.BMparasha.ToString() == "מסעי");
                case "מסעי":
                    return
                        GetAllPrayers(
                            prayer =>
                                prayer.BMparasha.ToString() == bmParasha ||
                                prayer.BMparasha.ToString() == "מטות_מסעי");
                case "ניצבים":
                    return
                        GetAllPrayers(
                            prayer =>
                                prayer.BMparasha.ToString() == bmParasha ||
                                prayer.BMparasha.ToString() == "ניצבים_וילך");
                case "ניצבים_וילך":
                    return
                        GetAllPrayers(
                            prayer =>
                                prayer.BMparasha.ToString() == bmParasha ||
                                prayer.BMparasha.ToString() == "ניצבים" ||
                                prayer.BMparasha.ToString() == "וילך");
                case "וילך":
                    return
                        GetAllPrayers(
                            prayer =>
                                prayer.BMparasha.ToString() == bmParasha ||
                                prayer.BMparasha.ToString() == "ניצבים_וילך");
                default:
                    return GetAllPrayers(prayer => prayer.BMparasha.ToString() == bmParasha);
            }
        }

/*
                                                                                public IEnumerable<Prayer> SortByFirstName(IEnumerable<Prayer> prayers)
                                                                                {
                                                                                    return from prayer in prayers
                                                                                            orderby prayer.FirstName
                                                                                            select prayer;
                                                                                }
                                                                        */

        public IEnumerable<Prayer> SortByLastName(IEnumerable<Prayer> prayers)
        {
            return prayers.OrderBy(prayer => prayer.LastName);
        }

/*
                                                                        public IEnumerable<Prayer> SortByLeastAliya(IEnumerable<Prayer> prayers)
                                                                        {
                                                                            return from prayer in NumberOfAliyot(prayers, prayers.Count())
                                                                                   orderby prayer.LastName
                                                                                   select prayer;
                                                                        }
                                                                */
    }
}