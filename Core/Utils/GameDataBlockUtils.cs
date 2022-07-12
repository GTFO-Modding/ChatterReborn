using ChatterReborn.Data;
using GameData;
using System.IO;
using System.Text;

namespace ChatterReborn.Utils
{
    public partial class GameDataBlockUtils<T> where T : GameDataBlockBase<T>
    {
        public static void AddNewBlock(string name = "")
        {
            T newBlock = GameDataBlockBase<T>.AddNewBlock();
            if (!string.IsNullOrWhiteSpace(name))
            {
                newBlock.name = name;
            }
        }
    }
}
