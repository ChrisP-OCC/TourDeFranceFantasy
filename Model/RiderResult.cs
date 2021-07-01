using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class RiderResult
    {
        public string RiderName;
        public string TeamName;
        public int Score;
        public int NumberOfTeams;//how many teams have this rider 

        public RiderResult(StreamReader stream)
        {
            string _line;
            string[] _lineParts;
            _line = stream.ReadLine();
            _lineParts = _line.Split('\t');
            RiderName = _lineParts[0];
            TeamName = _lineParts[1];
            int.TryParse(_lineParts[2], out Score);
        }
    }
}
