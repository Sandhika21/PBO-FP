using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class Nilai
    {
        //Instance Var + Property 
        private int _nilai = 100; //Buat Ngecek apakah bisa diakses atau tidak?
        public int Simpan{ 
            get { return _nilai; } 
            set { _nilai = value; } 
        }
    }
}
