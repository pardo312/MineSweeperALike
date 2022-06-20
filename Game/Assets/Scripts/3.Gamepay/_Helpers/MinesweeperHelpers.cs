using System;

namespace JiufenGames.MineSweeperAlike.Board.Logic
{
    public static class MinesweeperHelpers
    {
        public static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }
    }
}
