using SadConsole;
using SadConsole.UI;
using System;

namespace RPG_game2
{
    internal class FallingGold
    {
        private Color _startYellow = Color.Yellow;
        private Color _endYellow = Color.DarkOrange;

        private Color[] _availableColors = new Color[]
        {
            new Color(191, 149, 63),
            new Color(179, 135, 40),
            new Color(170, 119, 28)
        };
        private Color _mainColor;

        private string _goldSymbol = "$";
        private int _counter = 0;

        private int _fallingSpeed = 1000;

        private ControlsConsole _screenToPrint;

        private int _minX = 15;
        private int _maxX = 45;

        private int _xToFall;

        private Timer _timer;

        public FallingGold(ControlsConsole screenToPrint, float movesPerSecond) 
        {
            _fallingSpeed = (int)(1000 / movesPerSecond);

            _screenToPrint = screenToPrint;

            _mainColor = _availableColors[new Random().Next(0, _availableColors.Length)];

            StartToFall();
        }

        private void StartToFall()
        {
            _timer = new Timer(MoveDown!, null, 0, _fallingSpeed);

            int randomX = new Random().Next(_minX, _maxX);
            _xToFall = randomX;
        }

        private void MoveDown(Object o)
        {
            if (_counter < GameHost.Instance.ScreenCellsY)
            {
                _screenToPrint.Clear(_xToFall, _counter - 1);
                _screenToPrint.Fill(new Rectangle(_xToFall, _counter, 1, 1), _mainColor, Color.Black, 0, Mirror.None);
                _screenToPrint.Print(_xToFall, _counter, _goldSymbol);

                _counter++;
            } else
            {
                _screenToPrint.Clear(_xToFall, _counter - 1);
                _timer.Dispose();
            }
        }
    }
}
