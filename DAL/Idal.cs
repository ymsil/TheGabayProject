using System;
using System.Collections.Generic;
using BE;

namespace DAL
{
    public interface IDal
    {
        void AddPrayer(Prayer prayer);
        bool RemovePrayer(int prayerId); 
        void UpdatePrayer(Prayer prayer);
        Prayer GetPrayer(int prayerId);

        IEnumerable<Prayer> GetAllPrayers(Func<Prayer, bool> p = null);

        bool Empty(Prayer prayer);
    }
}
