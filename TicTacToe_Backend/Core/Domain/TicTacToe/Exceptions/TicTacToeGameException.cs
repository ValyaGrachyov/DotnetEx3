using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.TicTacToe.Exceptions;

public class TicTacToeGameException : Exception
{
	public TicTacToeGameException()
	{
	}

	public TicTacToeGameException(string message) : base(message)
	{
	}
}
