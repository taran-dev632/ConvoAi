using System.Numerics.Tensors;

namespace LocalAIAssistant.Models
{
    public class VectorStore
    {
        private readonly List<DocumentChunk> _chunks = new();

        public void Add(DocumentChunk chunk)
        {
            _chunks.Add(chunk);
        }

        public List<(DocumentChunk Chunk, float Score)> SearchWithScore(float[] queryEmbedding, int topK)
        {
            return _chunks
                .Select(c => (
                    Chunk: c,
                    Score: TensorPrimitives.CosineSimilarity(c.Embedding, queryEmbedding)
                ))
                .OrderByDescending(x => x.Score)
                .Take(topK)
                .ToList();
        }
    }
}