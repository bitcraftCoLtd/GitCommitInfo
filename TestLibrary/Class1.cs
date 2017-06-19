using System;

namespace TestLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class Class1
    {
        public void Func()
        {
            Console.WriteLine($"{GitCommitInfo.Instance.ShortCommitHash}");
        }
    }
}
