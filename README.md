# Programming Languages and Compilers

This repository contains my implementation of the assignment for the Czech
course [Programovací jazyky a překladače (PJP)](https://edison.sso.vsb.cz/cz.vsb.edison.edu.study.prepare.web/SubjectVersion.faces?version=460-2018/03&subjectBlockAssignmentId=518302&studyFormId=1&studyPlanId=25873&locale=en)
at VŠB-TUO. The goal of the course was to get an overview of the area of programming, main programming paradigms (imperative, functional, logic) and their typical representatives.

Page of the course: http://behalek.cs.vsb.cz/wiki/index.php/Programming_Languages_and_Compilers

## Prerequisites
Project is configured for .NET 8.0. You can download the latest version of .NET from [here](https://dotnet.microsoft.com/download).

## How to run
The easiest way to run the project is to open the solution in IDE (Visual Studio, ...). You need to specify the input file as an argument `--input <file>`. 

## Project assignment
Solution contains one project. The application takes an input file and generates output, which is then run by the interpreter.

### Basic description
Project will be composed of following steps:

1. **Using ANTLR**, implement a parser for the language specified bellow. If there is at least one syntax error, report this error (or errors) and stop the computations.
2. If there are no syntactic errors, perform the type checking. If there are some type errors, report all these errors and stop the computation.
3. If there are no type errors, generate appropriate target code. It will be a text file composed from stack-based instructions that are defined bellow.
4. Implement an interpreter, that gets a text file with these instructions and evaluates them.

### Language specification
#### Program's formatting
The program consists of a sequence of commands. Commands are written with free formatting. Comments, spaces, tabs, and line breaks serve only as delimiters and do not affect the meaning of the program. Comments are bounded by two slashes and the end of the line. Keywords are reserved. Identifiers and keywords are case sensitive.

#### Literals
There are following literals:

- integers - `int` - sequence of digits.
- floating point numbers - `float` - sequence of digits containing a `'.'` character.
- booleans - `bool` - values: `true` and `false`.
- strings - `string` - text given in quotation marks: `"text"`. Escape sequences are optional in our strings.

#### Variables
Variable's identifiers are composed from letters and digits, and it must start with a letter. Each variable must be declared before it is used. Repeated declaration of a variable with the same name is an error. Variables must have one of the following types: `int`, `float`, `bool` or `string`. After the variables are declared, they have initial values: `0`, `0.0`, `""` respectively `false`.

#### Statements
Following statements are defined:

- `;` - empty command.
- `type variable, variable, ... ;` - declaration of variables, all these variables have the same type `type`. It can be one of: `int`, `float`, `bool`, `String`
- `expression ;` - it evaluates given expression, the resulting value of the expression is ignored. Note, there can be some side effects like an assignment to a variable.
- `read variable, variable, ... ;` - it reads values from standard input and then these values are assigned to corresponding variables. Each of these input values is on a separate line and it is verified, that have an appropriate type.
- `write expression, expression, ... ;` - it writes values of expressions to standard output. The `"\n"` character is written after the value of the last expression.
- `{ statement statement ... }` - block of statements.
- `if (condition) statement [else statement]` - conditional statement - condition is an expression with a type: `bool`. The else part of the statement is optional.
- `while (condition) statement` - a cycle - condition must be a `bool` expression. This cycle repeats the given statement while the condition holds (it is `true`).

#### Expression
Lists in expressions trees are literals or variables. Types of operands must preserve the type of the operator. If necessary, `int` values are **automatically** cast to `float`. In other word, the type of `5 + 5.5` is `float`, and number `5` which type `int` is automatically converted to `float`. There is **no** conversion from `float` to `int`!

Following table defines operators in our expressions. Operator Signature is defined using letters: *I*, *R*, *B*, *S* which corespods to types: `int`, `float`, `bool`, `string`.

| Description                 | Operator   | Operator's Signature                                     |
|:----------------------------|:-----------|:---------------------------------------------------------|
| unnary minus                | -          | $I \rightarrow I \lor F \rightarrow F$                   |
| binary arithmetic operators | +, -, *, / | $I \times I \rightarrow I \lor F \times F \rightarrow F$ |
| modulo                      | %          | $I \times I \rightarrow I$                               |
| concatenation of strings    | .          | $S \times S \rightarrow S$                               |
| relational operators        | < >        | $x \times x \rightarrow B,\ where\ x \in \{I,F\}$        |
| comparison                  | == !=      | $x \times x \rightarrow B,\ where\ x \in \{I,F,S\}$      |
| logic and, or               | &&, \|\|   | $B \times B \rightarrow B$                               |
| logic not                   | !          | $B \rightarrow B$                                        |
| assignment                  | =          | $x \times x \rightarrow x,\ where\ x \in \{I,F,S,B\}$    |

In the assignment, left operand is strictly a variable and the right operand is expression. The type of the variable is the type of the left operand. A side effect is storing the value on the right side into the variable. The automatic conversion cannot change the type of the variable, i.e., it is impossible to store `float` value in `int` variable.

We can **use parentheses** in expressions.

All operators (except `=`) have left associativity (`=` have right associativity), and their priority is (from lowest to highest):

1. =
2. ||
3. &&
4. == !=
5. < >
6. \+ - .
7. \* / %
8. !
9. unary -

### Inputs
Inputs are located in the [input](/Project/input) directory. Each input file contains a program that should be executed

### Our (Stack-based) Instructions Set

All instructions are stack based. The main memory is a stack and while evaluating the instructions, the input data are taken from stack and the results are put also in stack.


| Instruction | Description                                                                                                                                                        |
|:------------|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| add	        | binary `+`                                                                                                                                                         |
| sub	        | binary `-`                                                                                                                                                         |
| mul	        | binary `*`                                                                                                                                                         |
| div	        | binary `/`                                                                                                                                                         |
| mod	        | binary `%`                                                                                                                                                         |
| uminus	     | unary `-`                                                                                                                                                          |
| concat	     | binary `.` - concatenation od strings                                                                                                                              |
| and	        | binary `&&`                                                                                                                                                        |
| or	         | binary `\|\|`                                                                                                                                                      |
| gt	         | binary `>`                                                                                                                                                         |
| lt	         | binary `<`                                                                                                                                                         |
| eq	         | binary `==` - compares two values                                                                                                                                  |
| not	        | unary `!` - negating boolean value                                                                                                                                 |
| itof        | Instruction takes int value from the stack, converts it to float and returns it to stack.                                                                          |
| push T x	   | Instruction pushs the value x of type `T`. Where `T` represents `I - int`, `F - float`, `S - string`, `B - bool`. Example: push I 10, push B true, push S "A B C " |
| pop	        | Instruction takes on value from the stack and discards it.                                                                                                         |
| load id	    | Instruction loads value of variable `id` on stack.                                                                                                                 |
| save id	    | Instruction takes value from the top of the stack and stores it into the variable with name `id`                                                                   |
| label n	    | Instruction marks the spot in source code with unique number `n`                                                                                                   |
| jmp n	      | Instruction jumps to the label defined by unique number `n`                                                                                                        |
| fjmp n	     | Instruction takes boolean value from the stack and if it is `false`, it will perform a jump to a label with unique number `n`                                      |
| print n	    | Instruction takes `n` values from stack and prints them on standard output                                                                                         |
| read T	     | Instruction reads value of type `T` (`I - int`, `F - float`, `S - string`, `B - bool`) from standard input and stores in on the stack                              |
