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

        public List<List<Rectangle>> Felder
        { get; set; }


        public GOLService(int cellNumberWidth, int cellNumberHeight, List<List<Rectangle>> felder)
        {
            this.CellNumberWidth = cellNumberWidth;
            this.CellNumberHeight = cellNumberHeight;
            this.Felder = felder;
        }

        public List<List<Rectangle>> adjustList(List<List<Rectangle>> list, int numberoflists)
        {
            for (int i = 0; i < numberoflists; i++)
            {
                list.Add(new List<Rectangle>());
            }
            return list;
        }


        public List<List<Rectangle>> updateCells()
        {
            int[,] anzahlNachbarn = new int[CellNumberHeight, CellNumberWidth];

            for (int height = 0; height < CellNumberWidth; height++)
            {
                for (int width = 0; width < CellNumberHeight; width++)
                {
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

                    int nachbarn = 0;

                    if (Felder[topCell][leftCell].Fill == Brushes.DeepPink)
                    { nachbarn++; }
                    if (Felder[topCell][width].Fill == Brushes.DeepPink)
                    { nachbarn++; }
                    if (Felder[topCell][rightCell].Fill == Brushes.DeepPink)
                    { nachbarn++; }
                    if (Felder[height][leftCell].Fill == Brushes.DeepPink)
                    { nachbarn++; }
                    if (Felder[height][rightCell].Fill == Brushes.DeepPink)
                    { nachbarn++; }
                    if (Felder[botCell][leftCell].Fill == Brushes.DeepPink)
                    { nachbarn++; }
                    if (Felder[botCell][width].Fill == Brushes.DeepPink)
                    { nachbarn++; }
                    if (Felder[botCell][rightCell].Fill == Brushes.DeepPink)
                    { nachbarn++; }

                    anzahlNachbarn[height, width] = nachbarn;


                }
            }

            for (int height = 0; height < CellNumberWidth; height++)
            {
                for (int width = 0; width < CellNumberHeight; width++)
                {
                    if (height == CellNumberHeight-1 || width == CellNumberWidth-1 || height == 0 || width == 0)
                    {
                        Felder[height][width].Fill = Brushes.Gray;
                    }
                    else
                    {
                        if (anzahlNachbarn[height, width] < 2 || anzahlNachbarn[height, width] > 3)
                        {
                            Felder[height][width].Fill = Brushes.MediumAquamarine;
                        }
                        else if (anzahlNachbarn[height, width] == 3)
                        {
                            Felder[height][width].Fill = Brushes.DeepPink;
                        }
                    }
                }
            }

            return Felder;
        }

    }


}