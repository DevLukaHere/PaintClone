using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    public struct CmykColor
    {
        private readonly float _c;

        private readonly float _m;

        private readonly float _y;

        private readonly float _k;

        public CmykColor(float c, float m, float y, float k)
        {
            _c = c;
            _m = m;
            _y = y;
            _k = k;
        }

        public float C
        {
            get { return _c; }
        }

        public float M
        {
            get { return _m; }
        }

        public float Y
        {
            get { return _y; }
        }

        public float K
        {
            get { return _k; }
        }
    }
}