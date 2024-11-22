# Usar imagem base oficial do Jenkins LTS
FROM jenkins/jenkins:latest

# Definir o ambiente como não interativo
ENV DEBIAN_FRONTEND=noninteractive

# Atualizar e instalar dependências, incluindo Docker e outras utilitárias
USER root
RUN apt-get update && apt-get install -y \
    sudo \
    curl \
    wget \
    unzip \
    git \
    apt-transport-https \
    ca-certificates \
    software-properties-common \
    && curl -fsSL https://download.docker.com/linux/debian/gpg | apt-key add - \
    && add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/debian $(lsb_release -cs) stable" \
    && apt-get update \
    && apt-get install -y docker-ce docker-ce-cli containerd.io \
    && usermod -aG docker jenkins \
    && rm -rf /var/lib/apt/lists/*

# Verificar se o Docker CLI foi instalado corretamente
RUN docker --version

# Copiar o arquivo plugins.txt para o container
COPY plugins.txt /usr/share/jenkins/ref/plugins.txt

# Instalar os plugins listados em plugins.txt
RUN jenkins-plugin-cli --plugins --verbose < /usr/share/jenkins/ref/plugins.txt

# Configurar o volume para persistência
VOLUME /var/jenkins_home

# Expor a porta padrão do Jenkins
EXPOSE 8080

# Expor a porta para os agentes do Jenkins
EXPOSE 50000

# Comando para inicializar o Jenkins
CMD ["java", "-jar", "/usr/share/jenkins/jenkins.war"]
