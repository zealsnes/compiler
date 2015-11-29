grammar ZealCpu;

// =========
// = Lexer =
// =========
HEADER: 'header';
VECTORS: 'vectors';
PROCEDURE: 'procedure';

IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]* ;

STRING_LITERAL: '"' .*? '"' ;
HEX_LITERAL: '$' [0-9a-fA-F]+ ;
INTEGER_LITERAL: '-'? [0-9]+ ;
BINARY_LITERAL: '%' [0-1]+;

WHITESPACE : (' '|'\t'|'\n')+ -> channel(HIDDEN) ;

// = Parser =
// ==========
root
	: headerDeclaration?
	;

headerDeclaration
	: HEADER '{'
		headerInfo*
	'}'
	;

headerInfo
	: IDENTIFIER '=' literal
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
