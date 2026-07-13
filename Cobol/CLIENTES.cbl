IDENTIFICATION DIVISION.
       PROGRAM-ID. CLIENTES.
       
       ENVIRONMENT DIVISION.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.
           SELECT CLIENTE-FILE ASSIGN TO 'clientes.dat'
           ORGANIZATION IS INDEXED
           ACCESS MODE IS DYNAMIC
           RECORD KEY IS FD-CPF
           FILE STATUS IS WS-FILE-STATUS.

       DATA DIVISION.
       FILE SECTION.
       FD  CLIENTE-FILE.
       01  FD-CLIENTE-REC.
           05 FD-CPF         PIC X(11).
           05 FD-NOME        PIC X(30).
           05 FD-TELEFONE    PIC X(15).
           05 FD-EMAIL       PIC X(40).

       WORKING-STORAGE SECTION.
       01  WS-FILE-STATUS    PIC X(2).
       01  WS-CMD-LINE       PIC X(99).

       01  WS-DADOS.
           05 WS-ACAO        PIC X(1).
           05 WS-CPF         PIC X(11).
           05 WS-NOME        PIC X(30).
           05 WS-TELEFONE    PIC X(15).
           05 WS-EMAIL       PIC X(40).
           05 WS-STATUS      PIC X(2).

       PROCEDURE DIVISION.
       INICIO.
           ACCEPT WS-CMD-LINE FROM COMMAND-LINE.
           MOVE WS-CMD-LINE TO WS-DADOS.

           OPEN I-O CLIENTE-FILE.
           
           IF WS-FILE-STATUS = '35' OR WS-FILE-STATUS = '39'
              OPEN OUTPUT CLIENTE-FILE
              MOVE '12345678901' TO FD-CPF
              MOVE 'CLIENTE TESTE PROFESSORA' TO FD-NOME
              MOVE '11999998888' TO FD-TELEFONE
              MOVE 'teste@alfa.com' TO FD-EMAIL
              WRITE FD-CLIENTE-REC
              CLOSE CLIENTE-FILE
              OPEN I-O CLIENTE-FILE
           END-IF.

           IF WS-ACAO = 'C'
              PERFORM ROTINA-CONSULTA
           ELSE IF WS-ACAO = 'A'
              PERFORM ROTINA-ATUALIZACAO
           ELSE IF WS-ACAO = 'I'
              PERFORM ROTINA-INSERCAO
           ELSE IF WS-ACAO = 'E'
              PERFORM ROTINA-EXCLUSAO
           END-IF.

           CLOSE CLIENTE-FILE.
           DISPLAY WS-DADOS.
           STOP RUN.

       ROTINA-CONSULTA.
           MOVE WS-CPF TO FD-CPF.
           READ CLIENTE-FILE
               INVALID KEY
                   MOVE '04' TO WS-STATUS
               NOT INVALID KEY
                   MOVE FD-NOME TO WS-NOME
                   MOVE FD-TELEFONE TO WS-TELEFONE
                   MOVE FD-EMAIL TO WS-EMAIL
                   MOVE '00' TO WS-STATUS
           END-READ.

       ROTINA-ATUALIZACAO.
           MOVE WS-CPF TO FD-CPF.
           READ CLIENTE-FILE
               INVALID KEY
                   MOVE '04' TO WS-STATUS
               NOT INVALID KEY
                   MOVE WS-NOME TO FD-NOME
                   MOVE WS-TELEFONE TO FD-TELEFONE
                   MOVE WS-EMAIL TO FD-EMAIL
                   REWRITE FD-CLIENTE-REC
                   MOVE '00' TO WS-STATUS
           END-READ.

       ROTINA-INSERCAO.
           MOVE WS-CPF TO FD-CPF.
           READ CLIENTE-FILE
               NOT INVALID KEY
                   MOVE '05' TO WS-STATUS
               INVALID KEY
                   MOVE WS-CPF TO FD-CPF
                   MOVE WS-NOME TO FD-NOME
                   MOVE WS-TELEFONE TO FD-TELEFONE
                   MOVE WS-EMAIL TO FD-EMAIL
                   WRITE FD-CLIENTE-REC
                   MOVE '00' TO WS-STATUS
           END-READ.

       ROTINA-EXCLUSAO.
           MOVE WS-CPF TO FD-CPF.
           DELETE CLIENTE-FILE
               INVALID KEY
                   MOVE '04' TO WS-STATUS
               NOT INVALID KEY
                   MOVE '00' TO WS-STATUS
           END-DELETE.