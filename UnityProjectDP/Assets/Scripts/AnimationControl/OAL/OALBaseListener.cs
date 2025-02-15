//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.9.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from OAL.g4 by ANTLR 4.9.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419


using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IOALListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.9.2")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class OALBaseListener : IOALListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.lines"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLines([NotNull] OALParser.LinesContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.lines"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLines([NotNull] OALParser.LinesContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.line"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLine([NotNull] OALParser.LineContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.line"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLine([NotNull] OALParser.LineContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.parCommand"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterParCommand([NotNull] OALParser.ParCommandContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.parCommand"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitParCommand([NotNull] OALParser.ParCommandContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.ifCommand"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIfCommand([NotNull] OALParser.IfCommandContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.ifCommand"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIfCommand([NotNull] OALParser.IfCommandContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.whileCommand"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterWhileCommand([NotNull] OALParser.WhileCommandContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.whileCommand"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitWhileCommand([NotNull] OALParser.WhileCommandContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.foreachCommand"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterForeachCommand([NotNull] OALParser.ForeachCommandContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.foreachCommand"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitForeachCommand([NotNull] OALParser.ForeachCommandContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.continueCommand"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterContinueCommand([NotNull] OALParser.ContinueCommandContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.continueCommand"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitContinueCommand([NotNull] OALParser.ContinueCommandContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.breakCommand"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBreakCommand([NotNull] OALParser.BreakCommandContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.breakCommand"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBreakCommand([NotNull] OALParser.BreakCommandContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.commentCommand"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCommentCommand([NotNull] OALParser.CommentCommandContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.commentCommand"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCommentCommand([NotNull] OALParser.CommentCommandContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.exeCommandQueryCreate"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExeCommandQueryCreate([NotNull] OALParser.ExeCommandQueryCreateContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.exeCommandQueryCreate"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExeCommandQueryCreate([NotNull] OALParser.ExeCommandQueryCreateContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.exeCommandQueryRelate"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExeCommandQueryRelate([NotNull] OALParser.ExeCommandQueryRelateContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.exeCommandQueryRelate"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExeCommandQueryRelate([NotNull] OALParser.ExeCommandQueryRelateContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.exeCommandQuerySelect"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExeCommandQuerySelect([NotNull] OALParser.ExeCommandQuerySelectContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.exeCommandQuerySelect"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExeCommandQuerySelect([NotNull] OALParser.ExeCommandQuerySelectContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.exeCommandQuerySelectRelatedBy"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExeCommandQuerySelectRelatedBy([NotNull] OALParser.ExeCommandQuerySelectRelatedByContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.exeCommandQuerySelectRelatedBy"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExeCommandQuerySelectRelatedBy([NotNull] OALParser.ExeCommandQuerySelectRelatedByContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.exeCommandQueryDelete"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExeCommandQueryDelete([NotNull] OALParser.ExeCommandQueryDeleteContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.exeCommandQueryDelete"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExeCommandQueryDelete([NotNull] OALParser.ExeCommandQueryDeleteContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.exeCommandQueryUnrelate"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExeCommandQueryUnrelate([NotNull] OALParser.ExeCommandQueryUnrelateContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.exeCommandQueryUnrelate"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExeCommandQueryUnrelate([NotNull] OALParser.ExeCommandQueryUnrelateContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.exeCommandAssignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExeCommandAssignment([NotNull] OALParser.ExeCommandAssignmentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.exeCommandAssignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExeCommandAssignment([NotNull] OALParser.ExeCommandAssignmentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.exeCommandCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExeCommandCall([NotNull] OALParser.ExeCommandCallContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.exeCommandCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExeCommandCall([NotNull] OALParser.ExeCommandCallContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.exeCommandCreateList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExeCommandCreateList([NotNull] OALParser.ExeCommandCreateListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.exeCommandCreateList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExeCommandCreateList([NotNull] OALParser.ExeCommandCreateListContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.exeCommandAddingToList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExeCommandAddingToList([NotNull] OALParser.ExeCommandAddingToListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.exeCommandAddingToList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExeCommandAddingToList([NotNull] OALParser.ExeCommandAddingToListContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.exeCommandRemovingFromList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExeCommandRemovingFromList([NotNull] OALParser.ExeCommandRemovingFromListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.exeCommandRemovingFromList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExeCommandRemovingFromList([NotNull] OALParser.ExeCommandRemovingFromListContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.exeCommandWrite"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExeCommandWrite([NotNull] OALParser.ExeCommandWriteContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.exeCommandWrite"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExeCommandWrite([NotNull] OALParser.ExeCommandWriteContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.exeCommandRead"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExeCommandRead([NotNull] OALParser.ExeCommandReadContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.exeCommandRead"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExeCommandRead([NotNull] OALParser.ExeCommandReadContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.returnCommand"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterReturnCommand([NotNull] OALParser.ReturnCommandContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.returnCommand"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitReturnCommand([NotNull] OALParser.ReturnCommandContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExpr([NotNull] OALParser.ExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExpr([NotNull] OALParser.ExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.instanceHandle"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterInstanceHandle([NotNull] OALParser.InstanceHandleContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.instanceHandle"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitInstanceHandle([NotNull] OALParser.InstanceHandleContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.instanceName"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterInstanceName([NotNull] OALParser.InstanceNameContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.instanceName"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitInstanceName([NotNull] OALParser.InstanceNameContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.keyLetter"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterKeyLetter([NotNull] OALParser.KeyLetterContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.keyLetter"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitKeyLetter([NotNull] OALParser.KeyLetterContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.whereExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterWhereExpression([NotNull] OALParser.WhereExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.whereExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitWhereExpression([NotNull] OALParser.WhereExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.className"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterClassName([NotNull] OALParser.ClassNameContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.className"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitClassName([NotNull] OALParser.ClassNameContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.variableName"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariableName([NotNull] OALParser.VariableNameContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.variableName"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariableName([NotNull] OALParser.VariableNameContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.methodName"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMethodName([NotNull] OALParser.MethodNameContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.methodName"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMethodName([NotNull] OALParser.MethodNameContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.attribute"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAttribute([NotNull] OALParser.AttributeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.attribute"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAttribute([NotNull] OALParser.AttributeContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.relationshipLink"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterRelationshipLink([NotNull] OALParser.RelationshipLinkContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.relationshipLink"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitRelationshipLink([NotNull] OALParser.RelationshipLinkContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="OALParser.relationshipSpecification"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterRelationshipSpecification([NotNull] OALParser.RelationshipSpecificationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="OALParser.relationshipSpecification"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitRelationshipSpecification([NotNull] OALParser.RelationshipSpecificationContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
