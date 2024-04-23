using Antlr4.Runtime;

namespace Project;

public class ErrorListener : BaseErrorListener
{
    public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    {
        IList<string> stack = ((Parser)recognizer).GetRuleInvocationStack();
        _ = stack.Reverse();

        Console.Error.WriteLine("rule stack: " + String.Join(", ", stack));
        Console.Error.WriteLine("line " + line + ":" + charPositionInLine + " at " + offendingSymbol + ": " + msg);
        // Environment.Exit(1);
    }
}