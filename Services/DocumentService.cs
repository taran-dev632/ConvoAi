using LocalAIAssistant.Core;
using LocalAIAssistant.Models;
using Microsoft.Extensions.AI;

namespace LocalAIAssistant.Services
{
    public class DocumentService
    {
        private readonly VectorStore _store;
        private readonly IEmbeddingGenerator<string, Embedding<float>> _generator;

        public DocumentService(VectorStore store, IEmbeddingGenerator<string, Embedding<float>> generator)
        {
            _store = store;
            _generator = generator;
        }

        public async Task<int> ProcessDocument(string text)
        {
            var chunks = ChunkingHelper.ChunkText(text, 500, 100);

            foreach (var chunk in chunks)
            {
                var embedding = await _generator.GenerateVectorAsync(chunk);

                _store.Add(new DocumentChunk
                {
                    Text = chunk,
                    Embedding = embedding.ToArray()
                });
            }

            return chunks.Count;
        }
    }
}
