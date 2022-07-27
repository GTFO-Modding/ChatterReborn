using GameData;

namespace ChatterReborn.Managers
{
    public class TextDataBlockExtendedManager : ChatterManager<TextDataBlockExtendedManager>
    {
        private void TestLocalization()
        {
            m_testBlock = TextDataBlock.AddNewBlock();

            m_testBlock.English = "this is a test for localization";
            m_testBlock.Spanish = "esta es una prueba de localización";

        }


        private TextDataBlock m_testBlock;

    }
}
