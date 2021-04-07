namespace FruitBowl
{
    [System.Serializable]
    public struct GameSettings
    {   
        public GridSettings gridSettings;
        public GameFruitData fruitSettings;
        public int startCells;
        public int cellsAddedPerMove;
    }
}