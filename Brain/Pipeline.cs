using System;
using GlmSharp;

namespace game_engine.Brain
{
    
    public struct mPersProj
    {
        public float FOV;
        public float width;
        public float height;
        public float zNear;
        public float zFar;
    }

    public class Pipeline
    {
        private vec3 mScale;
        private vec3 mRotation;
        private vec3 mPosition;
        private mat4 mTransformation;
        private mPersProj mProj;

        public Pipeline() 
        {
            mScale = new vec3(1.0f, 1.0f, 1.0f);
            mRotation = new vec3(0.0f, 0.0f, 0.0f);
            mPosition = new vec3(0.0f, 0.0f, 0.0f);
            mTransformation = new mat4();
            mProj = new mPersProj();
        }

        public void Scale(float scaleX, float scaleY, float scaleZ)
        {
            mScale.x = scaleX; mScale.y = scaleY; mScale.z = scaleZ;
        }

        public void Rotate(float angleX, float angleY, float angleZ) 
        {
            mRotation.x = angleX; mRotation.y = angleY; mRotation.z = angleZ;
        }

        public void Position(float x, float y, float z)
        {
            mPosition.x = x; mPosition.y = y;   mPosition.z = z;
        }

        public void PerspectiveProj(float FOV, float width, float height, float zNear, float zFar)
        {
            mProj.FOV = FOV; mProj.width = width; mProj.height = height; mProj.zNear = zNear; mProj.zFar = zFar; 
        }

        public mat4 getTransformation()
        {
            mTransformation = InitScaleTransform() * InitRotateTransform() * InitTranslationTransform() * InitPerspectiveProj();
            return mTransformation;
        }

        private mat4 InitScaleTransform()
        {
            mat4 m = new mat4();

            m[0, 0] = mScale.x;     m[0, 1] = 0.0f;         m[0, 2] = 0.0f;         m[0, 3] = 0.0f;
            m[1, 0] = 0.0f;         m[1, 1] = mScale.y;     m[1, 2] = 0.0f;         m[1, 3] = 0.0f;
            m[2, 0] = 0.0f;         m[2, 1] = 0.0f;         m[2, 2] = mScale.z;     m[2, 3] = 0.0f;
            m[3, 0] = 0.0f;         m[3, 1] = 0.0f;         m[3, 2] = 0.0f;         m[3, 3] = 1.0f;

            return m;
        }

        private mat4 InitRotateTransform()
        {
            mat4 m = new mat4();

            mat4 rx = new mat4(), ry = new mat4(), rz = new mat4();
            float x = ToRadian(mRotation.x);
            float y = ToRadian(mRotation.y);
            float z = ToRadian(mRotation.z);

            rx[0, 0] = 1.0f;                    rx[0, 1] = 0.0f;                    rx[0, 2] = 0.0f;                    rx[0, 3] = 0.0f;
            rx[1, 0] = 0.0f;                    rx[1, 1] = (float)(Math.Cos(x));    rx[1, 2] = -(float)(Math.Sin(x));   rx[1, 3] = 0.0f;
            rx[2, 0] = 0.0f;                    rx[2, 1] = (float)(Math.Sin(x));    rx[2, 2] = (float)(Math.Cos(x));    rx[2, 3] = 0.0f;
            rx[3, 0] = 0.0f;                    rx[3, 1] = 0.0f;                    rx[3, 2] = 0.0f;                    rx[3, 3] = 1.0f;

            ry[0, 0] = (float)(Math.Cos(y));    ry[0, 1] = 0.0f;                    ry[0, 2] = -(float)(Math.Sin(y));   ry[0, 3] = 0.0f;
            ry[1, 0] = 0.0f;                    ry[1, 1] = 1.0f;                    ry[1, 2] = 0.0f;                    ry[1, 3] = 0.0f;
            ry[2, 0] = (float)(Math.Sin(y));    ry[2, 1] = 0.0f;                    ry[2, 2] = (float)(Math.Cos(y));    ry[2, 3] = 0.0f;
            ry[3, 0] = 0.0f;                    ry[3, 1] = 0.0f;                    ry[3, 2] = 0.0f;                    ry[3, 3] = 1.0f;

            rz[0, 0] = (float)(Math.Cos(z));    rz[0, 1] = -(float)(Math.Sin(z));   rz[0, 2] = 0.0f;                    rz[0, 3] = 0.0f;
            rz[1, 0] = (float)(Math.Sin(z));    rz[1, 1] = (float)(Math.Cos(z));    rz[1, 2] = 0.0f;                    rz[1, 3] = 0.0f;
            rz[2, 0] = 0.0f;                    rz[2, 1] = 0.0f;                    rz[2, 2] = 1.0f;                    rz[2, 3] = 0.0f;
            rz[3, 0] = 0.0f;                    rz[3, 1] = 0.0f;                    rz[3, 2] = 0.0f;                    rz[3, 3] = 1.0f;

            m = rz * ry * rx;
            return m;
        }

        private mat4 InitTranslationTransform()
        {
            mat4 m = new mat4();

            m[0, 0] = 1.0f; m[0, 1] = 0.0f; m[0, 2] = 0.0f; m[0, 3] = mPosition.x;
            m[1, 0] = 0.0f; m[1, 1] = 1.0f; m[1, 2] = 0.0f; m[1, 3] = mPosition.y;
            m[2, 0] = 0.0f; m[2, 1] = 0.0f; m[2, 2] = 1.0f; m[2, 3] = mPosition.z;
            m[3, 0] = 0.0f; m[3, 1] = 0.0f; m[3, 2] = 0.0f; m[3, 3] = 1.0f;
            return m;
        }

        private mat4 InitPerspectiveProj()
        {
            mat4 m = new mat4();
            float ar = mProj.width / mProj.height;
            float zNear = mProj.zNear;
            float zFar = mProj.zFar;
            float zRange = zNear - zFar;
            float tanHalfFOV = (float)(Math.Tan(ToRadian(mProj.FOV / 2.0f)));

            m[0, 0] = 1.0f / (tanHalfFOV * ar);     m[0, 1] = 0.0f;                 m[0, 2] = 0.0f;                             m[0, 3] = 0.0f;
            m[1, 0] = 0.0f;                         m[1, 1] = 1.0f / tanHalfFOV;    m[1, 2] = 0.0f;                             m[1, 3] = 0.0f;
            m[2, 0] = 0.0f;                         m[2, 1] = 0.0f;                 m[2, 2] = (-zNear - zFar) / zRange;         m[2, 3] = 0.0f;
            m[3, 0] = 0.0f;                         m[3, 1] = 0.0f;                 m[3, 2] = 2.0f * zFar * zNear / zRange;     m[3, 3] = 1.0f;
            return m;
        }

        private float ToRadian(float x)
        {
            return x * (float)Math.PI / 180.0f;
        }
    }
}
