using System.Text;

namespace Project;

public class InstructionVisitor : GrammarBaseVisitor<string>
{
    private readonly Dictionary<string, string> _variables = new();
    private long _labelCounter;
    private readonly TypeCheckVisitor _typeCheckVisitor = new();
    
    public override string VisitProgram(GrammarParser.ProgramContext context)
    {
        var sb = new StringBuilder();
        foreach (var statement in context.statement())
        {
            sb.Append(Visit(statement));
        }
        return sb.ToString();
    }

    public override string VisitEmptyCommand(GrammarParser.EmptyCommandContext context)
    {
        return "";
    }

    public override string VisitDeclarationVariable(GrammarParser.DeclarationVariableContext context)
    {
        var sb = new StringBuilder();
        var type = Visit(context.type());
        foreach(var variable in context.VARIABLE())
        {
            switch (type)
            {
                case "int":
                    sb.AppendLine("push I 0");
                    _variables.TryAdd(variable.GetText(), "I");
                    break;
                case "float":
                    sb.AppendLine("push F 0.0");
                    _variables.TryAdd(variable.GetText(), "F");
                    break;
                case "bool":
                    sb.AppendLine("push B false");
                    _variables.TryAdd(variable.GetText(), "B");
                    break;
                case "string":
                    sb.AppendLine("push S \"\"");
                    _variables.TryAdd(variable.GetText(), "S");
                    break;
            }
            sb.AppendLine($"save {variable.GetText()}");
        }

        return sb.ToString();
    }

    public override string VisitExpressionEvaluation(GrammarParser.ExpressionEvaluationContext context)
    {
        var sb = new StringBuilder();
        sb.Append(Visit(context.expression()));
        sb.AppendLine("pop");
        return sb.ToString();
    }

    public override string VisitReadInput(GrammarParser.ReadInputContext context)
    {
        var sb = new StringBuilder();
        foreach (var variable in context.VARIABLE())
        {
            _variables.TryGetValue(variable.GetText(), out var type);
            sb.AppendLine($"read {type}");
            sb.AppendLine($"save {variable.GetText()}");
        }
        return sb.ToString();
    }

    public override string VisitWriteOutput(GrammarParser.WriteOutputContext context)
    {
        var sb = new StringBuilder();
        foreach (var expression in context.expression())
        {
            sb.Append(Visit(expression));
        }
        sb.AppendLine($"print {context.expression()?.Length}");
        return sb.ToString();
    }

    public override string VisitBlockStatement(GrammarParser.BlockStatementContext context)
    {
        var sb = new StringBuilder();
        foreach (var statement in context.statement())
        {
            sb.Append(Visit(statement));
        }
        return sb.ToString();
    }

    public override string VisitConditionalStatement(GrammarParser.ConditionalStatementContext context)
    {
        var sb = new StringBuilder();
        sb.Append(Visit(context.expression()));
        var labelOne = _labelCounter++;
        var labelTwo = _labelCounter++;
        sb.AppendLine($"fjmp {labelOne}");
        sb.Append(Visit(context.statement(0)));
        sb.AppendLine($"jmp {labelTwo}");
        sb.AppendLine($"label {labelOne}");
        if (context.statement().Length > 1)
        {
            sb.Append(Visit(context.statement(1)));
        }
        sb.AppendLine($"label {labelTwo}");
        return sb.ToString();
    }

    public override string VisitWhileCycle(GrammarParser.WhileCycleContext context)
    {
        var sb = new StringBuilder();
        var labelOne = _labelCounter++;
        var labelTwo = _labelCounter++;
        sb.AppendLine($"label {labelOne}");
        sb.Append(Visit(context.expression()));
        sb.AppendLine($"fjmp {labelTwo}");
        sb.Append(Visit(context.statement()));
        sb.AppendLine($"jmp {labelOne}");
        sb.AppendLine($"label {labelTwo}");
        return sb.ToString();
    }

    public override string VisitForCycle(GrammarParser.ForCycleContext context)
    {
        var sb = new StringBuilder();
        sb.Append(Visit(context.expression(0)));
        sb.AppendLine("pop");
        var labelOne = _labelCounter++;
        var labelTwo = _labelCounter++;
        sb.AppendLine($"label {labelOne}");
        sb.Append(Visit(context.expression(1)));
        sb.AppendLine($"fjmp {labelTwo}");
        sb.Append(Visit(context.statement()));
        sb.Append(Visit(context.expression(2)));
        sb.AppendLine("pop");
        sb.AppendLine($"jmp {labelOne}");
        sb.AppendLine($"label {labelTwo}");
        return sb.ToString();
    }

    public override string VisitPlusMinusConcat(GrammarParser.PlusMinusConcatContext context)
    {
        var sb = new StringBuilder();
        var leftType = _typeCheckVisitor.Visit(context.expression(0));
        var rightType = _typeCheckVisitor.Visit(context.expression(1));
        sb.Append(Visit(context.expression(0)));
        if ((context.op == context.PLUS()?.Symbol || context.op == context.MINUS()?.Symbol) && leftType == typeof(int) && rightType == typeof(float))
        {
            sb.AppendLine("itof");
        }
        sb.Append(Visit(context.expression(1)));
        if ((context.op == context.PLUS()?.Symbol || context.op == context.MINUS()?.Symbol) && leftType == typeof(float) && rightType == typeof(int))
        {
            sb.AppendLine("itof");
        }
        
        switch (context.op.Text)
        {
            case "+":
                sb.AppendLine("add");
                break;
            case "-":
                sb.AppendLine("sub");
                break;
            case ".":
                sb.AppendLine("concat");
                break;
        }
        return sb.ToString();
    }

    public override string VisitParentheses(GrammarParser.ParenthesesContext context)
    {
        return Visit(context.expression());
    }

    public override string VisitNegation(GrammarParser.NegationContext context)
    {
        var sb = new StringBuilder();
        sb.Append(Visit(context.expression()));
        sb.AppendLine("not");
        return sb.ToString();
    }

    public override string VisitComparison(GrammarParser.ComparisonContext context)
    {
        var sb = new StringBuilder();
        var leftType = _typeCheckVisitor.Visit(context.expression(0));
        var rightType = _typeCheckVisitor.Visit(context.expression(1));
        sb.Append(Visit(context.expression(0)));
        if (leftType == typeof(int) && rightType == typeof(float))
        {
            sb.AppendLine("itof");
            leftType = typeof(float);
        }
        sb.Append(Visit(context.expression(1)));
        if (leftType == typeof(float) && rightType == typeof(int))
        {
            sb.AppendLine("itof");
        }
        var type = "";
        if (leftType == typeof(int))
        {
            type = "I";
        }
        else if (leftType == typeof(float))
        {
            type = "F";
        }
        else if (leftType == typeof(string))
        {
            type = "S";
        }
        else if (leftType == typeof(bool))
        {
            type = "B";
        }
        
        switch (context.op.Text)
        {
            case "<":
                sb.AppendLine($"lt {type}");
                break;
            case ">":
                sb.AppendLine($"gt {type}");
                break;
        }
        return sb.ToString();
    }

    public override string VisitOr(GrammarParser.OrContext context)
    {
        var sb = new StringBuilder();
        sb.Append(Visit(context.expression(0)));
        sb.Append(Visit(context.expression(1)));
        sb.AppendLine("or");
        return sb.ToString();
    }

    public override string VisitString(GrammarParser.StringContext context)
    {
        return $"push S {context.STRING().GetText()}\n";
    }

    public override string VisitAssignment(GrammarParser.AssignmentContext context)
    {
        var sb = new StringBuilder();
        sb.Append(Visit(context.expression()));
        var expressionType = _typeCheckVisitor.Visit(context.expression());
        _variables.TryGetValue(context.VARIABLE().GetText(), out var variableType);
        if (expressionType == typeof(int) && variableType == "F")
        {
            sb.AppendLine("itof");
        }
        sb.AppendLine($"save {context.VARIABLE().GetText()}");
        sb.AppendLine($"load {context.VARIABLE().GetText()}");
        
        return sb.ToString();
    }

    public override string VisitInteger(GrammarParser.IntegerContext context)
    {
        return $"push I {context.INT().GetText()}\n";
    }

    public override string VisitFloat(GrammarParser.FloatContext context)
    {
        return $"push F {context.FLOAT().GetText()}\n";
    }

    public override string VisitBoolean(GrammarParser.BooleanContext context)
    {
        return $"push B {context.BOOL().GetText()}\n";
    }

    public override string VisitMultiplyDivideModulo(GrammarParser.MultiplyDivideModuloContext context)
    {
        var sb = new StringBuilder();
        var leftType = _typeCheckVisitor.Visit(context.expression(0));
        var rightType = _typeCheckVisitor.Visit(context.expression(1));
        sb.Append(Visit(context.expression(0)));
        if ((context.op == context.MULTIPLY()?.Symbol || context.op == context.DIVIDE()?.Symbol) && leftType == typeof(int) && rightType == typeof(float))
        {
            sb.AppendLine("itof");
        }
        sb.Append(Visit(context.expression(1)));
        if ((context.op == context.MULTIPLY()?.Symbol || context.op == context.DIVIDE()?.Symbol) && leftType == typeof(float) && rightType == typeof(int))
        {
            sb.AppendLine("itof");
        }
        
        switch (context.op.Text)
        {
            case "*":
                sb.AppendLine("mul");
                break;
            case "/":
                sb.AppendLine("div");
                break;
            case "%":
                sb.AppendLine("mod");
                break;
        }
        return sb.ToString();
    }

    public override string VisitAnd(GrammarParser.AndContext context)
    {
        var sb = new StringBuilder();
        sb.Append(Visit(context.expression(0)));
        sb.Append(Visit(context.expression(1)));
        sb.AppendLine("and");
        return sb.ToString();
    }

    public override string VisitVariable(GrammarParser.VariableContext context)
    {
        return $"load {context.VARIABLE().GetText()}\n";
    }

    public override string VisitUnaryMinus(GrammarParser.UnaryMinusContext context)
    {
        var sb = new StringBuilder();
        sb.Append(Visit(context.expression()));
        sb.AppendLine("uminus");
        return sb.ToString();
    }

    public override string VisitEquality(GrammarParser.EqualityContext context)
    {
        var sb = new StringBuilder();
        var leftType = _typeCheckVisitor.Visit(context.expression(0));
        var rightType = _typeCheckVisitor.Visit(context.expression(1));
        sb.Append(Visit(context.expression(0)));
        if (leftType == typeof(int) && rightType == typeof(float))
        {
            sb.AppendLine("itof");
            leftType = typeof(float);
        }
        sb.Append(Visit(context.expression(1)));
        if (leftType == typeof(float) && rightType == typeof(int))
        {
            sb.AppendLine("itof");
        }

        string type = "";
        if (leftType == typeof(int))
        {
            type = "I";
        }
        if (leftType == typeof(float))
        {
            type = "F";
        }
        if (leftType == typeof(string))
        {
            type = "S";
        }
        if (leftType == typeof(bool))
        {
            type = "B";
        }
        
        sb.AppendLine($"eq {type}");
        if (context.op.Text == "!=")
        {
            sb.AppendLine("not");
        }
        return sb.ToString();
    }

    public override string VisitTernary(GrammarParser.TernaryContext context)
    {
        var sb = new StringBuilder();
        sb.Append(Visit(context.expression(0)));
        var labelOne = _labelCounter++;
        var labelTwo = _labelCounter++;
        sb.AppendLine($"fjmp {labelOne}");
        sb.Append(Visit(context.expression(1)));
        sb.AppendLine($"jmp {labelTwo}");
        sb.AppendLine($"label {labelOne}");
        sb.Append(Visit(context.expression(2)));
        sb.AppendLine($"label {labelTwo}");
        return sb.ToString();
    }

    public override string VisitType(GrammarParser.TypeContext context)
    {
        return context.GetText();
    }
}