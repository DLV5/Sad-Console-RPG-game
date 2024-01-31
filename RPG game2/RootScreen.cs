using SadConsole.UI;
using SadConsole.UI.Controls;
using System.Diagnostics.Metrics;

namespace RPG_game2.Scenes
{
    internal class RootScreen : ScreenObject
    {
        private ControlsConsole _console;

        private int _goldCount = 0;

        private int _goldPerClick = 1;

        private Timer _passiveGoldTimer;

        private int _passiveGoldPerSecond = 0;

        private int _goldPerClickUpgradeCost = 10;
        private int _passiveGoldPerSecondUpgradeCost = 50;

        Button _upgradeGoldPerClickButton;

        Button _upgradePassiveGoldPerSecondButton;

        private bool _wasGoldPerClickUpgradePrinted = false;

        private bool _wasPassiveGoldPerSecondUpgradePrinted = false;

        public RootScreen()
        {
            _console = new(GameHost.Instance.ScreenCellsX, GameHost.Instance.ScreenCellsY);

            _console.Fill(new Rectangle(4, 4, 10, 1), Color.Yellow, Color.Black, 0, Mirror.None);

            _console.Print(4, 4, "Gold");

            PrintGoldAmount();

            Button mineGoldButton;

            mineGoldButton = new Button(6, 1)
            {
                Text = "Mine",
                Position = new Point(4, 6)
            };

            mineGoldButton.Click += (s, a) => MineGold();

            _console.Controls.Add(mineGoldButton);

            Children.Add(_console);
        }

        private void MineGold()
        {
            _goldCount += _goldPerClick;
            PrintGoldAmount();
            CreateFallingGold();

            if(!_wasGoldPerClickUpgradePrinted && _goldCount >= 10)
            {
                PrintGoldPerClickUpgradeForTheFirstTime();
            }
            
            if(!_wasPassiveGoldPerSecondUpgradePrinted && _goldCount >= 50)
            {
                PrintPassiveGoldPerSecondUpgradeForTheFirstTime();
            }
        }

        private void MineGold(Object o)
        {
            _goldCount += 1;
            PrintGoldAmount();
            CreateFallingGold();

            if (!_wasGoldPerClickUpgradePrinted && _goldCount >= 10)
            {
                PrintGoldPerClickUpgradeForTheFirstTime();
            }

            if (!_wasPassiveGoldPerSecondUpgradePrinted && _goldCount >= 50)
            {
                PrintPassiveGoldPerSecondUpgradeForTheFirstTime();
            }
        }

        private void PrintGoldPerClickUpgradeForTheFirstTime()
        {
            _wasGoldPerClickUpgradePrinted = true;

            _console.Fill(new Rectangle(50, 4, 23, 1), Color.Yellow, Color.Black, 0, Mirror.None);

            _console.Print(50, 4, "Increase gold per click");

            _upgradeGoldPerClickButton = new Button(8, 1)
            {
                Text = _goldPerClickUpgradeCost + "$",
                Position = new Point(58, 6)
            };

            _upgradeGoldPerClickButton.Click += (s, a) => UpgradeClick();

            _console.Controls.Add(_upgradeGoldPerClickButton);
        }

        private void PrintPassiveGoldPerSecondUpgradeForTheFirstTime()
        {
            _wasPassiveGoldPerSecondUpgradePrinted = true;

            _console.Fill(new Rectangle(50, 14, 32, 1), Color.Yellow, Color.Black, 0, Mirror.None);

            _console.Print(50, 14, "Increase passive gold per second");

            _upgradePassiveGoldPerSecondButton = new Button(18, 1)
            {
                Text = _passiveGoldPerSecondUpgradeCost + "$",
                Position = new Point(58, 16)
            };

            _upgradePassiveGoldPerSecondButton.Click += (s, a) => UpgradePassive();

            _console.Controls.Add(_upgradePassiveGoldPerSecondButton);
        }

        private void UpgradeClick()
        {
            if(_goldCount - _goldPerClickUpgradeCost >= 0)
            {
                _goldPerClick++;

                _goldCount -= _goldPerClickUpgradeCost;

                _goldPerClickUpgradeCost = (int)(_goldPerClickUpgradeCost * 1.1f + 5);

                _upgradeGoldPerClickButton.Text = _goldPerClickUpgradeCost + "$";

                PrintGoldAmount();
            }
        }
        
        private void UpgradePassive()
        {
            if(_goldCount - _passiveGoldPerSecondUpgradeCost >= 0)
            {
                if(_passiveGoldTimer == null)
                {
                    _passiveGoldTimer = new Timer(MineGold, null, 0, 1000);
                } else
                {
                    _passiveGoldPerSecond++;
                    _passiveGoldTimer.Change(0, 1000 / _passiveGoldPerSecond);
                }
                _goldCount -= _passiveGoldPerSecondUpgradeCost;

                _passiveGoldPerSecondUpgradeCost = (int)(_passiveGoldPerSecondUpgradeCost * 1.1f + 5);

                _upgradePassiveGoldPerSecondButton.Text = _passiveGoldPerSecondUpgradeCost + "$";

                PrintGoldAmount();
            }
        }

        private void PrintGoldAmount()
        {
            _console.Clear(9, 4, 6);
            _console.Fill(new Rectangle(8, 4, 6, 1), Color.Yellow, Color.Black, 0, Mirror.None);
            _console.Print(9, 4, _goldCount.ToString());
        }

        private void CreateFallingGold()
        {
            FallingGold falling = new FallingGold(_console, new Random().Next(1, 10));
        }
    }
}