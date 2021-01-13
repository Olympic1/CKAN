using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CKAN;
using CKAN.CmdLine;

namespace Tests.CmdLine
{
    internal static class Utils
    {
        /// <summary>
        /// Splits a string into substrings based on the strings in an array.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>An array which contains the substrings in the string separated with new lines.</returns>
        public static string[] ToLines(this string str)
        {
            return str.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        /// <summary>
        /// Splits a string into substrings based on the strings in an array. Removes all empty entries.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>An array which contains the substrings in the string separated with new lines and removed all empty entries.</returns>
        public static string[] ToLinesNoEmpty(this string str)
        {
            return str.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Removes all leading and trailing white-spaces of every string in the array.
        /// </summary>
        /// <param name="array">The array to trim.</param>
        /// <returns>An array of the strings that remain after all white-spaces are removed.</returns>
        public static string[] TrimStringArray(this IEnumerable<string> array)
        {
            return array.Select(item => item.Trim()).ToArray();
        }
    }

    internal class ConsoleOutput : IDisposable
    {
        /// <summary>
        /// Gets the exit code that was returned from the command line interface.
        /// </summary>
        public int ExitCode { get; private set; }

        /// <summary>
        /// Gets the output that was written into the command line interface.
        /// </summary>
        public string[] Output { get; private set; }

        private readonly TextWriter _normalOutput;
        private readonly TextWriter _errorOutput;
        private readonly StringWriter _stringWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tests.CmdLine.ConsoleOutput"/> class.
        /// </summary>
        public ConsoleOutput()
        {
            _stringWriter = new StringWriter();
            _normalOutput = Console.Out;
            _errorOutput = Console.Error;
            Console.SetOut(_stringWriter);
            Console.SetError(_stringWriter);
        }

        /// <inheritdoc cref="System.IDisposable.Dispose"/>
        public void Dispose()
        {
            Console.SetOut(_normalOutput);
            Console.SetError(_errorOutput);
            _stringWriter.Dispose();
        }

        /// <summary>
        /// Parses the arguments into the command line.
        /// Sets the <see cref="Tests.CmdLine.ConsoleOutput.ExitCode"/> and <see cref="Tests.CmdLine.ConsoleOutput.Output"/> with the returned values of the command line.
        /// </summary>
        /// <param name="args">The arguments to parse into the command line.</param>
        /// <param name="manager">The dummy manager.</param>
        public void ParseArguments(string args, GameInstanceManager manager = null)
        {
            ExitCode = MainClass.MainForTests(args.Split(), manager);
            Output = GetOutput().ToLinesNoEmpty().TrimStringArray();
        }

        /// <summary>
        /// Gets the output of the console that was passed via <see cref="System.Console.WriteLine()"/>.
        /// </summary>
        /// <returns>The output as a <see langword="string"/>.</returns>
        public string GetOutput()
        {
            return _stringWriter.ToString();
        }
    }
}