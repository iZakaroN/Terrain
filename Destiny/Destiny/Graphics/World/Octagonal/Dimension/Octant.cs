using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destiny.Graphics.World.Octagonal.Dimension
{
	public class Octant// : VisualElement
	{
		Octant[] _childs;

		protected Octant()
		{
		}

		public Octant(int childPos, Octant child)
		{
			_childs = new Octant[8];
			_childs[childPos] = child;
		}

		public Octant this[int child] { 
			get { return _childs[child]; } 
			set { _childs[child] = value; } 
		}

		public Octant GetOrAllocate(int child)
		{
			if (_childs[child] == null)
				_childs[child] = new Octant(0, null);
			return _childs[child];
		}

		public bool HasChilds { get { return _childs != null; } }
        virtual public OctantNode Node { get { return null; } }

		public Octant[] Childs
		{
			get { return _childs; }
		}

	}
}
