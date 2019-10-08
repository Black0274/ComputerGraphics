using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jarvis
{
    public partial class Form1 : Form
    {
        Graphics graphics;
        List<Point> points;

        public Form1()
        {
            InitializeComponent();
            Image image = new Bitmap(pictureBox.Width, pictureBox.Height);
            graphics = Graphics.FromImage(image);
            pictureBox.Image = image;
            points = new List<Point>();
        }

        private void DrawConvexHull_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.Transparent);
            pictureBox.Refresh();
            foreach (Point p in points)
                DrawCrossPoint(p.X, p.Y);

            List<Point> hull = JarvisAlgorithm();
            for (int i = 1; i < hull.Count; i++)
            {
                graphics.DrawLine(Pens.Black, hull[i - 1], hull[i]);
                //pictureBox.Refresh();
                //System.Threading.Thread.Sleep(500);
            }
            graphics.DrawLine(Pens.Black, hull[0], hull.Last());
            pictureBox.Refresh();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.Transparent);
            pictureBox.Refresh();
            points.Clear();
        }

        private void PictureBox_Click(object sender, MouseEventArgs e)
        {
            AddPoint(e.Location.X, e.Location.Y);
        }

        private void AddPoint(int x, int y)
        {
            Point point = new Point(x, y);
            points.Add(point);
            DrawCrossPoint(x, y);
            pictureBox.Refresh();
        }

        static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        private void DrawPoint(int x, int y) { graphics.FillRectangle(Brushes.Black, x, y, 1, 1); }

        private void DrawCrossPoint(int x, int y)
        {
            graphics.FillRectangle(Brushes.Black, x, y, 1, 1);
            graphics.FillRectangle(Brushes.Black, x - 1, y, 1, 1);
            graphics.FillRectangle(Brushes.Black, x, y - 1, 1, 1);
            graphics.FillRectangle(Brushes.Black, x + 1, y, 1, 1);
            graphics.FillRectangle(Brushes.Black, x, y + 1, 1, 1);
        }


        public List<Point> JarvisAlgorithm()
        {
            List<Point> hull = new List<Point>();

            Point vPointOnHull = points.Where(p => p.X == points.Min(min => min.X)).First();

            Point vEndPoint;
            do
            {
                hull.Add(vPointOnHull);
                vEndPoint = points[0];

                for (int i = 1; i < points.Count; i++)
                {
                    if ((vPointOnHull == vEndPoint) || rotate(vPointOnHull, vEndPoint, points[i]))
                        vEndPoint = points[i];
                }

                vPointOnHull = vEndPoint;
            }
            while (vEndPoint != hull[0]);

            return hull;
        }

        // Возвращает truе если С находится слева от АВ, false если справа.
        private bool rotate(Point A, Point B, Point C)
        {
            return (B.X - A.X) * (C.Y - B.Y) - (B.Y - A.Y) * (C.X - B.X) > 0;
        }
    }
}
