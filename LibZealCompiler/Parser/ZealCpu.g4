grammar ZealCpu;

// =========
// = Lexer =
// =========
HEADER: 'header';
VECTORS: 'vectors';
PROCEDURE: 'procedure';
INTERRUPT: 'interrupt';

ADC: 'adc';
AND: 'and';
ASL: 'asl';
BCC: 'bcc';
BCS: 'bcs';
BEQ: 'beq';
BIT: 'bit';
BMI: 'bmi';
BNE: 'bne';
BPL: 'bpl';
BRA: 'bra';
BRK: 'brk';
BRL: 'brl';
BVC: 'bvc';
BVS: 'bvs';
CLC: 'clc';
CLD: 'cld';
CLI: 'cli';
CLV: 'clv';
CMP: 'cmp';
COP: 'cop';
CPX: 'cpx';
CPY: 'cpy';
DEC: 'dec';
DEX: 'dex';
DEY: 'dey';
EOR: 'eor';
INC: 'inc';
INX: 'inx';
INY: 'iny';
JMP: 'jmp';
JML: 'jml';
JSR: 'jsr';
JSL: 'jsl';
LDA: 'lda';
LDX: 'ldx';
LDY: 'ldy';
LSR: 'lsr';
MVN: 'mvn';
MVP: 'mvp';
NOP: 'nop';
ORA: 'ora';
PEA: 'pea';
PER: 'per';
PHA: 'pha';
PHB: 'phb';
PHD: 'phd';
PHK: 'phk';
PHP: 'php';
PHX: 'phx';
PHY: 'phy';
PLA: 'pla';
PLB: 'plb';
PLD: 'pld';
PLP: 'plp';
PLX: 'plx';
PLY: 'ply';
REP: 'rep';
ROL: 'rol';
ROR: 'ror';
RTI: 'rti';
RTL: 'rtl';
RTS: 'rts';
SBC: 'sbc';
SEC: 'sec';
SED: 'sed';
SEI: 'sei';
SEP: 'sep';
STA: 'sta';
STP: 'stp';
STX: 'stx';
STY: 'sty';
STZ: 'stz';
TAX: 'tax';
TAY: 'tay';
TCD: 'tcd';
TCS: 'tcs';
TDC: 'tdc';
TRB: 'trb';
TSB: 'tsb';
TSC: 'tsc';
TSX: 'tsx';
TXA: 'txa';
TXS: 'txs';
TXY: 'txy';
TYA: 'tya';
TYX: 'tyx';
WAI: 'wai';
XBA: 'xba';
XCE: 'xce';

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
	: label? opcode argument?
	;

opcode
	: ADC
	| AND
	| ASL
	| BCC
	| BCS
	| BEQ
	| BIT
	| BMI
	| BNE
	| BPL
	| BRA
	| BRK
	| BRL
	| BVC
	| BVS
	| CLC
	| CLD
	| CLI
	| CLV
	| CMP
	| COP
	| CPX
	| CPY
	| DEC
	| DEX
	| DEY
	| EOR
	| INC
	| INX
	| INY
	| JMP
	| JML
	| JSR
	| JSL
	| LDA
	| LDX
	| LDY
	| LSR
	| MVN
	| MVP
	| NOP
	| ORA
	| PEA
	| PER
	| PHA
	| PHB
	| PHD
	| PHK
	| PHP
	| PHX
	| PHY
	| PLA
	| PLB
	| PLD
	| PLP
	| PLX
	| PLY
	| REP
	| ROL
	| ROR
	| RTI
	| RTL
	| RTS
	| SBC
	| SEC
	| SED
	| SEI
	| SEP
	| STA
	| STP
	| STX
	| STY
	| STZ
	| TAX
	| TAY
	| TCD
	| TCS
	| TDC
	| TRB
	| TSB
	| TSC
	| TSX
	| TXA
	| TXS
	| TXY
	| TYA
	| TYX
	| WAI
	| XBA
	| XCE
	;

argument
	: argumentLiteral # Address
	| '#' numberLiteral # Immediate
	;

literal
	: STRING_LITERAL
	| IDENTIFIER
	| numberLiteral
	;

argumentLiteral
	: IDENTIFIER
	| numberLiteral
	;

numberLiteral
	: HEX_LITERAL
	| INTEGER_LITERAL
	| BINARY_LITERAL
	;

label:
	IDENTIFIER ':'
	;
