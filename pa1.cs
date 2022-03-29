//#nullable enable
using System;
using static System.Console;

namespace Bme121
{
    record Player( string Colour, string Symbol, string Name );
    
    // The `record` is a kind of automatic class in the sense that the compiler generates
    // the fields and constructor and some other things just from this one line.
    // There's a rationale for the capital letters on the variable names (later).
    // For now you can think of it as roughly equivalent to a nonstatic `class` with three
    // public fields and a three-parameter constructor which initializes the fields.
    // It would be like the following. The 'readonly' means you can't change the field value
    // after it is set by the constructor method.
    /*
    class Player
    {
        public readonly string Colour;
        public readonly string Symbol;
        public readonly string Name;
        
        public Player( string Colour, string Symbol, string Name )
        {
            this.Colour = Colour;
            this.Symbol = Symbol;
            this.Name = Name;
        }
    }
    */
    static partial class Program
    {
        // Display common text for the top of the screen.
        
        static void Welcome( )
        {
            WriteLine("Welcome to Othello! please pick your player names and board size below.");
            WriteLine();
        }
        
        // Collect a player name or default to form the player record.
        
        static Player NewPlayer( string colour, string symbol, string defaultName )
        {
            //Ask user what name to use, give colour and symbol
            //collect response
            //if empty use default 
            //else use reponse as name
            Write("Type the {0} disc ({1}) player name [or <Enter> for '{0}']: ",colour,symbol);
            string name = ReadLine();
            if(name.Length==0)
            {
                name=defaultName;
            }
            return new Player( colour, symbol, name );
        }
        
        // Determine which player goes first or default.
        
        static int GetFirstTurn( Player[ ] players, int defaultFirst )
        {
            Write("Choose who will play first [or <Enter> for {0}/{1}/{2}]: ",players[0].Colour,players[0].Symbol,players[0].Name);
            string first = ReadLine();
            
            if (first==players[1].Name)
            {
                return 1;
            } else
            {
                return 0;
            }
            
        }
        // -----------------------------------------------------------------------------------------
        // Return the single-character string "a".."z" corresponding to its index 0..25. 
        // Return " " for an invalid index.
        
        static string LetterAtIndex( int number )
        {
            if( number < 0 || number > 25 ) return " ";
            else return "abcdefghijklmnopqrstuvwxyz"[ number ].ToString( );
        }
        
        // -----------------------------------------------------------------------------------------
        // Return the index 0..25 corresponding to its single-character string "a".."z". 
        // Return -1 for an invalid string.
        
        static int IndexAtLetter( string letter )
        {
            if( letter.Length != 1 ) return -1;
            else return "abcdefghijklmnopqrstuvwxyz".IndexOf( letter[ 0 ] );
        }
        
        // -----------------------------------------------------------------------------------------
        // Create a new Othello game board, initialized with four pieces in their starting
        // positions. The counts of rows and columns must be no less than 4, no greater than 26,
        // and not an odd number. If not, the new game board is created as an empty array.
        
        static string[ , ] NewBoard( int rows, int cols )
        {
            const string blank = " ";
            const string white = "O";
            const string black = "X";
            
            if(    rows < 4 || rows > 26 || rows % 2 == 1
                || cols < 4 || cols > 26 || cols % 2 == 1 ) return new string[ 0, 0 ];
                
            string[ , ] board = new string[ rows, cols ];
            
            for( int row = 0; row < rows; row ++ )
            {
                for( int col = 0; col < cols; col ++ )
                {
                    board[ row, col ] = blank;
                }
            }
            
            board[ rows / 2 - 1, cols / 2 - 1 ] = white;
            board[ rows / 2 - 1, cols / 2     ] = black;
            board[ rows / 2,     cols / 2 - 1 ] = black;
            board[ rows / 2,     cols / 2     ] = white;
            
            return board;
        }

        // -----------------------------------------------------------------------------------------
        // Display the Othello game board on the Console.
        // All information about the game is held in the two-dimensional string array.
        
        static void DisplayBoard( string[ , ] board )
        {
            const string h  = "\u2500"; // horizontal line
            const string v  = "\u2502"; // vertical line
            const string tl = "\u250c"; // top left corner
            const string tr = "\u2510"; // top right corner
            const string bl = "\u2514"; // bottom left corner
            const string br = "\u2518"; // bottom right corner
            const string vr = "\u251c"; // vertical join from right
            const string vl = "\u2524"; // vertical join from left
            const string hb = "\u252c"; // horizontal join from below
            const string ha = "\u2534"; // horizontal join from above
            const string hv = "\u253c"; // horizontal vertical cross
            const string mx = "\u256c"; // marked horizontal vertical cross
            const string sp =      " "; // space

            // Nothing to display?
            if( board == null ) return;
            
            int rows = board.GetLength( 0 );
            int cols = board.GetLength( 1 );
            if( rows == 0 || cols == 0 ) return;
            
            // Display the board row by row.
            for( int row = 0; row < rows; row ++ )
            {
                if( row == 0 )
                {
                    // Labels above top edge.
                    for( int col = 0; col < cols; col ++ )
                    {
                        if( col == 0 ) Write( "   {0}{0}{1}{0}", sp, LetterAtIndex( col ) );
                        else Write( "{0}{0}{1}{0}", sp, LetterAtIndex( col ) );
                    }
                    WriteLine( );
                    
                    // Border above top row.
                    for( int col = 0; col < cols; col ++ )
                    {
                        if( col == 0 ) Write( "   {0}{1}{1}{1}", tl, h );
                        else Write( "{0}{1}{1}{1}", hb, h );
                        if( col == cols - 1 ) Write( "{0}", tr );
                    }
                    WriteLine( );
                }
                else
                {
                    // Border above a row which is not the top row.
                    for( int col = 0; col < cols; col ++ )
                    {
                        if(    rows > 5 && cols > 5 && row ==        2 && col ==        2 
                            || rows > 5 && cols > 5 && row ==        2 && col == cols - 2 
                            || rows > 5 && cols > 5 && row == rows - 2 && col ==        2 
                            || rows > 5 && cols > 5 && row == rows - 2 && col == cols - 2 )  
                            Write( "{0}{1}{1}{1}", mx, h );
                        else if( col == 0 ) Write( "   {0}{1}{1}{1}", vr, h );
                        else Write( "{0}{1}{1}{1}", hv, h );
                        if( col == cols - 1 ) Write( "{0}", vl );
                    }
                    WriteLine( );
                }
                
                // Row content displayed column by column.
                for( int col = 0; col < cols; col ++ ) 
                {
                    if( col == 0 ) Write( " {0,-2}", LetterAtIndex( row ) ); // Labels on left side
                    Write( "{0} {1} ", v, board[ row, col ] );
                    if( col == cols - 1 ) Write( "{0}", v );
                }
                WriteLine( );
                
                if( row == rows - 1 )
                {
                    // Border below last row.
                    for( int col = 0; col < cols; col ++ )
                    {
                        if( col == 0 ) Write( "   {0}{1}{1}{1}", bl, h );
                        else Write( "{0}{1}{1}{1}", ha, h );
                        if( col == cols - 1 ) Write( "{0}", br );
                    }
                    WriteLine( );
                }
            }
        }
        // Get a board size (between 4 and 26 and even) or default, for one direction.
        
        static int GetBoardSize( string direction, int defaultSize )
        {
            Write("Enter board {0} (4-26 and even) [or <Enter> for '8']: ",direction);
                string txtSize = ReadLine();
                int size;
                if(int.TryParse(txtSize, out size)){
                    
                    if(size>=4 && size<=26 && size%2==0)
                    {
                        return size;
                    }
                    else
                    {
                        WriteLine("Incorrect Format, default size(8) selected");
                        return 8;   
                    }
                }
                else
                {
                    return 8;
                }
        }
        // Get a move from a player.
        
        static string GetMove( Player player )
        {
            WriteLine("it's {0}'s({1}) turn",player.Name,player.Symbol);
            WriteLine("Pick a cell by its row then column name (Ex. bc) to play there");
            WriteLine("Use 'skip' to give up your turn. Use 'quit' to end the game.");
            Write("Enter your choice:");
            
            
            string move = ReadLine();
            
            while (string.IsNullOrEmpty(move))
            {
                Write("Error: choice can not be empty, Try again:");
                move = ReadLine();
            }
            
            return move;
        }
        
        // Try to make a move. Return true if it worked.
        
        static bool TryMove( string[ , ] board, Player player, string move )
        {
            if(move == "skip") return true;
            if(move.Length != 2) return false;
            
            int moveCol = IndexAtLetter(move.Substring(1,1));
            
            int moveRow = IndexAtLetter(move.Substring(0,1));
            
            if(moveRow==-1 || moveCol==-1)return false;
            
            //check if move is off board 
            if(moveCol>board.GetLength(1)-1)return false;
            if(moveRow>board.GetLength(0)-1)return false;
            
            //check if board is occupied 
            if(board[ moveRow, moveCol ] == "X" || board[ moveRow, moveCol ] == "O")return false;
            
            //temporary place symbol
            
            
            //Call TryDirection eight times 
            bool valid = false;
            valid = valid | TryDirection(board, player, moveRow, 0, moveCol, 1);
            valid = valid | TryDirection(board, player, moveRow, 1, moveCol, 1);
            valid = valid | TryDirection(board, player, moveRow, 1, moveCol, 0);
            valid = valid | TryDirection(board, player, moveRow, 1, moveCol, -1);
            valid = valid | TryDirection(board, player, moveRow, 0, moveCol, -1);
            valid = valid | TryDirection(board, player, moveRow, -1, moveCol, -1);
            valid = valid | TryDirection(board, player, moveRow, -1, moveCol, 0);
            valid = valid | TryDirection(board, player, moveRow, -1, moveCol, 1);
            if (valid )
            {
                board[ moveRow, moveCol ] = player.Symbol;
            }
            return valid;
            
        }
        
        // Do the flips along a direction specified by the row and column delta for one step.
        
        static bool TryDirection( string[ , ] board, Player player,
            int moveRow, int deltaRow, int moveCol, int deltaCol )
        {
            int currentRow = moveRow;
            int currentCol = moveCol;
            int countFlips = 0;
            bool end = false; 
            while(! end)
            {
                //moving along
                currentRow = currentRow + deltaRow;
                currentCol = currentCol + deltaCol;
                
                //still on board
                if(currentRow < 0 ) return false;
                if(currentRow > board.GetLength(0) -1) return false;
                if(currentCol < 0 ) return false;
                if(currentCol > board.GetLength(0) -1) return false;
                
                //not blank
                if(board[ currentRow, currentCol ] == " " ) return false;
                
                //not my symbol add flip else symbol found=end
                if(board[ currentRow, currentCol ] != player.Symbol) countFlips++;
                else end=true;
            }
            
            if( countFlips == 0)return false;
            //do flips 
            currentRow = moveRow;
            currentCol = moveCol;
            
            for(int i=0; i<countFlips; i++)
            {
                //move
                currentRow = currentRow + deltaRow;
                currentCol = currentCol + deltaCol;
                //flip
                board[ currentRow, currentCol] = player.Symbol;
            }
            return true;
        }
        
        // Count the discs to find the score for a player.
        
        static int GetScore( string[ , ] board, Player player )
        {
            int score = 0;
            
            for(int r=0; r < board.GetLength(0); r++)
            {
                for(int c=0; c < board.GetLength(1); c++)
                {
                    if(board[r,c]==player.Symbol) score++;
                }
            }
            return score;
        }
        
        // Display a line of scores for all players.
        
        static void DisplayScores( string[ , ] board, Player[ ] players )
        {
            int score1 = GetScore(board, players[0]);
            int score2 = GetScore(board, players[1]);
            WriteLine("Score: {0}: {1}, {2}: {3}",players[0].Name,score1,players[1].Name,score2 );
        }
        
        // Display winner(s) and categorize their win over the defeated player(s).
        
        static void DisplayWinners( string[ , ] board, Player[ ] players )
        {
            int score1 = GetScore(board, players[0]);
            int score2 = GetScore(board, players[1]);
            WriteLine("Final Scores: {0}: {1}, {2}: {3}",players[0].Name,score1,players[1].Name,score2 );
            if(score1 > score2) WriteLine("Congratulations {0} has won",players[0].Name);
            else if(score1 == score2)WriteLine("The game is a draw... Rematch?");
            else if(score1 < score2) WriteLine("Congratulations {0} has won",players[1].Name);
           
            
        }
        
        static void Main( )
        {
            // Set up the players and game.
            // Note: I used an array of 'Player' objects to hold information about the players.
            // This allowed me to just pass one 'Player' object to methods needing to use
            // the player name, colour, or symbol in 'WriteLine' messages or board operation.
            // The array aspect allowed me to use the index to keep track or whose turn it is.
            // It is also possible to use separate variables or separate arrays instead
            // of an array of objects. It is possible to put the player information in
            // global variables (static field variables of the 'Program' class) so they
            // can be accessed by any of the methods without passing them directly as arguments.
            
            Welcome( );
            
            Player[ ] players = new Player[ ] 
            {
                NewPlayer( colour: "black", symbol: "X", defaultName: "Black" ),
                NewPlayer( colour: "white", symbol: "O", defaultName: "White" ),
            };
            
            int turn = GetFirstTurn( players, defaultFirst: 0 );
           
            int rows = GetBoardSize( direction: "rows",    defaultSize: 8 );
            int cols = GetBoardSize( direction: "columns", defaultSize: 8 );
            
            string[ , ] game = NewBoard( rows, cols );
            
            // Play the game.
            
            bool gameOver = false;
            while( ! gameOver )
            {
                //Welcome( );
                DisplayBoard( game ); 
                DisplayScores( game, players );
                
                string move = GetMove( players[ turn ] );
                if( move == "quit" ) 
                {
                gameOver = true;
                }
                else
                {
                    bool madeMove = TryMove( game, players[ turn ], move );
                    if( madeMove ) turn = ( turn + 1 ) % players.Length;
                    else 
                    {
                        Write( " Your choice didn't work!" );
                        Write( " Press <Enter> to try again." );
                        ReadLine( ); 
                    }
                }
            }
            
            // Show the final results.
            
            DisplayWinners( game, players );
            WriteLine( );
            WriteLine("Thank you for playing. Game terminated");
        }
    }
}
