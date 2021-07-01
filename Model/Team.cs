using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Team
    {
        public string Name;
        public string Owner;
        public List<StageResult> Stages;
        public int OverallScore;

        public Team(string filename)
        {
            Stages = new List<StageResult>();
            using (StreamReader _stream = new StreamReader(filename))
            {
                Name = _stream.ReadLine();
                Owner = _stream.ReadLine();
                _stream.ReadLine();//empty line
                while (!_stream.EndOfStream)
                {
                    Stages.Add(new StageResult(_stream));
                }
            }
        }
    }
}
