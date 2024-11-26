pipeline {
    agent { label 'windows-node' }

    environment {
        UNITY_PATH = 'C:\\Program Files\\Unity\\Hub\\Editor\\6000.0.24f1\\Editor\\Unity.exe'
        PROJECT_PATH = 'C:\\jenkins\\workspace\\AssassinsChris'
        RECIPIENT_EMAIL = credentials('EMAIL_VAR')
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
            when({expression {params.Windows_build}})
             steps {
                 echo "Build Windows executada em: ${new Date()}"
                 bat """
                 "${env.UNITY_PATH}" -quit -batchmode -projectPath "${env.PROJECT_PATH}" -executeMethod BuildScript.BuildWindows -logFile -
                 """
             }
        }

        stage('Exec Script'){
            steps {
                sh 'chmod +x send_email.sh && ./send_email.sh'
            }
        }
    }

    post {
        always {
            archiveArtifacts artifacts: 'Builds/**.zip', allowEmptyArchive: true
            archiveArtifacts artifacts: 'Builds/TestResults/*.xml', allowEmptyArchive: true
        }
    }
}
