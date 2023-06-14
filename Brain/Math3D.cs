using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_engine.Brain
{
    public class vector2
    {
        public float x;
        public float y;
    }

    public class vector3
    {
        public float x;
        public float y;
        public float z;

        public vector3()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public vector3(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public vector3 Cross(vector3 v)
        {
            float _x = y * v.z - z * v.y;
            float _y = z * v.x - x * v.z;
            float _z = x * v.y - y * v.x;
            return new vector3(_x, _y, _z);
        }

        public vector3 Normalize()
        {
            float Length = (float)Math.Sqrt(x * x + y * y + z * z);

            x /= Length;
            y /= Length;
            z /= Length;

            return this;
        }

        public void Rotate(float Angle, vector3 Axe)
        {
            float SinHalfAngle = (float)Math.Sin(math3d.ToRadian(Angle / 2));
            float CosHalfAngle = math3d.cos(math3d.ToRadian(Angle / 2));

            float Rx = Axe.x * SinHalfAngle;
            float Ry = Axe.y * SinHalfAngle;
            float Rz = Axe.z * SinHalfAngle;
            float Rw = CosHalfAngle;
            quaternion RotationQ = new quaternion(Rx, Ry, Rz, Rw);
            quaternion ConjugateQ = RotationQ.Conjugate();
            quaternion W = RotationQ * (this) * ConjugateQ;

            x = W.x;
            y = W.y;
            z = W.z;
        }

        public static vector3 operator +(vector3 l, vector3 r)
        {
            return new vector3(l.x + r.x, l.y + r.y, l.z + r.z);
        }

        public static vector3 operator -(vector3 l, vector3 r)
        {
            return new vector3(l.x - r.x, l.y - r.y, l.z - r.z);
        }

        public static vector3 operator *(vector3 l, float f)
        {
            return new vector3(l.x * f, l.y * f, l.z * f);
        }
    }

    public class matrix4
    {
        public float[,] m = new float[4, 4];

        public matrix4()
        {

        }

        public void InitIdentity()
        {
            m[0, 0] = 1.0f; m[0, 1] = 0.0f; m[0, 2] = 0.0f; m[0, 3] = 0.0f;
            m[1, 0] = 0.0f; m[1, 1] = 1.0f; m[1, 2] = 0.0f; m[1, 3] = 0.0f;
            m[2, 0] = 0.0f; m[2, 1] = 0.0f; m[2, 2] = 1.0f; m[2, 3] = 0.0f;
            m[3, 0] = 0.0f; m[3, 1] = 0.0f; m[3, 2] = 0.0f; m[3, 3] = 1.0f;
        }

        public static matrix4 operator *(matrix4 Left, matrix4 Right)
        {
            matrix4 Ret = new matrix4();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Ret.m[i, j] =
                        Left.m[i, 0] * Right.m[0, j] +
                        Left.m[i, 1] * Right.m[1, j] +
                        Left.m[i, 2] * Right.m[2, j] +
                        Left.m[i, 3] * Right.m[3, j];
                }
            }
            return Ret;
        }

        public void InitRotateTransform(float RotateX, float RotateY, float RotateZ)
        {
            matrix4 rx = new matrix4(), ry = new matrix4(), rz = new matrix4();
            float x = math3d.ToRadian(RotateX);
            float y = math3d.ToRadian(RotateY);
            float z = math3d.ToRadian(RotateZ);

            rx.m[0, 0] = 1.0f; rx.m[0, 1] = 0.0f; rx.m[0, 2] = 0.0f; rx.m[0, 3] = 0.0f;
            rx.m[1, 0] = 0.0f; rx.m[1, 1] = math3d.cos(x); rx.m[1, 2] = -math3d.sin(x); rx.m[1, 3] = 0.0f;
            rx.m[2, 0] = 0.0f; rx.m[2, 1] = math3d.sin(x); rx.m[2, 2] = math3d.cos(x); rx.m[2, 3] = 0.0f;
            rx.m[3, 0] = 0.0f; rx.m[3, 1] = 0.0f; rx.m[3, 2] = 0.0f; rx.m[3, 3] = 1.0f;

            ry.m[0, 0] = math3d.cos(y); ry.m[0, 1] = 0.0f; ry.m[0, 2] = -math3d.sin(y); ry.m[0, 3] = 0.0f;
            ry.m[1, 0] = 0.0f; ry.m[1, 1] = 1.0f; ry.m[1, 2] = 0.0f; ry.m[1, 3] = 0.0f;
            ry.m[2, 0] = math3d.sin(y); ry.m[2, 1] = 0.0f; ry.m[2, 2] = math3d.cos(y); ry.m[2, 3] = 0.0f;
            ry.m[3, 0] = 0.0f; ry.m[3, 1] = 0.0f; ry.m[3, 2] = 0.0f; ry.m[3, 3] = 1.0f;

            rz.m[0, 0] = math3d.cos(z); rz.m[0, 1] = -math3d.sin(z); rz.m[0, 2] = 0.0f; rz.m[0, 3] = 0.0f;
            rz.m[1, 0] = math3d.sin(z); rz.m[1, 1] = math3d.cos(z); rz.m[1, 2] = 0.0f; rz.m[1, 3] = 0.0f;
            rz.m[2, 0] = 0.0f; rz.m[2, 1] = 0.0f; rz.m[2, 2] = 1.0f; rz.m[2, 3] = 0.0f;
            rz.m[3, 0] = 0.0f; rz.m[3, 1] = 0.0f; rz.m[3, 2] = 0.0f; rz.m[3, 3] = 1.0f;

            m = (rz * ry * rx).m;
        }

        public void InitTranslationTransform(float x, float y, float z)
        {
            m[0, 0] = 1.0f; m[0, 1] = 0.0f; m[0, 2] = 0.0f; m[0, 3] = x;
            m[1, 0] = 0.0f; m[1, 1] = 1.0f; m[1, 2] = 0.0f; m[1, 3] = y;
            m[2, 0] = 0.0f; m[2, 1] = 0.0f; m[2, 2] = 1.0f; m[2, 3] = z;
            m[3, 0] = 0.0f; m[3, 1] = 0.0f; m[3, 2] = 0.0f; m[3, 3] = 1.0f;
        }

        public void InitCameraTransform(vector3 Target, vector3 Up)
        {
            vector3 N = Target;
            N.Normalize();
            vector3 U = Up;
            U.Normalize();
            U = U.Cross(N);
            vector3 V = N.Cross(U);

            m[0, 0] = U.x; m[0, 1] = U.y; m[0, 2] = U.z; m[0, 3] = 0.0f;
            m[1, 0] = V.x; m[1, 1] = V.y; m[1, 2] = V.z; m[1, 3] = 0.0f;
            m[2, 0] = N.x; m[2, 1] = N.y; m[2, 2] = N.z; m[2, 3] = 0.0f;
            m[3, 0] = 0.0f; m[3, 1] = 0.0f; m[3, 2] = 0.0f; m[3, 3] = 1.0f;
        }

        public void InitPersProjTransform(float FOV, float Width, float Height, float zNear, float zFar)
        {
            float ar = Width / Height;
            float zRange = zNear - zFar;
            float tanHalfFOV = math3d.tan(math3d.ToRadian(FOV / 2.0f));

            m[0, 0] = 1.0f / (tanHalfFOV * ar); m[0, 1] = 0.0f; m[0, 2] = 0.0f; m[0, 3] = 0;
            m[1, 0] = 0.0f; m[1, 1] = 1.0f / tanHalfFOV; m[1, 2] = 0.0f; m[1, 3] = 0;
            m[2, 0] = 0.0f; m[2, 1] = 0.0f; m[2, 2] = (-zNear - zFar) / zRange; m[2, 3] = 2.0f * zFar * zNear / zRange;
            m[3, 0] = 0.0f; m[3, 1] = 0.0f; m[3, 2] = 1.0f; m[3, 3] = 0;
        }

    }

    public class math3d
    {
        public static float ToRadian(float x)
        {
            return x * (float)Math.PI / 180.0f;
        }
        public static float ToDegree(float x)
        {
            return x / (float)Math.PI * 180.0f;
        }
        public static float sin(float x)
        {
            return (float)Math.Sin((float)x);
        }
        public static float cos(float x)
        {
            return (float)Math.Cos((float)x);
        }
        public static float tan(float x)
        {
            return (float)Math.Tan((float)x);
        }
        public static float atan(float x)
        {
            return (float)Math.Atan((float)x);
        }
        public static float abs(float x)
        {
            return (float)Math.Abs((float)x);
        }
        public static float sqrt(float x)
        {
            return (float)Math.Sqrt((float)x);
        }
        public static float pow(float x, float y)
        {
            return (float)Math.Pow((float)x, (float)y);
        }
    }

    public class quaternion //вращение на определенный угол вокруг произвольной оси
    {
        public float x, y, z, w;

        public quaternion(float _x, float _y, float _z, float _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }

        public void Normalize()
        {
            float Length = math3d.sqrt(x * x + y * y + z * z + w * w);

            x /= Length;
            y /= Length;
            z /= Length;
            w /= Length;
        }

        public quaternion Conjugate()
        {
            return new quaternion(-x, -y, -z, w);
        }

        public static quaternion operator *(quaternion l, quaternion r)
        {
            float w = (l.w * r.w) - (l.x * r.x) - (l.y * r.y) - (l.z * r.z);
            float x = (l.x * r.w) + (l.w * r.x) + (l.y * r.z) - (l.z * r.y);
            float y = (l.y * r.w) + (l.w * r.y) + (l.z * r.x) - (l.x * r.z);
            float z = (l.z * r.w) + (l.w * r.z) + (l.x * r.y) - (l.y * r.x);

            return new quaternion(x, y, z, w);
        }

        public static quaternion operator *(quaternion q, vector3 v)
        {
            float w = -(q.x * v.x) - (q.y * v.y) - (q.z * v.z);
            float x = (q.w * v.x) + (q.y * v.z) - (q.z * v.y);
            float y = (q.w * v.y) + (q.z * v.x) - (q.x * v.z);
            float z = (q.w * v.z) + (q.x * v.y) - (q.y * v.x);

            return new quaternion(x, y, z, w);
        }
    }
}
