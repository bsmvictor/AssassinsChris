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
             steps {
                 echo "Build Windows executada em: ${new Date()}"
                 bat """
                 "${env.UNITY_PATH}" -quit -batchmode -projectPath "${env.PROJECT_PATH}" -executeMethod BuildScript.BuildWindows -logFile -
                 """
             }
        }

        stage('Send Email') {
            steps {
                script {
                    def subject = "Pipeline Executado!"
                    def body = "A execução do pipeline foi concluída com sucesso."
                    def recipient = env.RECIPIENT_EMAIL

                    bat """
                    powershell -Command \"Send-MailMessage -From 'jenkins@domain.com' -To '${recipient}' -Subject '${subject}' -Body '${body}' -SmtpServer 'smtp.yourserver.com' -Port 587 -Credential (New-Object System.Management.Automation.PSCredential('your_username', (ConvertTo-SecureString 'your_password' -AsPlainText -Force)))\"
                    """
                }
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
