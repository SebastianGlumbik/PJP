// Inspired by https://github.com/antlr/grammars-v4/blob/master/java/java20/Java20Lexer.g4
grammar Grammar;

program: statement+;

// Statement
statement
    : SEMICOLON                                                                     # emptyCommand
    | type VARIABLE (COMMA VARIABLE)* SEMICOLON                                     # declarationVariable 
    | expression SEMICOLON                                                          # expressionEvaluation
    | READ VARIABLE (COMMA VARIABLE)* SEMICOLON                                     # readInput
    | WRITE expression (COMMA expression)* SEMICOLON                                # writeOutput
    | '{' statement* '}'                                                            # blockStatement
    | IF '(' expression ')' statement (ELSE statement)?                             # conditionalStatement
    | WHILE '(' expression ')' statement                                            # whileCycle
    | FOR '(' expression SEMICOLON expression SEMICOLON expression ')' statement    # forCycle
    ;

// Expression
expression
    : '(' expression ')'                                                        # parentheses
    | INT                                                                       # integer
    | FLOAT                                                                     # float
    | BOOL                                                                      # boolean
    | STRING                                                                    # string
    | VARIABLE                                                                  # variable
    | MINUS expression                                                          # unaryMinus
    | NEGATION expression                                                       # negation
    | expression op=(MULTIPLY | DIVIDE | MODULO) expression                     # multiplyDivideModulo
    | expression op=(PLUS | MINUS | CONCAT) expression                          # plusMinusConcat
    | expression op=(LESS | GREATER) expression                                 # comparison
    | expression op=(EQUAL | NOT_EQUAL) expression                              # equality
    | expression AND expression                                                 # and
    | expression OR expression                                                  # or
    | <assoc=right> expression QUESTION_MARK expression COLON expression        # ternary
    | <assoc=right> VARIABLE ASSIGN expression                                  # assignment
    ;

// Type
type
    : INT_KEYWORD
    | FLOAT_KEYWORD
    | BOOL_KEYWORD
    | STRING_KEYWORD
    ;

// Keywords
INT_KEYWORD: 'int';
FLOAT_KEYWORD: 'float';
BOOL_KEYWORD: 'bool';
STRING_KEYWORD: 'string';
READ: 'read';
WRITE: 'write';
IF: 'if';
ELSE: 'else';
WHILE: 'while';
FOR: 'for';
QUESTION_MARK: '?';
COLON: ':';
SEMICOLON : ';';
COMMA : ',';
NEGATION : '!';
MULTIPLY : '*';
DIVIDE : '/';
MODULO : '%';
PLUS : '+';
MINUS : '-';
CONCAT : '.';
LESS : '<';
GREATER : '>';
EQUAL : '==';
NOT_EQUAL : '!=';
AND : '&&';
OR : '||';
ASSIGN : '=';

// Literals
INT: [0-9]+;
FLOAT: [0-9]+'.'[0-9]+;
BOOL: 'true' | 'false';
STRING: '"' StringCharacter* '"';

fragment StringCharacter: ~["\\\r\n] | EscapeSequence;
fragment EscapeSequence: '\\' [btnfr"'\\] | OctalEscape;
fragment OctalEscape: '\\' ([0-3]? [0-7])? [0-7];

// Variables
VARIABLE: [a-zA-Z]([a-zA-Z0-9])*;

// Skip spaces and comments
COMMENT: '//' ~[\r\n]* -> skip;
WHITESPACE : [ \t\r\n]+ -> skip;