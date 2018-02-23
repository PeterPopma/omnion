using Omnion.NeuralNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Omnion.Forms
{
    public partial class FormLoadSave : Form
    {
        private FormMain myParent = null;
        private List<Recording> Recordings = new List<Recording>();
        NumberFormatInfo providerDecimalPoint = new NumberFormatInfo();
        const string VERSION_NUMBER = "1.0";


        public FormLoadSave()
        {
            InitializeComponent();
            providerDecimalPoint.NumberDecimalSeparator = ".";
        }

        public FormMain MyParent
        {
            get
            {
                return myParent;
            }

            set
            {
                myParent = value;
            }
        }

        private void gradientButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void gradientButtonLoadSave_Click(object sender, EventArgs e)
        {
            if (this.Text.Equals("Save"))
            {
                if (textBoxName.Text.Length == 0)
                {
                    MessageBox.Show("You must use a name!");
                    return;
                }
                else
                {
                    SaveRecording();
                }
            }
            if (this.Text.Equals("Load"))
            {
                if (textBoxName.Text.Length == 0)
                {
                    MessageBox.Show("You must select a file!");
                    return;
                }
                else
                {
                    LoadRecording();
                }
            }
            Close();
        }

        private Int32 GetStreamPosition(StreamReader s)
        {
            Int32 charpos = (Int32)s.GetType().InvokeMember("charPos", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, s, null);

            return charpos;
        }

        private void LoadCompressedData(string FileName, int layerCount, double learningRate, int inputNeurons, int outputNeurons, int hiddenNeurons, long learningIterations)
        {
            using (ZipArchive archive = ZipFile.OpenRead(FileName + "_data"))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.Equals("objectdata"))
                    {
                        using (var stream = new StreamReader(entry.Open()))
                        {
                            LoadData(stream, layerCount, learningRate, inputNeurons, outputNeurons, hiddenNeurons, learningIterations);
                        }
                    }
                }
            }
        }

        private void LoadRecording()
        {
            Recording rec = (Recording)listBoxRecordings.SelectedItem;
            var lines = File.ReadLines(rec.FileName).Take(4).ToArray();

            using (StreamReader srFile = new StreamReader(rec.FileName))
            {
                string version = srFile.ReadLine();      // version
                Double versionNumber = Convert.ToDouble(version.Substring(1), providerDecimalPoint);
                if(versionNumber<Convert.ToDouble(VERSION_NUMBER))
                {
                    MessageBox.Show("The recording you are trying to load is of version " + version.Substring(1) + ". Only recordings of version +" + VERSION_NUMBER + "+ can be loaded by this program.", "Older version recording");
                    return;
                }
                if(versionNumber != Convert.ToDouble(VERSION_NUMBER))
                {
                    MessageBox.Show("The recording you are trying to load is of an unknown type and cannot be loaded.", "Unknown recording file");
                    return;
                }

                panelBusy.Visible = true;
                Refresh();

                srFile.ReadLine();      // name
                srFile.ReadLine();      // savedate
                long totalNeurons = Convert.ToInt64(srFile.ReadLine());
                int layerCount = Convert.ToInt32(srFile.ReadLine());
                long learningIterations = Convert.ToInt64(srFile.ReadLine());
                double learningRate = Convert.ToDouble((srFile.ReadLine()), CultureInfo.InvariantCulture.NumberFormat);
                int inputNeurons = Convert.ToInt32(srFile.ReadLine());
                int hiddenNeurons = Convert.ToInt32(srFile.ReadLine());
                int outputNeurons = Convert.ToInt32(srFile.ReadLine());
                bool compressed = Convert.ToBoolean(srFile.ReadLine());

                if (compressed)
                {
                    LoadCompressedData(rec.FileName, layerCount, learningRate, inputNeurons, outputNeurons, hiddenNeurons, learningIterations);
                }
                else
                {
                    LoadData(srFile, layerCount, learningRate, inputNeurons, outputNeurons, hiddenNeurons, learningIterations);
                }

                myParent.LearningRate = learningRate;
                myParent.NumLayers = myParent.NeuralNetwork.LayerCount;
                myParent.InputLayerNeurons = inputNeurons;
                myParent.HiddenLayerNeurons = hiddenNeurons;
                myParent.OutputLayerNeurons = outputNeurons;
                myParent.labelTrainingIterations.Text = learningIterations.ToString();
                myParent.UpdateValues();
            }
        }

        private void LoadData(StreamReader srFile, int layerCount, double learningRate, int inputNeurons, int outputNeurons, int hiddenNeurons, long learningIterations)
        {
            myParent.NeuralNetwork = new NeuralNetwork(learningRate, inputNeurons, hiddenNeurons, outputNeurons);
            myParent.NeuralNetwork.Layers = new List<Layer>();
            myParent.NeuralNetwork.LearningIterations = learningIterations;

            for (int l = 0; l < layerCount; l++)
            {
                int numNeurons = hiddenNeurons;
                if (l == 0)
                {
                    numNeurons = inputNeurons;
                }
                if (l == layerCount - 1)
                {
                    numNeurons = outputNeurons;
                }
                Layer layer = new Layer(numNeurons);
                for (int n = 0; n < numNeurons; n++)
                {
                    Neuron neuron = new Neuron();
                    neuron.Bias = Convert.ToDouble(srFile.ReadLine());
                    neuron.Delta = Convert.ToDouble(srFile.ReadLine());
                    neuron.Value = Convert.ToDouble(srFile.ReadLine());
                    int dendriteCount = Convert.ToInt32(srFile.ReadLine());
                    for (int k = 0; k < dendriteCount; k++)
                    {
                        neuron.Dendrites.Add(new Dendrite(Convert.ToDouble(srFile.ReadLine())));
                    }
                    layer.Neurons.Add(neuron);
                }

                myParent.NeuralNetwork.Layers.Add(layer);
            }
        }

        private bool SaveRecording()
        {
            string FileName = CurrentRecordingFilename();

            if (FileName != null)        // file exists
            {
                DialogResult result = MessageBox.Show("That name already exists. Do you want to overwrite it?", "Existing recording", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return false;
                }
                else
                {
                    File.Delete(CurrentRecordingFilename() + "_data");      // delete compressed data if it exists
                }
            }
            else
            {
                FileName = FindUniqueFileName(myParent.SaveDir, "recording.omnion");
            }
            panelBusy.Visible = true;
            Refresh();

            using (StreamWriter srFile = new StreamWriter(FileName))
            {
                srFile.WriteLine("v"+VERSION_NUMBER);
                srFile.WriteLine(textBoxName.Text);
                srFile.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture));
                srFile.WriteLine(myParent.NeuralNetwork.TotalNeurons());
                srFile.WriteLine(myParent.NeuralNetwork.LayerCount);
                srFile.WriteLine(myParent.NeuralNetwork.LearningIterations);        // Until here, data is also used for file info, so that data should be first
                srFile.WriteLine(myParent.NeuralNetwork.LearningRate);
                srFile.WriteLine(myParent.NeuralNetwork.InputNeurons);
                srFile.WriteLine(myParent.NeuralNetwork.HiddenNeurons);
                srFile.WriteLine(myParent.NeuralNetwork.OutputNeurons);
                srFile.WriteLine(myParent.CompressRecordings);
                srFile.Flush();
                if (myParent.CompressRecordings)
                {
                    SaveCompressedData(FileName);
                }
                else
                {
                    SaveData(srFile);
                }
            }

            return true;
        }

        private void SaveData(StreamWriter writer)
        {
            for (int l = 0; l < myParent.NeuralNetwork.Layers.Count; l++)
            {
                Layer layer = myParent.NeuralNetwork.Layers[l];

                for (int n = 0; n < layer.Neurons.Count; n++)
                {
                    Neuron neuron = layer.Neurons[n];
                    writer.WriteLine(neuron.Bias);
                    writer.WriteLine(neuron.Delta);
                    writer.WriteLine(neuron.Value);
                    writer.WriteLine(neuron.DendriteCount);
                    neuron.Dendrites.ForEach((dendrite) =>
                    {
                        writer.WriteLine(dendrite.Weight.ToString());
                    });
                }
            }
        }

        public void SaveCompressedData(string FileName)
        {
            using (FileStream zipToOpen = new FileStream(FileName + "_data", FileMode.CreateNew))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                {
                    ZipArchiveEntry readmeEntry = archive.CreateEntry("objectdata");
                    using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                    {
                        SaveData(writer);
                    }
                }
            }
        }

        private static byte[] Compress(Stream input)
        {
            using (var compressStream = new MemoryStream())
            using (var compressor = new DeflateStream(compressStream, CompressionMode.Compress))
            {
                input.CopyTo(compressor);
                compressor.Close();
                return compressStream.ToArray();
            }
        }

        private static MemoryStream Decompress(Stream compressedStream)
        {
            var output = new MemoryStream();

            using (var decompressor = new DeflateStream(compressedStream, CompressionMode.Decompress))
                decompressor.CopyTo(output);

            output.Position = 0;
            return output;
        }



        static FileStream CreateFile(string fullPath)
        {
            try
            {
                return new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write);
            }
            catch (DirectoryNotFoundException) { throw; }
            catch (DriveNotFoundException) { throw; }
            catch (IOException)
            {
                // Will occur if another thread created a file with this 
                // name since we created the HashSet. Ignore this and just
                // try with the next filename.
            }

            return null;
        }

        static string FindUniqueFileName(string folder, string fileName, int maxAttempts = 10000)
        {
            // get filename base and extension
            var fileBase = Path.GetFileNameWithoutExtension(fileName);
            var ext = Path.GetExtension(fileName);
            // build hash set of filenames for performance
            var files = new HashSet<string>(Directory.GetFiles(folder));

            for (var index = 0; index < maxAttempts; index++)
            {
                // first try with the original filename, else try incrementally adding an index
                var name = (index == 0)
                    ? fileName
                    : String.Format("{0}{1}{2}", fileBase, index, ext);

                // check if exists
                var fullPath = Path.Combine(folder, name);
                if (files.Contains(fullPath))
                    continue;

                return fullPath;
            }

            throw new Exception("Could not create unique filename in " + maxAttempts + " attempts");
        }

        public void EnumerateRecordings()
        {
            try
            {
                var files = Directory.EnumerateFiles(myParent.SaveDir, "*.omnion", SearchOption.AllDirectories);
                Recordings.Clear();
                listBoxRecordings.Items.Clear();
                foreach (var f in files)
                {
                    Recording recording = new Recording();
                    var lines = File.ReadLines(f).Take(6).ToArray();
                    if (lines.Length == 6)
                    {
                        recording.FileName = f;
                        recording.Name = lines[1];
                        recording.Date = DateTime.ParseExact(lines[2], "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                        recording.NeuronLayers = lines[3] + " (" + lines[4] + ")";
                        recording.LearingIterations = Convert.ToInt32(lines[5]);
                        recording.DisplayText = recording.Name.PadRight(32, ' ') + " " + recording.Date.ToString("dd/MM/yyyy H:mm").PadRight(16, ' ') + " neurons(layers): " + recording.NeuronLayers.PadRight(8, ' ') + " training: " + recording.LearingIterations.ToString().PadRight(10, ' ');
                        Recordings.Add(recording);
                        listBoxRecordings.Items.Add(recording);
                    }
                }
                Console.WriteLine("{0} files found.", files.Count().ToString());
            }
            catch (UnauthorizedAccessException UAEx)
            {
                Console.WriteLine(UAEx.Message);
            }
            catch (PathTooLongException PathEx)
            {
                Console.WriteLine(PathEx.Message);
            }

        }

        private void listBoxRecordings_SelectedIndexChanged(object sender, EventArgs e)
        {
            Recording rec = (Recording)listBoxRecordings.SelectedItem;
            if (rec!=null)
            {
                textBoxName.Text = rec.Name;
                textBoxDate.Text = rec.Date.ToString("dd/MM/yyyy H:mm");
                textBoxNumberOfObjects.Text = rec.NeuronLayers;
                textBoxCalcsDone.Text = rec.LearingIterations.ToString();
            }
        }

        private void listBoxRecordings_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Text.Equals("Load"))
            {
                LoadRecording();
            }
            else
            {
                SaveRecording();
            }
            Close();
        }

        private string CurrentRecordingFilename()
        {
            foreach(Recording item in listBoxRecordings.Items)
            {
                if (item.Name.Equals(textBoxName.Text))
                {
                    return item.FileName;
                }
            }
            return null;
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (CurrentRecordingFilename() !=null)
            {
                gradientButtonDelete.Visible = true;
            }
            else
            {
                gradientButtonDelete.Visible = false;
            }

        }

        private void gradientButtonDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this recording?", "Delete recording", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                File.Delete(CurrentRecordingFilename());
                File.Delete(CurrentRecordingFilename()+"_data");
                EnumerateRecordings();
            }
        }

        private void panelBusy_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);

            using (var brush = new LinearGradientBrush(this.ClientRectangle,
                       Color.White, Color.Black, LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
    }
}
