using System;
using System.Collections;
using System.Collections.Generic;

namespace csDelaunay {
	public class Polygon {

		private List<Vector2f> vertices;

        public List<Vector2f> Vertices
        {
            get { return vertices; }
        }

		public Polygon(List<Vector2f> vertices) {
			this.vertices = vertices;
		}

        public Rectf Bounds()
        {
            float small_x = 9999999, small_y = 99999999, large_x = 0, large_y = 0;
            foreach (var vert in vertices)
            {
                if (vert.x < small_x) small_x = vert.x;
                if (vert.x > large_x) large_x = vert.x;
                if (vert.y < small_y) small_y = vert.y;
                if (vert.y > large_y) large_y = vert.y;
            }

            return new Rectf(small_x, small_y, large_x - small_x, large_y - small_y);
        }

		public float Area() {
			return Math.Abs(SignedDoubleArea() * 0.5f);
		}

		public Winding PolyWinding() {
			float signedDoubleArea = SignedDoubleArea();
			if (signedDoubleArea < 0) {
				return Winding.CLOCKWISE;
			}
			if (signedDoubleArea > 0) {
				return Winding.COUNTERCLOCKWISE;
			}
			return Winding.NONE;
		}

		private float SignedDoubleArea() {
			int index, nextIndex;
			int n = vertices.Count;
			Vector2f point, next;
			float signedDoubleArea = 0;

			for (index = 0; index < n; index++) {
				nextIndex = (index+1) % n;
				point = vertices[index];
				next = vertices[nextIndex];
				signedDoubleArea += point.x * next.y - next.x * point.y;
			}

			return signedDoubleArea;
		}

        public bool Contains(Vector2f point)
        {
            int i, j, nvert = vertices.Count;
            bool c = false;

            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (((vertices[i].y >= point.y) != (vertices[j].y >= point.y)) &&
                    (point.x <= (vertices[j].x - vertices[i].x) * (point.y - vertices[i].y) / (vertices[j].y - vertices[i].y) + vertices[i].x)
                  )
                    c = !c;
            }

            return c;
        }

        private List<Polygon> neighbours = new List<Polygon>();

        public List<Polygon> Neighbours
        {
            get { return neighbours; }
        }

        internal void AddNeighbour(Polygon polygon)
        {
            if (!neighbours.Contains(polygon))
            {
                neighbours.Add(polygon);
            }
        }
    }
}
