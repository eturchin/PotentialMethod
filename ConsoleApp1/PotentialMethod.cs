using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Source
{
    public class TransportTask
    {
        private Point[] _allowed;

        public List<float> Ma;

        public List<float> Mb;

        public List<List<float>> Mc;

        public TransportTask(List<float> ma, List<float> mb, List<List<float>> price)
        {
            var aSum = ma.Sum();
            var bSum = mb.Sum();
            if (aSum < bSum)
            {
                var tmp = bSum - aSum;
                ma.Add(tmp);
                var zeroLine = new List<float>();
                Enumerable.Range(0, mb.Count).ToList().ForEach(_ => zeroLine.Add(0));
                price.Add(zeroLine);
            }
            else
            {
                var tmp = aSum - bSum;
                mb.Add(tmp);
                price.ForEach(line => line.Add(0));
            }

            Mb = mb;
            Ma = ma;
            Mc = price;
        }

        public List<List<float>> NorthWestern()
        {
            int i, j;
            var outArr = new List<List<float>>();
            for (i = 0; i < Ma.Count; i++)
            {
                var tmp = new List<float>();
                Enumerable.Range(0, Mb.Count).ToList().ForEach(_ => tmp.Add(0));

                outArr.Add(tmp);
            }

            EmptyToNan(outArr);
            i = 0;
            j = 0;

            while (!(IsEmpty(Ma) && IsEmpty(Mb)))
            {
                var dif = Math.Min(Ma[i], Mb[j]);
                outArr[i][j] = dif;
                Ma[i] -= dif;
                Mb[j] -= dif;
                if (Ma[i] == 0 && Mb[j] == 0 && j + 1 < Mb.Count) outArr[i][j + 1] = 0;
                if (Ma[i] == 0) i++;
                if (Mb[j] == 0) j++;
            }

            return outArr;
        }

        private bool IsEmpty(IEnumerable<float> arr)
        {
            return arr.All(x => x == 0);
        }

        private void EmptyToNan(IEnumerable<List<float>> outArr)
        {
            foreach (var t in outArr)
                for (var j = 0; j < t.Count; j++)
                    if (t[j] == 0)
                        t[j] = float.NaN;
        }

        private void PotentialFinder(IList<float> u, IList<float> v, float[,] helpMatr)
        {
            var u1 = new bool[Ma.Count];
            var u2 = new bool[Ma.Count];
            var v1 = new bool[Mb.Count];
            var v2 = new bool[Mb.Count];
            while (!(AllTrue(v1) && AllTrue(u1)))
            {
                var i = -1;
                var j = -1;
                for (var i1 = Mb.Count - 1; i1 >= 0; i1--)
                    if (v1[i1] && !v2[i1])
                        i = i1;
                for (var j1 = Ma.Count - 1; j1 >= 0; j1--)
                    if (u1[j1] && !u2[j1])
                        j = j1;

                if (j == -1 && i == -1)
                    for (var i1 = Mb.Count - 1; i1 >= 0; i1--)
                        if (!v1[i1] && !v2[i1])
                        {
                            i = i1;
                            v[i] = 0;
                            v1[i] = true;
                            break;
                        }

                if (j == -1 && i == -1)
                    for (var j1 = Ma.Count - 1; j1 >= 0; j1--)
                        if (!u1[j1] && !u2[j1])
                        {
                            j = j1;
                            u[j] = 0;
                            u1[j] = true;
                            break;
                        }

                if (i != -1)
                {
                    for (var j1 = 0; j1 < Ma.Count; j1++)
                    {
                        if (!u1[j1]) u[j1] = helpMatr[j1, i] - v[i];
                        if (!float.IsNaN(u[j1])) u1[j1] = true;
                    }

                    v2[i] = true;
                }

                if (j == -1) continue;
                {
                    for (var i1 = 0; i1 < Mb.Count; i1++)
                    {
                        if (!v1[i1]) v[i1] = helpMatr[j, i1] - u[j];
                        if (!float.IsNaN(v[i1])) v1[i1] = true;
                    }

                    u2[j] = true;
                }
            }
        }

        private bool AllSatisfactory(float[,] m)
        {
            var aSize = m.GetLength(0);
            var bSize = m.GetLength(1);
            var p = true;
            for (var i = 0; i < aSize && p; i++)
            for (var j = 0; j < bSize && p; j++)
                if (m[i, j] < 0)
                    p = false;
            return p;
        }

        private bool AllTrue(bool[] arr)
        {
            return Array.TrueForAll(arr, x => x);
        }

        private float[,] MakeBufMatrix(float[,] m, IReadOnlyList<float> u, IReadOnlyList<float> v)
        {
            var hm = new float[Ma.Count, Mb.Count];
            for (var i = 0; i < Ma.Count; i++)
            for (var j = 0; j < Mb.Count; j++)
            {
                hm[i, j] = m[i, j];
                if (float.IsNaN(hm[i, j]))
                    hm[i, j] = Mc[i][j] - (u[i] + v[j]);
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

        private void Roll(List<List<float>> m, float[,] sm)
        {
            var minInd = new Point();
            var min = float.MaxValue;
            var k = 0;
            _allowed = new Point[Ma.Count + Mb.Count];
            for (var i = 0; i < Ma.Count; i++)
            for (var j = 0; j < Mb.Count; j++)
            {
                if (!float.IsNaN(m[i][j]))
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
                cycles[i] = m[cycle[i].X][cycle[i].Y];
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
                    m[cycle[i].X][cycle[i].Y] -= min;
                }
                else
                {
                    cycles[i] += min;
                    if (float.IsNaN(m[cycle[i].X][cycle[i].Y])) m[cycle[i].X][cycle[i].Y] = 0;
                    m[cycle[i].X][cycle[i].Y] += min;
                }

            m[minInd.X][minInd.Y] = float.NaN;
        }

        public List<List<float>> PotentialMethod(List<List<float>> supArr)
        {
            var helpMatrix = new float[Ma.Count, Mb.Count];
            for (var i = 0; i < Ma.Count; i++)
            for (var j = 0; j < Mb.Count; j++)
                if (!float.IsNaN(supArr[i][j])) helpMatrix[i, j] = Mc[i][j];
                else helpMatrix[i, j] = float.NaN;

            var u = new float[Ma.Count];
            var v = new float[Mb.Count];
            PotentialFinder(u, v, helpMatrix);
            var sMatr = MakeBufMatrix(helpMatrix, u, v);
            while (!AllSatisfactory(sMatr))
            {
                Roll(supArr, sMatr);
                for (var i = 0; i < Ma.Count; i++)
                for (var j = 0; j < Mb.Count; j++)
                {
                    if (float.IsPositiveInfinity(supArr[i][j]))
                    {
                        helpMatrix[i, j] = Mc[i][j];
                        supArr[i][j] = 0;
                        continue;
                    }

                    if (!float.IsNaN(supArr[i][j]))
                        helpMatrix[i, j] = Mc[i][j];
                    else
                        helpMatrix[i, j] = float.NaN;
                }

                PotentialFinder(u, v, helpMatrix);
                sMatr = MakeBufMatrix(helpMatrix, u, v);
            }

            return supArr;
        }

        private class FindWay
        {
            private readonly Point _begining;
            private readonly FindWay _father;
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