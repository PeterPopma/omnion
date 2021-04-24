using Omnion.ChessClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Omnion.ChessClient.ChessPiece;

namespace Omnion.ChessCode
{
    class ChessGame
    {
        ChessboardPosition squarePosition = new ChessboardPosition();
        ChessboardPosition squareSelected = new ChessboardPosition();
        ChessboardPosition squarePossible = new ChessboardPosition();
        ChessboardPosition previousPositionFrom;
        ChessboardPosition previousPositionTo;
        ChessPiece previousPieceFrom;
        ChessPiece previousPieceTo;
        List<ChessboardPosition> possibleMoves = new List<ChessboardPosition>();
        ChessPiece[][] chessBoard = new ChessPiece[8][];
        List<ChessPiece> missingPieces = new List<ChessPiece>();
        bool activePlayer;
        const bool PlayerWhite = true;
        const bool PlayerBlack = false;
        const int ChessSquareSize = 120;
        const int ChessboardStartX = 29;
        const int ChessboardStartY = 29;
        ChessPieceColor playerWon;
        bool playingGame;
        int numMoves;

        public ChessPiece[][] ChessBoard { get => chessBoard; set => chessBoard = value; }
        public ChessPieceColor PlayerWon { get => playerWon; set => playerWon = value; }
        public bool PlayingGame { get => playingGame; set => playingGame = value; }
        public bool ActivePlayer { get => activePlayer; set => activePlayer = value; }

        void ClearChessboard(ChessPiece[][] a)
        {
            for (int i = 0; i < 8; i++)
            {
                a[i] = new ChessPiece[8];
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    a[i][j] = new ChessPiece();
                }
            }
        }

        public void InitGame()
        {
            ClearChessboard(ChessBoard);
            ChessBoard[0][0] = new ChessPiece(ChessPieceType.Rook, ChessPieceColor.Black);
            ChessBoard[1][0] = new ChessPiece(ChessPieceType.Horse, ChessPieceColor.Black);
            ChessBoard[2][0] = new ChessPiece(ChessPieceType.Bishop, ChessPieceColor.Black);
            ChessBoard[3][0] = new ChessPiece(ChessPieceType.Queen, ChessPieceColor.Black);
            ChessBoard[4][0] = new ChessPiece(ChessPieceType.King, ChessPieceColor.Black);
            ChessBoard[5][0] = new ChessPiece(ChessPieceType.Bishop, ChessPieceColor.Black);
            ChessBoard[6][0] = new ChessPiece(ChessPieceType.Horse, ChessPieceColor.Black);
            ChessBoard[7][0] = new ChessPiece(ChessPieceType.Rook, ChessPieceColor.Black);
            ChessBoard[0][1] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.Black);
            ChessBoard[1][1] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.Black);
            ChessBoard[2][1] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.Black);
            ChessBoard[3][1] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.Black);
            ChessBoard[4][1] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.Black);
            ChessBoard[5][1] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.Black);
            ChessBoard[6][1] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.Black);
            ChessBoard[7][1] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.Black);

            ChessBoard[0][6] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.White);
            ChessBoard[1][6] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.White);
            ChessBoard[2][6] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.White);
            ChessBoard[3][6] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.White);
            ChessBoard[4][6] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.White);
            ChessBoard[5][6] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.White);
            ChessBoard[6][6] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.White);
            ChessBoard[7][6] = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.White);
            ChessBoard[0][7] = new ChessPiece(ChessPieceType.Rook, ChessPieceColor.White);
            ChessBoard[1][7] = new ChessPiece(ChessPieceType.Horse, ChessPieceColor.White);
            ChessBoard[2][7] = new ChessPiece(ChessPieceType.Bishop, ChessPieceColor.White);
            ChessBoard[3][7] = new ChessPiece(ChessPieceType.Queen, ChessPieceColor.White);
            ChessBoard[4][7] = new ChessPiece(ChessPieceType.King, ChessPieceColor.White);
            ChessBoard[5][7] = new ChessPiece(ChessPieceType.Bishop, ChessPieceColor.White);
            ChessBoard[6][7] = new ChessPiece(ChessPieceType.Horse, ChessPieceColor.White);
            ChessBoard[7][7] = new ChessPiece(ChessPieceType.Rook, ChessPieceColor.White);

            missingPieces.Clear();

            PlayerWon = ChessPieceColor.None;
            ActivePlayer = PlayerWhite;
            PlayingGame = true;
            numMoves = 0;
        }

        List<ChessboardPosition> checkRookMoves(ChessboardPosition position, bool isWhite)
        {
            List<ChessboardPosition> moves = new List<ChessboardPosition>();

            int y = position.Y + 1;
            while (y < 8)
            {
                if (ChessBoard[position.X][y].IsNone || ChessBoard[position.X][y].IsWhite != isWhite)
                {
                    moves.Add(new ChessboardPosition(position.X, y));
                }
                if (!ChessBoard[position.X][y].IsNone)
                {
                    y = 8;      // encountered a piece, so bail out
                }
                else
                {
                    y++;
                }
            }

            y = position.Y - 1;
            while (y >= 0)
            {
                if (ChessBoard[position.X][y].IsNone || ChessBoard[position.X][y].IsWhite != isWhite)
                {
                    moves.Add(new ChessboardPosition(position.X, y));
                }
                if (!ChessBoard[position.X][y].IsNone)
                {
                    y = -1;      // encountered a piece, so bail out
                }
                else
                {
                    y--;
                }
            }

            int x = position.X + 1;
            while (x < 8)
            {
                if (ChessBoard[x][position.Y].IsNone || ChessBoard[x][position.Y].IsWhite != isWhite)
                {
                    moves.Add(new ChessboardPosition(x, position.Y));
                }
                if (!ChessBoard[x][position.Y].IsNone)
                {
                    x = 8;      // encountered a piece, so bail out
                }
                else
                {
                    x++;
                }
            }

            x = position.X - 1;
            while (x >= 0)
            {
                if (ChessBoard[x][position.Y].IsNone || ChessBoard[x][position.Y].IsWhite != isWhite)
                {
                    moves.Add(new ChessboardPosition(x, position.Y));
                }
                if (!ChessBoard[x][position.Y].IsNone)
                {
                    x = -1;      // encountered a piece, so bail out
                }
                else
                {
                    x--;
                }
            }

            return moves;
        }

        bool CheckMove(int x, int y, bool isWhite)
        {
            if (x >= 0 && y >= 0 && x < 8 && y < 8)
            {
                if (ChessBoard[x][y].IsNone || ChessBoard[x][y].IsWhite != isWhite)
                {
                    return true;
                }
            }
            return false;
        }

        List<ChessboardPosition> ValidateMovesInCheck(List<ChessboardPosition> moves, ChessboardPosition startPosition, bool isWhite)
        {
            List<ChessboardPosition> validMoves = new List<ChessboardPosition>();
            foreach (ChessboardPosition position in moves)
            {
                if (!MoveInCheck(startPosition, position))
                    validMoves.Add(position);
            }

            return validMoves;
        }

        List<ChessboardPosition> CheckCastling(ChessboardPosition position, bool isWhite)
        {
            List<ChessboardPosition> moves = new List<ChessboardPosition>();

            ChessPiece piece = ChessBoard[position.X][position.Y];
            if (!piece.HasMoved)
            {
                if (isWhite)
                {
                    if (ChessBoard[0][7].Type.Equals(ChessPieceType.Rook)
                        && !ChessBoard[0][7].HasMoved
                        && ChessBoard[1][7].IsNone
                        && ChessBoard[2][7].IsNone
                        && ChessBoard[3][7].IsNone)
                    {
                        if (!MoveInCheck(position, new ChessboardPosition(position.X, position.Y)) &&
                            !MoveInCheck(position, new ChessboardPosition(position.X - 1, position.Y)) &&
                            !MoveInCheck(position, new ChessboardPosition(position.X - 2, position.Y)))
                        {
                            moves.Add(new ChessboardPosition(position.X - 2, position.Y));
                        }
                    }
                    if (ChessBoard[7][7].Type.Equals(ChessPieceType.Rook)
                        && !ChessBoard[7][7].HasMoved
                        && ChessBoard[5][7].IsNone
                        && ChessBoard[6][7].IsNone)
                    {
                        if (!MoveInCheck(position, new ChessboardPosition(position.X, position.Y)) &&
                            !MoveInCheck(position, new ChessboardPosition(position.X + 1, position.Y)) &&
                            !MoveInCheck(position, new ChessboardPosition(position.X + 2, position.Y)))
                        {
                            moves.Add(new ChessboardPosition(position.X + 2, position.Y));
                        }
                    }
                }
                else
                {
                    if (ChessBoard[0][0].Type.Equals(ChessPieceType.Rook)
                        && !ChessBoard[0][0].HasMoved
                        && ChessBoard[1][0].IsNone
                        && ChessBoard[2][0].IsNone
                        && ChessBoard[3][0].IsNone)
                    {
                        if (!MoveInCheck(position, new ChessboardPosition(position.X, position.Y)) &&
                            !MoveInCheck(position, new ChessboardPosition(position.X - 1, position.Y)) &&
                            !MoveInCheck(position, new ChessboardPosition(position.X - 2, position.Y)))
                        {
                            moves.Add(new ChessboardPosition(position.X - 2, position.Y));
                        }
                    }
                    if (ChessBoard[7][0].Type.Equals(ChessPieceType.Rook)
                        && !ChessBoard[7][0].HasMoved
                        && ChessBoard[5][0].IsNone
                        && ChessBoard[6][0].IsNone)
                    {
                        if (!MoveInCheck(position, new ChessboardPosition(position.X, position.Y)) &&
                            !MoveInCheck(position, new ChessboardPosition(position.X + 1, position.Y)) &&
                            !MoveInCheck(position, new ChessboardPosition(position.X + 2, position.Y)))
                        {
                            moves.Add(new ChessboardPosition(position.X + 2, position.Y));
                        }
                    }
                }
            }

            return moves;
        }

        List<ChessboardPosition> checkKingMoves(ChessboardPosition position, bool isWhite)
        {
            List<ChessboardPosition> moves = new List<ChessboardPosition>();
            for (int x = position.X - 1; x <= position.X + 1; x++)
            {
                if (CheckMove(x, position.Y - 1, isWhite))
                {
                    moves.Add(new ChessboardPosition(x, position.Y - 1));
                }
                if (x != position.X && CheckMove(x, position.Y, isWhite))
                {
                    moves.Add(new ChessboardPosition(x, position.Y));
                }
                if (CheckMove(x, position.Y + 1, isWhite))
                {
                    moves.Add(new ChessboardPosition(x, position.Y + 1));
                }
            }
            return moves;
        }

        List<ChessboardPosition> checkPawnMoves(ChessboardPosition position, bool isWhite)
        {
            List<ChessboardPosition> moves = new List<ChessboardPosition>();
            if (isWhite)
            {
                if (CheckMove(position.X, position.Y - 1, isWhite) && !ChessBoard[position.X][position.Y - 1].IsBlack)
                {
                    moves.Add(new ChessboardPosition(position.X, position.Y - 1));
                }
                if (CheckMove(position.X + 1, position.Y - 1, isWhite) && ChessBoard[position.X + 1][position.Y - 1].IsBlack)
                {
                    moves.Add(new ChessboardPosition(position.X + 1, position.Y - 1));
                }
                if (CheckMove(position.X - 1, position.Y - 1, isWhite) && ChessBoard[position.X - 1][position.Y - 1].IsBlack)
                {
                    moves.Add(new ChessboardPosition(position.X - 1, position.Y - 1));
                }
                if (position.Y == 6 && CheckMove(position.X, position.Y - 2, isWhite) && !ChessBoard[position.X][position.Y - 2].IsBlack && ChessBoard[position.X][position.Y - 1].IsNone)
                {
                    moves.Add(new ChessboardPosition(position.X, position.Y - 2));
                }
            }
            else
            {
                if (CheckMove(position.X, position.Y + 1, isWhite) && !ChessBoard[position.X][position.Y + 1].IsWhite)
                {
                    moves.Add(new ChessboardPosition(position.X, position.Y + 1));
                }
                if (CheckMove(position.X + 1, position.Y + 1, isWhite) && ChessBoard[position.X + 1][position.Y + 1].IsWhite)
                {
                    moves.Add(new ChessboardPosition(position.X + 1, position.Y + 1));
                }
                if (CheckMove(position.X - 1, position.Y + 1, isWhite) && ChessBoard[position.X - 1][position.Y + 1].IsWhite)
                {
                    moves.Add(new ChessboardPosition(position.X - 1, position.Y + 1));
                }
                if (position.Y == 1 && CheckMove(position.X, position.Y + 2, isWhite) && !ChessBoard[position.X][position.Y + 2].IsWhite && ChessBoard[position.X][position.Y + 1].IsNone)
                {
                    moves.Add(new ChessboardPosition(position.X, position.Y + 2));
                }
            }

            return moves;
        }


        List<ChessboardPosition> checkHorseMoves(ChessboardPosition position, bool isWhite)
        {
            List<ChessboardPosition> moves = new List<ChessboardPosition>();

            int x = position.X + 2;
            int y = position.Y + 1;
            if (CheckMove(x, y, isWhite))
            {
                moves.Add(new ChessboardPosition(x, y));
            }
            x = position.X + 1;
            y = position.Y + 2;
            if (CheckMove(x, y, isWhite))
            {
                moves.Add(new ChessboardPosition(x, y));
            }
            x = position.X - 1;
            y = position.Y - 2;
            if (CheckMove(x, y, isWhite))
            {
                moves.Add(new ChessboardPosition(x, y));
            }
            x = position.X - 2;
            y = position.Y - 1;
            if (CheckMove(x, y, isWhite))
            {
                moves.Add(new ChessboardPosition(x, y));
            }
            x = position.X - 1;
            y = position.Y + 2;
            if (CheckMove(x, y, isWhite))
            {
                moves.Add(new ChessboardPosition(x, y));
            }
            x = position.X - 2;
            y = position.Y + 1;
            if (CheckMove(x, y, isWhite))
            {
                moves.Add(new ChessboardPosition(x, y));
            }
            x = position.X + 1;
            y = position.Y - 2;
            if (CheckMove(x, y, isWhite))
            {
                moves.Add(new ChessboardPosition(x, y));
            }
            x = position.X + 2;
            y = position.Y - 1;
            if (CheckMove(x, y, isWhite))
            {
                moves.Add(new ChessboardPosition(x, y));
            }

            return moves;
        }


        List<ChessboardPosition> checkBishopMoves(ChessboardPosition position, bool isWhite)
        {
            List<ChessboardPosition> moves = new List<ChessboardPosition>();

            int x = position.X + 1;
            int y = position.Y + 1;
            while (y < 8 && x < 8)
            {
                if (ChessBoard[x][y].IsNone || ChessBoard[x][y].IsWhite != isWhite)
                {
                    moves.Add(new ChessboardPosition(x, y));
                }
                if (!ChessBoard[x][y].IsNone)
                {
                    y = 8;      // encountered a piece, so bail out
                }
                else
                {
                    x++;
                    y++;
                }
            }

            x = position.X + 1;
            y = position.Y - 1;
            while (x < 8 && y >= 0)
            {
                if (ChessBoard[x][y].IsNone || ChessBoard[x][y].IsWhite != isWhite)
                {
                    moves.Add(new ChessboardPosition(x, y));
                }
                if (!ChessBoard[x][y].IsNone)
                {
                    y = -1;      // encountered a piece, so bail out
                }
                else
                {
                    x++;
                    y--;
                }
            }

            x = position.X - 1;
            y = position.Y + 1;
            while (x >= 0 && y < 8)
            {
                if (ChessBoard[x][y].IsNone || ChessBoard[x][y].IsWhite != isWhite)
                {
                    moves.Add(new ChessboardPosition(x, y));
                }
                if (!ChessBoard[x][y].IsNone)
                {
                    y = 8;      // encountered a piece, so bail out
                }
                else
                {
                    x--;
                    y++;
                }
            }

            x = position.X - 1;
            y = position.Y - 1;
            while (x >= 0 && y >= 0)
            {
                if (ChessBoard[x][y].IsNone || ChessBoard[x][y].IsWhite != isWhite)
                {
                    moves.Add(new ChessboardPosition(x, y));
                }
                if (!ChessBoard[x][y].IsNone)
                {
                    x = -1;      // encountered a piece, so bail out
                }
                else
                {
                    x--;
                    y--;
                }
            }

            return moves;
        }

        public List<ChessboardPosition> CheckPossibleMoves(int x, int y)
        {
            return CheckPossibleMoves(new ChessboardPosition(x, y));
        }

        List<ChessboardPosition> CheckPossibleMoves(ChessboardPosition position)
        {
            List<ChessboardPosition> moves = new List<ChessboardPosition>();
            ChessPiece piece = ChessBoard[position.X][position.Y];

            switch (piece.PieceName)
            {
                case "Rook":
                    moves = checkRookMoves(position, piece.IsWhite);
                    break;
                case "Bishop":
                    moves = checkBishopMoves(position, piece.IsWhite);
                    break;
                case "Horse":
                    moves = checkHorseMoves(position, piece.IsWhite);
                    break;
                case "Queen":
                    moves = checkRookMoves(position, piece.IsWhite);
                    moves.AddRange(checkBishopMoves(position, piece.IsWhite));
                    break;
                case "King":
                    moves = checkKingMoves(position, piece.IsWhite);
                    break;
                case "Pawn":
                    moves = checkPawnMoves(position, piece.IsWhite);
                    break;
            }

            return moves;
        }

        bool IsCheck(ChessboardPosition kingPosition, bool isWhite)
        {
            bool isCheck = false;

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (/*!ChessBoard[x, y].Type.Equals(ChessPieceType.King) &&*/ ((isWhite && ChessBoard[x][y].IsBlack) || (!isWhite && ChessBoard[x][y].IsWhite)))
                    {
                        List<ChessboardPosition> moves = CheckPossibleMoves(new ChessboardPosition(x, y));
                        if (moves.Exists(obj => obj.X == kingPosition.X && obj.Y == kingPosition.Y))
                        {
                            isCheck = true;
                        }
                    }
                }
            }

            return isCheck;
        }

        bool IsCheck(bool isWhite)
        {
            ChessboardPosition position = null;
            // find player's king
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (ChessBoard[x][y].Type.Equals(ChessPieceType.King) && ChessBoard[x][y].IsWhite == isWhite)
                    {
                        position = new ChessboardPosition(x, y);
                        break;
                    }
                }
            }

            return IsCheck(position, isWhite);
        }

        int CountPieces(ChessPieceType type, bool isWhite)
        {
            int count = 0;

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (ChessBoard[x][y].Type.Equals(type) && ChessBoard[x][y].IsWhite == isWhite)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        void PromotePawn(ChessboardPosition position, bool isWhite)
        {
            missingPieces.Add(new ChessPiece(ChessPieceType.Pawn, isWhite ? ChessPieceColor.White : ChessPieceColor.Black, true));
            if (CountPieces(ChessPieceType.Queen, isWhite) == 0)
            {
                missingPieces.Remove(missingPieces.Find(x => (x.IsWhite == isWhite && x.Type.Equals(ChessPieceType.Queen))));
                ChessBoard[position.X][position.Y].Type = ChessPieceType.Queen;
                return;
            }
            if (CountPieces(ChessPieceType.Rook, isWhite) < 2)
            {
                missingPieces.Remove(missingPieces.Find(x => (x.IsWhite == isWhite && x.Type.Equals(ChessPieceType.Rook))));
                ChessBoard[position.X][position.Y].Type = ChessPieceType.Rook;
                return;
            }
            if (CountPieces(ChessPieceType.Bishop, isWhite) < 2)
            {
                missingPieces.Remove(missingPieces.Find(x => (x.IsWhite == isWhite && x.Type.Equals(ChessPieceType.Bishop))));
                ChessBoard[position.X][position.Y].Type = ChessPieceType.Bishop;
                return;
            }
            if (CountPieces(ChessPieceType.Horse, isWhite) < 2)
            {
                missingPieces.Remove(missingPieces.Find(x => (x.IsWhite == isWhite && x.Type.Equals(ChessPieceType.Horse))));
                ChessBoard[position.X][position.Y].Type = ChessPieceType.Horse;
                return;
            }
        }

        void CheckPawnPromotion(bool isWhite)
        {
            for (int x = 0; x < 8; x++)
            {
                if (isWhite)
                {
                    if (ChessBoard[x][0].Type.Equals(ChessPieceType.Pawn) && ChessBoard[x][0].IsWhite)
                    {
                        PromotePawn(new ChessboardPosition(x, 0), isWhite);
                    }
                }
                else
                {
                    if (ChessBoard[x][7].Type.Equals(ChessPieceType.Pawn) && ChessBoard[x][7].IsBlack)
                    {
                        PromotePawn(new ChessboardPosition(x, 7), isWhite);
                    }
                }
            }
        }

        public List<ChessboardPosition> GetMoveablePositions()
        {
            List<ChessboardPosition> pieces = new List<ChessboardPosition>(); ;

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (!ChessBoard[x][y].Type.Equals(ChessPieceType.None) && ChessBoard[x][y].IsWhite == ActivePlayer)
                    {
                        ChessboardPosition position = new ChessboardPosition(x, y);
                        if (FindPossibleMoves(position).Count > 0)
                        {
                            pieces.Add(new ChessboardPosition(x, y));
                        }
                    }
                }
            }

            return pieces;
        }


        // Check all possible moves; if none -> checkmate or stalemate!
        bool IsCheckMate()
        {
            List<ChessboardPosition> moves = new List<ChessboardPosition>();

            if (missingPieces.Count == 30)     // only 2 pieces left is considered stalemate
            {
                PlayerWon = ChessPieceColor.None;
                PlayingGame = false;
            }

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (!ChessBoard[x][y].Type.Equals(ChessPieceType.None) && ChessBoard[x][y].IsWhite == ActivePlayer)
                    {
                        ChessboardPosition position = new ChessboardPosition(x, y);
                        if (FindPossibleMoves(position).Count > 0)
                        {
                            return false;
                        }
                    }
                }
            }
            if (IsCheck(ActivePlayer))
            {
                if (ActivePlayer == PlayerWhite)
                    PlayerWon = ChessPieceColor.Black;
                else
                    PlayerWon = ChessPieceColor.White;
            }
            else
            {
                PlayerWon = ChessPieceColor.None;       // Stalemate
            }

            PlayingGame = false;

            return true;
        }

        // Check if this is removed, the player would be in check
        // We do this by temporary removing the piece from the board
        bool MoveInCheck(ChessboardPosition fromPosition)
        {
            bool inCheck = false;
            ChessPiece oldFrom = ChessBoard[fromPosition.X][fromPosition.Y];
            ChessBoard[fromPosition.X][fromPosition.Y] = new ChessPiece();
            if (IsCheck(oldFrom.IsWhite))
            {
                inCheck = true;
            }
            ChessBoard[fromPosition.X][fromPosition.Y] = oldFrom;

            return inCheck;
        }

        // Check if this move is made, the player would be in check
        // We do this by temporary moving the piece on the board
        bool MoveInCheck(ChessboardPosition fromPosition, ChessboardPosition toPosition)
        {
            bool inCheck = false;
            ChessPiece oldFrom = ChessBoard[fromPosition.X][fromPosition.Y];
            ChessPiece oldTo = ChessBoard[toPosition.X][toPosition.Y];
            ChessBoard[fromPosition.X][fromPosition.Y] = new ChessPiece();
            ChessBoard[toPosition.X][toPosition.Y] = oldFrom;
            if (IsCheck(oldFrom.IsWhite))
            {
                inCheck = true;
            }
            ChessBoard[fromPosition.X][fromPosition.Y] = oldFrom;
            ChessBoard[toPosition.X][toPosition.Y] = oldTo;

            return inCheck;
        }

        bool IsOwnPiece(ChessPiece piece)
        {
            return (ActivePlayer == PlayerWhite && piece.IsWhite) || (ActivePlayer == PlayerBlack && piece.IsBlack);
        }

        public void MakeMove(int xfrom, int yfrom, int xto, int yto)
        {
            squareSelected = new ChessboardPosition(xfrom, yfrom);
            possibleMoves = FindPossibleMoves(squareSelected);
            squarePosition = new ChessboardPosition(xto, yto);
            MakeMove();
            squareSelected.SetNone();
            possibleMoves.Clear();

            IsCheckMate();
        }

        void MakeMove()
        {
            ChessPiece selectedPiece = ChessBoard[squareSelected.X][squareSelected.Y];
            // Check for castling and move rook
            if (selectedPiece.Type.Equals(ChessPieceType.King))
            {
                if (selectedPiece.IsWhite)
                {
                    if (squareSelected.X == 4 && squareSelected.Y == 7 && squarePosition.X == 2 && squarePosition.Y == 7)
                    {
                        ChessBoard[3][7] = ChessBoard[0][7];
                        ChessBoard[3][7].HasMoved = true;
                        ChessBoard[0][7] = new ChessPiece();
                    }
                    if (squareSelected.X == 4 && squareSelected.Y == 7 && squarePosition.X == 6 && squarePosition.Y == 7)
                    {
                        ChessBoard[5][7] = ChessBoard[7][7];
                        ChessBoard[5][7].HasMoved = true;
                        ChessBoard[7][7] = new ChessPiece();
                    }
                }
                else
                {
                    if (squareSelected.X == 4 && squareSelected.Y == 0 && squarePosition.X == 2 && squarePosition.Y == 0)
                    {
                        ChessBoard[3][0] = ChessBoard[0][0];
                        ChessBoard[3][0].HasMoved = true;
                        ChessBoard[0][0] = new ChessPiece();
                    }
                    if (squareSelected.X == 4 && squareSelected.Y == 0 && squarePosition.X == 6 && squarePosition.Y == 0)
                    {
                        ChessBoard[5][0] = ChessBoard[7][0];
                        ChessBoard[5][0].HasMoved = true;
                        ChessBoard[7][0] = new ChessPiece();
                    }
                }
            }
            previousPositionFrom = new ChessboardPosition(squarePosition.X, squarePosition.Y);
            previousPositionTo = new ChessboardPosition(squareSelected.X, squareSelected.Y);
            previousPieceFrom = ChessBoard[squarePosition.X][squarePosition.Y];
            previousPieceTo = new ChessPiece(selectedPiece.Type, selectedPiece.Color, selectedPiece.HasMoved);

            selectedPiece.HasMoved = true;
            if (!ChessBoard[squarePosition.X][squarePosition.Y].IsNone)
            {
                missingPieces.Add(ChessBoard[squarePosition.X][squarePosition.Y]);
            }
            ChessBoard[squarePosition.X][squarePosition.Y] = selectedPiece;
            ChessBoard[squareSelected.X][squareSelected.Y] = new ChessPiece();
            CheckPawnPromotion(ActivePlayer);
            numMoves++;
            NextPlayer();
        }

        void NextPlayer()
        {
            ActivePlayer = !ActivePlayer;
        }

        public List<ChessboardPosition> FindPossibleMoves(ChessboardPosition position)
        {
            List<ChessboardPosition> possibleMoves = new List<ChessboardPosition>();
            ChessPiece pieceSelected = ChessBoard[position.X][position.Y];
            if (IsOwnPiece(pieceSelected))
            {
                possibleMoves = CheckPossibleMoves(position);
                possibleMoves = ValidateMovesInCheck(possibleMoves, position, pieceSelected.IsWhite);
                if (pieceSelected.Type.Equals(ChessPieceType.King))
                {
                    possibleMoves.AddRange(CheckCastling(position, pieceSelected.IsWhite));
                }
            }

            return possibleMoves;
        }

        public List<ChessMove> FindAllPossibleMoves()
        {
            List<ChessMove> chessMoves = new List<ChessMove>();
            List<ChessboardPosition> possibleMoves = GetMoveablePositions();
            foreach (ChessboardPosition position in possibleMoves)
            {
                ChessPiece pieceSelected = ChessBoard[position.X][position.Y];
                if (IsOwnPiece(pieceSelected))
                {
                    possibleMoves = CheckPossibleMoves(position);
                    possibleMoves = ValidateMovesInCheck(possibleMoves, position, pieceSelected.IsWhite);
                    if (pieceSelected.Type.Equals(ChessPieceType.King))
                    {
                        possibleMoves.AddRange(CheckCastling(position, pieceSelected.IsWhite));
                    }
                    foreach (ChessboardPosition move in possibleMoves)
                    {
                        chessMoves.Add(new ChessMove(position.X, position.Y, move.X, move.Y));
                    }
                }
            }

            return chessMoves;
        }
    }
}
