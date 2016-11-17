namespace BL
{
    public static class FactoryBl
    {
        static IBL bl = null;
        public static IBL getBL()
        {
            ///BL layer implentation using a singelton
            if (bl != null) return bl;
            bl = new BL_imp();
            return bl;
        }
    }
}
