# Usar imagem base oficial do Jenkins LTS
FROM jenkins/jenkins:lts
COPY jenkins_home /var/jenkins_home

# Definir o ambiente como não interativo
ENV DEBIAN_FRONTEND=noninteractive

# Atualizar e instalar dependências básicas (opcional, para extensibilidade)
USER root
RUN apt-get update && apt-get install -y \
    sudo \
    curl \
    wget \
    unzip \
    git \
    && rm -rf /var/lib/apt/lists/*

# Configurar o Jenkins para o usuário padrão
USER jenkins

# Copiar o arquivo plugins.txt para dentro do container
COPY plugins.txt /usr/share/jenkins/ref/plugins.txt

# Usar o comando oficial para instalar os plugins listados no plugins.txt
RUN jenkins-plugin-cli --plugins --verbose < /usr/share/jenkins/ref/plugins.txt

# Configurar o volume de dados (para persistência)
VOLUME /var/jenkins_home

# Expor a porta padrão do Jenkins
EXPOSE 8080

# Expor a porta de agente JNLP (para agentes conectarem ao Jenkins master)
EXPOSE 50000

# Inicializar o Jenkins
CMD ["java", "-jar", "/usr/share/jenkins/jenkins.war"]