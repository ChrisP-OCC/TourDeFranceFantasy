using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ViewModel
{
    public class StageScoreVM
    {
        public StageScoreVM(SolidColorBrush color)
        {
            _color = color;
        }


        private SolidColorBrush _color;
        public SolidColorBrush Color
        {
            get
            {
                return _color;
            }
        }

        public string SeriesType 
        { 
            get
            {
                return "Line";
            }
        }

        public ObservableCollection<StageScore> Items { get; set; }
    }

    public class StageScore
    {
        public StageScore(string stage, int overallScore)
        {
            _stage = stage;
            _overallScore = overallScore;
        }

        private string _stage;
        public string Stage
        {
            get
            {
                return _stage;
            }
        }

        private int _overallScore;
        public int OverallScore
        {
            get
            {
                return _overallScore;
            }
        }
    }
}
