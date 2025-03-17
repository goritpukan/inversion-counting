namespace inversion_counting
{
    public static class InputOutput
    {
        public static void PrintArray(int[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write(array[i, j] + " ");
                }

                Console.WriteLine();
            }
        }
        public static int[,] GetArrayFromFile(string fileName)
        {
            StreamReader streamReader = new StreamReader(fileName);
            string? line = streamReader.ReadLine();
            if (string.IsNullOrEmpty(line)) throw new FileLoadException();
            int usersAmount = Convert.ToInt32(line.Split(" ")[0]);
            int filmsAmount = Convert.ToInt32(line.Split(" ")[1]);
            int[,] array = new int[usersAmount, filmsAmount + 1];
            for (int i = 0; i < usersAmount; i++)
            {
                line = streamReader.ReadLine();
                if (string.IsNullOrEmpty(line)) break;

                string[] splitedLine = line.Split(" ");
                int userId = Convert.ToInt32(splitedLine[0]);

                array[i, 0] = userId;
                for (int j = 1; j <= filmsAmount; j++)
                {
                    array[i, j] = Convert.ToInt32(splitedLine[j]);
                }
            }

            return array;
        }

        public static void WriteResult(int[,] array, string fileName)
        {
            StreamWriter streamWriter = new StreamWriter(fileName);
            int length = array.GetLength(0);
            for (int i = 0; i < length; i++)
            {
                streamWriter.WriteLine($"{array[i, 0]} {array[i, 1]}");
                streamWriter.Flush();
            }
        }

        public static int GetUserId(int[,] array)
        {
            int usersAmount = array.GetLength(0);
            int userId;
            Console.WriteLine("Enter user's id");
            userId = Convert.ToInt32(Console.ReadLine());
            if (userId < 1 || userId > usersAmount)
            {
                Console.WriteLine("Wrong user's id");
                throw new IndexOutOfRangeException("Wrong user's id");
            }
            return userId;
        }

        public static Dictionary<int, int> GetUserDictionary(int userId, int[,] array)
        {
            int filmsAmount = array.GetLength(1);
            Dictionary<int, int> userDictionary = new Dictionary<int, int>();
            for (int i = 0; i < filmsAmount - 1; i++)
            {
                userDictionary[i] = array[userId - 1, i + 1];
            }
            return userDictionary;
        }
    }

    public static class Inversions
    {
        private static int MergeAndCountSplitInv(int[] A, int[] L, int[] R)
        {
            int n1 = L.Length, n2 = R.Length;
            
            int[] left = new int[n1 + 1];
            int[] right = new int[n2 + 1];
            
            for (int k = 0; k < n1; k++) left[k] = L[k]; 
            for (int p = 0; p < n2; p++) right[p] = R[p];
            
            left[n1] = int.MaxValue;
            right[n2] = int.MaxValue;
            
            int i = 0, j = 0, c = 0;
            for (int k = 0; k < A.Length; k++)
            {
                if (left[i] <= right[j])
                {
                    A[k] = left[i];
                    i++;
                }
                else
                {
                    A[k] = right[j];
                    j++;
                    c += n1 - i;
                }
            }

            return c;
        }

        public static int SortAndCountInv(int[] A)
        {
            int n = A.Length;
            if (n == 1) return 0;
            int mid = n / 2;
            
            int[] left = new int[mid];
            int[] right = new int[n - mid];
            
            Array.Copy(A, 0, left, 0, mid);
            Array.Copy(A, mid, right, 0, n - mid);

            int x = SortAndCountInv(left);
            int y = SortAndCountInv(right);
            int z = Inversions.MergeAndCountSplitInv(A, left, right);

            return x + y + z;
        }
    }
    public static class Program
    {
        public static void Main()
        {
            int[,] array = InputOutput.GetArrayFromFile("/Users/front-end/RiderProjects/inversion-counting/inversion-counting/input_720_6.txt");
            Dictionary<int, int> userDictionary = InputOutput.GetUserDictionary(InputOutput.GetUserId(array), array);
            InputOutput.WriteResult(array, "result.txt");
        }
    }

}