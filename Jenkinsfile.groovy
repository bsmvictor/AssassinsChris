pipeline {
    agent { label 'windows-node' }

    environment {
        UNITY_PATH = 'C:\\Program Files\\Unity\\Hub\\Editor\\6000.0.24f1\\Editor\\Unity.exe'
        PROJECT_PATH = 'C:\\jenkins_home\\workspace\\unity-pipeline\\AssassinsChrisGame'
        DISCORD_WEBHOOK_URL = 'https://discord.com/api/webhooks/1311152479865405581/TlEfJv9XXJLcasfjX8PGmyANyMxFgb58OQuVIvf3eIkZA7Kx41UMC9C2dhffotpYEuNd'
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

            echo "Enviando notificação para o Discord"
            script {
                def message = [
                    content: "Pipeline finalizada! \nResultado: ${currentBuild.result} \nProjeto: Assassins Chris \nBuild número: ${env.BUILD_NUMBER}"
                ]

                writeFile file: 'discord_message.json', text: groovy.json.JsonOutput.toJson(message)

                bat """
                curl -H "Content-Type: application/json" -X POST -d @discord_message.json "${env.DISCORD_WEBHOOK_URL}"
                """
            }
        }
    }
}
