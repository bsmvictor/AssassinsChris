#!/bin/bash

# Obter o e-mail do destinatário a partir da variável de ambiente
RECIPIENT_EMAIL=${RECIPIENT_EMAIL}

if [ -z "$RECIPIENT_EMAIL" ]; then
  echo "Erro: A variável de ambiente RECIPIENT_EMAIL não está configurada."
  exit 1
fi

# Assunto e corpo do e-mail
SUBJECT="Pipeline Executado!"
BODY="A execução do pipeline foi concluída com sucesso."

# Enviar o e-mail usando o comando mail
echo "$BODY" | mail -s "$SUBJECT" "$RECIPIENT_EMAIL"

# Verificar se o e-mail foi enviado com sucesso
if [ $? -eq 0 ]; then
  echo "E-mail enviado com sucesso para $RECIPIENT_EMAIL!"
else
  echo "Erro ao enviar o e-mail."
  exit 1
fi
