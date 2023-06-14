using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using game_engine.Brain;
using GlmSharp;

namespace game_engine.Brain
{
    public class Camera
    {
        private vector3 m_pos;
        private vector3 m_target;
        private vector3 m_up;

        private int m_windowWidth;
        private int m_windowHeight;

        private float m_AngleH;
        private float m_AngleV;

        private vector2 m_mousePos;

        public Camera(int WindowWidth, int WindowHeight)
        {
            m_windowWidth = WindowWidth;
            m_windowHeight = WindowHeight;
            m_pos = new vector3(0.0f, 0.0f, 0.0f);
            m_target = new vector3(0.0f, 0.0f, 1.0f);
            m_target.Normalize();
            m_up = new vector3(0.0f, 1.0f, 0.0f);

            Init();
        }

        public Camera(int WindowWidth, int WindowHeight, vector3 Pos, vector3 Target, vector3 Up)
        {
            m_windowWidth = WindowWidth;
            m_windowHeight = WindowHeight;
            m_pos = Pos;

            m_target = Target;
            m_target.Normalize();

            m_up = Up;
            m_up.Normalize();

            Init();
        }

        public void Init()
        {
            vector3 HTarget = new vector3(m_target.x, 0, m_target.z);
            HTarget.Normalize();

            if (HTarget.z >= 0.0f)
            {
                if (HTarget.x >= 0.0f)
                {
                    m_AngleH = 360.0f - math3d.ToDegree(math3d.sin(HTarget.z));
                }
                else
                {
                    m_AngleH = 180.0f + math3d.ToDegree(math3d.sin(HTarget.z));
                }
            }
            else
            {
                if (HTarget.x >= 0.0f)
                {
                    m_AngleH = math3d.ToDegree(math3d.sin(-HTarget.z));
                }
                else
                {
                    m_AngleH = 90.0f + math3d.ToDegree(math3d.sin(-HTarget.z));
                }
            }
            m_AngleV = -math3d.ToDegree(math3d.sin(m_target.y));

            m_mousePos.x = m_windowWidth / 2;
            m_mousePos.y = m_windowHeight / 2;

            //glutWarpPointer(m_mousePos.x, m_mousePos.y);
        }

    }
}
