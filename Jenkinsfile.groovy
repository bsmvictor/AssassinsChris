pipeline {
    agent {
        docker {
            image 'unityci/editor:6000.0.28f1-webgl-3'
            args '-v /c/Jenkins/workspace/Unity/AssassinsChrisGame:/workspace'
        }
    }

    environment {
        UNITY_LICENSE = credentials('unity-license-key') // Adicione sua licença Unity às credenciais do Jenkins
        PROJECT_PATH = 'C:\\jenkins_home\\workspace\\Unity\\AssassinsChrisGame' // Caminho do projeto Unity dentro do container
    }

    stages {
        stage('Activate License') {
            steps {
                echo 'Ativando licença do Unity...'
                sh """
                unity-editor \
                -batchmode \
                -logFile /dev/stdout \
                -username <SEU_USUÁRIO_UNITY> \
                -password <SUA_SENHA_UNITY> \
                -serial ${UNITY_LICENSE}
                """
            }
        }

        stage('Run Tests') {
            steps {
                echo 'Executando testes...'
                sh """
                unity-editor \
                -quit -batchmode \
                -projectPath ${PROJECT_PATH} \
                -executeMethod TestReportGenerator.RunTestsAndGenerateReport \
                -logFile /dev/stdout
                """
            }
        }

        stage('Build WebGL') {
            steps {
                echo 'Iniciando build para WebGL...'
                sh """
                unity-editor \
                -quit -batchmode \
                -projectPath ${PROJECT_PATH} \
                -executeMethod BuildScript.BuildWebGL \
                -buildTarget WebGL \
                -logFile /dev/stdout
                """
            }
        }
    }

    post {
        success {
            echo 'Build concluída com sucesso!'
            archiveArtifacts artifacts: 'Builds/**', allowEmptyArchive: true
        }
        failure {
            echo 'Build falhou. Verifique os logs.'
        }
    }
}
