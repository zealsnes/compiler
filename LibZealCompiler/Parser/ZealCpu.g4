grammar ZealCpu;

// =========
// = Lexer =
// =========
HEADER: 'header';
VECTORS: 'vectors';
PROCEDURE: 'procedure';
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
	| interruptDeclaration
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

interruptDeclaration
	: INTERRUPT name=IDENTIFIER '{'
		statement*
	'}'
	;

statement
	: instructionStatement
	;

instructionStatement
	: opcode=INSTRUCTION argument
	;

argument
	: numberLiteral # Address
	| '#' numberLiteral # Immediate
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
