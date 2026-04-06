using LocalAIAssistant.Models;
using Microsoft.Extensions.AI;
using System.Numerics.Tensors;

namespace LocalAIAssistant.Services
{
    public class AskService
    {
        private readonly VectorStore _store;
        private readonly IEmbeddingGenerator<string, Embedding<float>> _generator;
        private readonly IChatClient _chat;

        public AskService(VectorStore store,
            IEmbeddingGenerator<string, Embedding<float>> generator,
            IChatClient chat)
        {
            _store = store;
            _generator = generator;
            _chat = chat;
        }

        public async Task<object> Ask(string question)
        {
            var queryEmbedding = await _generator.GenerateVectorAsync(question);

            var topChunks = _store.SearchWithScore(queryEmbedding.ToArray(), 3);

            // Add threshold check
            if (!topChunks.Any() || topChunks.First().Score < 0.5f)
            {
                return new
                {
                    Answer = "I don't know based on the provided documents."
                };
            }

            var context = string.Join("\n---\n", topChunks.Select(c => c.Chunk.Text));

            var prompt = $@"
            You are an AI assistant.
            
            Rules:
            1. Answer ONLY from the provided context
            2. If the question is not related to the context, say:
               'I don't know based on the provided documents.'
            3. Do NOT guess or assume anything
            
            Context:
            {context}
            
            Question:
            {question}
            ";

            var response = await _chat.GetResponseAsync(prompt);

            return new
            {
                Answer = response.Text,
                ContextUsed = topChunks.Select(c => c.Chunk.Text)
            };
        }
    }
}
