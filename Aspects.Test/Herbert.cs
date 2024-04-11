﻿
namespace Aspects.Test
{
    public class Herbert<K, V> where K : struct, IEquatable<K> where V : notnull
    {
        public K Key { get; set; }

        public V Value { get; set; }

    }
}
