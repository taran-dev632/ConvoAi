using LocalAIAssistant.Services;

namespace LocalAIAssistant.Endpoints
{
    public static class DocumentEndpoints
    {
        public static void MapDocumentEndpoints(this WebApplication app)
        {
            app.MapPost("/documents", async (IFormFile file, DocumentService service) =>
            {
                if (file == null || file.Length == 0)
                    return Results.BadRequest("File missing");

                using var reader = new StreamReader(file.OpenReadStream());
                var text = await reader.ReadToEndAsync();

                var count = await service.ProcessDocument(text);

                return Results.Ok(new { message = $"{count} chunks processed" });
            }).DisableAntiforgery();
        }
    }
}
