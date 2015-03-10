using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhiteBot
{
    public partial class DirectionVisualizer : UserControl
    {
        private float m_x = 1.0f;
        private float m_y;
        public float X
        {
            get
            {
                return m_x;
            }
            set
            {
                if(m_x != value)
                {
                    m_x = value;
                    Invalidate();
                }
            }
        }
        public float Y
        {
            get
            {
                return m_y;
            }
            set
            {
                if (m_y != value)
                {
                    m_y = value;
                    Invalidate();
                }
            }
        }

        public void Direction(float x, float y)
        {
            if(m_x != x || m_y != y)
            {
                m_x = x;
                m_y = y;
                Invalidate();
            }
        }

        private Color m_indicatorColor = Color.Green;
        public Color IndicatorColor
        {
            get
            {
                return m_indicatorColor;
            }
            set
            {
                if(m_indicatorColor != value)
                {
                    m_indicatorColor = value;
                    m_pen = new Pen(value);
                    Invalidate();
                }
            }
        }
        private float m_indicatorScale = 1.0f;
        private Pen m_pen = new Pen(Color.Green);

        public float IndicatorScale
        {
            get
            {
                return m_indicatorScale;
            }
            set
            {
                if (m_indicatorScale != value)
                {
                    m_indicatorScale = value;
                    Invalidate();
                }
            }
        }

        public DirectionVisualizer()
        {
            InitializeComponent();
        }

        private void DirectionVisualizer_Paint(object sender, PaintEventArgs e)
        {
            paintDirectionCoordinates(e.Graphics);
            drawRotationIndicator(e.Graphics, 25, 25, 20 * m_indicatorScale, m_x, m_y, m_pen);
        }

        private void paintDirectionCoordinates(Graphics g)
        {
            g.FillEllipse(Brushes.Black, 23, 23, 5, 5);
            g.DrawEllipse(Pens.Black, 3, 3, 45, 45);
        }

        private void drawRotationIndicator(Graphics g, int x, int y, float size, float dirX, float dirY, Pen color)
        {
            g.DrawLine(color, x, y, x + dirX * size, y + dirY * size);
        }
    }
}
