using R3;

namespace RMC.MyProject.Scenes
{
    /// <summary>
    /// Stores data for the game.
    /// </summary>
    public class GameModel
    {
        // Constants --------------------------------------
        public const int ScoreMin = 0;
        public const int ScoreMax = 5;
        public const int LivesMax = 3;
        public const float MoveInputMinimumMagnitude = 0.1f;
        public const float PhysicsRaycastMaximumDistance = 0.6f;

        // Reactive Properties ----------------------------
        public ReactiveProperty<int> Score = new ReactiveProperty<int>(ScoreMin);
        public ReactiveProperty<int> Lives = new ReactiveProperty<int>(LivesMax);
        public ReactiveProperty<string> Title = new ReactiveProperty<string>();
        public ReactiveProperty<string> Instructions = new ReactiveProperty<string>();
    }
}