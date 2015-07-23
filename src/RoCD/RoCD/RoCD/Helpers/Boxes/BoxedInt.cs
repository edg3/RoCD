using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoCD.Helpers.Boxes
{
    public class BoxedInt
    {
        private int _value = 0;
        public int Value
        {
            get { return _value; }
        }

        private List<BInt> boxes = new List<BInt>();

        public void AddBox(BInt box)
        {
            box.PropertyChanged += box_PropertyChanged;
            boxes.Add(box);
        }

        void box_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _value = 0;
            for (int i = 0; i < boxes.Count; i++)
            {
                _value += boxes[i].Value;
            }
        }

        public void RemBox(BInt box)
        {
            box.PropertyChanged -= box_PropertyChanged;
            boxes.Remove(box);
        }
    }
}
