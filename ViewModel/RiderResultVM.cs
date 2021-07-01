using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ViewModel
{
    public class RiderResultVM : BaseVM
    {
        public RiderResultVM(RiderResult rider)
        {
            Model = rider;
        }

        private RiderResult _model;
        public RiderResult Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                OnPropertyChanged(nameof(Model));
            }
        }

        public string RiderName
        {
            get
            {
                return Model.RiderName;
            }
        }

        public int Score
        {
            get
            {
                return Model.Score;
            }
        }

        public string TeamName
        {
            get
            {
                return Model.TeamName;
            }
        }

        public int NumberOfTeams
        {
            get
            {
                return Model.NumberOfTeams;
            }
            set
            {
                Model.NumberOfTeams = value;
                OnPropertyChanged(nameof(NumberOfTeams));
            }
        }

        public int DifferenceMakerFactor
        {
            get
            {
                return Convert.ToInt32((Math.Min((double)Score, 40.0)) / 40.0 * 255.0 / (double)NumberOfTeams);
            }
        }
        public SolidColorBrush DifferenceMakerColor
        {
            get
            {
                return new SolidColorBrush(Color.FromArgb(Convert.ToByte(DifferenceMakerFactor), Colors.Yellow.R, Colors.Yellow.G, Colors.Yellow.B));
            }
        }
    }
}
