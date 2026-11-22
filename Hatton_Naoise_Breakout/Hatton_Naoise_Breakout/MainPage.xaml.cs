using System.Runtime.CompilerServices;

namespace Hatton_Naoise_Breakout
{
    public partial class MainPage : ContentPage
    {
        private int _paddleDirection = 0;
        private bool _gamePlaying;
        private IDispatcherTimer? _gameTimer;
        private int _paddleSpeed = 10;
        

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
            MovePaddle();
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
            await Ball.TranslateToAsync(GameArea.Width / 2 - Ball.Width / 2, GameArea.Height - Paddle.Height - Ball.Width, 0);
        }
        
    }
}
