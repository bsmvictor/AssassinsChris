pipeline {
    agent { label 'windows-node' }

    environment {
        UNITY_PATH = 'C:\\Program Files\\Unity\\Hub\\Editor\\6000.0.24f1\\Editor\\Unity.exe'
        PROJECT_PATH = 'C:\\jenkins_home\\workspace\\unity-pipeline\\AssassinsChrisGame'
    }

    stages {
        stage('Checkout') {
            steps {
                echo "Iniciando o checkout do repositório"
                checkout([$class: 'GitSCM',
                          branches: [[name: '*/main']],
                          userRemoteConfigs: [[
                              url: 'https://github.com/bsmvictor/AssassinsChris.git',
                              credentialsId: 'github-key'
                          ]]])
            }
        }

        stage('Run Tests') {
            steps {
                echo "Executando testes com Unity"
                echo "Unity Path: ${env.UNITY_PATH}"
                echo "Project Path: ${env.PROJECT_PATH}"
                bat """
                "${env.UNITY_PATH}" -quit -batchmode -projectPath "${env.PROJECT_PATH}" -executeMethod TestReportGenerator.RunTestsAndGenerateReport -logFile -
                """
            }
        }

        stage('Build Windows') {
            steps {
                echo "Iniciando build para Windows"
                bat """
                "${env.UNITY_PATH}" -quit -batchmode -projectPath "${env.PROJECT_PATH}" -executeMethod BuildScript.BuildWindows -logFile -
                """
            }
        }
    }

    post {
        always {
            echo "Arquivando artefatos"
            archiveArtifacts artifacts: '**/Builds/**.zip', allowEmptyArchive: true
            archiveArtifacts artifacts: '**/Builds/TestResults/*.xml', allowEmptyArchive: true
        }
    }
}
