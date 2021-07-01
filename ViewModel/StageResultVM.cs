using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ViewModel
{
    public class StageResultVM : BaseVM
    {
        public StageResultVM(StageResult stage)
        {
            Model = stage;
            _riders = new ObservableCollection<RiderResultVM>();
            foreach (RiderResult _rider in Model.Riders)
            {
                _riders.Add(new RiderResultVM(_rider));
            }
            OnPropertyChanged(nameof(Riders));
        }

        public ObservableCollection<RiderResultVM> _riders;
        public ObservableCollection<RiderResultVM> Riders
        {
            get
            {
                return _riders;
            }
        }

        private StageResult _model;
        public StageResult Model
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
                return Model.Name;
            }
        }

        public string Type
        {
            get
            {
                return Model.Type.ToString();
            }
        }

        public string Distance
        {
            get
            {
                return Model.Distance;
            }
        }

        public int Transfers
        {
            get
            {
                return Model.Transfers;
            }
        }

        public int Score
        {
            get
            {
                int _score = 0;
                foreach(RiderResult _rider in Model.Riders)
                {
                    _score += _rider.Score;
                }
                return _score;
            }
        }
    }
}
