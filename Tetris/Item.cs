using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class Item
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
}
