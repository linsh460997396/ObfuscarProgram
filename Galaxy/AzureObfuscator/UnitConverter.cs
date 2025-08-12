using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public class UnitConverter
{
    public static string Go(string code,int idCounter)
    {
        StringBuilder result = new StringBuilder();
        string z = ", 0";

        try
        {
            string[] lines = code.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                Match match = Regex.Match(
                    line,
                    @"UnitCreate\((\d+),\s*""([^""]+)"",\s*(\d+),\s*[^,]+,\s*Point\(([^)]+)\),\s*([^)]+)\);"
                );

                if (match.Success)
                {
                    string newFormat =
                        $"<ObjectUnit Id=\"{idCounter++}\" Position=\"{match.Groups[4].Value + z}\" " +
                        $"Scale=\"1,1,1\" UnitType=\"{match.Groups[2].Value}\"> </ObjectUnit>";

                    result.AppendLine(newFormat);
                }
            }

            return result.ToString();
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    //public static void Go()
    //{
    //    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    //    string inputFile = Path.Combine(desktopPath, "T.xml");
    //    string outputFile = Path.Combine(desktopPath, "output.xml");
    //    string z = ", 0";

    //    try
    //    {
    //        // 读取输入文件
    //        string[] lines = File.ReadAllLines(inputFile);
    //        using (StreamWriter writer = new StreamWriter(outputFile))
    //        {
    //            int idCounter = 2001;

    //            foreach (string line in lines)
    //            {
    //                // 使用正则表达式提取参数
    //                Match match = Regex.Match(
    //                    line,
    //                    @"UnitCreate\((\d+),\s*""([^""]+)"",\s*(\d+),\s*[^,]+,\s*Point\(([^)]+)\),\s*([^)]+)\);"
    //                );

    //                if (match.Success)
    //                {
    //                    // 构造新的XML格式
    //                    string newFormat =
    //                        $"<ObjectUnit Id=\"{idCounter++}\" Position=\"{match.Groups[4].Value + z}\" "
    //                        + $"Scale=\"1,1,1\" UnitType=\"{match.Groups[2].Value}\"> </ObjectUnit>";

    //                    // 输出到控制台和文件
    //                    Console.WriteLine(newFormat);
    //                    writer.WriteLine(newFormat);
    //                }
    //            }
    //        }

    //        Console.WriteLine($"转换完成！结果已保存到: {outputFile}");
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"发生错误: {ex.Message}");
    //    }

    //    Console.WriteLine("按任意键退出...");
    //    Console.ReadKey();
    //}
}
