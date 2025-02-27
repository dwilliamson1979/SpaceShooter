using com.dhcc.framework;

namespace com.dhcc.spaceshooter
{
    public static class GameEvents
    {
        public static GameEvent StartRound = new();
        //public static GameEvent PlayerDied = new();
        public static GameEvent GameOver = new();

        public static GameEvent<int, HealthComp> PlayerHealthChanged = new();
        public static GameEvent<int> AddPoints = new();
        public static GameEvent<int> AddAmmo = new();
    }
}