using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife
{
    public class GOLService
    {
        public int CellNumberWidth
        { get; set; }

        public int CellNumberHeight
        { get; set; }

        public List<List<Rectangle>> Fields
        { get; set; }
        
        public bool useTorus
        { get; set; }


        public GOLService(int cellNumberWidth, int cellNumberHeight, List<List<Rectangle>> fields, bool useTorus)
        {
            this.CellNumberWidth = cellNumberWidth;
            this.CellNumberHeight = cellNumberHeight;
            this.Fields = fields;
            this.useTorus = useTorus;
        }

        public List<List<Rectangle>> adjustList(List<List<Rectangle>> list, int numberoflists)
        {
            for (int i = 0; i < numberoflists; i++)
            {
                list.Add(new List<Rectangle>());
            }
            return list;
        }

        /*
         * We take our gamefield the "two-dimensional List" and check if a cell is alive or not, via two for loops.
         * The living cells are getting counted (neighbours).
         * The number of neighbours is getting saved in a two dimensional Array, at the position of the currently iterated cell.
         */
        public List<List<Rectangle>> updateCells()
        {
            int[,] neighbournumber = new int[CellNumberHeight, CellNumberWidth];

            for (int height = 0; height < CellNumberHeight; height++)
            {
                for (int width = 0; width < CellNumberWidth; width++)
                {
                    /*
                     * This Part is to get the neighbours of a cell
                     */
                    int topCell = height - 1;
                    if (topCell < 0)
                        topCell = CellNumberHeight - 1;
                    int botCell = height + 1;
                    if (botCell >= CellNumberHeight)
                        botCell = 0;
                    int leftCell = width - 1;
                    if (leftCell < 0)
                        leftCell = CellNumberWidth - 1;
                    int rightCell = width + 1;
                    if (rightCell >= CellNumberWidth)
                        rightCell = 0;

                    int neighbours = 0;

                    if (Fields[topCell][leftCell].Fill == Brushes.DeepPink)
                    { neighbours++; }
                    if (Fields[topCell][width].Fill == Brushes.DeepPink)
                    { neighbours++; }
                    if (Fields[topCell][rightCell].Fill == Brushes.DeepPink)
                    { neighbours++; }
                    if (Fields[height][leftCell].Fill == Brushes.DeepPink)
                    { neighbours++; }
                    if (Fields[height][rightCell].Fill == Brushes.DeepPink)
                    { neighbours++; }
                    if (Fields[botCell][leftCell].Fill == Brushes.DeepPink)
                    { neighbours++; }
                    if (Fields[botCell][width].Fill == Brushes.DeepPink)
                    { neighbours++; }
                    if (Fields[botCell][rightCell].Fill == Brushes.DeepPink)
                    { neighbours++; }

                    neighbournumber[height, width] = neighbours;


                }
            }
            /*
             * On the basis of the numbers of neighbours (neighbournumber) we iterate through each cell and change the 
             * status of the cell (dead/alive) by color change.
             * Additionally we differentiate the torus case and with a non torus case a.
             */
            for (int height = 0; height < CellNumberHeight; height++)
            {
                for (int width = 0; width < CellNumberWidth; width++)
                {
                    if (!useTorus && (height == CellNumberHeight - 1 || width == CellNumberWidth - 1 || height == 0 || width == 0))
                    {
                        Fields[height][width].Fill = Brushes.Gray;
                    }
                    else
                    {
                        if (neighbournumber[height, width] < 2 || neighbournumber[height, width] > 3)
                        {
                            Fields[height][width].Fill = Brushes.MediumAquamarine;
                        }
                        else if (neighbournumber[height, width] == 3)
                        {
                            Fields[height][width].Fill = Brushes.DeepPink;
                        }
                    }
                }
            }

            return Fields;
        }

    }


}