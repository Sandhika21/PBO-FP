using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Tetris
{
    public interface IBuff
    {
        void ApplyBuff(IBuff score);
    }

    public abstract class Scoring
    {
        protected int _score;

        public Scoring()
        {
            _score = 0;
        }

        public int Score 
        {  
            get { return _score; } 
            set { _score = value; }
        }
    }

    public class BalanceScore : Scoring, IBuff
    {
        private int _bonusScore;

        public BalanceScore() : base()
        {
            BonusScore = 1;
        }

        public int BonusScore 
        { 
            get { return _bonusScore; } 
            set { _bonusScore = value; }
        }
        public void ApplyBuff(IBuff score_item)
        {
            CreditScore cs = (CreditScore)score_item;
            if (_score - cs.Score > 7)
            {
                GenerateBlocks.isSame = true;
            }
            else
            {
                GenerateBlocks.isSame = false;
            }
        }
    }

    public class CreditScore : Scoring, IBuff
    {
        public CreditScore() : base() { }
        public void ApplyBuff(IBuff score_item)
        {
            BalanceScore bs = (BalanceScore)score_item;
            if(_score - bs.Score >= 5)
            {
                bs.BonusScore = Math.Min((int)((_score - bs.Score) / 5), 3)+1;
            }
            else
            {
                bs.BonusScore = 1;
            }
            
        }
    }

    public class Score
    {
        public static BalanceScore balanceScore;
        public static CreditScore creditScore;

        public static void Reset()
        {
            balanceScore = new BalanceScore();
            creditScore = new CreditScore();
        }
    }
}
