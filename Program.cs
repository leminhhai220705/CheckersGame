using System;
using System.Linq;

namespace CheckersGame
{
    public enum Piece
    {
        Empty,
        Player1,
        Player2,
        King1,
        King2
    }

    class Program
    {
        static int boardSize = 8;
        static Piece[,] board = new Piece[boardSize, boardSize];
        static bool player1Turn = true;

        static void Main(string[] args)
        {
            InitializeBoard();
            while (true)
            {
                PrintBoard();
                PlayerMove();
            }
        }

        static void InitializeBoard()
        {
            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    if ((row + col) % 2 == 1)
                    {
                        if (row < 3)
                            board[row, col] = Piece.Player1;
                        else if (row > 4)
                            board[row, col] = Piece.Player2;
                        else
                            board[row, col] = Piece.Empty;
                    }
                }
            }
        }

        static void PrintBoard()
        {
            Console.Clear();
            Console.WriteLine("  A B C D E F G H");

            for (int row = 0; row < boardSize; row++)
            {
                Console.Write(row + 1 + " ");
                for (int col = 0; col < boardSize; col++)
                {
                    char symbol = '□';
                    switch (board[row, col])
                    {
                        case Piece.Player1:
                            symbol = 'X';
                            break;
                        case Piece.Player2:
                            symbol = 'O';
                            break;
                        case Piece.King1:
                            symbol = 'K';
                            break;
                        case Piece.King2:
                            symbol = 'Q';
                            break;
                    }
                    Console.Write(symbol + " ");
                }
                Console.WriteLine();
            }
        }

        static void PlayerMove()
        {
            Console.WriteLine(player1Turn ? "Player 1 (X), your move." : "Player 2 (O), your move.");
            string move = Console.ReadLine().ToUpper();

            if (!ValidateMoveInput(move))
            {
                Console.WriteLine("Invalid move format. Use format: A2B3.");
                return;
            }

            // Parse input
            int startX = move[0] - 'A';
            int startY = int.Parse(move[1].ToString()) - 1;
            int endX = move[2] - 'A';
            int endY = int.Parse(move[3].ToString()) - 1;

            if (!IsValidMove(startX, startY, endX, endY))
            {
                Console.WriteLine("Invalid move. Try again.");
                return;
            }

            MakeMove(startX, startY, endX, endY);
            if (ReachedEnd(endY))
                CrownPiece(endX, endY);

            player1Turn = !player1Turn;
        }

        static bool ValidateMoveInput(string move)
        {
            if (move.Length != 4) return false;
            return move[0] >= 'A' && move[0] <= 'H' && move[2] >= 'A' && move[2] <= 'H' &&
                   move[1] >= '1' && move[1] <= '8' && move[3] >= '1' && move[3] <= '8';
        }

        static bool IsValidMove(int startX, int startY, int endX, int endY)
        {
            if (board[startY, startX] == Piece.Empty || board[endY, endX] != Piece.Empty)
                return false;

            Piece piece = board[startY, startX];
            bool isPlayer1 = player1Turn && (piece == Piece.Player1 || piece == Piece.King1);
            bool isPlayer2 = !player1Turn && (piece == Piece.Player2 || piece == Piece.King2);

            if (!isPlayer1 && !isPlayer2)
                return false;

            int direction = (player1Turn || piece == Piece.King2) ? 1 : -1;
            bool isMoveForward = (endY - startY) == direction;

            if (Math.Abs(endX - startX) == 1 && isMoveForward)
            {
                // Normal move
                return true;
            }
            else if (Math.Abs(endX - startX) == 2 && Math.Abs(endY - startY) == 2)
            {
                // Capture move
                int captureX = (startX + endX) / 2;
                int captureY = (startY + endY) / 2;
                Piece capturePiece = board[captureY, captureX];

                if ((player1Turn && (capturePiece == Piece.Player2 || capturePiece == Piece.King2)) ||
                    (!player1Turn && (capturePiece == Piece.Player1 || capturePiece == Piece.King1)))
                {
                    board[captureY, captureX] = Piece.Empty;
                    return true;
                }
            }
            return false;
        }

        static void MakeMove(int startX, int startY, int endX, int endY)
        {
            board[endY, endX] = board[startY, startX];
            board[startY, startX] = Piece.Empty;
        }

        static bool ReachedEnd(int endY)
        {
            return (player1Turn && endY == boardSize - 1) || (!player1Turn && endY == 0);
        }

        static void CrownPiece(int x, int y)
        {
            if (board[y, x] == Piece.Player1)
                board[y, x] = Piece.King1;
            else if (board[y, x] == Piece.Player2)
                board[y, x] = Piece.King2;
        }
    }
}