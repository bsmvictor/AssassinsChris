pipeline {
    agent { label 'windows-node' }

    environment {
        UNITY_PATH = 'C:\\Program Files\\Unity\\Hub\\Editor\\6000.0.24f1\\Editor\\Unity.exe'
        PROJECT_PATH = 'C:\\jenkins\\workspace\\AssassinsChris'
    //EMAIL_RECIPIENTS = credentials('email-recipients')
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

        stage('Build WebGL') {
            steps {
                echo "Build WebGL executada em: ${new Date()}"
                bat """
                "${env.UNITY_PATH}" -quit -batchmode -projectPath "${env.PROJECT_PATH}" -executeMethod BuildScript.BuildWebGL -logFile -
                """
            }
        }
    }
    
    post {
        always {
            archiveArtifacts artifacts: '**/*.zip', allowEmptyArchive: true
            archiveArtifacts artifacts: '**/TestResults/*.xml', allowEmptyArchive: true
        }
    }
}