//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="GrammarParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public interface IGrammarVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="GrammarParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProgram([NotNull] GrammarParser.ProgramContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>emptyCommand</c>
	/// labeled alternative in <see cref="GrammarParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEmptyCommand([NotNull] GrammarParser.EmptyCommandContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>declarationVariable</c>
	/// labeled alternative in <see cref="GrammarParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDeclarationVariable([NotNull] GrammarParser.DeclarationVariableContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionEvaluation</c>
	/// labeled alternative in <see cref="GrammarParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpressionEvaluation([NotNull] GrammarParser.ExpressionEvaluationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>readInput</c>
	/// labeled alternative in <see cref="GrammarParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitReadInput([NotNull] GrammarParser.ReadInputContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>writeOutput</c>
	/// labeled alternative in <see cref="GrammarParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWriteOutput([NotNull] GrammarParser.WriteOutputContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>blockStatement</c>
	/// labeled alternative in <see cref="GrammarParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBlockStatement([NotNull] GrammarParser.BlockStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>conditionalStatement</c>
	/// labeled alternative in <see cref="GrammarParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConditionalStatement([NotNull] GrammarParser.ConditionalStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>whileCycle</c>
	/// labeled alternative in <see cref="GrammarParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWhileCycle([NotNull] GrammarParser.WhileCycleContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>forCycle</c>
	/// labeled alternative in <see cref="GrammarParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitForCycle([NotNull] GrammarParser.ForCycleContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>plusMinusConcat</c>
	/// labeled alternative in <see cref="GrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPlusMinusConcat([NotNull] GrammarParser.PlusMinusConcatContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>parentheses</c>
	/// labeled alternative in <see cref="GrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParentheses([NotNull] GrammarParser.ParenthesesContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>negation</c>
	/// labeled alternative in <see cref="GrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNegation([NotNull] GrammarParser.NegationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>comparison</c>
	/// labeled alternative in <see cref="GrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComparison([NotNull] GrammarParser.ComparisonContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>or</c>
	/// labeled alternative in <see cref="GrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOr([NotNull] GrammarParser.OrContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>string</c>
	/// labeled alternative in <see cref="GrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitString([NotNull] GrammarParser.StringContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>assignment</c>
	/// labeled alternative in <see cref="GrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignment([NotNull] GrammarParser.AssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>integer</c>
	/// labeled alternative in <see cref="GrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInteger([NotNull] GrammarParser.IntegerContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>float</c>
	/// labeled alternative in <see cref="GrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFloat([NotNull] GrammarParser.FloatContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>boolean</c>
	/// labeled alternative in <see cref="GrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBoolean([NotNull] GrammarParser.BooleanContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>multiplyDivideModulo</c>
	/// labeled alternative in <see cref="GrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMultiplyDivideModulo([NotNull] GrammarParser.MultiplyDivideModuloContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>and</c>
	/// labeled alternative in <see cref="GrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAnd([NotNull] GrammarParser.AndContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>variable</c>
	/// labeled alternative in <see cref="GrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariable([NotNull] GrammarParser.VariableContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>unaryMinus</c>
	/// labeled alternative in <see cref="GrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUnaryMinus([NotNull] GrammarParser.UnaryMinusContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>equality</c>
	/// labeled alternative in <see cref="GrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEquality([NotNull] GrammarParser.EqualityContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ternary</c>
	/// labeled alternative in <see cref="GrammarParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTernary([NotNull] GrammarParser.TernaryContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="GrammarParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitType([NotNull] GrammarParser.TypeContext context);
}
