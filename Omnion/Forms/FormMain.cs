using Microsoft.Win32;
using Omnion.ChessClient;
using Omnion.ChessCode;
using Omnion.NeuralNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        double scaleFactor;
        bool trainingActive = false;
        bool trainingPaused = false;
        bool trainingOneStep = false;
        TimeSpan runningTime;

        Stopwatch stopWatch;

        ChessPieceColor currentChessplayer = ChessPieceColor.White;

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

        public void UpdateValues()
        {
            numericUpDownLearningRate.Value = Convert.ToDecimal(LearningRate);
            numericUpDownHiddenLayerNeurons.Value = HiddenLayerNeurons;
            numericUpDownInputNeurons.Value = InputLayerNeurons;
            numericUpDownOutputNeurons.Value = OutputLayerNeurons;
            numericUpDownNumberOfLayers.Value = NumLayers;
            numericUpDownScaleFactor.Value = Convert.ToDecimal(scaleFactor);
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

        private void numericUpDownLearningRate_ValueChanged(object sender, EventArgs e)
        {
            LearningRate = Convert.ToDouble(numericUpDownLearningRate.Value);
            neuralNetwork.LearningRate = LearningRate;
        }

        private void numericUpDownHiddenLayerNeurons_ValueChanged(object sender, EventArgs e)
        {
            HiddenLayerNeurons = Convert.ToInt32(numericUpDownHiddenLayerNeurons.Value);
        }

        private void gradientButtonLearnXor_Click(object sender, EventArgs e)
        {
            InputLayerNeurons = 2;
            HiddenLayerNeurons = 2;
            OutputLayerNeurons = 1;
            UpdateValues();

            NeuralNetwork = new NeuralNet.NeuralNetwork(LearningRate, new int[] { InputLayerNeurons, HiddenLayerNeurons, OutputLayerNeurons });
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            double ll, lh, hl, hh;
            List<double> output = new List<double>();
            List<double> inputs = new List<double>();
            long count = 0;

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
                runningTime = stopWatch.Elapsed;
                DisplayNeuralNetworkStats();
            }
            // really train this thing well...
            while (count<10000000 && (hh > .02
                || lh < .98
                || hl < .98
                || ll > .02));

            if (formNeuralLayout != null)
            {
                NetworkHelper.ToPictureBox(formNeuralLayout.pictureBoxLayout, NeuralNetwork);
            }
            stopWatch.Stop();
        }

        private void DisplayNeuralNetworkStats()
        {
            labelNetworkQuality.Text = NeuralNetwork.MeasureNetworkQuality().ToString();
            labelTrainingIterations.Text = NeuralNetwork.LearningIterations.ToString();
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            NeuralNetwork.TrainingTime.Hours, NeuralNetwork.TrainingTime.Minutes, NeuralNetwork.TrainingTime.Seconds,
            NeuralNetwork.TrainingTime.Milliseconds / 10);
            labelTrainingTime.Text = elapsedTime;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            runningTime.Hours, runningTime.Minutes, runningTime.Seconds,
            runningTime.Milliseconds / 10);
            labelRunningTime.Text = elapsedTime;
            labelTrainigRatio.Text = String.Format("{0:0.00}", NeuralNetwork.TrainingTime.Ticks / (double)runningTime.Ticks);
        }


        private void gradientButtonTest_Click(object sender, EventArgs e)
        {
            if (NeuralNetwork == null)
                return;
            List<double> inputs = new List<double>();
            string[] values = textBoxTestInput.Text.Split(',');
            foreach (string value in values)
            {
                // normalize inputs, so that maximum = 1
                inputs.Add(Convert.ToDouble(value) / scaleFactor);
            }
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
                // reverse normalizing, so actual predicted values are shown
                textBoxTestOutput.Text += (neuron.Value * scaleFactor);
            }
        }

        private void DisplayTestValues(List<double> inputs, double output)
        {
            List<double> outputs = new List<double>();
            outputs.Add(output);

            DisplayTestValues(inputs, outputs);
        }

        private void DisplayTestValues(List<double> inputs, List<double> outputs)
        {
            string inputText = "", outputText = "";
            bool isFirst = true;
            foreach(double input in inputs)
            {
                if(isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    inputText += ",";
                }
                inputText += input*scaleFactor;
            }
            isFirst = true;
            foreach (double output in outputs)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    outputText += ",";
                }
                outputText += output * scaleFactor;
            }
            textBoxTrainingInputValues.Text = inputText;
            textBoxTrainingOutputValues.Text = outputText;
        }

        private void gradientButtonSequence_Click(object sender, EventArgs e)
        {
            InputLayerNeurons = 3;
            HiddenLayerNeurons = 8;
            OutputLayerNeurons = 1;
            scaleFactor = 20;
            UpdateValues();
            numericUpDownHiddenLayerNeurons.Value = HiddenLayerNeurons;
            NeuralNetwork = new NeuralNet.NeuralNetwork(LearningRate, new int[] { InputLayerNeurons, HiddenLayerNeurons, OutputLayerNeurons });
            trainingActive = true;

            for (int k = 0; k < 10000; k++)
            {
                CryptoRandom n = new CryptoRandom();
                // input 1 = 2, input 2  = 4, ..
                int startvalue = ((int)(n.RandomValue * (7-InputLayerNeurons)) + 1) * 2;

                List<double> input = new List<double>();
                for(int i=0; i< InputLayerNeurons; i++)
                {
                    input.Add((startvalue+i*2) / scaleFactor);
                }
                if (!trainingActive)     // abort
                {
                    break;
                }
                double output = (startvalue + 2* InputLayerNeurons) / scaleFactor;
                NeuralNetwork.Train(input, output);
                DisplayTestValues(input, output);
                DisplayNeuralNetworkStats();
                if (trainingOneStep)
                {
                    trainingOneStep = false;
                }
                while (trainingPaused && !trainingOneStep)
                {
                    Application.DoEvents();
                }
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

        List<double> ChessboardToNeurons(ChessPiece[][] chessBoard)
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
                    int value = (int)chessBoard[x][y].Type;
                    if (chessBoard[x][y].Color.Equals(ChessPieceColor.Black))
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
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    int squareNumber = x + 8*y;
                    int value = 7 - (int)chessBoard[x, y, currentArrayPosition].Type;      // own king: 12, own queen: 11, neutral: 7, other pawn: 6, other king: 0
                    if(chessBoard[x, y, currentArrayPosition].Color.Equals(currentChessplayer))
                    {
                        value = (int)chessBoard[x, y, currentArrayPosition].Type + 7;
                    }
                    neurons.Add(value / 12.0d);
                }
            }

            return neurons;
        }

        List<double> ChessmoveToNeurons(ChessMove move)
        {
            List<double> neurons = new List<double>();
            int fromNeuronNumber = move.XFrom + 8 * move.YFrom;
            int toNeuronNumber = 64 + move.XTo + 8 * move.YTo;
            for (int k = 0; k < outputLayerNeurons; k++)
            {
                if(k==fromNeuronNumber || (k == toNeuronNumber))
                {
                    neurons.Add(1);
                }
                {
                    neurons.Add(0);
                }
            }

            return neurons;
        }

        private void TrainChessgame(ChessPiece[,,] chessBoardHistory, ChessMove[] chosenMoveHistory, int currentArrayPosition)
        {
            int movesToProcess = chosenMoveHistory.Length / 2;
            if (currentArrayPosition < 0)
            {
                currentArrayPosition += chosenMoveHistory.Length;
            }
            while (movesToProcess > 0)
            {
                NeuralNetwork.Train(ChessboardToNeurons(chessBoardHistory, currentArrayPosition), ChessmoveToNeurons(chosenMoveHistory[currentArrayPosition]));
                movesToProcess--;
                if (checkBoxTrainLastMovesMore.Checked)
                {
                    for (int k = 0; k < movesToProcess; k++)        // the later moves are more important, so train them more
                    {
                        NeuralNetwork.Train(ChessboardToNeurons(chessBoardHistory, currentArrayPosition), ChessmoveToNeurons(chosenMoveHistory[currentArrayPosition]));
                    }
                }
                currentArrayPosition -= 2;
                if(currentArrayPosition<0)
                {
                    currentArrayPosition += chosenMoveHistory.Length;
                }
            }

            DisplayNeuralNetworkStats();
        }

        private void gradientButtonPlay_Click(object sender, EventArgs e)
        {
            ChessGame chessGame = new ChessGame();
            chessGame.InitGame();

            trainingActive = true;

            if (neuralNetwork == null || (neuralNetwork.HiddenNeurons!=64 && OutputLayerNeurons!=128))
            {
                stopWatch = new Stopwatch();
                CreateChessNetwork();
            }

            // input: chessboard + current player
            // output: best move (from all possible moves)
            // learning condition: 
            // - CHECKMATE METHOD: as soon as the player is checkmate, the last 20 moves of the other player leading to that are fed to the training
            // - WINPIECE METHOD: 

            int gamesPlayed = 0;
            int turnsPlayed = 0;
            int checkMates = 0;
            int staleMates = 0;
            int maxTurnsToCheckmate = 0;
            currentChessplayer = ChessPieceColor.White;
            int chessMovesHistorysize = Convert.ToInt32(numericUpDownMovesHistorySize.Value) * 2;

            ChessPiece[,,] chessBoardHistory = new ChessPiece[8, 8, chessMovesHistorysize];
            ChessMove[] chosenMoveHistory = new ChessMove[chessMovesHistorysize];
            int currentArrayPosition = 0;
            int numMoveResults = 0;
            int netPointsWon = 0;

            labelCheckmates.Text = checkMates.ToString();
            labelStalemates.Text = staleMates.ToString();
            labelGamesPlayed.Text = gamesPlayed.ToString();
            labelMaxTurnsToCheckmate.Text = maxTurnsToCheckmate.ToString();
            labelNetworkQuality.Text = NeuralNetwork.MeasureNetworkQuality().ToString();

            stopWatch.Start();
            while (trainingActive && gamesPlayed < numericUpDownChessGames.Value)
            {
                turnsPlayed++;
                labelTurnsPlayed.Text = turnsPlayed.ToString();
                labelTrainingIterations.Text = NeuralNetwork.LearningIterations.ToString();

//                chessClient.GetGameStatus();
                /*
                while (!chessClient.ActivePlayer.Equals(myColor) && chessClient.PlayingGame.Equals(true))       // wait until it's my turn
                {
                    System.Threading.Thread.Sleep(100);
                    chessClient.GetGameStatus();
                }*/

                if (/*chessClient*/chessGame.PlayingGame.Equals(false) || turnsPlayed>500)          // game finished
                {
                    if (/*chessClient*/chessGame.ActivePlayer.Equals(ChessPieceColor.None))
                    {
                        staleMates++;
                        labelStalemates.Text = staleMates.ToString();
                    }
                    if (/*chessClient.GameWon*/!chessGame.PlayerWon.Equals(ChessPieceColor.None))
                    {
                        checkMates++;
                        if (maxTurnsToCheckmate<turnsPlayed)
                        {
                            maxTurnsToCheckmate = turnsPlayed;
                            labelMaxTurnsToCheckmate.Text = maxTurnsToCheckmate.ToString();
                        }
                        labelCheckmates.Text = checkMates.ToString();

                        // train
                        if(checkBoxTrainCheckmate.Checked)
                        {
                            Refresh();
                            runningTime = stopWatch.Elapsed;
                            TrainChessgame(chessBoardHistory, chosenMoveHistory, currentArrayPosition - 1);     // -1 because the previous move was the winning
                        }
                    }
                    turnsPlayed = 0;
                    gamesPlayed++;
                    labelGamesPlayed.Text = gamesPlayed.ToString();
                    if(checkBoxRest.Checked)
                    {
                        chessClient.PostReset();
                    }
                    chessGame.InitGame();
                    currentChessplayer = ChessPieceColor.White;
                }

                //chessClient.GetChessboard();
                List<ChessMove> possibleMoves = /*chessClient*/chessGame.FindAllPossibleMoves();               
                
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        chessBoardHistory[x, y, currentArrayPosition] = /*chessClient*/chessGame.ChessBoard[x][y];
                    }
                }

                // pick a move
                ChessMove myMove = null;
                if (checkBoxAggressivePlay.Checked)
                {
                    List<ChessMove> piecetakingMoves = new List<ChessMove>();

                    // find all the move where we can take a piece
                    foreach (ChessMove move in possibleMoves)
                    {
                        if (currentChessplayer.Equals(ChessPieceColor.White) && /*chessClient*/chessGame.ChessBoard[move.XTo][move.YTo].IsBlack ||
                            currentChessplayer.Equals(ChessPieceColor.Black) && /*chessClient*/chessGame.ChessBoard[move.XTo][move.YTo].IsWhite)
                        {
                            int thisMove = MoveToPoints(move, chessGame);
                            if (MoveToPoints(move, chessGame) > 0)
                            {
                                piecetakingMoves.Add(move);
                            }
                        }
                    }
                    if (piecetakingMoves.Count>0)
                    {
                        CryptoRandom rand = new CryptoRandom();
                        myMove = piecetakingMoves[rand.NextInt(piecetakingMoves.Count)];
                    }
                }
                if (myMove == null)
                {
                    CryptoRandom rand = new CryptoRandom();
                    myMove = possibleMoves[rand.NextInt(possibleMoves.Count)];
                }
                chosenMoveHistory[currentArrayPosition] = myMove;

                if (checkBoxTrainPieces.Checked)
                {
                    if (currentChessplayer.Equals(ChessPieceColor.White))
                    {
                        netPointsWon += MoveToPoints(myMove, chessGame);
                    }
                    else
                    {
                        netPointsWon -= MoveToPoints(myMove, chessGame);
                    }

                    numMoveResults++;
                    if (numMoveResults == chessMovesHistorysize)        // time to check for white training
                    {
                        if (netPointsWon > 0)       // white (first player) made profit
                        {
                            Refresh();
                            runningTime = stopWatch.Elapsed;
                            TrainChessgame(chessBoardHistory, chosenMoveHistory, currentArrayPosition - 1);
                            numMoveResults = 0;
                            netPointsWon = 0;
                        }
                    }
                    if (numMoveResults == chessMovesHistorysize + 1)        // time to check for black training
                    {
                        if (netPointsWon < 0)       // black made profit
                        {
                            Refresh();
                            runningTime = stopWatch.Elapsed;
                            TrainChessgame(chessBoardHistory, chosenMoveHistory, currentArrayPosition - 1);
                            numMoveResults = -1;        // minus 1 because we start on white
                            netPointsWon = 0;
                        }
                        else
                        {
                            numMoveResults -= 2;    // nobody made profit yet; keep trying last white and black move
                        }
                    }
                }

                if (checkBoxRest.Checked)
                {
                    chessClient.PostMove(myMove.XFrom, myMove.YFrom, myMove.XTo, myMove.YTo);
                }
                chessGame.MakeMove(myMove.XFrom, myMove.YFrom, myMove.XTo, myMove.YTo);

                currentArrayPosition++;
                if (currentArrayPosition == chessMovesHistorysize)
                {
                    currentArrayPosition = 0;
                }

                if (currentChessplayer.Equals(ChessPieceColor.White))           // change player
                {
                    currentChessplayer = ChessPieceColor.Black;
                }
                else
                {
                    currentChessplayer = ChessPieceColor.White;
                }
            }
        }


        private void gradientButtonGetAllPossibleMoves_Click(object sender, EventArgs e)
        {
            chessClient.GetMoveablePositions();
            List<Point> moves = chessClient.MoveablePositions;
            labelAllPossibleMoves.Text = moves.Count.ToString();
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

        private void gradientButtonPlayWhite_Click(object sender, EventArgs e)
        {
            Play(ChessPieceColor.White);
        }

        private void gradientButtonPlayBlack_Click(object sender, EventArgs e)
        {
            Play(ChessPieceColor.Black);
        }

        private void CreateChessNetwork()
        {
            // Here we want to use the combination of chessboard and all possible moves.
            // That gives an almost unlimited amount of possibilities
            // So, instead we use the maximum possible size of neurons and layers that we can work with
            // We then map those numbers to different combinations.
            InputLayerNeurons = HiddenLayerNeurons = 64;   // map to chessboard values
            OutputLayerNeurons = 64 + 64;   // map to chosen (best) moves (square-from, square-to)
            LearningRate = 10.0;
            NumLayers = 3;

            NeuralNetwork = new NeuralNet.NeuralNetwork(LearningRate, new int[] { InputLayerNeurons, HiddenLayerNeurons, OutputLayerNeurons }, NumLayers);
            UpdateValues();
            MessageBox.Show("Created chess network.");
        }

        private int MoveToPoints(ChessMove move, ChessGame chessGame)
        {
            if (/*chessClient*/chessGame.ChessBoard[move.XTo][move.YTo].Type.Equals(ChessPieceType.Pawn))
            {
                return 1;
            }
            if (/*chessClient*/chessGame.ChessBoard[move.XTo][move.YTo].Type.Equals(ChessPieceType.Bishop))
            {
                return 3;
            }
            if (/*chessClient*/chessGame.ChessBoard[move.XTo][move.YTo].Type.Equals(ChessPieceType.Horse))
            {
                return 4;
            }
            if (/*chessClient*/chessGame.ChessBoard[move.XTo][move.YTo].Type.Equals(ChessPieceType.Rook))
            {
                return 5;
            }
            if (/*chessClient*/chessGame.ChessBoard[move.XTo][move.YTo].Type.Equals(ChessPieceType.Queen))
            {
                return 8;
            }

            return 0;
        }

        private void gradientButtonAbortTraining_Click(object sender, EventArgs e)
        {
            trainingActive = false;
        }

        private void gradientButtonLearnBell_Click(object sender, EventArgs e)
        {
            // Each layer represents a part of a sequence of values feeded to both detectors
            // each neuron represent an angle (degrees) of 1 of the detectors

            // Input: angle of A and B
            // Output: sequence of values for B (where A=0)
            // Learning condition: B resembles squared cosine of angle A-B
            InputLayerNeurons = 2;
            HiddenLayerNeurons = 2;
            OutputLayerNeurons = 1;
            NumLayers = 100;
            UpdateValues();

            NeuralNetwork = new NeuralNet.NeuralNetwork(LearningRate, new int[] { InputLayerNeurons, HiddenLayerNeurons, OutputLayerNeurons });
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            double ll, lh, hl, hh;
            List<double> output = new List<double>();
            List<double> inputs = new List<double>();
            long count = 0;

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
                runningTime = stopWatch.Elapsed;
                DisplayNeuralNetworkStats();
            }
            // really train this thing well...
            while (count < 10000000 && (hh > .02
                || lh < .98
                || hl < .98
                || ll > .02));

            if (formNeuralLayout != null)
            {
                NetworkHelper.ToPictureBox(formNeuralLayout.pictureBoxLayout, NeuralNetwork);
            }
            stopWatch.Stop();
        }

        private void numericUpDownNumberOfLayers_ValueChanged(object sender, EventArgs e)
        {

        }

        private void gradientButton3_Click(object sender, EventArgs e)
        {
            trainingPaused = !trainingPaused;
            if(trainingPaused)
            {
                gradientButtonPauseTraining.Text = "Continue training";
            }
            else
            {
                gradientButtonPauseTraining.Text = "Pause training";
            }
        }

        private void gradientButtonOnStepTraining_Click(object sender, EventArgs e)
        {
            trainingPaused = true;
            gradientButtonPauseTraining.Text = "Continue training";
            trainingOneStep = true;
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
