using System;
using System.Collections.Generic;

namespace ImageZoom.Adapters
{
    public static class LinqAdapter
    {
        public static IEnumerable<byte[]> Chunk(this byte[] self, int chunkSize)
        {
            for (int i = 0; i < self.Length; i += chunkSize)
            {
                var chunk = new byte[Math.Min(chunkSize, self.Length - i)];
                Array.Copy(self, i, chunk, 0, chunk.Length);
                yield return chunk;
            }
        }
    }
}
