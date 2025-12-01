using DungeonExplorerApi.API.Requests;

namespace DungeonExplorerApi.API.Validations
{
    public class CreateMapRequestValidator
    {
        public static ValidationResult Validate(MapRequest request)
        {
            if (request.Width < 5 || request.Width > 50) return new ValidationResult("Width cannot be less than 5 or greater than 50.");
            if (request.Height < 5 || request.Height > 50) return new ValidationResult("Height cannot be less than 5 or greater than 50.");

            if (!IsInside(request.Start, request)) return new ValidationResult("Start must be inside the grid.");
            if (!IsInside(request.Goal, request)) return new ValidationResult("Goal must be inside the grid.");
            if (request.Obstacles.Any(x => x.X == request.Start.X && x.Y == request.Start.Y)) return new ValidationResult("Start cannot be an obstacle.");
            if (request.Obstacles.Any(x => x.X == request.Goal.X && x.Y == request.Goal.Y)) return new ValidationResult("Goal cannot be an obstacle.");

            foreach (var o in request.Obstacles)
            {
                if (!IsInside(o, request))
                    return new ValidationResult("Obstacle outside bounds.");
            }

            return new ValidationResult(IsValid: true);
        }

        public static bool IsInside(Position p, MapRequest request) => p.X >= 0 && p.Y >= 0 && p.X < request.Width && p.Y < request.Height;
        public static bool IsObstacle(in Position p, in MapRequest request) => request.Obstacles.Contains(p);
    }
}
