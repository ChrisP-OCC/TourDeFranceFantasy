using Model;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace ViewModel
{
    public class TeamVM : BaseVM
    {
        public const int StartingTransfers = 45;
        TourVM Parent;

        public TeamVM(string filepath, TourVM parent, SolidColorBrush teamColor)
        {
            Parent = parent;
            Model = new Team(filepath);
            TeamColor = teamColor;
            _stages = new ObservableCollection<StageResultVM>();
            foreach (StageResult _stage in Model.Stages)
            {
                _stages.Add(new StageResultVM(_stage));
            }
            OnPropertyChanged(nameof(Stages));
        }

        public ObservableCollection<StageResultVM> _stages;
        public ObservableCollection<StageResultVM> Stages
        {
            get
            {
                return _stages;
            }
        }

        public StageResultVM CurrentStage
        {
            get
            {
                return Stages[Parent.StageNumber];
            }
        }

        public void NewStageIndex()
        {
            OnPropertyChanged(nameof(StageRank));
            OnPropertyChanged(nameof(StageRankColor));
            OnPropertyChanged(nameof(StageScore)); 
            OnPropertyChanged(nameof(OverallScore));
            OnPropertyChanged(nameof(OverallRank)); 
            OnPropertyChanged(nameof(YellowJerseyVisibility));
            OnPropertyChanged(nameof(Back));
            OnPropertyChanged(nameof(GreenScore));
            OnPropertyChanged(nameof(GreenRank));
            OnPropertyChanged(nameof(GreenJerseyVisibility));
            OnPropertyChanged(nameof(PolkaScore));
            OnPropertyChanged(nameof(PolkaRank)); 
            OnPropertyChanged(nameof(PolkaJerseyVisibility));
            OnPropertyChanged(nameof(CurrentStage)); 
            OnPropertyChanged(nameof(Transfers)); 
            OnPropertyChanged(nameof(TotalTransfers));

            foreach (RiderResultVM _rider in CurrentStage.Riders)
            {
                _rider.NumberOfTeams = Parent.RiderCountsPerStage[Parent.StageNumber][TourVM.TrimOutRiderType(_rider.RiderName)];
            }
        }

        private Team _model;
        public Team Model
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

        public string Name
        {
            get
            {
                return _model.Name;
            }
            set
            {
                _model.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private SolidColorBrush _teamColor;
        public SolidColorBrush TeamColor
        {
            get
            {
                return _teamColor;
            }
            set
            {
                _teamColor = value;
                OnPropertyChanged(nameof(TeamColor));
            }
        }

        public string Owner
        {
            get
            {
                return _model.Owner;
            }
            set
            {
                _model.Owner = value;
                OnPropertyChanged(nameof(Owner));
            }
        }

        public int OverallScore
        {
            get
            {
                return GetStageOverallScore(Parent.StageNumber);
            }
        }

        public int GetStageOverallScore(int stage)
        {
            int _score = 0;
            for (int i = 0; i <= stage; i++)
            {
                _score += Stages[i].Score;
            }
            return _score;
        }

        public int OverallRank
        {
            get
            {
                List<int> _scores = new List<int>();
                foreach (TeamVM _team in Parent.Teams)
                {
                    _scores.Add(_team.OverallScore);
                }
                _scores.Sort();
                _scores.Reverse();
                return _scores.IndexOf(OverallScore) + 1;//++ 0 to 1 based index
            }
        }

        public Visibility YellowJerseyVisibility
        {
            get
            {
                return OverallRank == 1 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public int StageScore
        {
            get
            {
                return GetStageScore(Parent.StageNumber);
            }
        }

        public int GetStageScore(int stage)
        {
            return Stages[stage].Score;
        }

        public int StageRank
        {
            get
            {
                return GetStageRank(Parent.StageNumber);
            }
        }

        public SolidColorBrush StageRankColor
        {
            get
            {
                return StageRank == 1 ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Black);
            }
        }

        public int GetStageRank(int stage)
        {
            List<int> _scores = new List<int>();
            foreach (TeamVM _team in Parent.Teams)
            {
                _scores.Add(_team.GetStageScore(stage));
            }
            _scores.Sort();
            _scores.Reverse();
            return _scores.IndexOf(GetStageScore(stage)) + 1;//++ 0 to 1 based index
        }

        public int Back
        {
            get
            {
                int _highScore = 0;
                foreach (TeamVM _team in Parent.Teams)
                {
                    _highScore = Math.Max(_highScore, _team.OverallScore);
                }
                return OverallScore - _highScore;
            }
        }

        public int GreenScore
        {
            get
            {
                return Stages[Parent.StageNumber].Model.GreenScore;
            }
        }

        public int GreenRank
        {
            get
            {
                List<int> _scores = new List<int>();
                foreach (TeamVM _team in Parent.Teams)
                {
                    _scores.Add(_team.GreenScore);
                }
                _scores.Sort();
                _scores.Reverse();
                return _scores.IndexOf(GreenScore) + 1;//++ 0 to 1 based index
            }
        }

        public Visibility GreenJerseyVisibility
        {
            get
            {
                return GreenRank == 1 && GreenScore > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public int PolkaScore
        {
            get
            {
                return Stages[Parent.StageNumber].Model.PolkaScore;
            }
        }

        public int PolkaRank
        {
            get
            {
                List<int> _scores = new List<int>();
                foreach (TeamVM _team in Parent.Teams)
                {
                    _scores.Add(_team.PolkaScore);
                }
                _scores.Sort();
                _scores.Reverse();
                return _scores.IndexOf(PolkaScore) + 1;//++ 0 to 1 based index
            }
        }

        public Visibility PolkaJerseyVisibility
        {
            get
            {
                return PolkaRank == 1 && PolkaScore > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        
        public int Transfers
        {
            get
            {
                return CurrentStage.Transfers;
            }
        }

        public int TotalTransfers
        {
            get
            {
                int _transfers = 0;
                for (int i = 0; i <= Parent.StageNumber; i++)
                {
                    _transfers += Stages[i].Transfers;
                }
                return _transfers;
            }
        }

    }
}
