using System.Collections.Generic;
using System.Windows.Shapes;

namespace TOIKTapp
{
    class Graf
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public bool IsActiv { get; set; }

        public static int count = 0;

        public List<Graf> AdjacentGraphs { get; set; }

        public int Color { get; set; }
       
        public Ellipse CircleGraf { get; set; }
        public Graf()
        {
            AdjacentGraphs = new List<Graf>();
            CircleGraf = new Ellipse();
        }
    }
}
