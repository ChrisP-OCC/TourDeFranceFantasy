using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class StageResult
    {
        public List<RiderResult> Riders;
        public string Name;
        public StageType Type;
        public string Distance;
        public int GreenScore;
        public int PolkaScore;
        public int Transfers;

        public StageResult(StreamReader stream)
        {
            Riders = new List<RiderResult>();
            int _riderCount = 0;
            stream.ReadLine();//empty above stage name
            Name = stream.ReadLine();
            while (!stream.EndOfStream && _riderCount < 8)
            {
                Riders.Add(new RiderResult(stream));
                _riderCount++;
            }
        }

        public void SetTypeWithString(string typeString)
        {
            if (typeString.Contains(StageType.FL.ToString()))
                Type = StageType.FL;
            else if (typeString.Contains(StageType.MM.ToString()))
                Type = StageType.MM;
            else if (typeString.Contains(StageType.HM.ToString()))
                Type = StageType.HM;
            else if (typeString.Contains(StageType.SF.ToString()))
                Type = StageType.SF;
            else if (typeString.Contains(StageType.IT.ToString()))
                Type = StageType.IT;
        }

        public enum StageType
        {
            FL,
            MM,
            HM,
            SF,
            IT
        }
    }
}
