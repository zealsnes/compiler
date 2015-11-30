grammar ZealCpu;

// =========
// = Lexer =
// =========
HEADER: 'header';
VECTORS: 'vectors';
PROCEDURE: 'procedure';
FUNCTION: 'function';
INTERRUPT: 'interrupt';

INSTRUCTION: [a-z]+ ;
IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]* ;

STRING_LITERAL: '"' .*? '"' ;
HEX_LITERAL: '$' [0-9a-fA-F]+ ;
INTEGER_LITERAL: '-'? [0-9]+ ;
BINARY_LITERAL: '%' [0-1]+;

LINE_COMMENT: ';' ~[\r\n]* -> skip;
WHITESPACE : (' '|'\t'|'\n'|'\r')+ -> channel(HIDDEN) ;

// ==========
// = Parser =
// ==========
root
	: (
	  headerDeclaration
	| vectorsDeclaration
	| procedureDeclaration
	)*
	;

headerDeclaration
	: HEADER '{'
		headerInfo*
	'}'
	;

headerInfo
	: headerType=IDENTIFIER '=' headerValue=literal
	;

vectorsDeclaration
	: VECTORS '{'
		vectorInfo*
	'}'
	;

vectorInfo
	: vectorType=IDENTIFIER '=' labelName=IDENTIFIER
	;

procedureDeclaration
	: PROCEDURE name=IDENTIFIER '{'
		statement*
	'}'
	;

statement
	: instructionStatement
	;

instructionStatement
	: impliedInstruction
	;

impliedInstruction
	: opcode=INSTRUCTION
	;

literal
	: STRING_LITERAL
	| IDENTIFIER
	| numberLiteral
	;

numberLiteral
	: HEX_LITERAL
	| INTEGER_LITERAL
	| BINARY_LITERAL
	;
