using System;
using System.IO;
using System.Net;
using System.Text;

namespace Lab1
{
  internal class Program
  {
    public static void Main(string[] args)
    {
      string pathToResourceFile = "..\\..\\resources\\resource.db";
      string pathToResultFile = "..\\..\\result.db";

      try
      {
        StreamReader fileReader = new StreamReader(pathToResourceFile);
        StreamWriter fileWriter = new StreamWriter(pathToResultFile);

        string name = fileReader.ReadLine(),
          lastName = fileReader.ReadLine(),
          tripledString = StringStuffing(name),
          reversedString = ReverseString(name),
          concatedString = name + lastName;
        int stringLength = name.Length;
        StringBuilder builder = new StringBuilder("name : ").Append(name).Append("\nsrename : ").Append(lastName)
          .Append("\ntripled : ").Append(tripledString).Append("\nreversed : ").Append(reversedString)
          .Append("\nconcated : ").Append(concatedString).Append("\nlength : ").Append(stringLength);
        fileWriter.WriteLine(builder.ToString());
        fileReader.Close();
        fileWriter.Close();
      }
      catch (FileNotFoundException e)
      {
        Console.WriteLine(e);
        throw;
      }    
    }

    private static string ReverseString(string res)
    {
      char[] resCharArray = res.ToCharArray();
      Array.Reverse(resCharArray);
      return new string(resCharArray);
    }

    private static string StringStuffing(string res, int times = 1)
    {
      StringBuilder builder = new StringBuilder(res);

      for (int i = 1; i < times; ++i)
      {
        builder.Append(res);
      }

      return builder.ToString();
    }
  }
}