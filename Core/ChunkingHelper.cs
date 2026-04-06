namespace LocalAIAssistant.Core
{
    public static class ChunkingHelper
    {
        public static List<string> ChunkText(string text, int chunkSize, int overlap)
        {
            var chunks = new List<string>();

            int index = 0;
            while (index < text.Length)
            {
                int length = Math.Min(chunkSize, text.Length - index);
                chunks.Add(text.Substring(index, length));
                index += (chunkSize - overlap);
            }

            return chunks;
        }
    }
}
