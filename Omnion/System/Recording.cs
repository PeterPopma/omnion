using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omnion
{
    class Recording
    {
        string name;
        string fileName;
        DateTime date;
        string neuronLayers;
        int learingIterations;
        string displayText;

        public string Name { get => name; set => name = value; }
        public string FileName { get => fileName; set => fileName = value; }
        public DateTime Date { get => date; set => date = value; }
        public string NeuronLayers { get => neuronLayers; set => neuronLayers = value; }
        public int LearingIterations { get => learingIterations; set => learingIterations = value; }
        public string DisplayText { get => displayText; set => displayText = value; }
    }
}
