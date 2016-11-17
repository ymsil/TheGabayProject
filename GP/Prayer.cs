using System;

namespace BE
{
    public enum Parashot
    {
        לא_ידוע,
        בראשית,
        נח,
        לך_לך,
        וירא,
        חיי_שרה,
        תולדות,
        ויצא,
        וישלח,
        וישב,
        מקץ,
        ויגש,
        ויחי,
        שמות,
        וארא,
        בא,
        בשלח,
        יתרו,
        משפטים,
        תרומה,
        תצוה,
        כי_תשא,
        ויקהל,
        ויקהל_פקודי,
        פקודי,
        ויקרא,
        צו,
        שמיני,
        תזריע,
        תזריע_מצורע,
        פסח,
        מצורע,
        אחרי_מות,
        אחרי_מות_קדושים,
        קדושים,
        אמור,
        בהר,
        בהר_בחוקותי,
        בחוקתי,
        במדבר,
        שבועות,
        נשא,
        בהעלותך,
        שלח,
        קרח,
        חקת,
        בלק,
        פנחס,
        מטות,
        מטות_מסעי,
        מסעי,
        דברים,
        ואתחנן,
        עקב,
        ראה,
        שופטים,
        כי_תצא,
        כי_תבוא,
        ניצבים,
        ניצבים_וילך,
        ראש_השנה,
        וילך,
        יום_כיפור,
        האזינו,
        סוכות,
        שמחת_תורה,
        וזאת_הברכה
    }
    public enum Lineage
    {
        ישראל, לוי, כהן
    }
    public class Prayer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Parashot BMparasha { get; set; }
        public Lineage Tribe { get; set; }
        public Parashot LastAliyaParasha { get; set; }
        public bool LastAliyaInThisYear { get; set; }

        private static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public override string ToString()
        {
            string s = "Prayer Id: " + Id.ToString() + "\n"
                        + "First Name: " + Reverse(FirstName) + "\n"
                        + "Last Name: " + Reverse(LastName) + "\n"
                        + "Bar-Mitzva Parasha: " + Reverse(BMparasha.ToString()) + "\n"
                        + "Tribe: " + Tribe.ToString() + "\n"
                        + "Last Aliya parasha: " + Reverse(LastAliyaParasha.ToString()) + "\n"
                        +"Last Aliya in current year: " + LastAliyaInThisYear.ToString() + "\n";
            return s;
        }
    }
}
