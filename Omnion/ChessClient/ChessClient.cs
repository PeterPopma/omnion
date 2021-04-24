using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static Omnion.ChessClient.ChessPiece;

namespace Omnion.ChessClient
{
    class ChessClient
    {
        static HttpClient client = new HttpClient();
        private static ChessPiece[][] chessBoard;
        ChessPieceColor activePlayer;
        bool playingGame;
        bool gameWon;
        List<Point> possibleMoves;
        List<Point> moveablePositions;
        List<ChessMove> allPossibleMoves;

        public ChessClient()
        {
            client.BaseAddress = new Uri("http://localhost:8000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));
        }

        public ChessPiece[][] ChessBoard { get => chessBoard; set => chessBoard = value; }
        public ChessPieceColor ActivePlayer { get => activePlayer; set => activePlayer = value; }
        public bool PlayingGame { get => playingGame; set => playingGame = value; }
        public List<Point> PossibleMoves { get => possibleMoves; set => possibleMoves = value; }
        public List<Point> MoveablePositions { get => moveablePositions; set => moveablePositions = value; }
        public bool GameWon { get => gameWon; set => gameWon = value; }
        internal List<ChessMove> AllPossibleMoves { get => allPossibleMoves; set => allPossibleMoves = value; }

        public void GetChessboard()
        {
            HttpResponseMessage response = null;
            try
            {
                response = client.GetAsync("GetChessboard").GetAwaiter().GetResult();
            } catch(HttpRequestException)
            {
                MessageBox.Show("Cannot connect to Chess program");
                return;
            }

            if (response.IsSuccessStatusCode)
            {
                string chessboardString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(chessboardString);
                chessBoard = new ChessPiece[8][];
                for (int i = 0; i < 8; i++)
                {
                    chessBoard[i] = new ChessPiece[8];
                }
                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    int x = Convert.ToInt32(node.Attributes["x"].Value);
                    int y = Convert.ToInt32(node.Attributes["y"].Value);
                    ChessPieceColor color = ChessPieceColor.None;
                    ChessPieceType type = ChessPieceType.None;
                    foreach(XmlNode childNode in node.ChildNodes)
                    {
                        if(childNode.Name.Equals("Color"))
                        {
                            if(childNode.InnerText.Equals("white"))
                            {
                                color = ChessPieceColor.White;
                            }
                            if (childNode.InnerText.Equals("black"))
                            {
                                color = ChessPieceColor.Black;
                            }
                            if (childNode.InnerText.Equals("none"))
                            {
                                color = ChessPieceColor.None;
                            }
                        }
                        if (childNode.Name.Equals("PieceType"))
                        {
                            if (childNode.InnerText.Equals("Pawn"))
                            {
                                type = ChessPieceType.Pawn;
                            }
                            if (childNode.InnerText.Equals("Rook"))
                            {
                                type = ChessPieceType.Rook;
                            }
                            if (childNode.InnerText.Equals("Bishop"))
                            {
                                type = ChessPieceType.Bishop;
                            }
                            if (childNode.InnerText.Equals("Horse"))
                            {
                                type = ChessPieceType.Horse;
                            }
                            if (childNode.InnerText.Equals("Queen"))
                            {
                                type = ChessPieceType.Queen;
                            }
                            if (childNode.InnerText.Equals("King"))
                            {
                                type = ChessPieceType.King;
                            }
                            if (childNode.InnerText.Equals("None"))
                            {
                                type = ChessPieceType.None;
                            }
                        }
                    }
                    ChessPiece piece = new ChessPiece(type, color);
                    chessBoard[x][y] = piece;
                }
            }
        }
        public void GetMoveablePositions()
        {
            HttpResponseMessage response = null;
            try
            {
                response = client.GetAsync("GetMoveablePositions").GetAwaiter().GetResult();
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Cannot connect to Chess program");
                return;
            }

            if (response.IsSuccessStatusCode)
            {
                string moveablePositionsString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(moveablePositionsString);
                moveablePositions = new List<Point>();
                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    Point point = new Point(Convert.ToInt32(node.Attributes["x"].Value), Convert.ToInt32(node.Attributes["y"].Value));
                    moveablePositions.Add(point);
                }
            }
        }

        public List<ChessMove> GetAllPossibleMoves()
        {
            allPossibleMoves = new List<ChessMove>();
            HttpResponseMessage response = null;
            try
            {
                response = client.GetAsync("GetAllPossibleMoves").GetAwaiter().GetResult();
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Cannot connect to Chess program");
                return allPossibleMoves;
            }

            if (response.IsSuccessStatusCode)
            {
                string possibleMovesString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(possibleMovesString);
                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    ChessMove move = new ChessMove(Convert.ToInt32(node.Attributes["x_from"].Value), Convert.ToInt32(node.Attributes["y_from"].Value), Convert.ToInt32(node.Attributes["x_to"].Value), Convert.ToInt32(node.Attributes["y_to"].Value));
                    allPossibleMoves.Add(move);
                }
            }

            return allPossibleMoves;
        }

        public void GetPossibleMoves(int x, int y)
        {
            HttpResponseMessage response = null;
            try
            {
                response = client.GetAsync("GetPossibleMoves?x="+x+"&y="+y).GetAwaiter().GetResult();
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Cannot connect to Chess program");
                return;
            }

            if (response.IsSuccessStatusCode)
            {
                string possibleMovesString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(possibleMovesString);
                possibleMoves = new List<Point>();
                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    Point point = new Point(Convert.ToInt32(node.Attributes["x"].Value), Convert.ToInt32(node.Attributes["y"].Value));
                    possibleMoves.Add(point);
                }
            }
        }

        public void GetGameStatus()
        {
            HttpResponseMessage response = null;
            try
            {
                response = client.GetAsync("GetGameStatus").GetAwaiter().GetResult();
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Cannot connect to Chess program");
                return;
            }

            if (response.IsSuccessStatusCode)
            {
                string statusString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(statusString);
                string status = doc.DocumentElement.Attributes["status"].Value;
                string player = doc.DocumentElement.Attributes["player"].Value;
                GameWon = false;
                PlayingGame = true;
                ActivePlayer = ChessPieceColor.White;
                if (player.Equals("black"))
                {
                    ActivePlayer = ChessPieceColor.Black;
                }
                if (player.Equals("none"))
                {
                    ActivePlayer = ChessPieceColor.None;
                }
                if (!status.Equals("playing"))
                {
                    PlayingGame = false;
                }
                if (status.Equals("won"))
                {
                    GameWon = true;
                }
            }
        }
        /*
                public async Task<bool> PostMove(int xfrom, int yfrom, int xto, int yto)
                {
                    HttpResponseMessage response = await client.PostAsync("Move?xfrom=" + xfrom + "&yfrom=" + yfrom + "&xto=" + xto + "&yto=" + yto, null);
                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();

                    return true;
                }
        */
        public void PostMove(ChessMove move)
        {
            PostMove(move.XFrom, move.YFrom, move.XTo, move.YTo);
        }

        public void PostMove(int xfrom, int yfrom, int xto, int yto)
        {
            HttpResponseMessage response = null;
            try
            {
                response = client.PostAsync("Move?xfrom=" + xfrom + "&yfrom=" + yfrom + "&xto=" + xto + "&yto=" + yto, null).GetAwaiter().GetResult();
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Cannot connect to Chess program");
                return;
            }
        }

        public void PostReset()
        {
            HttpResponseMessage response = null;
            try
            {
                response = client.PostAsync("Reset", null).GetAwaiter().GetResult();
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Cannot connect to Chess program");
                return;
            }
        }
    }
}
