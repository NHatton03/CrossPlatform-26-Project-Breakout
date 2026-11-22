using System.Runtime.CompilerServices;

namespace Hatton_Naoise_Breakout
{
    public partial class MainPage : ContentPage
    {
        private int _paddleDirection = 0;
        private bool _gamePlaying;
        private IDispatcherTimer? _gameTimer;
        private int _paddleSpeed = 10;
        private int _ballSpeed = 10;
        private int _ballDx;
        private int _ballDy;

        private double UpperBallBound => GameArea.Height - Ball.Width;





        public MainPage()
        {
            InitializeComponent();
            _gameTimer = Dispatcher.CreateTimer();
            _gameTimer.Interval = TimeSpan.FromMilliseconds(16);
            _gameTimer.Tick += OnGameTimerTick;
            Loaded += OnPageLoaded;          
        }

       
        //GameArea.Height/Width are not populated on constructor call so assigned this function to be called on Loaded.
        private void OnPageLoaded(object? sender, EventArgs e) { 
            Loaded -= OnPageLoaded;
            StartGame();
        }

        private void OnGameTimerTick(object? sender, EventArgs e)
        {
            HandleCollisions();
            MovePaddle();
            MoveBall();
            if (!_gamePlaying) {
                _gameTimer?.Stop();
            }

        }

        private void RightButton_Pressed(object sender, EventArgs e)
        {
            _paddleDirection = 1;
        }

        private void LeftButton_Pressed(object sender, EventArgs e)
        {
            _paddleDirection = -1;
        }

        
        private void RightButton_Released(object sender, EventArgs e)
        {
            StopPaddle();
        }

        private void LeftButton_Released(object sender, EventArgs e)
        {
            StopPaddle();
        }

        private void MovePaddle() {
            double desiredPosition = Paddle.TranslationX + _paddleDirection * _paddleSpeed;
            double actualPosition = Math.Clamp(desiredPosition, 0, GameArea.Width - Paddle.Width);
            Paddle.TranslationX = actualPosition;
        }

        private void StopPaddle() {
            _paddleDirection = 0;
        }

        private async void StartGame() { 
            _gamePlaying = true;
            _gameTimer?.Start();
            await Paddle.TranslateToAsync(GameArea.Width / 2 - Paddle.Width / 2, GameArea.Height - Paddle.Height, 0);
            //-1 to ensure that the ball moves from it's start position and doesn't reverse as soon as the game loads.
            await Ball.TranslateToAsync(GameArea.Width / 2 - Ball.Width / 2, GameArea.Height - Paddle.Height - Ball.Width-1, 0);
            _ballDy = -1;
        }

        private void MoveBall() { 
            double desiredYPosition = Ball.TranslationY + _ballDy * _ballSpeed;
            double yClamp = Math.Clamp(desiredYPosition, 0, UpperBallBound);
            Ball.TranslationY = yClamp;
        }

        private void HandleCollisions() {
            //Checking if ball is hitting ceiling, and if it is swapping its movement direction.
            if (Ball.TranslationY <= 0) {
                _ballDy *= -1;
            }
            //Check to see if the ball has hit the floor.
            if (Ball.TranslationY >= UpperBallBound) {
                //_gamePlaying = false;
            }
            bool atOrBelowPaddle = Ball.TranslationY >= (UpperBallBound - Paddle.Height);
            bool withinPaddleBounds = Ball.TranslationX >= Paddle.TranslationX && Ball.TranslationX <= (Paddle.TranslationX + Paddle.Width);
            if (atOrBelowPaddle && withinPaddleBounds) {
                _ballDy *= -1;
            }
        }
        
    }
}
