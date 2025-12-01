namespace DungeonExplorerApi.API.Validations
{
    public record ValidationResult(string Message = "", bool IsValid = false);
}
