using com.dhcc.framework;

namespace com.dhcc.spaceshooter
{
    public static class GameEvents
    {
        public static GameEvent StartGame = new();
        public static GameEvent<int, HealthComp> PlayerHealthChanged = new();
    }
}