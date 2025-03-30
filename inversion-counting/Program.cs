namespace inversion_counting
{
    public static class InputOutput
    { 
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

        public static void WriteResult(Dictionary<int, int> dictionary, int id ,string fileName)
        {
            StreamWriter streamWriter = new StreamWriter(fileName);
            int length = dictionary.Count;
            streamWriter.WriteLine(id);
            streamWriter.Flush();
            foreach (var el in dictionary)
            {
                streamWriter.WriteLine($"{el.Key} {el.Value}");
                streamWriter.Flush();
            }
        }

        public static int GetUserId(int[,] array)
        {
            int usersAmount = array.GetLength(0);
            int userId;
            Console.Write("Enter user's id : ");
            userId = Convert.ToInt32(Console.ReadLine());
            if (userId < 1 || userId > usersAmount)
            {
                Console.WriteLine("Wrong user's id");
                throw new IndexOutOfRangeException("Wrong user's id");
            }
            return userId;
        }

        public static int[] GetArrayById(int id, int[,] array)
        {
            int length = array.GetLength(1) - 1;
            int[] arr = new int[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = array[id - 1, i + 1];
            }
            return arr;
        }

        public static int[] GetArrayForCounting(int[] arr1, int[] arr2)
        {
            int n = arr1.Length;
            var pos = new Dictionary<int, int>();

            for (int i = 0; i < n; i++)
            {
                pos.Add(arr1[i], arr2[i]);
            }

            int[] resultArray = new int[n];

            for (int i = 0; i < n; i++)
            {
                resultArray[i] = pos[i + 1];
            }
            return resultArray;
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
            int z = MergeAndCountSplitInv(A, left, right);

            return x + y + z;
        }
    }

    public static class Sorting
    {
       public static Dictionary<int, int> BubbleSort(Dictionary<int, int> dict)
        {
            var keys = new List<int>(dict.Keys);
            var values = new List<int>(dict.Values);
            int n = values.Count;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (values[j] > values[j + 1]) 
                    {
                        (values[j], values[j + 1]) = (values[j + 1], values[j]);
                        (keys[j], keys[j + 1]) = (keys[j + 1], keys[j]);
                    }
                }
            }
            var sortedDict = new Dictionary<int, int>();
            for (int i = 0; i < keys.Count; i++)
            {
                sortedDict[keys[i]] = values[i];
            }
            return sortedDict;
        }
    }

    public static class Program
    {
        public static void Main()
        {
            int[,] array = InputOutput.GetArrayFromFile("./input_720_6.txt");
            int id = InputOutput.GetUserId(array);
            
            int[] userArr = InputOutput.GetArrayById(id, array);
             
            var resultDict = new Dictionary<int, int>();
            
            for (int i = 0; i < array.GetLength(0); i++)
            {
                int currentId = array[i, 0];
                if (currentId == id)
                    continue;
                int[] arr = InputOutput.GetArrayById(currentId, array);
                int[] arrForCounting = InputOutput.GetArrayForCounting(userArr, arr);
                
                resultDict.Add(currentId, Inversions.SortAndCountInv(arrForCounting));
            }
            
            resultDict = Sorting.BubbleSort(resultDict);
            
            InputOutput.WriteResult(resultDict, id, "result.txt");
            
        }
    }

}