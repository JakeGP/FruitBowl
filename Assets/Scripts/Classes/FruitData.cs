using System.Collections.Generic;

namespace FruitBowl
{
    public enum FruitType
    {
        Debug,
        Lemon,
        Orange,
        Apple
    }

    public enum FruitSize
    {
        Quarter = 2,
        Half = 4,
        Slice = 8,
        Whole = 16,
        Bunch = 32
    }

    [System.Serializable]
    public struct FruitTarget
    {
        public FruitType type;
        public int quantity;
    }

    [System.Serializable]
    public struct GameFruitData
    {
        public List<FruitTarget> targetFruits;
        public List<FruitType> allowedFruits;
        public FruitSize maxFruitSize;
    }
}