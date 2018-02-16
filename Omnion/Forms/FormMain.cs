using Microsoft.Win32;
using Omnion.ChessClient;
using Omnion.NeuralNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Omnion.ChessClient.ChessPiece;

namespace Omnion.Forms
{
    public partial class FormMain : Form
    {
        private Panel panel1;
        private Omnion.CustomControls.GradientPanel gradientPanel1;
        private Panel panel2;
        private Omnion.CustomControls.GradientButton gradientButtonSave;
        private Omnion.CustomControls.GradientButton gradientButtonLoad;
        private Omnion.CustomControls.GradientPanel2 gradientPanel21;
        private ChessClient.ChessClient chessClient = new ChessClient.ChessClient();

        private string saveDir = Application.StartupPath + System.IO.Path.DirectorySeparatorChar + "Recordings";
        private bool compressRecordings;
        double high = .99, low = .01;

        double learningRate = 0.9;
        int hiddenLayerNeurons = 4; 
        int inputLayerNeurons = 4;
        int outputLayerNeurons = 4;
        int numLayers = 3;

        Random rnd = new Random();

        NeuralNet.NeuralNetwork neuralNetwork;
        FormNeuralLayout formNeuralLayout;

        public string SaveDir { get => saveDir; set => saveDir = value; }
        public bool CompressRecordings { get => compressRecordings; set => compressRecordings = value; }
        public NeuralNetwork NeuralNetwork { get => neuralNetwork; set => neuralNetwork = value; }
        public double LearningRate { get => learningRate; set => learningRate = value; }
        public int HiddenLayerNeurons { get => hiddenLayerNeurons; set => hiddenLayerNeurons = value; }
        public int InputLayerNeurons { get => inputLayerNeurons; set => inputLayerNeurons = value; }
        public int OutputLayerNeurons { get => outputLayerNeurons; set => outputLayerNeurons = value; }
        public int NumLayers { get => numLayers; set => numLayers = value; }

        public FormMain()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        public void UpdateValues()
        {
            numericUpDownLearningRate.Value = Convert.ToDecimal(LearningRate);
            numericUpDownHiddenLayerNeurons.Value = HiddenLayerNeurons;
            numericUpDownInputNeurons.Value = InputLayerNeurons;
            numericUpDownOutputNeurons.Value = OutputLayerNeurons;
            numericUpDownLearningRate.Value = NumLayers;
        }

        private void ResetToDefaultNetwork()
        {
            high = .99;
            low = .01;
            LearningRate = 0.9;
            HiddenLayerNeurons = 4;
            InputLayerNeurons = 2;
            OutputLayerNeurons = 2;
            NumLayers = 3;

            UpdateValues();
        }

        private void gradientButtonLoad_Click(object sender, EventArgs e)
        {
            FormLoadSave frmLoadSave = new FormLoadSave();
            frmLoadSave.MyParent = this;
            frmLoadSave.Text = "Load";
            frmLoadSave.gradientButtonLoadSave.Text = "Load";
            frmLoadSave.gradientButtonDelete.Visible = false;
            frmLoadSave.listBoxRecordings.Enabled = true;
            frmLoadSave.textBoxName.Enabled = false;
            frmLoadSave.EnumerateRecordings();
            frmLoadSave.labelBusy.Text = "Loading..";
            frmLoadSave.panelBusy.Visible = false;
            frmLoadSave.ShowDialog();
        }

        private void gradientPanel21_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gradientButtonSave_Click(object sender, EventArgs e)
        {
            if(neuralNetwork==null)
            {
                return;
            }
            FormLoadSave frmLoadSave = new FormLoadSave();
            frmLoadSave.MyParent = this;
            frmLoadSave.Text = "Save";
            frmLoadSave.gradientButtonLoadSave.Text = "Save";
            frmLoadSave.gradientButtonDelete.Visible = false;
            frmLoadSave.textBoxName.Enabled = true;
            frmLoadSave.textBoxDate.Text = string.Format("{0:dd/MM/yy H:mm}", DateTime.Now);
            frmLoadSave.textBoxNumberOfObjects.Text = neuralNetwork.TotalNeurons() + "(" + neuralNetwork.LayerCount + ")";
            frmLoadSave.textBoxCalcsDone.Text = neuralNetwork.LearningIterations.ToString();
            frmLoadSave.labelBusy.Text = "Saving..";
            frmLoadSave.panelBusy.Visible = false;
            frmLoadSave.EnumerateRecordings();
            frmLoadSave.ShowDialog();
        }


        private void LoadSettings()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Peter Popma\\Omnion");
            if (key != null)
            {
                compressRecordings = Convert.ToBoolean(key.GetValue("CompressRecordings", false));
                saveDir = (key.GetValue("SaveDir") == null ? "" : key.GetValue("SaveDir").ToString());
                if (saveDir.Length == 0)
                {
                    saveDir = Application.StartupPath + System.IO.Path.DirectorySeparatorChar + "Recordings";
                    System.IO.Directory.CreateDirectory(saveDir);       // create if not exists
                }
            }
            else     // default values
            {
                ResetToDefaults();
            }
        }

        private void SaveSettings()
        {
            // Create or get existing subkey
            RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Peter Popma\\Omnion");

            key.SetValue("SaveDir", SaveDir);
            key.SetValue("CompressRecordings", compressRecordings);
        }

        public void ResetToDefaults()
        {
            saveDir = Application.StartupPath + System.IO.Path.DirectorySeparatorChar + "Recordings";
            try
            {
                System.IO.Directory.CreateDirectory(saveDir);       // create if not exists
            }
            catch (System.UnauthorizedAccessException e)
            {
                Console.WriteLine("Exception caught: {0}", e);
            }
            compressRecordings = true;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void gradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void numericUpDownLearningRate_ValueChanged(object sender, EventArgs e)
        {
            LearningRate = Convert.ToDouble(numericUpDownLearningRate.Value);
        }

        private void numericUpDownHiddenLayerNeurons_ValueChanged(object sender, EventArgs e)
        {
            HiddenLayerNeurons = Convert.ToInt32(numericUpDownHiddenLayerNeurons.Value);
        }

        private void gradientButtonLearnXor_Click(object sender, EventArgs e)
        {
            InputLayerNeurons = 2;
            HiddenLayerNeurons = 2;
            OutputLayerNeurons = 3;

            NeuralNetwork = new NeuralNet.NeuralNetwork(LearningRate, new int[] { InputLayerNeurons, HiddenLayerNeurons, OutputLayerNeurons });

            double ll, lh, hl, hh;
            List<double> output = new List<double>();
            List<double> inputs = new List<double>();
            int count = 0;

            do
            {
                inputs.Clear();
                inputs.Add(high);
                inputs.Add(high);
                output.Clear();
                output.Add(low);
                output.Add(low);
                output.Add(low);
                NeuralNetwork.Train(inputs, output);
                hh = NeuralNetwork.OutputLayer().Neurons[0].Value;

                inputs.Clear();
                inputs.Add(high);
                inputs.Add(low);
                output.Clear();
                output.Add(high);
                output.Add(high);
                output.Add(high);
                NeuralNetwork.Train(inputs, output);
                hl = NeuralNetwork.OutputLayer().Neurons[0].Value;

                inputs.Clear();
                inputs.Add(low);
                inputs.Add(high);
                output.Clear();
                output.Add(high);
                output.Add(high);
                output.Add(high);
                NeuralNetwork.Train(inputs, output);
                lh = NeuralNetwork.OutputLayer().Neurons[0].Value;

                inputs.Clear();
                inputs.Add(low);
                inputs.Add(low);
                output.Clear();
                output.Add(low);
                output.Add(low);
                output.Add(low);
                NeuralNetwork.Train(inputs, output);
                ll = NeuralNetwork.OutputLayer().Neurons[0].Value;

                count++;
            }
            // really train this thing well...
            while (count<1000000 && (hh > .1
                || lh < .9
                || hl < .9
                || ll > .1));

            if (formNeuralLayout != null)
            {
                NetworkHelper.ToPictureBox(formNeuralLayout.pictureBoxLayout, NeuralNetwork);
            }
            labelTrainingIterations.Text = NeuralNetwork.LearningIterations.ToString();
        }

        private void gradientButtonLearnPrimes_Click(object sender, EventArgs e)
        {

        }

        private void gradientButtonTest_Click(object sender, EventArgs e)
        {
            if (NeuralNetwork == null)
                return;
            List<double> inputs = new List<double>();
            string[] values = textBoxTestInput.Text.Split(',');
            foreach (string value in values)
            {
                inputs.Add(Convert.ToDouble(value));
            }

            //TODO: normalize inputs, so that maximum = 1

            NeuralNetwork.Run(inputs);
            textBoxTestOutput.Text = "";
            bool isFirst = true;
            foreach (Neuron neuron in NeuralNetwork.OutputLayer().Neurons)
            {
                if (!isFirst)
                {
                    textBoxTestOutput.Text += ", ";
                }
                else
                {
                    isFirst = false;
                }
                textBoxTestOutput.Text += (neuron.Value);
                //TODO: reverse normalizing, so actual predicted values are shown
            }
        }

        private void gradientButtonSequence_Click(object sender, EventArgs e)
        {
            InputLayerNeurons = 3;
            HiddenLayerNeurons = 8;
            OutputLayerNeurons = 1;
            numericUpDownHiddenLayerNeurons.Value = HiddenLayerNeurons;
            NeuralNetwork = new NeuralNet.NeuralNetwork(LearningRate, new int[] { InputLayerNeurons, HiddenLayerNeurons, OutputLayerNeurons });

            for (int k = 0; k < 100000; k++)
            {
                CryptoRandom n = new CryptoRandom();
                // input 1 = 2, input 2  = 4, ..
                int startvalue = ((int)(n.RandomValue * (7- InputLayerNeurons)) + 1) * 2;
                List<double> input = new List<double>();
                for(int i=0; i< InputLayerNeurons; i++)
                {
                    input.Add((startvalue+i*2) / 20.0);
                }
                double output = (startvalue + 2* InputLayerNeurons) / 20.0;
                NeuralNetwork.Train(input, output);
            }

            if (formNeuralLayout != null)
            {
                NetworkHelper.ToPictureBox(formNeuralLayout.pictureBoxLayout, NeuralNetwork);
            }
            labelTrainingIterations.Text = NeuralNetwork.LearningIterations.ToString();
        }

        private List<double> TranslateToNeural(int value)
        {
            List<double> values = new List<double>();
            for(int k=2; k<18; k+=2)
            {
                if(value == k )
                {
                    values.Add(1);
                }
                else
                {
                    values.Add(0);
                }
            }
            return values;
        }

        private int TranslateFromNeural(List<double> values)
        {
            int k = 0;
            foreach (double value in values)
            {
                if (value> 0.8)
                {
                    k+=2;
                }
            }
            return k;
        }

        private void gradientButtonTrain_Click(object sender, EventArgs e)
        {
            NeuralNetwork = new NeuralNet.NeuralNetwork(LearningRate, new int[] { InputLayerNeurons, HiddenLayerNeurons, OutputLayerNeurons });

        }

        private void gradientButtonSequence2_Click(object sender, EventArgs e)
        {
            HiddenLayerNeurons = 8;
            numericUpDownHiddenLayerNeurons.Value = HiddenLayerNeurons;
            NeuralNetwork = new NeuralNet.NeuralNetwork(LearningRate, new int[] { 3, HiddenLayerNeurons, 1 });

            for (int k = 0; k < 100000; k++)
            {
                CryptoRandom n = new CryptoRandom();
                // input 1 = 2, input 2  = 4, ..
                int startvalue = ((int)(n.RandomValue * 4) + 1) * 2;
                List<double> input = new List<double>();
                input.Add((startvalue) / 20.0);
                input.Add((startvalue+2) / 20.0);
                input.Add((startvalue+4) / 20.0);
                double output = (startvalue + 6) / 20.0;
                NeuralNetwork.Train(input, output);
            }

            if (formNeuralLayout != null)
            {
                NetworkHelper.ToPictureBox(formNeuralLayout.pictureBoxLayout, NeuralNetwork);
            }
            labelTrainingIterations.Text = NeuralNetwork.LearningIterations.ToString();
        }

        private void numericUpDownInputNeurons_ValueChanged(object sender, EventArgs e)
        {
            InputLayerNeurons = Convert.ToInt32(numericUpDownInputNeurons.Value); 
        }

        private void gradientButtonPreferences_Click(object sender, EventArgs e)
        {
            FormPreferences frmPreferences = new FormPreferences();
            frmPreferences.MyParent = this;
            frmPreferences.Initialize();
            frmPreferences.ShowDialog();
        }

        private void gradientButtonMakeMove_Click(object sender, EventArgs e)
        {
            chessClient.PostMove(Convert.ToInt32(numericUpDownXfrom.Value), Convert.ToInt32(numericUpDownYfrom.Value), Convert.ToInt32(numericUpDownXto.Value), Convert.ToInt32(numericUpDownYto.Value));
        }

        private void gradientButtonGetChessBoard_Click(object sender, EventArgs e)
        {
            chessClient.GetChessboard();
        }

        private void gradientButton1_Click(object sender, EventArgs e)
        {
            chessClient.GetGameStatus();
            labelStatus.Text = "status: " + (chessClient.PlayingGame?"playing":"won") + " player: " + chessClient.ActivePlayer;
        }

        private void gradientButtonGetPossibleMoves_Click(object sender, EventArgs e)
        {
            chessClient.GetPossibleMoves(Convert.ToInt32(numericUpDownXPossibleMoves.Value), Convert.ToInt32(numericUpDownYPossibleMoves.Value));
            List<Point> moves = chessClient.PossibleMoves;
        }

        private void gradientButtonGetMoveablePositions_Click(object sender, EventArgs e)
        {
            chessClient.GetMoveablePositions();
            List<Point> moves = chessClient.MoveablePositions;
        }

        private void gradientButtonReset_Click(object sender, EventArgs e)
        {
            ResetToDefaultNetwork();
        }

        List<double> ChessboardToNeurons(ChessPiece[,] chessBoard)
        {
            List<double> neurons = new List<double>();
            for (int k = 0; k < inputLayerNeurons; k++)
            {
                neurons.Add(0);
            }
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    int squareNumber = x + 8 * y;
                    int value = (int)chessBoard[x, y].Type;
                    if (chessBoard[x, y].Color.Equals(ChessPieceColor.Black))
                    {
                        value += 6;
                    }
                    neurons[squareNumber + value] = 1;          // fill one of the 13 possible square values 
                }
            }

            return neurons;
        }

        List<double> ChessboardToNeurons(ChessPiece[,,] chessBoard, int currentArrayPosition)
        {
            List<double> neurons = new List<double>();
            for (int k = 0; k < inputLayerNeurons; k++)
            {
                neurons.Add(0);
            }
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    int squareNumber = x + 8*y;
                    int value = (int)chessBoard[x, y, currentArrayPosition].Type;
                    if(chessBoard[x, y, currentArrayPosition].Color.Equals(ChessPieceColor.Black))
                    {
                        value += 6;
                    }
                    neurons[squareNumber + value] = 1;          // fill one of the 13 possible square values 
                }
            }

            return neurons;
        }

        List<double> ChessmoveToNeurons(ChessMove move)
        {
            List<double> neurons = new List<double>();
            for (int k = 0; k < outputLayerNeurons; k++)
            {
                neurons.Add(0);
            }
            int moveNeuronNumber = move.XFrom + 8 * move.YFrom + 64 * move.XTo + 512 * move.YTo;
            if (chessClient.ActivePlayer.Equals(ChessPieceColor.Black))
            {
                moveNeuronNumber += 4096;           // All moves from Black player are in second part of neurons
            }
            neurons[moveNeuronNumber] = 1;

            return neurons;
        }

        private void TrainChessgame(/*List<ChessMove>[] possibleMovesHistory, */ ChessPiece[,,] chessBoardHistory, ChessMove[] chosenMoveHistory, int currentArrayPosition, int chessMovesHistorysize)
        {
            int movesToProcess = chessMovesHistorysize;
            currentArrayPosition--; // the previous move was the winning
            if (currentArrayPosition < 0)
            {
                currentArrayPosition += chessMovesHistorysize * 2;
            }
            while (movesToProcess > 0)
            {
//                for (int k = 0; k < movesToProcess; k++)        // the later moves are more important for checkmate, so train them more
                {
                    NeuralNetwork.Train(ChessboardToNeurons(chessBoardHistory, currentArrayPosition), ChessmoveToNeurons(chosenMoveHistory[currentArrayPosition]));
                }
                movesToProcess--;
                currentArrayPosition -= 2;
                if(currentArrayPosition<0)
                {
                    currentArrayPosition += chessMovesHistorysize * 2;
                }
            }
        }

        private void gradientButtonPlay_Click(object sender, EventArgs e)
        {
            if (neuralNetwork == null || neuralNetwork.HiddenNeurons!= 64 * 13)
            {
                // Here we want to use the combination of chessboard and all possible moves.
                // That gives an almost unlimited amount of possibilities
                // So, instead we use the maximum possible size of neurons and layers that we can work with
                // We then map those numbers to different combinations.
                InputLayerNeurons = HiddenLayerNeurons = 64 * 13;   // map to chessboard values
                OutputLayerNeurons = 64 * 64 * 2;   // map to chosen (best) moves (square-from, square-to, divided in best moves for both players)
                LearningRate = 0.9;
                NumLayers = 3;

                NeuralNetwork = new NeuralNet.NeuralNetwork(LearningRate, new int[] { InputLayerNeurons, HiddenLayerNeurons, OutputLayerNeurons }, NumLayers);
                UpdateValues();
            }

            // input: current chessboard and all possible moves -> we use only all possible moves
            // output: best move (from all possible moves)
            // learning condition: 
            // - CHECKMATE METHOD: as soon as the player is checkmate, the last 20 moves of the other player leading to that are fed to the training
            // - WINPIECE METHOD: 

            int gamesPlayed = 0;
            int turnsPlayed = 0;
            int checkMates = 0;
            int staleMates = 0;
            int maxTurnsToCheckmate = 0;
            ChessPieceColor myColor = ChessPieceColor.White;
            int chessMovesHistorysize = Convert.ToInt32(numericUpDownMovesHistorySize.Value);

//            List<ChessMove>[] possibleMovesHistory = new List<ChessMove>[chessMovesHistorysize * 2];
            ChessPiece[,,] chessBoardHistory = new ChessPiece[8, 8, chessMovesHistorysize*2];
            ChessMove[] chosenMoveHistory = new ChessMove[chessMovesHistorysize * 2];
            int currentArrayPosition = 0;

            while (gamesPlayed < numericUpDownChessGames.Value)
            {
                turnsPlayed++;
                labelTurnsPlayed.Text = turnsPlayed.ToString();
//                System.Threading.Thread.Sleep(100);
                chessClient.GetGameStatus();
                while (!chessClient.ActivePlayer.Equals(myColor) && chessClient.PlayingGame.Equals(true))       // wait until it's my turn
                {
                    System.Threading.Thread.Sleep(100);
                    chessClient.GetGameStatus();
                }

                if (chessClient.PlayingGame.Equals(false) || turnsPlayed>500)          // game finished
                {
                    if (chessClient.ActivePlayer.Equals(ChessPieceColor.None))
                    {
                        staleMates++;
                        labelStalemates.Text = staleMates.ToString();
                    }
                    if (chessClient.GameWon)
                    {
                        checkMates++;
                        if (maxTurnsToCheckmate<turnsPlayed)
                        {
                            maxTurnsToCheckmate = turnsPlayed;
                            labelMaxTurnsToCheckmate.Text = maxTurnsToCheckmate.ToString();
                        }
                        labelCheckmates.Text = checkMates.ToString();
                        Refresh();

                        // train
                        TrainChessgame(/*possibleMovesHistory,*/ chessBoardHistory, chosenMoveHistory, currentArrayPosition, chessMovesHistorysize);
                    }
                    turnsPlayed = 0;
                    gamesPlayed++;
                    labelGamesPlayed.Text = gamesPlayed.ToString();
                    chessClient.PostReset();
                    myColor = ChessPieceColor.White;
                }

                chessClient.GetChessboard();
                List<ChessMove> possibleMoves = chessClient.GetAllPossibleMoves();               
//                possibleMovesHistory[currentArrayPosition] = possibleMoves;
                
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        chessBoardHistory[x, y, currentArrayPosition] = chessClient.ChessBoard[x, y];
                    }
                }

                // pick a move
                ChessMove myMove = null;
                if (checkBoxAggressivePlay.Checked)
                {
                    foreach(ChessMove move in possibleMoves)
                    {
                        if (myColor.Equals(ChessPieceColor.White) && chessClient.ChessBoard[move.XTo, move.YTo].IsBlack ||
                            myColor.Equals(ChessPieceColor.Black) && chessClient.ChessBoard[move.XTo, move.YTo].IsWhite)
                        {
                            myMove = move;          // select a square to take a piece from opponent
                        }
                    }
                }
                if (myMove == null)
                {
                    myMove = possibleMoves[rnd.Next(possibleMoves.Count)];
                }
                chosenMoveHistory[currentArrayPosition] = myMove;
                chessClient.PostMove(myMove.XFrom, myMove.YFrom, myMove.XTo, myMove.YTo);

                currentArrayPosition++;
                if (currentArrayPosition == chessMovesHistorysize * 2)
                {
                    currentArrayPosition = 0;
                }

                if (myColor.Equals(ChessPieceColor.White))           // change player
                {
                    myColor = ChessPieceColor.Black;
                }
                else
                {
                    myColor = ChessPieceColor.White;
                }
            }
        }

        private void gradientButtonPresetChess_Click(object sender, EventArgs e)
        {
            // Here we want to use the combination of chessboard and all possible moves.
            // That gives an almost unlimited amount of possibilities
            // So, instead we use the maximum possible size of neurons and layers that we can work with
            // We then map those numbers to different combinations.
            InputLayerNeurons = OutputLayerNeurons = HiddenLayerNeurons = 512;   // map to possible moves (xfrom, yfrom, xto, yto)
            LearningRate = 0.9;
            NumLayers = 10;

            NeuralNetwork = new NeuralNet.NeuralNetwork(LearningRate, new int[] { InputLayerNeurons, HiddenLayerNeurons, OutputLayerNeurons }, NumLayers);
            UpdateValues();
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void gradientButtonGetAllPossibleMoves_Click(object sender, EventArgs e)
        {
            chessClient.GetMoveablePositions();
            List<Point> moves = chessClient.MoveablePositions;
        }

        private void gradientButtonMakeOneMove_Click(object sender, EventArgs e)
        {
            chessClient.GetGameStatus();
            chessClient.GetChessboard();
            List<ChessMove> possibleMoves = chessClient.GetAllPossibleMoves();
            neuralNetwork.Run(ChessboardToNeurons(chessClient.ChessBoard));
            double bestValue = double.MinValue;
            ChessMove bestMove = null;
            foreach (ChessMove move in possibleMoves)
            {
                int whiteMoveNeuronNumber = move.XFrom + 8 * move.YFrom + 64 * move.XTo + 512 * move.YTo;
                int blackMoveNeuronNumber = whiteMoveNeuronNumber + 4096;
                double value = 0;
                if (chessClient.ActivePlayer.Equals(ChessPieceColor.White))
                {
                    value = NeuralNetwork.OutputLayer().Neurons[whiteMoveNeuronNumber].Value - NeuralNetwork.OutputLayer().Neurons[blackMoveNeuronNumber].Value;
                }
                else
                {
                    value = NeuralNetwork.OutputLayer().Neurons[blackMoveNeuronNumber].Value - NeuralNetwork.OutputLayer().Neurons[whiteMoveNeuronNumber].Value;
                }

                if (value>bestValue)
                {
                    bestMove = move;
                }
            }
            chessClient.PostMove(bestMove);
        }

        private void Play(ChessPieceColor myColor)
        {
            ChessMove previousMove = null;
            ChessMove previousPreviousMove = null;
            chessClient.GetGameStatus();
            while (chessClient.PlayingGame.Equals(true))
            {
                chessClient.GetGameStatus();
                while (!chessClient.ActivePlayer.Equals(myColor) && chessClient.PlayingGame.Equals(true))       // wait until it's my turn
                {
                    System.Threading.Thread.Sleep(100);
                    chessClient.GetGameStatus();
                }

                if(chessClient.PlayingGame.Equals(false))
                {
                    return;
                }

                chessClient.GetChessboard();
                List<ChessMove> possibleMoves = chessClient.GetAllPossibleMoves();
                neuralNetwork.Run(ChessboardToNeurons(chessClient.ChessBoard));
                double bestValue = double.MinValue;
                ChessMove bestMove = null;
                ChessMove backupMove = null;
                foreach (ChessMove move in possibleMoves)
                {
                    int whiteMoveNeuronNumber = move.XFrom + 8 * move.YFrom + 64 * move.XTo + 512 * move.YTo;
                    int blackMoveNeuronNumber = whiteMoveNeuronNumber + 4096;
                    double value = 0;
                    if (chessClient.ActivePlayer.Equals(ChessPieceColor.White))
                    {
                        value = NeuralNetwork.OutputLayer().Neurons[whiteMoveNeuronNumber].Value - NeuralNetwork.OutputLayer().Neurons[blackMoveNeuronNumber].Value;
                    }
                    else
                    {
                        value = NeuralNetwork.OutputLayer().Neurons[blackMoveNeuronNumber].Value - NeuralNetwork.OutputLayer().Neurons[whiteMoveNeuronNumber].Value;
                    }

                    if (value > bestValue)
                    {
                        if (move.Equals(previousPreviousMove))
                        {
                            backupMove = move;          // this move was used two moves ago.. to prevent boring play we use this as a backup move
                        }
                        else
                        {
                            bestMove = move;
                        }
                    }
                }
                if (bestMove==null)
                {
                    bestMove = backupMove;
                }
                chessClient.PostMove(bestMove);
                previousPreviousMove = previousMove;
                previousMove = bestMove;
            }
        }

        private void checkBoxPlayWhite_CheckedChanged(object sender, EventArgs e)
        {
            Play(ChessPieceColor.White);
        }

        private void checkBoxPlayBlack_CheckedChanged(object sender, EventArgs e)
        {
            Play(ChessPieceColor.Black);
        }

        private void gradientButtonPlayWhite_Click(object sender, EventArgs e)
        {
            Play(ChessPieceColor.White);
        }

        private void gradientButtonPlayBlack_Click(object sender, EventArgs e)
        {
            Play(ChessPieceColor.Black);
        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void gradientButtonTrainOnPieces_Click(object sender, EventArgs e)
        {
 /*           if (neuralNetwork == null || neuralNetwork.HiddenNeurons != 64 * 13)
            {
                // Here we want to use the combination of chessboard and all possible moves.
                // That gives an almost unlimited amount of possibilities
                // So, instead we use the maximum possible size of neurons and layers that we can work with
                // We then map those numbers to different combinations.
                InputLayerNeurons = HiddenLayerNeurons = 64 * 13;   // map to chessboard values
                OutputLayerNeurons = 64 * 64 * 2;   // map to chosen (best) moves (square-from, square-to, divided in best moves for both players)
                LearningRate = 0.9;
                NumLayers = 3;

                NeuralNetwork = new NeuralNet.NeuralNetwork(LearningRate, new int[] { InputLayerNeurons, HiddenLayerNeurons, OutputLayerNeurons }, NumLayers);
                UpdateValues();
            }
            */

            // input: current chessboard and all possible moves -> we use only all possible moves
            // output: best move (from all possible moves)
            // learning condition: 
            // - CHECKMATE METHOD: as soon as the player is checkmate, the last 20 moves of the other player leading to that are fed to the training
            // - WINPIECE METHOD: 

            int gamesPlayed = 0;
            int turnsPlayed = 0;
            int checkMates = 0;
            int staleMates = 0;
            int maxTurnsToCheckmate = 0;
            ChessPieceColor myColor = ChessPieceColor.White;
            int chessMovesHistorysize = Convert.ToInt32(numericUpDownMovesHistorySize.Value);

            //            List<ChessMove>[] possibleMovesHistory = new List<ChessMove>[chessMovesHistorysize * 2];
            ChessPiece[,,] chessBoardHistory = new ChessPiece[8, 8, chessMovesHistorysize * 2];
            ChessMove[] chosenMoveHistory = new ChessMove[chessMovesHistorysize * 2];
            int currentArrayPosition = 0;
            int[] pointsWonHistory = new int[chessMovesHistorysize * 2];
            int moveResultBufferSize = 0;

            while (gamesPlayed < numericUpDownChessGames.Value)
            {
                turnsPlayed++;
                labelTurnsPlayed.Text = turnsPlayed.ToString();
                //                System.Threading.Thread.Sleep(100);
                chessClient.GetGameStatus();
                while (!chessClient.ActivePlayer.Equals(myColor) && chessClient.PlayingGame.Equals(true))       // wait until it's my turn
                {
                    System.Threading.Thread.Sleep(100);
                    chessClient.GetGameStatus();
                }

                if (chessClient.PlayingGame.Equals(false) || turnsPlayed > 500)          // game finished
                {
                    if (chessClient.ActivePlayer.Equals(ChessPieceColor.None))
                    {
                        staleMates++;
                        labelStalemates.Text = staleMates.ToString();
                    }
                    if (chessClient.GameWon)
                    {
                        checkMates++;
                        if (maxTurnsToCheckmate < turnsPlayed)
                        {
                            maxTurnsToCheckmate = turnsPlayed;
                            labelMaxTurnsToCheckmate.Text = maxTurnsToCheckmate.ToString();
                        }
                        labelCheckmates.Text = checkMates.ToString();
                    }
                    turnsPlayed = 0;
                    gamesPlayed++;
                    labelGamesPlayed.Text = gamesPlayed.ToString();
                    chessClient.PostReset();
                    myColor = ChessPieceColor.White;
                }

                chessClient.GetChessboard();
                List<ChessMove> possibleMoves = chessClient.GetAllPossibleMoves();
                //                possibleMovesHistory[currentArrayPosition] = possibleMoves;

                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        chessBoardHistory[x, y, currentArrayPosition] = chessClient.ChessBoard[x, y];
                    }
                }

                // pick a move
                ChessMove myMove = null;
                if (checkBoxAggressivePlay.Checked)
                {
                    foreach (ChessMove move in possibleMoves)
                    {
                        if (myColor.Equals(ChessPieceColor.White) && chessClient.ChessBoard[move.XTo, move.YTo].IsBlack ||
                            myColor.Equals(ChessPieceColor.Black) && chessClient.ChessBoard[move.XTo, move.YTo].IsWhite)
                        {
                            myMove = move;          // select a square to take a piece from opponent
                        }
                    }
                }
                if (myMove == null)
                {
                    myMove = possibleMoves[rnd.Next(possibleMoves.Count)];
                }
                moveResultBufferSize++;
                pointsWonHistory[currentArrayPosition] = 0;
                if (myColor.Equals(ChessPieceColor.White) && chessClient.ChessBoard[myMove.XTo, myMove.YTo].IsBlack ||
                    myColor.Equals(ChessPieceColor.Black) && chessClient.ChessBoard[myMove.XTo, myMove.YTo].IsWhite)
                {
                    if (chessClient.ChessBoard[myMove.XTo, myMove.YTo].Type.Equals(ChessPieceType.Pawn))
                    {
                        pointsWonHistory[currentArrayPosition] = 1;
                    }
                    if (chessClient.ChessBoard[myMove.XTo, myMove.YTo].Type.Equals(ChessPieceType.Bishop))
                    {
                        pointsWonHistory[currentArrayPosition] = 3;
                    }
                    if (chessClient.ChessBoard[myMove.XTo, myMove.YTo].Type.Equals(ChessPieceType.Horse))
                    {
                        pointsWonHistory[currentArrayPosition] = 4;
                    }
                    if (chessClient.ChessBoard[myMove.XTo, myMove.YTo].Type.Equals(ChessPieceType.Rook))
                    {
                        pointsWonHistory[currentArrayPosition] = 5;
                    }
                    if (chessClient.ChessBoard[myMove.XTo, myMove.YTo].Type.Equals(ChessPieceType.Queen))
                    {
                        pointsWonHistory[currentArrayPosition] = 8;
                    }
                }
                chosenMoveHistory[currentArrayPosition] = myMove;
                chessClient.PostMove(myMove.XFrom, myMove.YFrom, myMove.XTo, myMove.YTo);

                if ((turnsPlayed>chessMovesHistorysize*2) && pointsWonHistory[currentArrayPosition] > 0)
                {
                    int myArrayPosition = currentArrayPosition - 1;
                    if (myArrayPosition < 0)
                    {
                        myArrayPosition = (chessMovesHistorysize * 2) - 1;
                    }
                    int netScore = pointsWonHistory[currentArrayPosition];
                    bool myTurn = true;
                    while (myArrayPosition != currentArrayPosition)     // go through the previous moves and calculate if there was profit
                    {
                        myTurn = !myTurn;
                        if(myTurn)
                        {
                            netScore += pointsWonHistory[myArrayPosition];
                        }
                        else
                        {
                            netScore -= pointsWonHistory[myArrayPosition];
                        }
                        myArrayPosition--;
                        if(myArrayPosition<0)
                        {
                            myArrayPosition = (chessMovesHistorysize * 2) - 1;
                        }
                    }
                    if (netScore > 0)
                    {
                        Refresh();
                        TrainChessgame(/*possibleMovesHistory,*/ chessBoardHistory, chosenMoveHistory, currentArrayPosition, chessMovesHistorysize);
                    }
                }

                currentArrayPosition++;
                if (currentArrayPosition == chessMovesHistorysize * 2)
                {
                    currentArrayPosition = 0;
                }

                if (myColor.Equals(ChessPieceColor.White))           // change player
                {
                    myColor = ChessPieceColor.Black;
                }
                else
                {
                    myColor = ChessPieceColor.White;
                }
            }
        }

        private void gradientButtonShowLayout_Click(object sender, EventArgs e)
        {
            formNeuralLayout = new FormNeuralLayout();
            formNeuralLayout.MyParent = this;
            if (NeuralNetwork != null)
            {
                NetworkHelper.ToPictureBox(formNeuralLayout.pictureBoxLayout, NeuralNetwork);
            }

            formNeuralLayout.Show();
        }

    }
}
