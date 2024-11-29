
int fieldSize = 10;
int mines = 8;

Random rng = new Random();
int[,] solvedField = new int[fieldSize, fieldSize];
int[,] problemField = new int[fieldSize, fieldSize];
bool lost = false;

for (int i = 0; i < problemField.GetLength(0); i++)
{
	for (int j = 0; j < problemField.GetLength(1); j++)
	{
		problemField[i, j] = -2;
	}
}

for (int i = 0; i < mines; i++)
{
	int x = rng.Next(solvedField.GetLength(0));
	int y = rng.Next(solvedField.GetLength(1));
	if (solvedField[x, y] != -1)
		solvedField[x, y] = -1;
	else
		i--;
}

for (int i = 0; i < solvedField.GetLength(0); i++)
{
	for (int j = 0; j < solvedField.GetLength(1); j++)
	{
		if (solvedField[i, j] != -1)
			solvedField[i, j] = CountNeighbors(i, j, solvedField, -1);
	}
}

int openX = rng.Next(solvedField.GetLength(0)), openY = rng.Next(solvedField.GetLength(1));
do
{
	while (true)
	{
		OpenFields(openX, openY);
		if (openX == -1 || openY == -1 || problemField[openX, openY] == -1)
		{
			lost = true;
			openX = rng.Next(solvedField.GetLength(0));
			openY = rng.Next(solvedField.GetLength(1));
			break;
		}
		bool unsolved = false;
		for (int i = 0; i < problemField.GetLength(0); i++)
		{
			for (int j = 0; j < problemField.GetLength(1); j++)
			{
				if (problemField[i, j] == -2)
				{
					unsolved = true;
					i = problemField.GetLength(0);
					break;
				}
			}
		}
		if (!unsolved)
		{
			lost = false;
			break;
		}
		for (int iteracija = 0; iteracija < 2; iteracija++)
		{
			if (iteracija == 1)
			{
				Console.WriteLine("Solved field:");
				PrintField();
				Console.WriteLine("Opened at x:" + openX + " y:" + openY);
				Console.WriteLine("Problem field:");
				PrintProblemField();
				Console.ReadLine();
			}
			for (int i = 0; i < problemField.GetLength(0); i++)
			{
				for (int j = 0; j < problemField.GetLength(1); j++)
				{
					if (problemField[i, j] != -2 && problemField[i, j] != -3)
					{
						int markedNeighbors = CountNeighbors(i, j, problemField, -3);
						int emptyNeighbors = CountNeighbors(i, j, problemField, -2);
						if (emptyNeighbors == problemField[i, j] - markedNeighbors)
							problemField[i, j] = -3;
						else if (markedNeighbors == problemField[i, j] || iteracija == 1)
						{
							FindFirstNeighbor(i, j, problemField, -2, out openX, out openY);
							i = problemField.GetLength(0);
							iteracija = 2;
							break;
						}
					}
				}
			}
		}
	}
} while (lost);


if (lost)
	Console.WriteLine("YOU LOSE!");
else
{
	Console.WriteLine("Solved field:");
	PrintField();
	Console.WriteLine("Opened at x:" + openX + " y:" + openY);
	Console.WriteLine("Problem field:");
	PrintProblemField();
}



void FindFirstNeighbor(int x, int y, int[,] array, int needle, out int xFound, out int yFound)
{
	xFound = -1;
	yFound = -1;
	if (array[x, y] != needle)
	{
		if (x > 0 && array[x - 1, y] == needle)
		{
			xFound = x - 1;
			yFound = y;
			return;
		}
		if (y > 0 && array[x, y - 1] == needle)
		{
			xFound = x;
			yFound = y - 1;
			return;
		}
		if (y > 0 && x > 0 && array[x - 1, y - 1] == needle)
		{
			xFound = x - 1;
			yFound = y - 1;
			return;
		}
		if (y < array.GetLength(1) - 1 && x < array.GetLength(0) - 1 && array[x + 1, y + 1] == needle)
		{
			xFound = x + 1;
			yFound = y + 1;
			return;
		}
		if (y > 0 && x < array.GetLength(0) - 1 && array[x + 1, y - 1] == needle)
		{
			xFound = x + 1;
			yFound = y - 1;
			return;
		}
		if (x > 0 && y < array.GetLength(1) - 1 && array[x - 1, y + 1] == needle)
		{
			xFound = x - 1;
			yFound = y + 1;
			return;
		}
		if (x < array.GetLength(0) - 1 && array[x + 1, y] == needle)
		{
			xFound = x + 1;
			yFound = y;
			return;
		}
		if (y < array.GetLength(1) - 1 && array[x, y + 1] == needle)
		{
			xFound = x;
			yFound = y + 1;
			return;
		}
	}
}

int CountNeighbors(int x, int y, int[,] array, int needle)
{
	int mine = 0;
	if (array[x, y] != needle)
	{
		if (x > 0 && array[x - 1, y] == needle)
			mine++;
		if (y > 0 && array[x, y - 1] == needle)
			mine++;
		if (y > 0 && x > 0 && array[x - 1, y - 1] == needle)
			mine++;
		if (y < array.GetLength(1) - 1 && x < array.GetLength(0) - 1 && array[x + 1, y + 1] == needle)
			mine++;
		if (y > 0 && x < array.GetLength(0) - 1 && array[x + 1, y - 1] == needle)
			mine++;
		if (x > 0 && y < array.GetLength(1) - 1 && array[x - 1, y + 1] == needle)
			mine++;
		if (x < array.GetLength(0) - 1 && array[x + 1, y] == needle)
			mine++;
		if (y < array.GetLength(1) - 1 && array[x, y + 1] == needle)
			mine++;
	}
	return mine;
}

void OpenFields(int x, int y)
{
	if (!(x < solvedField.GetLength(0) && y < solvedField.GetLength(1) && x >= 0 && y >= 0))
		return;
	if (solvedField[x, y] == 0 && problemField[x, y] == -2)
	{
		problemField[x, y] = 0;
		OpenFields(x + 1, y);
		OpenFields(x - 1, y);
		OpenFields(x, y + 1);
		OpenFields(x, y - 1);
		/*OpenFields(x + 1, y + 1);
		OpenFields(x - 1, y + 1);
		OpenFields(x - 1, y - 1);
		OpenFields(x + 1, y - 1);*/
	}
	else
		problemField[x, y] = solvedField[x, y];
}


void PrintField()
{
	for (int i = 0; i < solvedField.GetLength(0); i++)
	{
		for (int j = 0; j < solvedField.GetLength(1); j++)
		{
			Console.Write(solvedField[i, j]+"\t");
		}
		Console.WriteLine();
	}
}
void PrintProblemField()
{
	for (int i = 0; i < problemField.GetLength(0); i++)
	{
		for (int j = 0; j < problemField.GetLength(1); j++)
		{
			Console.Write(problemField[i, j] + "\t");
		}
		Console.WriteLine();
	}
}