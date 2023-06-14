using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using GlmSharp;

namespace game_engine
{
    public class Model
    {
        public List<vec3> Vertexes = new List<vec3>();
        public List<int> Fig = new List<int>();

        public void LoadFromObj(TextReader tr)
        {
            string line;
            Vertexes.Clear();
            Vertexes.Add(vec3.Zero);

            while ((line = tr.ReadLine()) != null)
            {
                var parts = line.Split(' ');
                if (parts.Length == 0) continue;
                switch (parts[0])
                {
                    case "v":
                        Vertexes.Add(new vec3(float.Parse(parts[1], CultureInfo.InvariantCulture),
                        float.Parse(parts[2], CultureInfo.InvariantCulture),
                        float.Parse(parts[3], CultureInfo.InvariantCulture)));
                        break;
                    case "f":
                        for (int i = 1; i < parts.Length; i++)
                            Fig.Add(int.Parse(parts[i].Split('/')[0]));
                        Fig.Add(0);
                        break;
                }
            }
        }
    }
}
