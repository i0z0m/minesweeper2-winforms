namespace Minesweeper2
{
    public class GameState
    {
        public enum State
        {
            NotStarted,
            Running,
            Paused,
            GameOver
        }

        private State _currentState;
        public State CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }

        public GameState()
        {
            _currentState = State.NotStarted;
        }
    }
}
