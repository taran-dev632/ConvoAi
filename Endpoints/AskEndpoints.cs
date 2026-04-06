using LocalAIAssistant.Models;
using LocalAIAssistant.Services;

namespace LocalAIAssistant.Endpoints
{
    public static class AskEndpoints
    {
        public static void MapAskEndpoints(this WebApplication app)
        {
            app.MapPost("/ask", async (AskRequest req, AskService service) =>
            {
                if (string.IsNullOrWhiteSpace(req.Question))
                    return Results.BadRequest("Question required");

                var result = await service.Ask(req.Question);

                return Results.Ok(result);
            });
        }
    }
}
