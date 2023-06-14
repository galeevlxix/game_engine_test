using game_engine.Brain;
using GlmSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace game_engine
{
    public partial class Form1 : Form
    {
        bool md = false;
        static float scale = 100;
        Label label1 = new Label();
        Model model;
        bool exit = false;
        private Timer moveTimer = new Timer();
        Pipeline p;

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);

            this.ClientSize = new Size(1000, 1000);
            this.BackColor = Color.Black;
            this.MouseDown += Form1_MouseDown;
            this.MouseUp += Form1_MouseUp;
            this.MouseMove += Form1_MouseMove;
            this.KeyDown += Form1_KeyDown;
            this.Paint += Form1_Paint;
            p = new Pipeline();

            label1.Text = scale.ToString();
            label1.Location = new Point(this.Width - 100, this.Height - 100);
            label1.Size = new Size(50, 50);
            label1.ForeColor = Color.White;

            this.Controls.Add(label1);

            model = new Model();
            /*model.LoadFromObj(new StreamReader(new WebClient().OpenRead("http://www.wonthelp.info/superjoebob/TutorialImages/objPlane.obj")));*/
            model.LoadFromObj(new StreamReader("C:\\Users\\Lenovo\\source\\repos\\test_game\\test_game\\Models\\objPlane.obj"));

            moveTimer.Interval = 30;
            moveTimer.Tick += new EventHandler(TimerTickHandler);
            moveTimer.Start();

        }

        void TimerTickHandler(object sender, EventArgs e)
        {
            if (!exit) scale += 2f;
            else moveTimer.Stop();
            Invalidate();
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            label1.Text = scale.ToString();
            p.Scale(-10f, -10f, -10f);
            p.Position(0.0f, 44, 0.0f);
            p.Rotate(0.0f, -(float)Math.Sin(scale / 100) * 200, 0.0f);
            p.PerspectiveProj(100.0f, 1000, 1000, 1.0f, 1000.0f);

            //умножаем вектора на матрицу
            //var vertexes = model.Vertexes.Select(v => Vector3.Transform(v, p.getTransformation())).ToList();

            List<vec3> vertexes = new List<vec3>();

            foreach (var vertex in model.Vertexes)
            {
                vertexes.Add(Transform(vertex, p.getTransformation()));
            }

            e.Graphics.TranslateTransform(this.Width / 2, this.Height / 2);

            //создаем graphicsPath
            using (var path = new GraphicsPath())
            {
                //создаем грани
                var prev = vec3.Zero;
                var prevF = 0;
                foreach (var f in model.Fig)
                {
                    if (f == 0) path.CloseFigure();
                    var v = vertexes[f];
                    if (prevF != 0 && f != 0)
                        path.AddLine(prev.x, prev.y, v.x, v.y);
                    prev = v;
                    prevF = f;
                }

                //отрисовываем
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.DrawPath(Pens.White, path);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.E:
                    exit = true;
                    break;
                default:
                    break;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            md = false;
        }

        float startx;
        float starty;

        float scalex = 10000;
        float scaley = 10000;

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (md)
            {
                scalex = scalex - (e.X - startx) / 40;
                scaley = scaley - (e.Y - starty) / 40;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            md = true;
            startx = e.X;
            starty = e.Y;
        }

        public static vec3 Transform(vec3 v, mat4 m)
        {
            float x = v.x * m[0, 0] + v.y * m[1, 0] + v.z * m[2, 0] + m[3, 0];
            float y = v.y * m[0, 1] + v.y * m[1, 1] + v.z * m[2, 1] + m[3, 1];
            float z = v.z * m[0, 2] + v.y * m[1, 2] + v.z * m[2, 2] + m[3, 2];
            return new vec3(x, y, z);
        }
    }
}
