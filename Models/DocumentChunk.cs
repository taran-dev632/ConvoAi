namespace LocalAIAssistant.Models
{
    public class DocumentChunk
    {
        public string Text { get; set; } = "";
        public float[] Embedding { get; set; } = [];
    }
}
