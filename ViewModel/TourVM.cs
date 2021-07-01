using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ViewModel
{
    public class TourVM : BaseVM
    {
        public static int TotalNumberOfTeams = 0;
        public TourVM()
        {
            _teams = new ObservableCollection<TeamVM>();
            _stageScores = new ObservableCollection<StageScoreVM>();
        }

        private int _stageNumber;
        public int StageNumber
        {
            get
            {
                return _stageNumber;
            }
            set
            {
                _stageNumber = value;
                OnPropertyChanged(nameof(StageNumber));
                OnPropertyChanged(nameof(StageNumberShow)); 
                OnPropertyChanged(nameof(StageName));
                OnPropertyChanged(nameof(StageType));
                OnPropertyChanged(nameof(StageDistance));
                UpdateScoreGraph();
                foreach (TeamVM _team in Teams)
                {
                    _team.NewStageIndex();
                }
            }
        }
        public int StageNumberShow
        {
            get
            {
                return _stageNumber + 1;
            }
        }

        public ObservableCollection<TeamVM> _teams;
        public ObservableCollection<TeamVM> Teams
        {
            get
            {
                return _teams;
            }
        }

        public ObservableCollection<StageScoreVM> _stageScores;
        public ObservableCollection<StageScoreVM> StageScores
        {
            get
            {
                return _stageScores;
            }
        }

        public List<Dictionary<string, int>> RiderCountsPerStage;

        private bool _isStageResultsView;
        public bool IsStageResultsView
        {
            get
            {
                return _isStageResultsView;
            }
            set
            {
                _isStageResultsView = value;
                OnPropertyChanged(nameof(IsStageResultsView));
                OnPropertyChanged(nameof(StageResultsVisibility));
            }
        }

        public Visibility StageResultsVisibility
        {
            get
            {
                return _isStageResultsView ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility OverallResultsVisibility
        {
            get
            {
                return !_isStageResultsView ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public string StageName
        {
            get
            {
                return Teams.Count > 0 ? Teams[0].Stages[StageNumber].Name : string.Empty;
            }
        }

        public string StageType
        {
            get
            {
                return Teams.Count > 0 ? Teams[0].Stages[StageNumber].Type : string.Empty;
            }
        }

        public string StageDistance
        {
            get
            {
                return Teams.Count > 0 ? Teams[0].Stages[StageNumber].Distance : string.Empty;
            }
        }

        public void LoadStages(string filename)
        {
            string[] _lineParts;
            int _stageIndex = 0;
            using (StreamReader _stream = new StreamReader(filename))
            {                
                while (!_stream.EndOfStream && _stageIndex < Teams[0].Stages.Count)
                {
                    _lineParts = _stream.ReadLine().Split('\t');
                    foreach (TeamVM _team in Teams)
                    {
                        _team.Stages[_stageIndex].Model.Name = _lineParts[0];
                        _team.Stages[_stageIndex].Model.Distance = _lineParts[1];
                        _team.Stages[_stageIndex].Model.SetTypeWithString(_lineParts[2]);
                    }
                    _stageIndex++;
                }
            }

            UpdateJerseyPoints();
            UpdateTransfers();
            CalculateRiderCounts();
        }

        public void UpdateJerseyPoints()
        {
            int _green, _polka, _score;
            foreach (TeamVM _team in Teams)
            {
                _green = 0;
                _polka = 0;
                for (int _stageIndex = 0; _stageIndex < _team.Stages.Count; _stageIndex++)
                {
                    _score = _team.GetStageScore(_stageIndex);
                    if (_team.Stages[_stageIndex].Model.Type == Model.StageResult.StageType.FL ||
                        _team.Stages[_stageIndex].Model.Type == Model.StageResult.StageType.MM)
                        _green += _team.GetStageScore(_stageIndex);
                    if (_team.Stages[_stageIndex].Model.Type == Model.StageResult.StageType.HM ||
                        _team.Stages[_stageIndex].Model.Type == Model.StageResult.StageType.SF)
                        _polka += _team.GetStageScore(_stageIndex);

                    _team.Stages[_stageIndex].Model.GreenScore = _green;
                    _team.Stages[_stageIndex].Model.PolkaScore = _polka;
                }
            }
        }

        public void UpdateTransfers()
        {
            List<string> _previousRoster;
            foreach (TeamVM _team in Teams)
            {
                for (int _stageIndex = 1; _stageIndex < _team.Stages.Count; _stageIndex++)
                {
                    _previousRoster = new List<string>();
                    foreach (RiderResultVM _rider in _team.Stages[_stageIndex - 1].Riders)
                    {
                        _previousRoster.Add(TrimOutRiderType(_rider.RiderName));
                    }
                    foreach (RiderResultVM _rider in _team.Stages[_stageIndex].Riders)
                    {
                        if (!_previousRoster.Contains(TrimOutRiderType(_rider.RiderName)))
                            _team.Stages[_stageIndex].Model.Transfers++;//not working....
                    }
                }
            }
        }

        public void CalculateRiderCounts()
        {
            RiderCountsPerStage = new List<Dictionary<string, int>>();
            for (int _stageIndex = 0; _stageIndex < Teams[0].Stages.Count; _stageIndex++)
            {
                RiderCountsPerStage.Add(new Dictionary<string, int>());
                foreach (TeamVM _team in Teams)
                {
                    foreach (RiderResultVM _rider in _team.Stages[_stageIndex].Riders)
                    {
                        if(!RiderCountsPerStage[_stageIndex].ContainsKey(_rider.RiderName))
                            RiderCountsPerStage[_stageIndex].Add(_rider.RiderName, 1);
                        else
                            RiderCountsPerStage[_stageIndex][_rider.RiderName]++;
                    }
                }
            }
        }

        public void UpdateScoreGraph()
        {
            _stageScores = new ObservableCollection<StageScoreVM>();
            
            for (int ii = 0; ii < Teams.Count(); ii++)
            {
                _stageScores.Add(new StageScoreVM(Teams[ii].TeamColor));
                _stageScores[ii].Items = new ObservableCollection<StageScore>();
                for (int i = 0; i <= StageNumber; i++)
                {
                    _stageScores[ii].Items.Add(new StageScore((i + 1).ToString(), Teams[ii].GetStageOverallScore(i)));
                }
            }

            OnPropertyChanged(nameof(StageScores));        
        }

        public string TrimOutRiderType(string input)
        {
            return input.Replace("DS", "").Replace("AR", "").Replace("PC", "").Replace("KM", "").Replace("GC", "").Trim();
        }
    }
}
