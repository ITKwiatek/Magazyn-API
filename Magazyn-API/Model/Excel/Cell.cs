using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Excel
{
    public class Cell
    {
        public Cell(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public Tuple<int, int>get()
        {
            return Tuple.Create(Row, Col);
        }
        public int Row { get; set; }
        public int Col { get; set; }
    }
}
