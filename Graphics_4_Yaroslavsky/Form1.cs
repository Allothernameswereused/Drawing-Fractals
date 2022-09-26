using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphics_4_Yaroslavsky
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Graphics gln;
        private List<PointF> Initiator; //Стартовые точки фрактала
        private float Scale; //Размер сегментов фрактала
        private List<float> FractalGenerator; //Углы и расстояния для генератора фрактала
        int depth; //Глубина фрактала
 
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                depth = int.Parse(textBox1.Text); //Пытается считать параметр глубины, введённый пользователем
            }
            catch
            {
                MessageBox.Show("Вы не ввели глубину фрактала (целое число)!");
            }

            gln = panel1.CreateGraphics();

            Scale = 1 / 3f; //Размер сегментов фрактала - 1/3 от старшего сегмента

            Initiator = new List<PointF>(); //Задаём координаты точек, используются для старта (видны при глубине 0)
            float height = 0.75f * (Math.Min(panel1.Width, panel1.Height) - 20);
            float width = (float)(height/Math.Sqrt(3.0)*2);
            float y3 = panel1.Height - 10;
            float y1 = y3 - height;
            float x3 = panel1.Height / 2;
            float x1 = x3 - width / 2;
            float x2 = x1 + width;
            Initiator.Add(new PointF(x1, y1));
            Initiator.Add(new PointF(x2, y1));
            Initiator.Add(new PointF(x3, y3));
            Initiator.Add(new PointF(x1, y1));

            FractalGenerator = new List<float>(); //Задаём углы линий при рисовании фрактала
            float pi_over_3 = (float)(Math.PI / 3f);
            FractalGenerator.Add(0);
            FractalGenerator.Add(-pi_over_3);
            FractalGenerator.Add(2*pi_over_3);
            FractalGenerator.Add(-pi_over_3);


            DrawFractal(gln, depth); //Рисуем фрактал с имеющимися данными
        }

        private void button2_Click(object sender, EventArgs e) //Очищает холст
        {
            clearfield();
        }


        private void DrawFractal (Graphics gln, int depth) //Рисует фрактал
        {
            clearfield();

            for (int counter = 1; counter < Initiator.Count; counter++)
            {
                PointF p1 = Initiator[counter - 1];
                PointF p2 = Initiator[counter];

                float dx = p2.X - p1.X;
                float dy = p2.Y - p1.Y;
                float length = (float)Math.Sqrt((dx * dx) + (dy * dy));
                float theta = (float)Math.Atan2(dy, dx);
                DrawEdge(gln, depth, ref p1, theta, length);
            }

        }

        private void DrawEdge (Graphics gln, int depth, ref PointF p1, float theta, float dist) //Рекурсивная функция, рисует уголок фрактала
        {
            if (depth == 0)
            {
                PointF p2 = new PointF
                    (
                    (float)(p1.X + dist * Math.Cos(theta)),
                    (float)(p1.Y + dist * Math.Sin(theta))
                    );
                gln.DrawLine(Pens.Black, p1, p2);
                p1 = p2;
                return;
            }

            dist *= Scale;
            for (int counter =0; counter < FractalGenerator.Count; counter++)
            {
                theta += FractalGenerator[counter];
                DrawEdge(gln, depth - 1, ref p1, theta, dist);
            }
        }

        private void clearfield() //Очищает холст
        {

            SolidBrush mass_eraser = new SolidBrush(panel1.BackColor);
            Rectangle field = new Rectangle(0, 0, panel1.Width, panel1.Height);
            gln.FillRectangle(mass_eraser, field);
        }

    }
}
