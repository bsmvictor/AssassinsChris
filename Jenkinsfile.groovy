pipeline {
    agent { label 'windows-node' }

    environment {
        UNITY_PATH = 'C:\\Program Files\\Unity\\Hub\\Editor\\6000.0.24f1\\Editor\\Unity.exe'
        PROJECT_PATH = 'C:\\Jenkins\\workspace\\AssassinsChris'
    }

    stages {
        stage('Run Tests') {
            steps {
                bat """
                "${env.UNITY_PATH}" -quit -batchmode -projectPath "${env.PROJECT_PATH}" -executeMethod TestReportGenerator.RunTestsAndGenerateReport -logFile -
                """
            }
        }

        stage('Build Windows') {
            steps {
                echo "Build executada em: ${new Date()}"
                bat """
                "${env.UNITY_PATH}" -quit -batchmode -projectPath "${env.PROJECT_PATH}" -executeMethod BuildScript.BuildWindows -logFile -
                """
            }
        }

        stage('Verify build') {
            steps {
                echo "Verificando conteúdo do diretório Builds:"
                bat 'dir "${env.PROJECT_PATH}\\Builds" /s'
            }

    }

    post {
        always {
            archiveArtifacts artifacts: 'AssassinsChris//Builds/**.zip', allowEmptyArchive: true
            archiveArtifacts artifacts: 'AssassinsChris//Builds/TestResults/*.xml', allowEmptyArchive: true
            }
        }
    }
}