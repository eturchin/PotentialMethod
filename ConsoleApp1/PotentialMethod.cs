using System;
using System.Drawing;

namespace Source
{
    public class TransportProblem
    {
        private Point[] _allowed; // хранит координаты клеток, в которых есть груз

        public int ASize;

        public int BSize;

        public float[] Ma;

        public float[] Mb;

        public float[,] Mc;

        public TransportProblem(float[] nA, float[] nB, float[,] nC)
        {
            if (nA.Length != nC.GetLength(0) || nB.Length != nC.GetLength(1))
                throw new InvalidInpFormat(
                    "Размеры массива затрат не соответствуют размерам массивов поставщиков и складов");
            Ma = nA;
            Mb = nB;
            Mc = nC;
            ASize = nA.Length;
            BSize = nB.Length;
        }

        public float[,] NordWest()
        {
            var aBuf = Ma;
            var bBuf   = Mb;
            int i = 0, j = 0;
            var outArr = new float[ASize, BSize];
            NanToEmpty(outArr);

            while (!(IsEmpty(aBuf) && IsEmpty(bBuf  )))
            {
                var dif = Math.Min(aBuf[i], bBuf  [j]);
                outArr[i, j] = dif;
                aBuf[i] -= dif;
                bBuf  [j] -= dif;
                if (aBuf[i] == 0 && bBuf  [j] == 0 && j + 1 < BSize) outArr[i, j + 1] = 0;
                if (aBuf[i] == 0) i++;
                if (bBuf  [j] == 0) j++;
            }

            return outArr;
        }

        private bool IsEmpty(float[] arr)
        {
            return Array.TrueForAll(arr, x => x == 0);
        }

        private void NanToEmpty(float[,] outArr)
        {
            for (var i = 0; i < ASize; i++)
            for (var j = 0; j < BSize; j++)
                if (outArr[i, j] == 0)
                    outArr[i, j] = float.NaN;
        }

        private void FindUv(float[] u, float[] v, float[,] helpMatr)
        {
            var u1 = new bool[ASize];
            var u2 = new bool[ASize];
            var v1 = new bool[BSize];
            var v2 = new bool[BSize];
            while (!(AllTrue(v1) && AllTrue(u1)))
            {
                var i = -1;
                var j = -1;
                for (var i1 = BSize - 1; i1 >= 0; i1--)
                    if (v1[i1] && !v2[i1])
                        i = i1;
                for (var j1 = ASize - 1; j1 >= 0; j1--)
                    if (u1[j1] && !u2[j1])
                        j = j1;

                if (j == -1 && i == -1)
                    for (var i1 = BSize - 1; i1 >= 0; i1--)
                        if (!v1[i1] && !v2[i1])
                        {
                            i = i1;
                            v[i] = 0;
                            v1[i] = true;
                            break;
                        }

                if (j == -1 && i == -1)
                    for (var j1 = ASize - 1; j1 >= 0; j1--)
                        if (!u1[j1] && !u2[j1])
                        {
                            j = j1;
                            u[j] = 0;
                            u1[j] = true;
                            break;
                        }

                if (i != -1)
                {
                    for (var j1 = 0; j1 < ASize; j1++)
                    {
                        if (!u1[j1]) u[j1] = helpMatr[j1, i] - v[i];
                        if (!float.IsNaN(u[j1])) u1[j1] = true;
                    }

                    v2[i] = true;
                }

                if (j == -1) continue;
                {
                    for (var i1 = 0; i1 < BSize; i1++)
                    {
                        if (!v1[i1]) v[i1] = helpMatr[j, i1] - u[j];
                        if (!float.IsNaN(v[i1])) v1[i1] = true;
                    }

                    u2[j] = true;
                }
            }
        }

        private bool AllPositive(float[,] m)
        {
            var p = true;
            for (var i = 0; i < ASize && p; i++)
            for (var j = 0; j < BSize && p; j++)
                if (m[i, j] < 0)
                    p = false;
            return p;
        }

        private bool AllTrue(bool[] arr)
        {
            return Array.TrueForAll(arr, x => x);
        }

        private float[,] MakeSMatr(float[,] m, float[] u, float[] v)
        {
            var hm = new float[ASize, BSize];
            for (var i = 0; i < ASize; i++)
            for (var j = 0; j < BSize; j++)
            {
                hm[i, j] = m[i, j];
                if (float.IsNaN(hm[i, j]))
                    hm[i, j] = Mc[i, j] - (u[i] + v[j]);
            }

            return hm;
        }

        private Point[] GetCycle(int x, int y)
        {
            var beg = new Point(x, y);
            var fw = new FindWay(x, y, true, _allowed, beg, null);
            fw.BuildTree();
            var way = Array.FindAll(_allowed, p => p.X != -1 && p.Y != -1);
            return way;
        }

        private void Roll(float[,] m, float[,] sm)
        {
            var minInd = new Point();
            var min = float.MaxValue;
            var k = 0;
            _allowed = new Point[ASize + BSize];
            for (var i = 0; i < ASize; i++)
            for (var j = 0; j < BSize; j++)
            {
                if (!float.IsNaN(m[i, j]))
                {
                    _allowed[k].X = i;
                    _allowed[k].Y = j;
                    k++;
                }

                if (!(sm[i, j] < min)) continue;
                min = sm[i, j];
                minInd.X = i;
                minInd.Y = j;
            }

            _allowed[^1] = minInd;
            var cycle = GetCycle(minInd.X, minInd.Y);
            var cycles = new float[cycle.Length];
            var bCycles = new bool[cycle.Length];
            for (var i = 0; i < bCycles.Length; i++)
                bCycles[i] = i != bCycles.Length - 1;
            min = float.MaxValue;

            for (var i = 0; i < cycle.Length; i++)
            {
                cycles[i] = m[cycle[i].X, cycle[i].Y];
                if (i % 2 == 0 && !float.IsNaN(cycles[i]) && cycles[i] < min)
                {
                    min = cycles[i];
                    minInd = cycle[i];
                }

                if (float.IsNaN(cycles[i])) cycles[i] = 0;
            }

            for (var i = 0; i < cycle.Length; i++)
                if (i % 2 == 0)
                {
                    cycles[i] -= min;
                    m[cycle[i].X, cycle[i].Y] -= min;
                }
                else
                {
                    cycles[i] += min;
                    if (float.IsNaN(m[cycle[i].X, cycle[i].Y])) m[cycle[i].X, cycle[i].Y] = 0;
                    m[cycle[i].X, cycle[i].Y] += min;
                }

            m[minInd.X, minInd.Y] = float.NaN;
        }

        public float[,] PotentialMethod(float[,] supArr)
        {
            var helpMatr = new float[ASize, BSize];
            for (var i = 0; i < ASize; i++)
            for (var j = 0; j < BSize; j++)
                if (!float.IsNaN(supArr[i, j])) helpMatr[i, j] = Mc[i, j];
                else helpMatr[i, j] = float.NaN;

            var u = new float[ASize];
            var v = new float[BSize];
            FindUv(u, v, helpMatr);
            var sMatr = MakeSMatr(helpMatr, u, v);
            while (!AllPositive(sMatr))
            {
                Roll(supArr, sMatr);
                for (var i = 0; i < ASize; i++)
                for (var j = 0; j < BSize; j++)
                {
                    if (float.IsPositiveInfinity(supArr[i, j]))
                    {
                        helpMatr[i, j] = Mc[i, j];
                        supArr[i, j] = 0;
                        continue;
                    }

                    if (!float.IsNaN(supArr[i, j]))
                        helpMatr[i, j] = Mc[i, j];
                    else
                        helpMatr[i, j] = float.NaN;
                }

                FindUv(u, v, helpMatr);
                sMatr = MakeSMatr(helpMatr, u, v);
            }

            return supArr;
        }

        private class InvalidInpFormat : ApplicationException
        {
            public InvalidInpFormat(string str) : base(str)
            {
            }

            public override string ToString()
            {
                return Message;
            }
        }

        private class FindWay
        {
            private readonly Point _begining;

            private readonly FindWay _father;

            //true - up/down
            //false - right/left
            private readonly bool _flag;
            private readonly Point[] _mAllowed;
            private readonly Point _root;
            private FindWay[] _children;

            public FindWay(int x, int y, bool flag, Point[] mAllowed, Point beg, FindWay father)
            {
                _begining = beg;
                _flag = flag;
                _root = new Point(x, y);
                _mAllowed = mAllowed;
                _father = father;
            }

            public bool BuildTree()
            {
                var ps = new Point[_mAllowed.Length];
                var count = 0;
                foreach (var t in _mAllowed)
                    if (_flag)
                    {
                        if (_root.Y != t.Y) continue;
                        count++;
                        ps[count - 1] = t;
                    }
                    else if (_root.X == t.X)
                    {
                        count++;
                        ps[count - 1] = t;
                    }

                var fwu = this;
                _children = new FindWay[count];
                var k = 0;
                for (var i = 0; i < count; i++)
                {
                    if (ps[i] == _root) continue;
                    if (ps[i] == _begining) 
                    {
                        while (fwu != null)
                        {
                            _mAllowed[k] = fwu._root;
                            fwu = fwu._father;
                            k++;
                        }

                        for (; k < _mAllowed.Length; k++) _mAllowed[k] = new Point(-1, -1);
                        return true;
                    }

                    if (Array.TrueForAll(ps, p => p.X == 0 && p.Y == 0)) continue;
                    _children[i] = new FindWay(ps[i].X, ps[i].Y, !_flag, _mAllowed, _begining, this);
                    var result = _children[i].BuildTree();
                    if (result) return true;
                }

                return false;
            }
        }
    }
}