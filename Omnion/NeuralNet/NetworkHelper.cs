using Omnion.NeuralNet;
using System.Drawing;
using System.Windows.Forms;

namespace Omnion.NeuralNet
{
    public static class NetworkHelper
    {
        public static void ToTreeView(TreeView t, NeuralNetwork nn)
        {
            t.Nodes.Clear();

            TreeNode root = new TreeNode("NeuralNetwork");

            nn.Layers.ForEach((layer) =>
            {
                TreeNode lnode = new TreeNode("Layer");

                layer.Neurons.ForEach((neuron) =>
                {
                    TreeNode nnode = new TreeNode("Neuron");
                    nnode.Nodes.Add("Bias: " + neuron.Bias.ToString());
                    nnode.Nodes.Add("Delta: " + neuron.Delta.ToString());
                    nnode.Nodes.Add("Value: " + neuron.Value.ToString());

                    neuron.Dendrites.ForEach((dendrite) =>
                    {
                        TreeNode dnode = new TreeNode("Dendrite");
                        dnode.Nodes.Add("Weight: " + dendrite.Weight.ToString());

                        nnode.Nodes.Add(dnode);
                    });

                    lnode.Nodes.Add(nnode);
                });

                root.Nodes.Add(lnode);
            });

            //root.ExpandAll();
            t.Nodes.Add(root);
        }

        public static void ToPictureBox(PictureBox p, NeuralNetwork nn)
        {
            int margins = 35;
            int neuronSize    = 30;
            int neuronDistance = 40;
            int layerDistance  = 100;
            int fontSize       = 8;
            //            int startX = 10;
            p.Width = margins * 2 + ((nn.MaxNeuronsInOneLayer()-1) * neuronDistance);
            p.Height = layerDistance * nn.Layers.Count;
            int centerX = p.Width / 2;

            Bitmap b = new Bitmap(p.Width, p.Height);
            Graphics g = Graphics.FromImage(b);

            g.FillRectangle(Brushes.White, g.ClipBounds);

            int y = margins;

            for(int l = 0; l < nn.Layers.Count; l++)
            {
                Layer layer = nn.Layers[l];
                
                int x = centerX - (neuronDistance * (layer.Neurons.Count / 2));

                for (int n = 0; n < layer.Neurons.Count; n++)
                {
                    Neuron neuron = layer.Neurons[n];

                    for (int d = 0; d < neuron.Dendrites.Count; d++)
                    {
                        int x_previous = neuronSize / 2 + neuronDistance * d + centerX - (neuronDistance * (nn.Layers[l-1].Neurons.Count / 2));
                        int y_previous = y - layerDistance + neuronSize;
                        // draw dendrites between neurons
                        int lineNumber = (n*neuron.Dendrites.Count + d);
                        g.DrawLine(Pens.Gray, x+ neuronSize/2, y, x_previous, y_previous);
                        float factor = (n % 4)/4.0f;
                        g.DrawString(neuron.Dendrites[d].Weight.ToString("0.00"), new Font("Arial", fontSize), Brushes.Black, (factor*x + (1-factor)*x_previous), (factor * y + (1 - factor) * y_previous));
                    };
                    byte red = (byte)(255 * neuron.Value);
                    byte green = (byte)(255 - (256*neuron.Value));
                    byte blue = (byte)((512 * neuron.Value)%256);
                    var brush = new SolidBrush(Color.FromArgb(255, red, green, blue));
                    g.FillEllipse(brush, x, y, neuronSize, neuronSize);
                    g.DrawEllipse(Pens.Black, x, y, neuronSize, neuronSize);
                    g.DrawString(neuron.Value.ToString("0.00"), new Font("Arial", fontSize), Brushes.Black, x + 2, y + (neuronSize / 2) - 5);

                    x += neuronDistance;
                };

                y += layerDistance;
            };

            p.Image = b;
        }

    }
}
