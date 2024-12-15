using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public abstract class Item
    {
        private int _bPrice, _cPrice;
        private string _name;
        private int bScore;
        public Button button;

        public Item(int _bPrice, int _cPrice, string _name, Point location, Size size)
        {
            this._bPrice= _bPrice;
            this._cPrice = _cPrice;
            this._name= _name;
            button = getButton(location, size);
        }

        public int BPrice { get { return _bPrice; } }

        public int CPrice { get { return _cPrice; } }

        public string Name { get { return _name; } }
        public abstract void ApplyBuff();

        public Button getButton(Point location, Size size)
        {
            var btn = new Button()
            {
                Text = $"Buy {Name} ({BPrice} B | {CPrice} C)",
                Location = location,
                Size = size
            };

            return btn;
        }
    }

    public class ExtraLife : Item
    {
        public ExtraLife() : base(150, 200, "Extra Life", new Point(50, 100), new Size(250,40))
        {

        }
        public override void ApplyBuff()
        {

        }
    }

    public class SpeedBoost : Item
    {
        public SpeedBoost() : base(130, 100, "Speed Boost", new Point(50, 160), new Size(250, 40))
        {

        }
        public override void ApplyBuff()
        {

        }
    }

    public class Bomb : Item
    {
        public Bomb() : base(170, 270, "Bomb", new Point(50, 220), new Size(250, 40))
        {

        }
        public override void ApplyBuff()
        {

        }
    }
}
