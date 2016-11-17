using System;
using System.Collections.Generic;
using BE;

namespace BL
{
    public interface IBL
    {
        void AddPrayer(Prayer prayer);
        bool RemovePrayer(int prayerId);
        void UpdatePrayer(Prayer prayer);
/*
        Prayer GetPrayer(int prayerId);
*/

        IEnumerable<Prayer> GetAllPrayers(Func<Prayer, bool> p = null);
        IEnumerable<Prayer> GetAllCohanim();
        IEnumerable<Prayer> GetAllLevits();
        IEnumerable<Prayer> GetAllIsraels();
        bool Empty(Prayer prayer);

        Prayer LeastAliyot(IEnumerable<Prayer> prayers);
        IEnumerable<Prayer> NumberOfAliyot(IEnumerable<Prayer> prayers, int num);

        void UpdateAllPrayers(Parashot lastShabat, IEnumerable<Prayer> aliyot);
        IEnumerable<Prayer> BmPrayers(string bmParasha);

/*
        IEnumerable<Prayer> SortByFirstName(IEnumerable<Prayer> prayers);
*/
        IEnumerable<Prayer> SortByLastName(IEnumerable<Prayer> prayers);
/*
        IEnumerable<Prayer> SortByLeastAliya(IEnumerable<Prayer> prayers);
*/
    }
}
