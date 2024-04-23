namespace Project;

public class TypeCheckVisitor : GrammarBaseVisitor<Type>
{
    private readonly Dictionary<string, Type> _variables = new();
    public List<string> Errors { get; } = [];

    // type VARIABLE (COMMA VARIABLE)* SEMICOLON
    // declaration of variables, all these variables have the same type type. It can be one of: int, float, bool, string.
    public override Type VisitDeclarationVariable(GrammarParser.DeclarationVariableContext context)
    {
        var type = Visit(context.type());
        foreach(var variable in context.VARIABLE())
        {
            if (_variables.ContainsKey(variable.GetText()))
            {
                Errors.Add($"line: {context.Start.Line} - Variable was already declared - {variable.GetText()}");
            }
            else
            {
                _variables.Add(variable.GetText(), type);
            }
        }

        return typeof(void);
    }

    // IF '(' expression ')' statement (ELSE statement)?
    // conditional statement - condition is an expression with a type: bool. The else part of the statement is optional.
    public override Type VisitConditionalStatement(GrammarParser.ConditionalStatementContext context)
    {
        if (Visit(context.expression()) != typeof(bool))
        {
            Errors.Add($"line: {context.Start.Line} - Condition must be of type bool - {context.expression().GetText()}");
        }
        Visit(context.statement(0));
        if (context.statement().Length == 2)
        {
            Visit(context.statement(1));
        }
        
        return typeof(void);
    }

    // WHILE '(' expression ')' statement
    // a cycle - condition must be a bool expression. This cycle repeats the given statement while the condition holds (it is true).
    public override Type VisitWhileCycle(GrammarParser.WhileCycleContext context)
    {
        if (Visit(context.expression()) != typeof(bool))
        {
            Errors.Add($"line: {context.Start.Line} - Condition must be of type bool - {context.expression().GetText()}");
        }
        Visit(context.statement());
        
        return typeof(void);
    }

    // FOR '(' expression SEMICOLON expression SEMICOLON expression ')' statement 
    // middle part must be of type bool.
    public override Type VisitForCycle(GrammarParser.ForCycleContext context)
    {
        Visit(context.expression(0));
        if (Visit(context.expression(1)) != typeof(bool))
        {
            Errors.Add($"line: {context.Start.Line} - Condition must be of type bool - {context.expression(1).GetText()}");
        }
        Visit(context.expression(2));
        Visit(context.statement());
        
        return typeof(void);
    }

    // expression op=(PLUS | MINUS | CONCAT) expression
    public override Type VisitPlusMinusConcat(GrammarParser.PlusMinusConcatContext context)
    {
        var type1 = Visit(context.expression(0));
        var type2 = Visit(context.expression(1));
        
        if (type1 == typeof(string) && type2 == typeof(string) && context.op == context.CONCAT()?.Symbol)
        {
            return typeof(string);
        }
        
        if (context.op == context.CONCAT()?.Symbol)
        {
            Errors.Add($"line: {context.Start.Line} - Concatenation can be applied only to strings - {context.GetText()}");
            return typeof(string);
        }
        
        if (type1 == typeof(int) && type2 == typeof(int))
        {
            return typeof(int);
        }
        
        if (type1 == typeof(float) && type2 == typeof(float))
        {
            return typeof(float);
        }
        
        if ((type1 == typeof(int) || type1 == typeof(float)) && (type2 == typeof(int) || type2 == typeof(float)))
        {
            return typeof(float);
        }
        
        Errors.Add($"line: {context.Start.Line} - Plus and minus can be applied only to int or float  - {context.GetText()}");
        return type2;
    }

    // '(' expression ')'
    public override Type VisitParentheses(GrammarParser.ParenthesesContext context)
    {
        return Visit(context.expression());
    }

    // NEGATION expression
    public override Type VisitNegation(GrammarParser.NegationContext context)
    {
        if (Visit(context.expression()) != typeof(bool))
        {
            Errors.Add($"line: {context.Start.Line} - Negation can be applied only to bool - {context.expression().GetText()}");
        }
        
        return typeof(bool);
    }

    // expression op=(LESS | GREATER) expression
    public override Type VisitComparison(GrammarParser.ComparisonContext context)
    {
        var type1 = Visit(context.expression(0));
        var type2 = Visit(context.expression(1));
        
        if (!((type1 == typeof(int) || type1 == typeof(float)) && (type2 == typeof(int) || type2 == typeof(float))))
        {
            Errors.Add($"line: {context.Start.Line} - Comparison can be applied only to int or float - {context.GetText()}");
        }
        
        return typeof(bool);
    }

    // expression OR expression 
    public override Type VisitOr(GrammarParser.OrContext context)
    {
        var type1 = Visit(context.expression(0));
        var type2 = Visit(context.expression(1));
        if (!(type1 == typeof(bool) && type2 == typeof(bool)))
        {
            Errors.Add($"line: {context.Start.Line} - OR can be applied only to bool - {context.GetText()}");
        }
        
        return typeof(bool);
    }

    // STRING
    public override Type VisitString(GrammarParser.StringContext context)
    {
        return typeof(string);
    }

    // <assoc=right> VARIABLE ASSIGN expression 
    public override Type VisitAssignment(GrammarParser.AssignmentContext context)
    {
        var variable = context.VARIABLE().GetText();
        if (!_variables.ContainsKey(variable))
        {
            Errors.Add($"line: {context.Start.Line} - Variable was not declared - {variable}");
            return typeof(void);
        }
        
        var type = Visit(context.expression());
        if (_variables[variable] == typeof(float) && type == typeof(int))
        {
            return typeof(float);
        }
        
        if (_variables[variable] != type)
        {
            Errors.Add($"line: {context.Start.Line} - Variable type does not match the assigned value type - {context.GetText()}");
        }
        
        return type;
    }

    // INT
    public override Type VisitInteger(GrammarParser.IntegerContext context)
    {
        return typeof(int);
    }
    
    // FLOAT
    public override Type VisitFloat(GrammarParser.FloatContext context)
    {
        return typeof(float);
    }

    // BOOL
    public override Type VisitBoolean(GrammarParser.BooleanContext context)
    {
        return typeof(bool);
    }

    // expression op=(MULTIPLY | DIVIDE | MODULO) expression
    public override Type VisitMultiplyDivideModulo(GrammarParser.MultiplyDivideModuloContext context)
    {
        var type1 = Visit(context.expression(0));
        var type2 = Visit(context.expression(1));
        
        if (type1 == typeof(int) && type2 == typeof(int))
        {
            return typeof(int);
        }
        
        if (context.op == context.MODULO()?.Symbol)
        {
            Errors.Add($"line: {context.Start.Line} - Modulo can be applied only to int - {context.GetText()}");
            return typeof(int);
        }
        
        if (type1 == typeof(float) && type2 == typeof(float))
        {
            return typeof(float);
        }
        
        if (type1 == typeof(int) && type2 == typeof(float) || type1 == typeof(float) && type2 == typeof(int))
        {
            return typeof(float);
        }
        
        Errors.Add($"line: {context.Start.Line} - Multiply and divide can be applied only to int or float - {context.GetText()}");
        return type2;
    }

    // expression AND expression
    public override Type VisitAnd(GrammarParser.AndContext context)
    {
        var type1 = Visit(context.expression(0));
        var type2 = Visit(context.expression(1));
        if (!(type1 == typeof(bool) && type2 == typeof(bool)))
        {
            Errors.Add($"line: {context.Start.Line} - AND can be applied only to bool - {context.GetText()}");
        }
        return typeof(bool);
    }

    // VARIABLE
    public override Type VisitVariable(GrammarParser.VariableContext context)
    {
        if (!_variables.ContainsKey(context.GetText()))
        {
            Errors.Add($"line: {context.Start.Line} - Variable was not declared - {context.GetText()}");
            return typeof(void);
        }
        return _variables[context.GetText()];
    }

    // MINUS expression
    public override Type VisitUnaryMinus(GrammarParser.UnaryMinusContext context)
    {
        var type = Visit(context.expression());
        if (type == typeof(int))
        {
            return typeof(int);
        }

        if (type == typeof(float))
        {
            return typeof(float);
        }

        Errors.Add($"line: {context.Start.Line} - Unary minus can be applied only to int or float - {context.expression().GetText()}");
        return type;
    }

    // expression op=(EQUAL | NOT_EQUAL) expression
    public override Type VisitEquality(GrammarParser.EqualityContext context)
    {
        var type1 = Visit(context.expression(0));
        var type2 = Visit(context.expression(1));
        
        if (!((type1 == typeof(int) || type1 == typeof(float) || type1 == typeof(string)) && 
            (type2 == typeof(int) || type2 == typeof(float) || type2 == typeof(string))))
        {
            Errors.Add($"line: {context.Start.Line} - Equality can be applied only to int, float or string - {context.GetText()}");
        }
        
        return typeof(bool);
    }

    public override Type VisitTernary(GrammarParser.TernaryContext context)
    {
        var type1 = Visit(context.expression(0));
        var type2 = Visit(context.expression(1));
        var type3 = Visit(context.expression(2));
        
        if (type1 != typeof(bool))
        {
            Errors.Add($"line: {context.Start.Line} - Ternary condition must be of type bool - {context.expression(0).GetText()}");
        }
        
        if (type2 == type3)
        {
            return type2;
        }
        
        if (type2 == typeof(int) && type3 == typeof(float) || type2 == typeof(float) && type3 == typeof(int))
        {
            return typeof(float);
        }

        Errors.Add($"line: {context.Start.Line} - Ternary branches must have the same type - {context.GetText()}");
        return type2;
    }

    // type
    public override Type VisitType(GrammarParser.TypeContext context)
    {
        switch (context.GetText())
        {
            case "int":
                return typeof(int);
            case "float":
                return typeof(float);
            case "bool":
                return typeof(bool);
            case "string":
                return typeof(string);
            default:
                Errors.Add($"line: {context.Start.Line} - Unknown type - {context.GetText()}");
                return typeof(void);
        }
    }
}