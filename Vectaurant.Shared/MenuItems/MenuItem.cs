﻿
using Microsoft.Extensions.VectorData;

namespace Vectaurant.Shared.MenuItems
{
    public class MenuItem
    {
        [VectorStoreKey]
        public ulong Id { get; set; }

        [VectorStoreData(IsIndexed = true)]
        public string Name { get; set; }

        [VectorStoreData(IsFullTextIndexed = true)]
        public string Description { get; set; }

        [VectorStoreData(IsIndexed = true)]
        public string Category { get; set; }

        [VectorStoreData]
        public int AvailableCount { get; set; }

        [VectorStoreData(IsIndexed = true)]
        public double Price { get; set; }

        [VectorStoreVector(3072)]
        public ReadOnlyMemory<float> TextEmbedding { get; set; }

        public bool TryReserve(int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

            if (AvailableCount < quantity) return false;

            AvailableCount -= quantity;
            return true;
        }
    }
}
