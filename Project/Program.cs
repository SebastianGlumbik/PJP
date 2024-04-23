using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Globalization;

namespace Project;

public static class Program
{
    /// <summary>
    /// Parse the input file, generates the instructions (output) and runs the interpreter.
    /// </summary>
    /// <param name="input">File to parse (required)</param>
    /// <param name="output">Specify output file (optional)</param>
    public static int Main(string? input, string? output)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        try
        {
            if (input == null)
            {
                throw new Exception("Input file not provided. (--input <file>)");
            }
            // Console.WriteLine("Parsing: " + input);
            
            var streamReader = new StreamReader(input);
            var antlrInputStream = new AntlrInputStream(streamReader);
            var lexer = new GrammarLexer(antlrInputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new GrammarParser(tokens);
        
            var errorListener = new ErrorListener();

            // There is no need to add a custom error listener, the default one will do just fine.
            //parser.AddErrorListener(errorListener);

            IParseTree tree = parser.program();

            if (parser.NumberOfSyntaxErrors != 0) 
                return 1;
        
            var typeCheck = new TypeCheckVisitor();
            typeCheck.Visit(tree);
            if (typeCheck.Errors.Count > 0)
            {
                foreach (var error in typeCheck.Errors)
                {
                    Console.WriteLine(error);
                }
                return 1;
            }
        
            // Console.WriteLine(tree.ToStringTree(parser));
        
            var instructionVisitor = new InstructionVisitor();
            var instructions = instructionVisitor.Visit(tree);
        
            output ??= "output/" + Path.GetFileName(input);
            var directory = Path.GetDirectoryName(output);
            //Create the path
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }
            var streamWriter = new StreamWriter(output);
            streamWriter.Write(instructions);
            streamWriter.Close();

            streamReader = new StreamReader(output);
            instructions = streamReader.ReadToEnd();
            streamReader.Close();
            var interpreter = new Interpreter(instructions);
            interpreter.Run();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Environment.Exit(1);
        }

        return 0;
    }
}