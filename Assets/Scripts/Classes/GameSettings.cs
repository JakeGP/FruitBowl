namespace FruitBowl
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Extreme
    }

    [System.Serializable]
    public struct GameSettings
    {   
        public int levelID;
        public Difficulty difficulty;
        public GridSettings gridSettings;
        public GameFruitData fruitSettings;
        public int startCells;
        public int cellsAddedPerMove;
    }
}